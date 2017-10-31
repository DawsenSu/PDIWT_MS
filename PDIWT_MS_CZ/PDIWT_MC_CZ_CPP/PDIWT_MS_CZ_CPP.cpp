// 这是主 DLL 文件。

#include "stdafx.h"

#include "PDIWT_MS_CZ_CPP.h"

PDIWT_MS_CZ_CPP::LockHeadDrawing::LockHeadDrawing(LockHeadParameters^ lockheadparam)
{
	LH_LockHeadParameter = lockheadparam;
}

StatusInt PDIWT_MS_CZ_CPP::LockHeadDrawing::DoDraw(const double x, const double y,const double z)
{
	EditElementHandle _cz_whole;
	ISolidKernelEntityPtr _cz_whole_SKE;
	DPoint3d _whole_anchor{ x,y,z };
#pragma region baseboard
	ISolidKernelEntityPtr _baseboard_SKE;
	if (DrawBaseBoard(_baseboard_SKE, _whole_anchor))
		return ERROR;
	_cz_whole_SKE = _baseboard_SKE;
#pragma endregion

#pragma region SidePier
	DPoint3d _sidepier_anchor = DPoint3d::FromSumOf(_whole_anchor, DPoint3d::From(-LH_LockHeadParameter->LH_BaseBoard->BaseBoardWidth / 2, 0, LH_LockHeadParameter->LH_BaseBoard->BaseBoardHeight));
	ISolidKernelEntityPtr _left_sidepier_SKE;
	if (DrawSidePier(_left_sidepier_SKE, _sidepier_anchor))
		return ERROR;

	//Mirror right 
	ISolidKernelEntityPtr _right_sidepier_SKE;
	if (SolidUtil::CopyEntity(_right_sidepier_SKE, *_left_sidepier_SKE))
		return ERROR;
	Transform _xymirrortrans;
	_xymirrortrans.InitFromMirrorPlane(_whole_anchor, DVec3d::UnitX());
	if (SolidUtil::Modify::TransformBody(_right_sidepier_SKE, _xymirrortrans))
		return ERROR;
	bvector<ISolidKernelEntityPtr> _sidepier_SKE{ _left_sidepier_SKE ,_right_sidepier_SKE };
	if (SolidUtil::Modify::BooleanUnion(_cz_whole_SKE, &_sidepier_SKE[0], _sidepier_SKE.size()))
		return ERROR;
#pragma endregion

#pragma region DoorSill
	DPoint3d _doorsill_anchor = DPoint3d::FromSumOf(_whole_anchor, DPoint3d::From(0, 0, LH_LockHeadParameter->LH_BaseBoard->BaseBoardHeight));
	ISolidKernelEntityPtr _doorsill_SKE;
	if (DrawDoorSill(_doorsill_SKE, _doorsill_anchor))
		return ERROR;
	if (SolidUtil::Modify::BooleanUnion(_cz_whole_SKE, &_doorsill_SKE, 1))
		return ERROR;
#pragma endregion

	if (LH_LockHeadParameter->LH_CulvertChoosenIndex == 1)
	{
#pragma region LocalConcerationCulvert
		DPoint3d _localconculvert_anchor = DPoint3d::FromSumOf(_whole_anchor, DPoint3d::From(0, LH_LockHeadParameter->LH_LocalConcertationCulvert->Culvert_Pier_BackDis, LH_LockHeadParameter->LH_BaseBoard->BaseBoardHeight));
		ISolidKernelEntityPtr _localconculvert_SKE;
		if (DrawLocalConcertationCulvert(_localconculvert_SKE, _localconculvert_anchor))
			return ERROR;
		if (SolidUtil::Modify::BooleanSubtract(_cz_whole_SKE, &_localconculvert_SKE, 1))
			return ERROR;
#pragma endregion
	}
	else
	{
#pragma region ShortCulvert
		DPoint3d _shortculvert_anchor = DPoint3d::FromSumOf(_whole_anchor, DPoint3d::From(-(LH_LockHeadParameter->LH_BaseBoard->BaseBoardWidth / 2 - LH_LockHeadParameter->LH_ShortCulvert->Culvert_Pier_RightDis), 0, LH_LockHeadParameter->LH_ShortCulvert->Culvert_Baseboard_BottomDis));
		ISolidKernelEntityPtr _left_shortculvert_SKE;
		if (DrawShortCulvert(_left_shortculvert_SKE, _shortculvert_anchor))
			return ERROR;

		//Mirror right 
		ISolidKernelEntityPtr _right_shortculvert_SKE;
		if (SolidUtil::CopyEntity(_right_shortculvert_SKE, *_left_shortculvert_SKE))
			return ERROR;
		if (SolidUtil::Modify::TransformBody(_right_shortculvert_SKE, _xymirrortrans))
			return ERROR;

		bvector<ISolidKernelEntityPtr> _shortculvert_SKE{ _left_shortculvert_SKE ,_right_shortculvert_SKE };
		if (SolidUtil::Modify::BooleanSubtract(_cz_whole_SKE, &_shortculvert_SKE[0], _shortculvert_SKE.size()))
			return ERROR;
#pragma endregion
	}
	if (LH_LockHeadParameter->LH_EmptyRectBoxs->Count > 0)
	{
#pragma region RectEmptyBox
		DPoint3d _rectemptybox_anchor = DPoint3d::FromSumOf(_sidepier_anchor, DPoint3d::From(0, 0, LH_LockHeadParameter->LH_SidePier->PierHeight));
		bvector<ISolidKernelEntityPtr> _rectemptyboxes;
		if (DrawRectEmptyBoxes(_rectemptyboxes, _rectemptybox_anchor))
			return ERROR;
		size_t _rectemptybox_count = _rectemptyboxes.size();
		ISolidKernelEntityPtr _temp;
		for (size_t i = 0; i < _rectemptybox_count; i++)
		{
			if (SolidUtil::CopyEntity(_temp, *_rectemptyboxes[i]))
				return ERROR;
			_temp->PreMultiplyEntityTransformInPlace(_xymirrortrans);
			_rectemptyboxes.push_back(_temp);
		}
		if (SolidUtil::Modify::BooleanSubtract(_cz_whole_SKE, &_rectemptyboxes[0], _rectemptyboxes.size()))
			return ERROR;

#pragma endregion
	}
	if (LH_LockHeadParameter->LH_EmptyZPlanBoxs->Count > 0)
	{
#pragma region ZPlanEmptyBox
		DPoint3d _zplanmptybox_anchor = DPoint3d::FromSumOf(_sidepier_anchor, DPoint3d::From(0, 0, LH_LockHeadParameter->LH_SidePier->PierHeight));
		bvector<ISolidKernelEntityPtr> _zplanemptyboxes;
		if (DrawZPlanEmptyBox(_zplanemptyboxes, _zplanmptybox_anchor))
			return ERROR;
		size_t _zplanemptybox_count = _zplanemptyboxes.size();
		ISolidKernelEntityPtr _temp;
		for (size_t i = 0; i < _zplanemptybox_count; i++)
		{
			if (SolidUtil::CopyEntity(_temp, *_zplanemptyboxes[i]))
				return ERROR;
			_temp->PreMultiplyEntityTransformInPlace(_xymirrortrans);
			_zplanemptyboxes.push_back(_temp);
		}
		if (SolidUtil::Modify::BooleanSubtract(_cz_whole_SKE, &_zplanemptyboxes[0], _zplanemptyboxes.size()))
			return ERROR;
#pragma endregion
	}



	if ((SolidUtil::Modify::TransformBody(_cz_whole_SKE, GetModelTransform(DPoint3d::FromZero())) != SUCCESS)
		||
		(SolidUtil::Convert::BodyToElement(_cz_whole, *_cz_whole_SKE, nullptr, *ACTIVEMODEL) != SUCCESS))
		return ERROR;

	_cz_whole.AddToModel();
	return SUCCESS;
}

