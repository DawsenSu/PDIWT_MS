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
	if (DrawBaseBoard(_baseboard_SKE, DPoint3d{ 0,0,0 }))
		return ERROR;
#pragma endregion

#pragma region SidePier
	ISolidKernelEntityPtr _sidepier_SKE;
	if (DrawSidePier(_sidepier_SKE, DPoint3d{ 0,0,0 }))
		return ERROR;
#pragma endregion

#pragma region DoorSill
	ISolidKernelEntityPtr _doorsill_SKE;
	if (DrawDoorSill(_doorsill_SKE, DPoint3d{ 0,0,0 }))
		return ERROR;
#pragma endregion

#pragma region LocalConcerationCulvert
	ISolidKernelEntityPtr _localconculvert_SKE;
	if (DrawLocalConcertationCulvert(_localconculvert_SKE, DPoint3d{ 0,0,0 }))
		return ERROR;
#pragma endregion


	if ((SolidUtil::Modify::TransformBody(_localconculvert_SKE, GetModelTransform(DPoint3d::FromZero())) != SUCCESS)
		||
		(SolidUtil::Convert::BodyToElement(_cz_whole, *_localconculvert_SKE, nullptr, *ACTIVEMODEL) != SUCCESS))
		return ERROR;

	_cz_whole.AddToModel();
	return SUCCESS;
}

Transform PDIWT_MS_CZ_CPP::LockHeadDrawing::GetModelTransform(DPoint3dCR _anchorpoint)
{
	double modelscale = ACTIVEMODEL->GetDgnModelP()->GetModelInfo().GetUorPerMeter() / 1000;
	return Transform::FromFixedPointAndScaleFactors(_anchorpoint, modelscale, modelscale, modelscale);
}

bvector<DPoint3d> PDIWT_MS_CZ_CPP::LockHeadDrawing::GetAddedPointVector(const DPoint3d originPoint, const bvector<DPoint3d>& relativeLengthVector)
{
	bvector<DPoint3d> _resultpoints;
	_resultpoints.push_back(originPoint);
	for (int index = 0; index < relativeLengthVector.size(); index++)
	{
		if (relativeLengthVector[index].AlmostEqual(DPoint3d::FromZero())) continue;
		_resultpoints.push_back(DPoint3d::FromSumOf(_resultpoints[index], relativeLengthVector[index]));

	}
	return _resultpoints;
}

StatusInt PDIWT_MS_CZ_CPP::LockHeadDrawing::CloneMirrorSolidAndUnion(ISolidKernelEntityPtr & _left_inoutSoild, DPoint3dCR _mirrorpoints)
{
	ISolidKernelEntityPtr _rightSolid;
	if (SolidUtil::CopyEntity(_rightSolid, *_left_inoutSoild))
		return ERROR;
	Transform _xymirrortrans;
	_xymirrortrans.InitFromMirrorPlane(_mirrorpoints, DVec3d::UnitX());
	if (SolidUtil::Modify::TransformBody(_rightSolid, _xymirrortrans))
		return ERROR;
	if (SolidUtil::Modify::BooleanUnion(_left_inoutSoild, &_rightSolid, 1))
		return ERROR;
	return SUCCESS;
}



