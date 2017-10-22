// 这是主 DLL 文件。

#include "stdafx.h"

#include "PDIWT_MS_CZ_CPP.h"

PDIWT_MS_CZ_CPP::LockHeadDrawing::LockHeadDrawing(LockHeadParameters^ lockheadparam)
{
	LH_LockHeadParameter = lockheadparam;
}

StatusInt PDIWT_MS_CZ_CPP::LockHeadDrawing::DoDraw()
{
	EditElementHandle _cz_whole;
	ISolidKernelEntityPtr _cz_whole_SKE;
#pragma region baseboard
	ISolidKernelEntityPtr _baseboard_SKE;
	if (SUCCESS != DrawBaseBoard(_baseboard_SKE, DPoint3d{ 0,0,0 }))
		return ERROR;
	ISolidKernelEntityPtr _baseboard_cut_SKE;
	if (SUCCESS != DrawBaseBoard_Cut(_baseboard_cut_SKE, DPoint3d{ 0,0,0 }))
		return ERROR;
#pragma endregion

	if ((SolidUtil::Modify::TransformBody(_baseboard_cut_SKE, GetModelTransform()) != SUCCESS)
		||
		(SolidUtil::Convert::BodyToElement(_cz_whole, *_baseboard_cut_SKE, nullptr, *ACTIVEMODEL) != SUCCESS))
		return ERROR;

	_cz_whole.AddToModel();
	return SUCCESS;
}

Transform PDIWT_MS_CZ_CPP::LockHeadDrawing::GetModelTransform()
{
	double modelscale = ACTIVEMODEL->GetDgnModelP()->GetModelInfo().GetUorPerMeter() / 1000;
	return Transform::FromFixedPointAndScaleFactors(DPoint3d::FromZero(), modelscale, modelscale, modelscale);
}

bvector<DPoint3d> PDIWT_MS_CZ_CPP::LockHeadDrawing::GetAddedPointVector(const DPoint3d originPoint, const bvector<DPoint3d>& relativeLengthVector)
{
	bvector<DPoint3d> _resultpoints;
	_resultpoints.push_back(originPoint);
	for (int index = 0; index < relativeLengthVector.size(); index++)
	{
		if (relativeLengthVector[index].AlmostEqual(DPoint3d::FromZero())) continue;
		_resultpoints.push_back(DPoint3d::FromSumOf(_resultpoints[index - 1], relativeLengthVector[index]));

	}
	return _resultpoints;
}



StatusInt PDIWT_MS_CZ_CPP::LockHeadDrawing::DrawBaseBoard(ISolidKernelEntityPtr &_outbaseboard, DPoint3dCR anchorPoint)
{
	BaseBoard^ _baseboardparam = LH_LockHeadParameter->LH_BaseBoard;

	//Draw Baseboard
	double _xCoord = -_baseboardparam->BaseBoardLength / 2;
	DgnBoxDetail _dgnBoxDeatil(
		Dpoint3d::FromSumOf(anchorPoint, DPoint3d{ _xCoord,0,0 }),
		Dpoint3d::FromSumOf(anchorPoint, DPoint3d{ _xCoord,0,_baseboardparam->BaseBoardHeight }),
		DVec3d::UnitX(),
		DVec3d::UnitY(),
		_baseboardparam->BaseBoardLength,
		_baseboardparam->BaseBoardWidth,
		_baseboardparam->BaseBoardLength,
		_baseboardparam->BaseBoardWidth,
		true);
	ISolidPrimitivePtr _boxspptr = ISolidPrimitive::CreateDgnBox(_dgnBoxDeatil);
	if (SolidUtil::Create::BodyFromSolidPrimitive(_outbaseboard, *_boxspptr, *ACTIVEMODEL) != SUCCESS)
		return ERROR;

	//Draw cut
	if (_baseboardparam->IsGrooving)
	{

	}
	return SUCCESS;
}

//此模块的anchor Point 地板前端底部中心点。
//  ------------------*------------------
// |									 |
// |									 |
// |									 |
// |									 |
//  -------------------------------------
StatusInt PDIWT_MS_CZ_CPP::LockHeadDrawing::DrawBaseBoard_Cut(ISolidKernelEntityPtr& _outbaseboardcut, DPoint3dCR anchorPoint)
{
	BaseBoard^ _baseboardparam = LH_LockHeadParameter->LH_BaseBoard;
	ShapeTGrooving^ _cutparam = _baseboardparam->TGrooving;

	double _y_adjust = _baseboardparam->BaseBoardWidth / 2;
	if (_cutparam->GroovingGradient <= 0) return ERROR;
	double _offset = _cutparam->GroovingHeight / _cutparam->GroovingGradient;
	bvector<DPoint3d> _bottom_section
	{
		DPoint3d::From(-_y_adjust,0),
		DPoint3d::From(0,-_cutparam->GroovingFrontLength),
		DPoint3d::From(_y_adjust - _cutparam->GroovingWidth / 2 ,0),
		DPoint3d::From(0,-_cutparam->GroovingBackLength),
		DPoint3d::From(_cutparam->GroovingWidth,0)
	};
	bvector<DPoint3d> _global_bottom_section_pts = GetAddedPointVector(anchorPoint, _bottom_section);
	auto _count = _global_bottom_section_pts.size();

	bvector<DPoint3d> _top_section
	{
		DPoint3d::From(-_y_adjust,0),
		DPoint3d::From(0,-_cutparam->GroovingFrontLength + _offset),
		DPoint3d::From(_y_adjust - _cutparam->GroovingWidth / 2 + _offset,0),
		DPoint3d::From(0,-_cutparam->GroovingBackLength),
		DPoint3d::From(_cutparam->GroovingWidth - _offset,0)
	};
	bvector<DPoint3d> _global_top_section_pts = GetAddedPointVector(DPoint3d::FromSumOf(anchorPoint, DPoint3d::From(0, 0, _cutparam->GroovingHeight)), _top_section);

	CurveVectorPtr _bottom_cv = CurveVector::CreateLinear(_global_bottom_section_pts, CurveVector::BOUNDARY_TYPE_Outer);
	CurveVectorPtr _top_cv = CurveVector::CreateLinear(_global_top_section_pts, CurveVector::BOUNDARY_TYPE_Outer);
	ISolidPrimitivePtr _cutptr = ISolidPrimitive::CreateDgnRuledSweep(DgnRuledSweepDetail(_bottom_cv, _top_cv, true));
	ISolidKernelEntityPtr  _outcut_right;
	if (SolidUtil::Create::BodyFromSolidPrimitive(_outbaseboardcut, *_cutptr, *ACTIVEMODEL) != SUCCESS)
		return ERROR;
	if (SolidUtil::CopyEntity(_outcut_right, *_outbaseboardcut) != SUCCESS)
		return ERROR;
	Transform _xplanmirrortrans;
	_xplanmirrortrans.InitFromMirrorPlane(DPoint3d::FromZero(), DVec3d::UnitX());
	if (SolidUtil::Modify::TransformBody(_outcut_right, _xplanmirrortrans) != SUCCESS)
		return ERROR;
	if (SolidUtil::Modify::BooleanUnion(_outbaseboardcut, &_outcut_right, 1) != SUCCESS)
		return ERROR;

	return SUCCESS;
}