StatusInt PDIWT_MS_CZ_CPP::LockHeadDrawing::DoTest()
{
	DPoint3d _rectemptybox_anchor = DPoint3d::FromSumOf(DPoint3d{ 0,0,0 }, DPoint3d::From(0, 0, LH_LockHeadParameter->LH_SidePier->PierHeight));
	bvector<ISolidKernelEntityPtr> _rectemptyboxes;
	if (DrawRectEmptyBoxes(_rectemptyboxes, _rectemptybox_anchor))
		return ERROR;
	DebugISolidKernelEntity(*_rectemptyboxes.at(0));
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
		//if (relativeLengthVector[index].AlmostEqual(DPoint3d::FromZero())) continue;
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

StatusInt PDIWT_MS_CZ_CPP::LockHeadDrawing::DrawRoundChamferCorner(CurveVectorPtr & _outcvptr, DPoint3dCR _anchorpoint, double _radius, Quadrant _phase)
{
	double _xsign = 1, _ysign = 1;
	switch (_phase)
	{
	case PDIWT_MS_CZ_CPP::Quadrant::One:
		_xsign = 1; _ysign = 1;
		break;
	case PDIWT_MS_CZ_CPP::Quadrant::Two:
		_xsign = -1; _ysign = 1;
		break;
	case PDIWT_MS_CZ_CPP::Quadrant::Three:
		_xsign = -1; _ysign = -1;
		break;
	case PDIWT_MS_CZ_CPP::Quadrant::Four:
		_xsign = 1; _ysign = -1;
		break;
	default:
		break;
	}
	bvector<DPoint3d> _pts
	{
		DPoint3d::From(_xsign*_radius,0),
		DPoint3d::From(-_xsign*_radius,_ysign*_radius)
	};
	bvector<DPoint3d> _global_pts = GetAddedPointVector(_anchorpoint, _pts);
	CurveVectorPtr _triangle_cv = CurveVector::CreateLinear(_global_pts, CurveVector::BOUNDARY_TYPE_Outer);
	auto _arc = DEllipse3d::FromArcCenterStartEnd(DPoint3d::FromSumOf(_anchorpoint, DPoint3d::From(_xsign*_radius, _ysign*_radius)), _global_pts[1], _global_pts[2]);
	CurveVectorPtr _arc_cv = CurveVector::CreateDisk(_arc);
	_outcvptr = CurveVector::AreaDifference(*_triangle_cv, *_arc_cv);
	return SUCCESS;
}



StatusInt PDIWT_MS_CZ_CPP::LockHeadDrawing::DebugCurveVector(CurveVectorCR _cv)
{
	EditElementHandle _eeh;
	if (DraftingElementSchema::ToElement(_eeh, _cv, nullptr, true, *ACTIVEMODEL))
		return ERROR;
	_eeh.AddToModel();
	return SUCCESS;
}

StatusInt PDIWT_MS_CZ_CPP::LockHeadDrawing::DebugISolidKernelEntity(ISolidKernelEntityCR _SKE)
{
	EditElementHandle _eeh;
	DraftingElementSchema::ToElement(_eeh, _SKE, nullptr, *ACTIVEMODEL);
	_eeh.AddToModel();
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

	_bottom_cv->SimplifyLinestrings(0.01, true, true);
	_top_cv->SimplifyLinestrings(0.01, true, true);

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
	_bottom_section_cv->SimplifyLinestrings(0.01, true, true);
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
	_doorsill_bottom_left_cv->SimplifyLinestrings(0.01, true, true);
	ISolidPrimitivePtr _doorsill_bottom_left_SP = ISolidPrimitive::CreateDgnExtrusion(DgnExtrusionDetail(_doorsill_bottom_left_cv, DVec3d::FromScale(DVec3d::UnitZ(), _doorsillparam->DoorSillHeight), true));
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
		DPoint3d::From(_localconculvertparam->Culvert_C,0),
		DPoint3d::From(0,-_localconculvertparam->Culvert_F),
		DPoint3d::From(-(_localconculvertparam->Culvert_C - _localconculvertparam->Culvert_B),0),
		DPoint3d::From(0,-(_localconculvertparam->Culvert_E - _localconculvertparam->Culvert_F - _localconculvertparam->Culvert_D)),
		DPoint3d::From(_localconculvertparam->Culvert_A - _localconculvertparam->Culvert_B,0),
		DPoint3d::From(0,-_localconculvertparam->Culvert_D)
	};
	bvector<DPoint3d> _global_bottom_section_pts = GetAddedPointVector(_anchorpoint, _bottom_section);
	CurveVectorPtr _bottom_section_cv = CurveVector::CreateLinear(_global_bottom_section_pts, CurveVector::BOUNDARY_TYPE_Outer);

	if (_localconculvertparam->IsChamfered)
	{
		CurveVectorPtr _r1_cv, _r2_cv, _r3_cv, _r4_cv;
		DrawRoundChamferCorner(_r1_cv, _global_bottom_section_pts[1], _localconculvertparam->Culvert_Chamfer_R1, Quadrant::One);
		DrawRoundChamferCorner(_r2_cv, _global_bottom_section_pts[6], _localconculvertparam->Culvert_Chamfer_R2, Quadrant::One);
		DrawRoundChamferCorner(_r3_cv, _global_bottom_section_pts[2], _localconculvertparam->Culvert_Chamfer_R3, Quadrant::Four);
		DrawRoundChamferCorner(_r4_cv, _global_bottom_section_pts[5], _localconculvertparam->Culvert_Chamfer_R4, Quadrant::Four);

		_bottom_section_cv = CurveVector::AreaUnion(*_bottom_section_cv, *_r2_cv);
		_bottom_section_cv = CurveVector::AreaUnion(*_bottom_section_cv, *_r4_cv);
		_bottom_section_cv = CurveVector::AreaDifference(*_bottom_section_cv, *_r1_cv);
		_bottom_section_cv = CurveVector::AreaDifference(*_bottom_section_cv, *_r3_cv);
	}

	_bottom_section_cv->SimplifyLinestrings(0.01, true, true);
	ISolidPrimitivePtr _localconculvert_bottom_section_SP = ISolidPrimitive::CreateDgnExtrusion(DgnExtrusionDetail(_bottom_section_cv, DVec3d::FromScale(DVec3d::UnitZ(), _localconculvertparam->Culvert_Height), true));
	if (SolidUtil::Create::BodyFromSolidPrimitive(_outconcertationculvert, *_localconculvert_bottom_section_SP, *ACTIVEMODEL))
		return ERROR;
	if (_localconculvertparam->IsIncludeWaterDivision)
	{
		ISolidKernelEntityPtr _waterdiv_SKE;
		if (DrawWaterDivision(_waterdiv_SKE, DPoint3d::FromSumOf(_anchorpoint, DPoint3d::From(-LH_LockHeadParameter->LH_DoorSill->DoorSill_A / 2, _localconculvertparam->Culvert_D / 2))))
			return ERROR;
		if (SolidUtil::Modify::BooleanSubtract(_outconcertationculvert, &_waterdiv_SKE, 1))
			return ERROR;
	}

	//绘制消力坎
	if (_localconculvertparam->IsIncludeBaffle)
	{
		bvector<ISolidKernelEntityPtr> _baffle_vector_SKE;
		for each (auto _baffle in _localconculvertparam->Culvert_Baffle)
		{
			DPoint3d _baseorigin = DPoint3d::FromSumOf(_anchorpoint, DPoint3d::From(-_baffle->Baffle_MidMidDis - _baffle->Baffle_Width / 2, 0));
			DPoint3d _toporigin = DPoint3d::FromSumOf(_baseorigin, DPoint3d::From(0, 0, _baffle->Baffle_Height));
			DgnBoxDetail _baffle_detail
			(
				_baseorigin,
				_toporigin,
				DVec3d::UnitX(),
				DVec3d::UnitY(),
				_baffle->Baffle_Width,
				_localconculvertparam->Culvert_D,
				_baffle->Baffle_Width,
				_localconculvertparam->Culvert_D,
				true
			);
			ISolidPrimitivePtr _baffle_SP = ISolidPrimitive::CreateDgnBox(_baffle_detail);
			ISolidKernelEntityPtr _baffle_SKE;
			if (SolidUtil::Create::BodyFromSolidPrimitive(_baffle_SKE, *_baffle_SP, *ACTIVEMODEL))
				return ERROR;
			_baffle_vector_SKE.push_back(_baffle_SKE);
		}
		if (SolidUtil::Modify::BooleanSubtract(_outconcertationculvert, &_baffle_vector_SKE[0], _baffle_vector_SKE.size()))
			return ERROR;
	}
	//绘制格栅
	if (_localconculvertparam->IsIncludeEnergyDisspater)
	{
		ObservableCollection<GrillInterval^>^ _grillwidthlistparam = _localconculvertparam->Culvert_EnergyDisspater->GrilleWidthList;
		int _grill_width_list_count = _grillwidthlistparam->Count;
		double _grill_height = LH_LockHeadParameter->LH_DoorSill->DoorSillHeight - _localconculvertparam->Culvert_Height;
		double _grill_length = (_localconculvertparam->Culvert_D - _localconculvertparam->Culvert_EnergyDisspater->Grille_TwolineInterval) / 2;
		DPoint3d _grill_temp_Point = Dpoint3d::FromSumOf(_anchorpoint, Dpoint3d::From(0, 0, _localconculvertparam->Culvert_Height));
		ISolidKernelEntityPtr _grill_SKE[2];
		for (int i = 0; i < _grill_width_list_count; i += 2)
		{
			double _radius_right = _grillwidthlistparam[i]->RoundChamferRadius;
			double _radius_left;
			if (i == _grill_width_list_count - 2)
				_radius_left = _radius_right;
			else
				_radius_left = _grillwidthlistparam[i + 2]->RoundChamferRadius;

			DPoint3d _offsetvector = DPoint3d::From(-(_grillwidthlistparam[i]->Interval + _grillwidthlistparam[i + 1]->Interval), 0);
			_grill_temp_Point = DPoint3d::FromSumOf(_grill_temp_Point, _offsetvector);
			if (DrawGrillInterval(_grill_SKE[0], _grillwidthlistparam[i + 1]->Interval, _grill_height, _grill_length, _radius_right, _radius_left, _grill_temp_Point))
				return ERROR;
			if (SolidUtil::CopyEntity(_grill_SKE[1], *_grill_SKE[0]))
				return ERROR;
			_grill_SKE[1]->PreMultiplyEntityTransformInPlace(Transform::From(DPoint3d::From(0, _grill_length + _localconculvertparam->Culvert_EnergyDisspater->Grille_TwolineInterval)));

			if (SolidUtil::Modify::BooleanUnion(_outconcertationculvert, &_grill_SKE[0], 2))
				return ERROR;
		}
	}


	if (CloneMirrorSolidAndUnion(_outconcertationculvert, _anchorpoint))
		return ERROR;

	return SUCCESS;

}