StatusInt PDIWT_MS_CZ_CPP::LockHeadDrawing::DrawBaseBoard(ISolidKernelEntityPtr &_outbaseboard, DPoint3dCR _anchorPoint)
{
	BaseBoard^ _baseboardparam = LH_LockHeadParameter->LH_BaseBoard;

	//Draw Baseboard
	double _xCoord = -_baseboardparam->BaseBoardWidth / 2;
	DgnBoxDetail _dgnBoxDeatil(
		Dpoint3d::FromSumOf(_anchorPoint, DPoint3d{ _xCoord,0,0 }),
		Dpoint3d::FromSumOf(_anchorPoint, DPoint3d{ _xCoord,0,_baseboardparam->BaseBoardHeight }),
		DVec3d::UnitX(),
		DVec3d::UnitY(),
		_baseboardparam->BaseBoardWidth,
		_baseboardparam->BaseBoardLength,
		_baseboardparam->BaseBoardWidth,
		_baseboardparam->BaseBoardLength,
		true);
	ISolidPrimitivePtr _boxspptr = ISolidPrimitive::CreateDgnBox(_dgnBoxDeatil);
	if (SolidUtil::Create::BodyFromSolidPrimitive(_outbaseboard, *_boxspptr, *ACTIVEMODEL) != SUCCESS)
		return ERROR;

	//Draw cut
	if (_baseboardparam->IsGrooving)
	{
		ISolidKernelEntityPtr _baseboard_cut_SKN;
		if (DrawBaseBoard_Cut(_baseboard_cut_SKN, DPoint3d::FromSumOf(_anchorPoint, DPoint3d::From(0, _baseboardparam->BaseBoardLength))))
			return ERROR;
		if (SolidUtil::Modify::BooleanSubtract(_outbaseboard, &_baseboard_cut_SKN, 1))
			return ERROR;
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
StatusInt PDIWT_MS_CZ_CPP::LockHeadDrawing::DrawBaseBoard_Cut(ISolidKernelEntityPtr& _outbaseboardcut, DPoint3dCR _anchorPoint)
{
	BaseBoard^ _baseboardparam = LH_LockHeadParameter->LH_BaseBoard;
	ShapeTGrooving^ _cutparam = _baseboardparam->TGrooving;

	double _x_adjust = _baseboardparam->BaseBoardWidth / 2;
	if (_cutparam->GroovingGradient <= 0) return ERROR;
	double _offset = _cutparam->GroovingHeight / _cutparam->GroovingGradient;
	bvector<DPoint3d> _bottom_section
	{
		DPoint3d::From(-_x_adjust,0),
		DPoint3d::From(0,-_cutparam->GroovingFrontLength),
		DPoint3d::From(_x_adjust - _cutparam->GroovingWidth / 2 ,0),
		DPoint3d::From(0,-_cutparam->GroovingBackLength),
		DPoint3d::From(_cutparam->GroovingWidth / 2,0)
	};
	bvector<DPoint3d> _global_bottom_section_pts = GetAddedPointVector(_anchorPoint, _bottom_section);

	bvector<DPoint3d> _top_section
	{
		DPoint3d::From(-_x_adjust,0),
		DPoint3d::From(0,-_cutparam->GroovingFrontLength + _offset),
		DPoint3d::From(_x_adjust - _cutparam->GroovingWidth / 2 + _offset,0),
		DPoint3d::From(0,-_cutparam->GroovingBackLength),
		DPoint3d::From(_cutparam->GroovingWidth / 2 - _offset,0)
	};
	bvector<DPoint3d> _global_top_section_pts = GetAddedPointVector(DPoint3d::FromSumOf(_anchorPoint, DPoint3d::From(0, 0, _cutparam->GroovingHeight)), _top_section);

	CurveVectorPtr _bottom_cv = CurveVector::CreateLinear(_global_bottom_section_pts, CurveVector::BOUNDARY_TYPE_Outer);
	CurveVectorPtr _top_cv = CurveVector::CreateLinear(_global_top_section_pts, CurveVector::BOUNDARY_TYPE_Outer);

	ISolidPrimitivePtr _cutptr = ISolidPrimitive::CreateDgnRuledSweep(DgnRuledSweepDetail(_bottom_cv, _top_cv, true));
	ISolidKernelEntityPtr  _outcut_right;
	if (SolidUtil::Create::BodyFromSolidPrimitive(_outbaseboardcut, *_cutptr, *ACTIVEMODEL) != SUCCESS)
		return ERROR;
	if (CloneMirrorSolidAndUnion(_outbaseboardcut, _anchorPoint))
		return ERROR;

	return SUCCESS;
}

//此模块的anchor point 为边墩的坐下脚点
// -----------
// |		  |
// |		  |
// |		---
// |		|
// |		|
// |		|
// |		-----
// |			|
// |			|
// |			|
// |			|
// *-------------
StatusInt PDIWT_MS_CZ_CPP::LockHeadDrawing::DrawSidePier(ISolidKernelEntityPtr & _outsidepier, DPoint3dCR _anchorpoint)
{
	SidePier^ _sidepierparam = LH_LockHeadParameter->LH_SidePier;
	bvector<DPoint3d> _bottom_section
	{
		DPoint3d::From(_sidepierparam->PierXY_A,0),
		DPoint3d::From(0,_sidepierparam->PierXY_D),
		DPoint3d::From(-_sidepierparam->PierXY_B,0),
		DPoint3d::From(0,_sidepierparam->PierXY_E),
		DPoint3d::From(_sidepierparam->PierXY_C,0),
		DPoint3d::From(0,_sidepierparam->PierXY_F),
		DPoint3d::From(-(_sidepierparam->PierXY_A - _sidepierparam->PierXY_B + _sidepierparam->PierXY_C),0)
	};
	bvector<DPoint3d> _global_bottom_section_pts = GetAddedPointVector(_anchorpoint, _bottom_section);
	CurveVectorPtr _bottom_section_cv = CurveVector::CreateLinear(_global_bottom_section_pts, CurveVector::BOUNDARY_TYPE_Outer);

	if (_sidepierparam->IsChamfered)
	{
		//下切角
		bvector<DPoint3d> _tx_ty_triangle
		{
			DPoint3d::From(_sidepierparam->PierChamfer_Tx,0),
			DPoint3d::From(-_sidepierparam->PierChamfer_Tx,_sidepierparam->PierChamfer_Ty)
		};
		bvector<DPoint3d> _global_tx_ty_pts = GetAddedPointVector(_global_bottom_section_pts[3], _tx_ty_triangle);
		CurveVectorPtr _tx_ty_cv = CurveVector::CreateLinear(_global_tx_ty_pts, CurveVector::BOUNDARY_TYPE_Outer);
		_bottom_section_cv = CurveVector::AreaUnion(*_bottom_section_cv, *_tx_ty_cv);

		//上圆弧
		bvector<DPoint3d> _r_arc
		{
			DPoint3d::From(0,_sidepierparam->PierChamfer_R),  //Arc EndPoint --1
			DPoint3d::From(-_sidepierparam->PierChamfer_R,0), //Arc CenterPoint--2
			DPoint3d::From(0,-_sidepierparam->PierChamfer_R), //Arc StartPoint--3
		};
		bvector<DPoint3d> _r_arc_pts = GetAddedPointVector(_global_bottom_section_pts[5], _r_arc);
		DEllipse3d _top_arc = DEllipse3d::FromArcCenterStartEnd(_r_arc_pts[2], _r_arc_pts[3], _r_arc_pts[1]);
		auto _top_disk_cv = CurveVector::CreateDisk(_top_arc);
		auto _top_triangle_cv = CurveVector::CreateLinear(bvector<DPoint3d>{_r_arc_pts[0], _r_arc_pts[1], _r_arc_pts[3]}, CurveVector::BOUNDARY_TYPE_Outer);
		auto _top_triangle_disk_diff_cv = CurveVector::AreaDifference(*_top_triangle_cv, *_top_disk_cv);
		_bottom_section_cv = CurveVector::AreaDifference(*_bottom_section_cv, *_top_triangle_disk_diff_cv);
	}
	ISolidPrimitivePtr _sidepier_SPP = ISolidPrimitive::CreateDgnExtrusion(DgnExtrusionDetail(_bottom_section_cv, DVec3d::FromScale(DVec3d::UnitZ(), _sidepierparam->PierHeight), true));
	if (SolidUtil::Create::BodyFromSolidPrimitive(_outsidepier, *_sidepier_SPP, *ACTIVEMODEL) != SUCCESS)
		return ERROR;

	return SUCCESS;
}

//此模块的anchor point为底板下中心点
//				------
//			  /        \
//		------			------
//     /					   \
// ----							----
// |								|
// |								|
// |								|
// ----------------*-----------------
StatusInt PDIWT_MS_CZ_CPP::LockHeadDrawing::DrawDoorSill(ISolidKernelEntityPtr& _outdoorsill, DPoint3dCR _anchorpoint)
{
	DoorSill^ _doorsillparam = LH_LockHeadParameter->LH_DoorSill;
	double _x_adjust = _doorsillparam->DoorSill_A / 2;
	bvector<DPoint3d> _bottom_section
	{
		DPoint3d::From(0,_doorsillparam->DoorSill_D + _doorsillparam->DoorSill_E + _doorsillparam->DoorSill_F),
		DPoint3d::From(-(_x_adjust - _doorsillparam->DoorSill_B - _doorsillparam->DoorSill_C),0),
		DPoint3d::From(-_doorsillparam->DoorSill_C,-_doorsillparam->DoorSill_F),
		DPoint3d::From(-_doorsillparam->DoorSill_B,-_doorsillparam->DoorSill_E),
		DPoint3d::From(0,-_doorsillparam->DoorSill_D)
	};
	bvector<DPoint3d> _global_bottom_section = GetAddedPointVector(_anchorpoint, _bottom_section);
	CurveVectorPtr _doorsill_bottom_left_cv = CurveVector::CreateLinear(_global_bottom_section, CurveVector::BOUNDARY_TYPE_Outer);
	ISolidPrimitivePtr _doorsill_bottom_left_SP = ISolidPrimitive::CreateDgnExtrusion(DgnExtrusionDetail(_doorsill_bottom_left_cv, DVec3d::FromScale(DVec3d::UnitZ(), _doorsillparam->DoorSillHeight),true));
	if (SolidUtil::Create::BodyFromSolidPrimitive(_outdoorsill, *_doorsill_bottom_left_SP, *ACTIVEMODEL))
		return ERROR;
	if (CloneMirrorSolidAndUnion(_outdoorsill, _anchorpoint))
		return ERROR;
	return SUCCESS;
	
}

//此模块的anchor point为集中输水廊道下中心点
// ----------------------
// |					|
// |					|
// |		   ----------
// |          |
// |          |
// |          |
// |          |
// |          ----------------
// |						 |
// |						 |
// --------------------------*
StatusInt PDIWT_MS_CZ_CPP::LockHeadDrawing::DrawLocalConcertationCulvert(ISolidKernelEntityPtr& _outconcertationculvert, DPoint3dCR _anchorpoint)
{
	LocalConcertationCulvert^ _localconculvertparam = LH_LockHeadParameter->LH_LocalConcertationCulvert;
	bvector<DPoint3d> _bottom_section
	{
		DPoint3d::From(-_localconculvertparam->Culvert_A,0),
		DPoint3d::From(0,_localconculvertparam->Culvert_E),
		DPoint3d::From(0,_localconculvertparam->Culvert_C),
		DPoint3d::From(0,-_localconculvertparam->Culvert_F),
		DPoint3d::From(-(_localconculvertparam->Culvert_C-_localconculvertparam->Culvert_B),0),
		DPoint3d::From(0,-(_localconculvertparam->Culvert_E-_localconculvertparam->Culvert_F-_localconculvertparam->Culvert_D)),
		DPoint3d::From(_localconculvertparam->Culvert_A-_localconculvertparam->Culvert_B,0),
		DPoint3d::From(0,-_localconculvertparam->Culvert_D)
	};
	bvector<DPoint3d> _global_bottom_section_pts = GetAddedPointVector(_anchorpoint, _bottom_section);
	CurveVectorPtr _bottom_section_cv = CurveVector::CreateLinear(_global_bottom_section_pts, CurveVector::BOUNDARY_TYPE_Outer);
	if (_localconculvertparam->IsChamfered)
	{

	}

	ISolidPrimitivePtr _localconculvert_bottom_section_SP = ISolidPrimitive::CreateDgnExtrusion(DgnExtrusionDetail(_bottom_section_cv, DVec3d::FromScale(DVec3d::UnitZ(), _localconculvertparam->Culvert_Height), true));
	if (SolidUtil::Create::BodyFromSolidPrimitive(_outconcertationculvert, *_localconculvert_bottom_section_SP, *ACTIVEMODEL))
		return ERROR;
	if (CloneMirrorSolidAndUnion(_outconcertationculvert, _anchorpoint))
		return ERROR;

	return SUCCESS;

}
