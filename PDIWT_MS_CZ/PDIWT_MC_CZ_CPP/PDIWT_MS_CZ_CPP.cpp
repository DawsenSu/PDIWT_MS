// 这是主 DLL 文件。

#include "stdafx.h"

#include "PDIWT_MS_CZ_CPP.h"

PDIWT_MS_CZ_CPP::LockHeadDrawing::LockHeadDrawing(LockHeadParameters ^lockheadparam)
{
	LH_LockHeadParameter = lockheadparam;
}

void PDIWT_MS_CZ_CPP::LockHeadDrawing::DoDraw()
{
	EditElementHandle _baseBoardHandle;
	if (DrawBaseBoard(&_baseBoardHandle) == SUCCESS)
		_baseBoardHandle.AddToModel();
}

Transform PDIWT_MS_CZ_CPP::LockHeadDrawing::GetModelTransform()
{
	double modelscale = ACTIVEMODEL->GetDgnModelP()->GetModelInfo().GetUorPerMeter() / 1000;
	return Transform::FromFixedPointAndScaleFactors(DPoint3d::FromZero(), modelscale, modelscale, modelscale);
}

StatusInt PDIWT_MS_CZ_CPP::LockHeadDrawing::DrawBaseBoard(EditElementHandleP _eeh)
{
	BaseBoard^ _baseboardparam = LH_LockHeadParameter->LH_BaseBoard;

	//Draw Baseboard
	double _xCoord = -_baseboardparam->BaseBoardLength / 2;
	DgnBoxDetail _dgnBoxDeatil(
		DPoint3d::From(_xCoord, 0, 0),
		DPoint3d::From(_xCoord, 0, _baseboardparam->BaseBoardHeight),
		DVec3d::UnitX(),
		DVec3d::UnitY(),
		_baseboardparam->BaseBoardLength,
		_baseboardparam->BaseBoardWidth,
		_baseboardparam->BaseBoardLength,
		_baseboardparam->BaseBoardWidth,
		true);
	ISolidPrimitivePtr _boxptr = ISolidPrimitive::CreateDgnBox(_dgnBoxDeatil);
	ISolidKernelEntityPtr _outbaseboard;
	if (SolidUtil::Create::BodyFromSolidPrimitive(_outbaseboard, *_boxptr, *ACTIVEMODEL) != SUCCESS)
		return ERROR;
	//Draw cut
	if (_baseboardparam->IsGrooving)
	{
		double _y_adjust = _baseboardparam->BaseBoardWidth;
		ShapeTGrooving^ _cutparam = _baseboardparam->TGrooving;
		double _offset = _cutparam->GroovingHeight / _cutparam->GroovingGradient;
		bvector<DPoint3d> _bottom_section
		{
			{ _xCoord,_y_adjust },
			{ _xCoord,_y_adjust - _cutparam->GroovingFrontLength },
			{ _xCoord + (_baseboardparam->BaseBoardLength - _cutparam->GroovingWidth) / 2,_y_adjust - _cutparam->GroovingFrontLength },
			{ _xCoord + (_baseboardparam->BaseBoardLength - _cutparam->GroovingWidth) / 2,_y_adjust - _cutparam->GroovingFrontLength - _cutparam->GroovingBackLength },
			{ 0,_y_adjust - _cutparam->GroovingFrontLength - _cutparam->GroovingBackLength },
			{ 0, _y_adjust }
		};
		bvector<DPoint3d> _top_section
		{
			{ _xCoord,_y_adjust, _cutparam->GroovingHeight},
			{ _xCoord,_y_adjust - _cutparam->GroovingFrontLength + _offset, _cutparam->GroovingHeight },
			{ _xCoord + (_baseboardparam->BaseBoardLength - _cutparam->GroovingWidth) / 2 + _offset,_y_adjust - _cutparam->GroovingFrontLength + _offset, _cutparam->GroovingHeight },
			{ _xCoord + (_baseboardparam->BaseBoardLength - _cutparam->GroovingWidth) / 2 + _offset,_y_adjust - _cutparam->GroovingFrontLength - _cutparam->GroovingBackLength + _offset , _cutparam->GroovingHeight },
			{ 0,_y_adjust - _cutparam->GroovingFrontLength - _cutparam->GroovingBackLength + _offset, _cutparam->GroovingHeight },
			{ 0, _y_adjust, _cutparam->GroovingHeight }
		};
		CurveVectorPtr _bottom_cv = CurveVector::CreateLinear(_bottom_section, CurveVector::BOUNDARY_TYPE_Outer); //简化
		CurveVectorPtr _top_cv = CurveVector::CreateLinear(_top_section, CurveVector::BOUNDARY_TYPE_Outer);
		ISolidPrimitivePtr _cutptr = ISolidPrimitive::CreateDgnRuledSweep(DgnRuledSweepDetail(_bottom_cv, _top_cv, true));
		ISolidKernelEntityPtr _outcut,_outcut1;
		if (SolidUtil::Create::BodyFromSolidPrimitive(_outcut, *_cutptr, *ACTIVEMODEL) != SUCCESS)
			return ERROR;
		if (SolidUtil::CopyEntity(_outcut1, *_outcut) != SUCCESS)
			return ERROR;
		Transform _xplanmirrortrans;
		_xplanmirrortrans.InitFromMirrorPlane(DPoint3d::FromZero(), DVec3d::UnitX());
		if (SolidUtil::Modify::TransformBody(_outcut1, _xplanmirrortrans) != SUCCESS)
			return ERROR;
		bvector<ISolidKernelEntityPtr> _outcut_array{ _outcut,_outcut1 };
		if (SolidUtil::Modify::BooleanSubtract(_outbaseboard,&_outcut_array[0],_outcut_array.size()) != SUCCESS)
			return ERROR;		
	}

	if ((SolidUtil::Modify::TransformBody(_outbaseboard, GetModelTransform()) == SUCCESS)
		&&
		(SolidUtil::Convert::BodyToElement(*_eeh, *_outbaseboard, nullptr, *ACTIVEMODEL) == SUCCESS))
	{
		return SUCCESS;
	}
	return ERROR;
}