//格栅的anchorpoint为左下角点
StatusInt PDIWT_MS_CZ_CPP::LockHeadDrawing::DrawGrillInterval(ISolidKernelEntityPtr & _outgrill, double _width, double _height, double _length, double _radius_right, double _radius_left, DPoint3dCR _anchorpoint)
{
	bvector<DPoint3d> _grill_section
	{
		DPoint3d::From(_width,0),
		DPoint3d::From(0,_height),
		DPoint3d::From(-_width,0)
	};
	bvector<DPoint3d> _global_grill_pts = GetAddedPointVector(_anchorpoint, _grill_section);
	CurveVectorPtr _grill_section_cv = CurveVector::CreateLinear(_global_grill_pts, CurveVector::BOUNDARY_TYPE_Outer);
	CurveVectorPtr _r1_cv, _r2_cv, _r3_cv, _r4_cv;
	DrawRoundChamferCorner(_r1_cv, _global_grill_pts[0], _radius_left, Quadrant::Two);
	DrawRoundChamferCorner(_r2_cv, _global_grill_pts[1], _radius_right, Quadrant::One);
	DrawRoundChamferCorner(_r3_cv, _global_grill_pts[2], _radius_right, Quadrant::Four);
	DrawRoundChamferCorner(_r4_cv, _global_grill_pts[3], _radius_left, Quadrant::Three);
	_grill_section_cv = CurveVector::AreaUnion(*_grill_section_cv, *_r1_cv);
	_grill_section_cv = CurveVector::AreaUnion(*_grill_section_cv, *_r2_cv);
	_grill_section_cv = CurveVector::AreaUnion(*_grill_section_cv, *_r3_cv);
	_grill_section_cv = CurveVector::AreaUnion(*_grill_section_cv, *_r4_cv);
	//DebugCurveVector(*_r1_cv);
	//DebugCurveVector(*_r2_cv);
	//DebugCurveVector(*_r3_cv);
	//DebugCurveVector(*_r4_cv);

	_grill_section_cv->SimplifyLinestrings(0.01, true, true);
	_grill_section_cv->TransformInPlace(Transform::FromAxisAndRotationAngle(DRay3d::FromOriginAndVector(_global_grill_pts[0], DVec3d::UnitX()), fc_piover2));
	//DebugCurveVector(*_grill_section_cv);

	DgnExtrusionDetail _grill_detai(_grill_section_cv, DVec3d::FromScale(DVec3d::UnitY(), _length), true);
	ISolidPrimitivePtr _grill_SP = ISolidPrimitive::CreateDgnExtrusion(_grill_detai);
	if (SolidUtil::Create::BodyFromSolidPrimitive(_outgrill, *_grill_SP, *ACTIVEMODEL))
		return ERROR;
	return SUCCESS;
}

//此模块的anchor point为分流墩右下半圆的圆心
//	  --
//   /  \
//  |	 |
//  |	 |
//  \	 \
//   \	  \
//    \	   \
//	   \	---------\
//		\			* |
//		 \-----------/
StatusInt PDIWT_MS_CZ_CPP::LockHeadDrawing::DrawWaterDivision(ISolidKernelEntityPtr &_outwaterdiv, DPoint3dCR _anchorpoint)
{
	WaterDivision^ _waterdivparam = LH_LockHeadParameter->LH_LocalConcertationCulvert->Culvert_WaterDivision;
	bvector<DPoint3d> _waterdiv_pts
	{
		DPoint3d::From(0,-_waterdivparam->WaterDivision_R3),
		DPoint3d::From(0,2 * _waterdivparam->WaterDivision_R3),
		DPoint3d::From(-_waterdivparam->WaterDivision_A,0),		//[3]
		DPoint3d::From(-_waterdivparam->WaterDivision_R1,_waterdivparam->WaterDivision_R1),
		DPoint3d::From(0,_waterdivparam->WaterDivision_B),//[5]
		DPoint3d::From(-(_waterdivparam->WaterDivision_R2 - _waterdivparam->WaterDivision_R1),0),
		DPoint3d::From(0,-(_waterdivparam->WaterDivision_B + _waterdivparam->WaterDivision_R1 + 2 * _waterdivparam->WaterDivision_R3 - _waterdivparam->WaterDivision_R2)),
		DPoint3d::From(_waterdivparam->WaterDivision_R2,-_waterdivparam->WaterDivision_R2)
	};
	bvector<DPoint3d> _global_waterdiv_pts = GetAddedPointVector(_anchorpoint, _waterdiv_pts);
	CurveVectorPtr _waterdiv_cv = CurveVector::Create(CurveVector::BOUNDARY_TYPE_Outer);
	DEllipse3d _arc_r3;
	_arc_r3.InitArcFromPointTangentPoint(_global_waterdiv_pts[1], DVec3d::UnitX(), _global_waterdiv_pts[2]);
	_waterdiv_cv->Add(ICurvePrimitive::CreateArc(_arc_r3));
	_waterdiv_cv->Add(ICurvePrimitive::CreateLine(DSegment3d::From(_global_waterdiv_pts[2], _global_waterdiv_pts[3])));

	DEllipse3d _arc_r1;
	_arc_r1.InitArcFromPointTangentPoint(_global_waterdiv_pts[3], DVec3d::From(-1, 0), _global_waterdiv_pts[4]);
	_waterdiv_cv->Add(ICurvePrimitive::CreateArc(_arc_r1));
	_waterdiv_cv->Add(ICurvePrimitive::CreateLine(DSegment3d::From(_global_waterdiv_pts[4], _global_waterdiv_pts[5])));
	double _r4 = _waterdivparam->WaterDivision_R2 - _waterdivparam->WaterDivision_R1;
	DEllipse3d _arc_r4;
	_arc_r4.InitArcFromPointTangentPoint(_global_waterdiv_pts[5], DVec3d::UnitY(), _global_waterdiv_pts[6]);
	_waterdiv_cv->Add(ICurvePrimitive::CreateArc(_arc_r4));
	_waterdiv_cv->Add(ICurvePrimitive::CreateLine(DSegment3d::From(_global_waterdiv_pts[6], _global_waterdiv_pts[7])));
	DEllipse3d _arc_r2;
	_arc_r2.InitArcFromPointTangentPoint(_global_waterdiv_pts[7], DVec3d::From(0, -1), _global_waterdiv_pts[8]);
	_waterdiv_cv->Add(ICurvePrimitive::CreateArc(_arc_r2));
	_waterdiv_cv->Add(ICurvePrimitive::CreateLine(DSegment3d::From(_global_waterdiv_pts[8], _global_waterdiv_pts[1])));
	_waterdiv_cv->SimplifyLinestrings(0.01, true, true);
	ISolidPrimitivePtr _waterdiv_SP = ISolidPrimitive::CreateDgnExtrusion(DgnExtrusionDetail(_waterdiv_cv, DVec3d::FromScale(DVec3d::UnitZ(), LH_LockHeadParameter->LH_LocalConcertationCulvert->Culvert_Height), true));
	if (SolidUtil::Create::BodyFromSolidPrimitive(_outwaterdiv, *_waterdiv_SP, *ACTIVEMODEL))
		return ERROR;
	return SUCCESS;
}

//此模块的anchorpoint为廊道右下角点
// ------------
// |			\
// 	-------\	 \
//          \	  \
//	         \	   \
//			  \	    -----------
//			   \			  |
//      		\-------------*	  

StatusInt PDIWT_MS_CZ_CPP::LockHeadDrawing::DrawShortCulvert(ISolidKernelEntityPtr& _outshortculvert, DPoint3dCR _anchorpoint)
{
	ShortCulvert^ _shortculvertparam = LH_LockHeadParameter->LH_ShortCulvert;
	double aplha, l;
	PDIWT_MS_CZ::Models::SolveEqution::GetAlphaAndL
	(
		_shortculvertparam->Culvert_A,
		_shortculvertparam->Culvert_B,
		_shortculvertparam->Culvert_C,
		_shortculvertparam->Culvert_D,
		_shortculvertparam->Culvert_R1,
		_shortculvertparam->Culvert_R2,
		aplha, l
	);

	bvector<DPoint3d> _shortculvert_section
	{
		DPoint3d::From(0,_shortculvertparam->Culvert_C,0),
		DPoint3d::From(0,_shortculvertparam->Culvert_B,_shortculvertparam->Culvert_D),
		DPoint3d::From(0,_shortculvertparam->Culvert_A,0)
	};
	bvector<DPoint3d> _global_section_pts = GetAddedPointVector(_anchorpoint, _shortculvert_section);
	CurveVectorPtr _shortcuvlert_path_cv = CurveVector::Create(CurveVector::BOUNDARY_TYPE_Open);
	_shortcuvlert_path_cv->Add(ICurvePrimitive::CreateLine(DSegment3d::From(_global_section_pts[0], _global_section_pts[1]))); //bug
	DEllipse3d _arc_r2;
	_arc_r2.InitFromPoints
	(
		DPoint3d::FromSumOf(_global_section_pts[1], DPoint3d::From(0, 0, _shortculvertparam->Culvert_R2)),
		_global_section_pts[1],
		DPoint3d::FromSumOf(_global_section_pts[1], DPoint3d::From(0, _shortculvertparam->Culvert_R2, _shortculvertparam->Culvert_R2)),
		0,
		aplha
	);
	_shortcuvlert_path_cv->Add(ICurvePrimitive::CreateArc(_arc_r2));
	DPoint3d _arc_r2_bg, _arc_r2_end;
	_arc_r2.EvaluateEndPoints(_arc_r2_bg, _arc_r2_end);
	DEllipse3d _arc_r1;
	_arc_r1.InitFromPoints
	(
		DPoint3d::FromSumOf(_global_section_pts[2], DPoint3d::From(0, 0, -_shortculvertparam->Culvert_R1)),
		_global_section_pts[2],
		DPoint3d::FromSumOf(_global_section_pts[2], DPoint3d::From(0, _shortculvertparam->Culvert_R1, -_shortculvertparam->Culvert_R1)),
		fc_2pi - aplha,
		aplha
	);
	DPoint3d _arc_r1_bg, _arc_r1_end;
	_arc_r1.EvaluateEndPoints(_arc_r1_bg, _arc_r1_end);
	_shortcuvlert_path_cv->Add(ICurvePrimitive::CreateLine(DSegment3d::From(_arc_r2_end, _arc_r1_bg)));
	_shortcuvlert_path_cv->Add(ICurvePrimitive::CreateArc(_arc_r1));
	_shortcuvlert_path_cv->Add(ICurvePrimitive::CreateLine(DSegment3d::From(_global_section_pts[2], _global_section_pts[3])));

	bvector<DPoint3d> _shortculvert_profile
	{
		DPoint3d::From(_shortculvertparam->Culvert_Width,0,0),
		DPoint3d::From(0,0,_shortculvertparam->Culvert_R2 - _shortculvertparam->Culvert_R1),
		DPoint3d::From(-_shortculvertparam->Culvert_Width,0,0)
	};
	bvector<DPoint3d> _global_shortculvert_pts = GetAddedPointVector(_anchorpoint, _shortculvert_profile);
	CurveVectorPtr _shortculvert_profile_cv = CurveVector::CreateLinear(_global_shortculvert_pts, CurveVector::BOUNDARY_TYPE_Outer);

	if (SolidUtil::Create::BodyFromSweep(_outshortculvert, *_shortculvert_profile_cv, *_shortcuvlert_path_cv, *ACTIVEMODEL, false, false, false))
		return ERROR;

	return SUCCESS;
}

//此模块的anchorpoint为矩形空箱的左上角点
//	   -----------
//	  /			 /|
//   *----------- |
//   |			| |
//   |			| |
//   |			|/
//   ------------

StatusInt PDIWT_MS_CZ_CPP::LockHeadDrawing::DrawRectEmptyBoxes(bvector<ISolidKernelEntityPtr> & _outrectemptyboxes, DPoint3dCR _anchorpoint)
{
	ObservableCollection<RectEmptyBox^>^ _rectemptyboxesparam = LH_LockHeadParameter->LH_EmptyRectBoxs;
	for each (RectEmptyBox^ _rectbox in _rectemptyboxesparam)
	{
		DPoint3d _topOrigin = DPoint3d::FromSumOf(_anchorpoint, DPoint3d::From(_rectbox->XDis, _rectbox->YDis, -_rectbox->ZDis));
		DPoint3d _baseOrigin = DPoint3d::FromSumOf(_topOrigin, DVec3d::UnitZ(), -_rectbox->EmptyBoxHeight);
		DgnBoxDetail _dgnbox
		(
			_baseOrigin,
			_topOrigin,
			DVec3d::UnitX(),
			DVec3d::UnitY(),
			_rectbox->EmptyBoxWidth,
			_rectbox->EmptyBoxLength,
			_rectbox->EmptyBoxWidth,
			_rectbox->EmptyBoxLength,
			true
		);
		ISolidPrimitivePtr _rectbox_ptr = ISolidPrimitive::CreateDgnBox(_dgnbox);
		ISolidKernelEntityPtr _rectbox_SKE;
		if (SolidUtil::Create::BodyFromSolidPrimitive(_rectbox_SKE, *_rectbox_ptr, *ACTIVEMODEL))
			return	ERROR;
		for each (auto _chamfer_info in _rectbox->ChamferInfos)
		{
			if (_chamfer_info->IsChamfered)
			{
				ISolidKernelEntityPtr _EdgeChamfer_SKE;
				if (DrawChamfer(_EdgeChamfer_SKE, _chamfer_info->EdgeIndicator, _topOrigin, _chamfer_info->ChamferLength, _chamfer_info->ChamferWidth, _rectbox->EmptyBoxWidth, _rectbox->EmptyBoxLength, _rectbox->EmptyBoxHeight))
					return ERROR;

				if (SolidUtil::Modify::BooleanSubtract(_rectbox_SKE, &_EdgeChamfer_SKE, 1))
					return ERROR;
			}
		}

		bvector<ISolidKernelEntityPtr> _EdgeChamfer_Coner_SKE;
		if (DrawChamferCorner(_EdgeChamfer_Coner_SKE, _rectbox, _topOrigin))
			return ERROR;
		if (_EdgeChamfer_Coner_SKE.size() > 0)
			if (SolidUtil::Modify::BooleanSubtract(_rectbox_SKE, &_EdgeChamfer_Coner_SKE[0], _EdgeChamfer_Coner_SKE.size()))
				return ERROR;

		_outrectemptyboxes.push_back(_rectbox_SKE);
	}
	return SUCCESS;
}

BENTLEY_NAMESPACE_NAME::StatusInt PDIWT_MS_CZ_CPP::LockHeadDrawing::DrawChamfer(ISolidKernelEntityPtr& _outchamfer, int _edgeindex, DPoint3dCR _anchorpoint, double _front, double _back, double _xlen, double _ylen, double _zlen)
{
	bvector<DPoint3d> _pts;
	DPoint3d _drawpoint;
	DVec3d _extrusion_vec;

	switch (_edgeindex)
	{
	case 0:
		_pts = { DPoint3d::From(0,_front),DPoint3d::From(0,-_front,_back) };
		_extrusion_vec = DVec3d::FromScale(DVec3d::UnitX(), _xlen);
		_drawpoint = DPoint3d::FromSumOf(_anchorpoint, DPoint3d::From(0, 0, -_zlen));
		break;
	case 1:
		_pts = { DPoint3d::From(0,0,_front),DPoint3d::From(0,-_back ,-_front) };
		_extrusion_vec = DVec3d::FromScale(DVec3d::UnitX(), _xlen);
		_drawpoint = DPoint3d::FromSumOf(_anchorpoint, DPoint3d::From(0, _ylen, -_zlen));
		break;
	case 2:
		_pts = { DPoint3d::From(0,-_front),DPoint3d::From(0,_front,-_back) };
		_extrusion_vec = DVec3d::FromScale(DVec3d::UnitX(), _xlen);
		_drawpoint = DPoint3d::FromSumOf(_anchorpoint, DPoint3d::From(0, _ylen, 0));
		break;
	case 3:
		_pts = { DPoint3d::From(0,0,-_front),DPoint3d::From(0,_back ,_front) };
		_extrusion_vec = DVec3d::FromScale(DVec3d::UnitX(), _xlen);
		_drawpoint = _anchorpoint;
		break;
	case 4:
		_pts = { DPoint3d::From(0,0,_front),DPoint3d::From(_back,0, -_front) };
		_extrusion_vec = DVec3d::FromScale(DVec3d::UnitY(), _ylen);
		_drawpoint = DPoint3d::FromSumOf(_anchorpoint, DPoint3d::From(0, 0, -_zlen));
		break;
	case 5:
		_pts = { DPoint3d::From(_front,0),DPoint3d::From(-_front,0,-_back) };
		_extrusion_vec = DVec3d::FromScale(DVec3d::UnitY(), _ylen);
		_drawpoint = _anchorpoint;
		break;
	case 6:
		_pts = { DPoint3d::From(0,0,-_front),DPoint3d::From(-_back,0, _front) };
		_extrusion_vec = DVec3d::FromScale(DVec3d::UnitY(), _ylen);
		_drawpoint = DPoint3d::FromSumOf(_anchorpoint, DPoint3d::From(_xlen, 0, 0));
		break;
	case 7:
		_pts = { DPoint3d::From(-_front,0),DPoint3d::From(_front,0,_back) };
		_extrusion_vec = DVec3d::FromScale(DVec3d::UnitY(), _ylen);
		_drawpoint = DPoint3d::FromSumOf(_anchorpoint, DPoint3d::From(_xlen, 0, -_zlen));
		break;
	case 8:
		_pts = { DPoint3d::From(_front,0),DPoint3d::From(-_front,_back) };
		_extrusion_vec = DVec3d::FromScale(DVec3d::UnitZ(), _zlen);
		_drawpoint = DPoint3d::FromSumOf(_anchorpoint, DPoint3d::From(0, 0, -_zlen));
		break;
	case 9:
		_pts = { DPoint3d::From(0,_front),DPoint3d::From(-_back,-_front) };
		_extrusion_vec = DVec3d::FromScale(DVec3d::UnitZ(), _zlen);
		_drawpoint = DPoint3d::FromSumOf(_anchorpoint, DPoint3d::From(_xlen, 0, -_zlen));
		break;
	case 10:
		_pts = { DPoint3d::From(-_front,0),DPoint3d::From(_front,-_back) };
		_extrusion_vec = DVec3d::FromScale(DVec3d::UnitZ(), _zlen);
		_drawpoint = DPoint3d::FromSumOf(_anchorpoint, DPoint3d::From(_xlen, _ylen, -_zlen));
		break;
	case 11:
		_pts = { DPoint3d::From(0,-_front),DPoint3d::From(_back,_front) };
		_extrusion_vec = DVec3d::FromScale(DVec3d::UnitZ(), _zlen);
		_drawpoint = DPoint3d::FromSumOf(_anchorpoint, DPoint3d::From(0, _ylen, -_zlen));
		break;
	}
	bvector<Dpoint3d> _global_pts = GetAddedPointVector(_drawpoint, _pts);
	ISolidPrimitivePtr _chamfer_SP = ISolidPrimitive::CreateDgnExtrusion(DgnExtrusionDetail(CurveVector::CreateLinear(_global_pts, CurveVector::BOUNDARY_TYPE_Outer), _extrusion_vec, true));
	if (SolidUtil::Create::BodyFromSolidPrimitive(_outchamfer, *_chamfer_SP, *ACTIVEMODEL))
		return ERROR;
	return SUCCESS;
}

StatusInt PDIWT_MS_CZ_CPP::LockHeadDrawing::DrawChamferCorner(bvector<ISolidKernelEntityPtr>& _outchamfercorner, RectEmptyBox ^ _rectbox, DPoint3dCR _anchorpoint)
{
	ISolidKernelEntityPtr _temp;
	ObservableCollection<EmptyBoxEdgeChameferInfo^>^ _chamferinfos = _rectbox->ChamferInfos;
	DPoint3d _basePoint{ 0,0,0 };
	bvector<bvector<DPoint3d>> _all_pts;
	if (_chamferinfos[0]->IsChamfered && _chamferinfos[4]->IsChamfered && _chamferinfos[8]->IsChamfered) //0
	{
		_basePoint = DPoint3d::FromSumOf(_anchorpoint, DPoint3d::From(0, 0, -_rectbox->EmptyBoxHeight));
		bvector<Dpoint3d> _v_pts
		{
			DPoint3d::FromSumOf(_basePoint,DPoint3d::From(_chamferinfos[4]->ChamferWidth,_chamferinfos[0]->ChamferLength,0)),
			DPoint3d::FromSumOf(_basePoint,DPoint3d::From(_chamferinfos[8]->ChamferLength,0,_chamferinfos[0]->ChamferWidth)),
			DPoint3d::FromSumOf(_basePoint,DPoint3d::From(0,_chamferinfos[8]->ChamferWidth,_chamferinfos[4]->ChamferLength))
		};
		_all_pts.push_back(_v_pts);
	}
	if (_chamferinfos[0]->IsChamfered && _chamferinfos[7]->IsChamfered && _chamferinfos[9]->IsChamfered) //1
	{
		_basePoint = DPoint3d::FromSumOf(_anchorpoint, DPoint3d::From(_rectbox->EmptyBoxWidth, 0, -_rectbox->EmptyBoxHeight));
		bvector<Dpoint3d> _v_pts
		{
			DPoint3d::FromSumOf(_basePoint,DPoint3d::From(-_chamferinfos[9]->ChamferWidth,0,_chamferinfos[0]->ChamferWidth)),
			DPoint3d::FromSumOf(_basePoint,DPoint3d::From(-_chamferinfos[7]->ChamferLength,_chamferinfos[0]->ChamferLength,0)),
			DPoint3d::FromSumOf(_basePoint,DPoint3d::From(0,_chamferinfos[9]->ChamferLength,_chamferinfos[7]->ChamferWidth))
		};
		_all_pts.push_back(_v_pts);
	}
	if (_chamferinfos[1]->IsChamfered && _chamferinfos[7]->IsChamfered && _chamferinfos[10]->IsChamfered) //2
	{
		_basePoint = DPoint3d::FromSumOf(_anchorpoint, DPoint3d::From(_rectbox->EmptyBoxWidth, _rectbox->EmptyBoxLength, -_rectbox->EmptyBoxHeight));
		bvector<Dpoint3d> _v_pts
		{
			DPoint3d::FromSumOf(_basePoint,DPoint3d::From(-_chamferinfos[7]->ChamferLength,-_chamferinfos[1]->ChamferWidth,0)),
			DPoint3d::FromSumOf(_basePoint,DPoint3d::From(-_chamferinfos[10]->ChamferLength,0,_chamferinfos[1]->ChamferLength)),
			DPoint3d::FromSumOf(_basePoint,DPoint3d::From(0,-_chamferinfos[10]->ChamferWidth,_chamferinfos[7]->ChamferWidth))
		};
		_all_pts.push_back(_v_pts);
	}
	if (_chamferinfos[1]->IsChamfered && _chamferinfos[4]->IsChamfered && _chamferinfos[11]->IsChamfered) //3
	{
		_basePoint = DPoint3d::FromSumOf(_anchorpoint, DPoint3d::From(0, _rectbox->EmptyBoxLength, -_rectbox->EmptyBoxHeight));
		bvector<Dpoint3d> _v_pts
		{
			DPoint3d::FromSumOf(_basePoint,DPoint3d::From(_chamferinfos[4]->ChamferWidth,-_chamferinfos[1]->ChamferWidth,0)),
			DPoint3d::FromSumOf(_basePoint,DPoint3d::From(0,-_chamferinfos[11]->ChamferLength,_chamferinfos[4]->ChamferLength)),
			DPoint3d::FromSumOf(_basePoint,DPoint3d::From(_chamferinfos[11]->ChamferWidth,0,_chamferinfos[1]->ChamferLength)),
		};
		_all_pts.push_back(_v_pts);
	}
	if (_chamferinfos[3]->IsChamfered && _chamferinfos[5]->IsChamfered && _chamferinfos[8]->IsChamfered) //4
	{
		_basePoint = _anchorpoint;
		bvector<Dpoint3d> _v_pts
		{
			DPoint3d::FromSumOf(_basePoint,DPoint3d::From(_chamferinfos[5]->ChamferLength,_chamferinfos[3]->ChamferWidth)),
			DPoint3d::FromSumOf(_basePoint,DPoint3d::From(0,_chamferinfos[8]->ChamferWidth,-_chamferinfos[5]->ChamferWidth)),
			DPoint3d::FromSumOf(_basePoint,DPoint3d::From(_chamferinfos[8]->ChamferLength,0,-_chamferinfos[3]->ChamferLength))
		};
		_all_pts.push_back(_v_pts);
	}
	if (_chamferinfos[3]->IsChamfered && _chamferinfos[6]->IsChamfered && _chamferinfos[9]->IsChamfered) //5
	{
		_basePoint = DPoint3d::FromSumOf(_anchorpoint, DPoint3d::From(_rectbox->EmptyBoxWidth, 0, 0));
		bvector<Dpoint3d> _v_pts
		{
			DPoint3d::FromSumOf(_basePoint,DPoint3d::From(-_chamferinfos[6]->ChamferWidth,_chamferinfos[3]->ChamferWidth,0)),
			DPoint3d::FromSumOf(_basePoint,DPoint3d::From(-_chamferinfos[9]->ChamferWidth,0,-_chamferinfos[3]->ChamferLength)),
			DPoint3d::FromSumOf(_basePoint,DPoint3d::From(0,_chamferinfos[9]->ChamferLength,-_chamferinfos[6]->ChamferLength))
		};
		_all_pts.push_back(_v_pts);
	}
	if (_chamferinfos[2]->IsChamfered && _chamferinfos[6]->IsChamfered && _chamferinfos[10]->IsChamfered) //6
	{
		_basePoint = DPoint3d::FromSumOf(_anchorpoint, DPoint3d::From(_rectbox->EmptyBoxWidth, _rectbox->EmptyBoxLength, 0));
		bvector<Dpoint3d> _v_pts
		{
			DPoint3d::FromSumOf(_basePoint,DPoint3d::From(-_chamferinfos[6]->ChamferWidth,-_chamferinfos[2]->ChamferLength,0)),
			DPoint3d::FromSumOf(_basePoint,DPoint3d::From(0,-_chamferinfos[10]->ChamferWidth,-_chamferinfos[6]->ChamferLength)),
			DPoint3d::FromSumOf(_basePoint,DPoint3d::From(-_chamferinfos[10]->ChamferLength,0,-_chamferinfos[2]->ChamferWidth))
		};
		_all_pts.push_back(_v_pts);
	}
	if (_chamferinfos[2]->IsChamfered && _chamferinfos[5]->IsChamfered && _chamferinfos[11]->IsChamfered) //7
	{
		_basePoint = DPoint3d::FromSumOf(_anchorpoint, DPoint3d::From(0, _rectbox->EmptyBoxLength, 0));
		bvector<Dpoint3d> _v_pts
		{
			DPoint3d::FromSumOf(_basePoint,DPoint3d::From(_chamferinfos[5]->ChamferLength,-_chamferinfos[2]->ChamferLength,0)),
			DPoint3d::FromSumOf(_basePoint,DPoint3d::From(_chamferinfos[11]->ChamferWidth,0,-_chamferinfos[2]->ChamferWidth)),
			DPoint3d::FromSumOf(_basePoint,DPoint3d::From(0,-_chamferinfos[11]->ChamferLength,-_chamferinfos[5]->ChamferWidth))
		};
		_all_pts.push_back(_v_pts);
	}
	for each (auto _pt in _all_pts)
	{
		CurveVectorPtr _cv = CurveVector::CreateLinear(_pt, CurveVector::BOUNDARY_TYPE_Outer);
		DVec3d _normal = DVec3d::FromCrossProductToPoints(_pt[0], _pt[1], _pt[2]);
		_normal.Normalize();
		_normal.Scale(_cv->Length());
		ISolidPrimitivePtr _sp = ISolidPrimitive::CreateDgnExtrusion(DgnExtrusionDetail(_cv, _normal, true));
		if (SolidUtil::Create::BodyFromSolidPrimitive(_temp, *_sp, *ACTIVEMODEL))
			return ERROR;
		_outchamfercorner.push_back(_temp);
	}
	return SUCCESS;
}

//此模块的anchorpoint为平面0点
StatusInt PDIWT_MS_CZ_CPP::LockHeadDrawing::DrawZPlanEmptyBox(bvector<ISolidKernelEntityPtr>& _outzplanemptybox, DPoint3dCR _anchorpoint)
{
	ObservableCollection<ZPlanEmptyBox^>^ _zplanemptybox = LH_LockHeadParameter->LH_EmptyZPlanBoxs;
	for each (ZPlanEmptyBox^ _zplanbox in _zplanemptybox)
	{
		DPoint3d _topOrigin = DPoint3d::FromSumOf(_anchorpoint, DPoint3d::From(_zplanbox->XDis, _zplanbox->YDis, -_zplanbox->ZDis));
		bvector<DPoint3d> _zplan_pts;
		for each (auto pt in _zplanbox->ZPlanInfos)
			_zplan_pts.push_back(DPoint3d::From(pt->X, pt->Y));
		CurveVectorPtr _zplanbox_cv = CurveVector::CreateLinear(_zplan_pts, CurveVector::BOUNDARY_TYPE_Outer);
		_zplanbox_cv->TransformInPlace(Transform::From(DVec3d::FromStartEnd(_zplan_pts[0], _topOrigin)));
		_zplanbox_cv->SimplifyLinestrings(0.01, true, true);

		ISolidPrimitivePtr _zplanbox_sp = ISolidPrimitive::CreateDgnExtrusion(DgnExtrusionDetail(_zplanbox_cv, DVec3d::FromScale(DVec3d::UnitZ(), -_zplanbox->EmptyBoxHeight), true));
		ISolidKernelEntityPtr _zplanbox_SKE;
		if (SolidUtil::Create::BodyFromSolidPrimitive(_zplanbox_SKE, *_zplanbox_sp, *ACTIVEMODEL))
			return ERROR;
		_outzplanemptybox.push_back(_zplanbox_SKE);
	}
	return SUCCESS;
}

