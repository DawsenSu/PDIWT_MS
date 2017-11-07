// 这是主 DLL 文件。

#include "stdafx.h"
#include "PDIWT_MS_PiledWharf.CPP.h"

String^ PDIWT_MS_PiledWharf_CPP::IntersectionPointQuery::GetNumber()
{
	ElementAgenda _bshandles;
	List<SoilLayerInfo^>^ _layerinfos = gcnew List<SoilLayerInfo^>();
	FinBSElement(_bshandles);

	GetBSLayerInfoFromHandle(_layerinfos, _bshandles);
	StringBuilder^ _sb = gcnew StringBuilder();
	for each (auto _layer in _layerinfos)
	{
		_sb->Append(_layer->SoilLayerNum + _layer->Qfi.ToString() + L"\n");
	}
	
	return _sb->ToString();
	//bvector<MSBsplineSurfacePtr> _bsv;
	//GetBSGeoPtrFromEditElementHandle(_bsv, _bshandles);

	//bvector<DPoint3d> _intersection_pts;
	//DPoint3d _toppt = DPoint3d::From(0, 0, 10000);
	//DPoint3d _bottompt = DPoint3d::From(0, 0, -20000);
	//EditElementHandle _eeh;
	//LineHandler::CreateLineElement(_eeh, nullptr, DSegment3d::From(_toppt, _bottompt), true, *ACTIVEMODEL);
	//_eeh.AddToModel();
	//auto _result = GetIntersectionPoints(_bsv, _toppt, _bottompt, _intersection_pts);
	//if (_result == QueryResultStatus::Success)
	//{
	//	for each (auto _pt in _intersection_pts)
	//	{
	//		DgnSphereDetail _sphere(_pt, 500);
	//		ISolidPrimitivePtr _sphere_sp = ISolidPrimitive::CreateDgnSphere(_sphere);
	//		EditElementHandle _eehsphere;
	//		DraftingElementSchema::ToElement(_eehsphere, *_sphere_sp, nullptr, *ACTIVEMODEL);
	//		_eehsphere.AddToModel();
	//	}
	//	return _intersection_pts.size();
	//}
	//else
	//	return (int)_result;
}

PDIWT_MS_PiledWharf_CPP::QueryResultStatus PDIWT_MS_PiledWharf_CPP::IntersectionPointQuery::GetIntersectionPointsAndLayerInfo(ObservableCollection<PilePieceInSoilLayerInfo^>^% _result, DPoint3dCR _toppoint, DPoint3dCR _bottompoint)
{
	_result = gcnew ObservableCollection<PilePieceInSoilLayerInfo ^>();

	ElementAgenda _bshandles;
	FinBSElement(_bshandles);

	List<SoilLayerInfo^>^ _layerinfos = gcnew List<SoilLayerInfo^>();
	GetBSLayerInfoFromHandle(_layerinfos, _bshandles);

	bvector<MSBsplineSurfacePtr> _bsv;
	GetBSGeoPtrFromEditElementHandle(_bsv, _bshandles);

	bvector<DPoint3d> _intersection_pts;
	bvector<int> _intersectionptr_layer_indexes;
	QueryResultStatus _queryresult = GetIntersectionPoints(_bsv, _toppoint, _bottompoint, _intersection_pts,_intersectionptr_layer_indexes);
	if (_queryresult == QueryResultStatus::Success)
	{
		double _uorpermeter = ACTIVEMODEL->GetModelInfoCP()->GetUorPerMeter();
		bstdmap<DPoint3d, int, DPoint3dZGreater> _intersectionmap;
		for (size_t i = 0; i < _intersection_pts.size(); i++)
			_intersectionmap.insert(make_bpair(_intersection_pts[i],_intersectionptr_layer_indexes[i]));
		
		for(auto _itr = _intersectionmap.begin(), _nextitr = ++(_intersectionmap.begin()); _itr != _intersectionmap.end(); ++_itr, ++_nextitr)
		{
			PilePieceInSoilLayerInfo^ _pilesoilinfo = gcnew PilePieceInSoilLayerInfo();
			_pilesoilinfo->CurrentSoilLayerInfo = _layerinfos[_itr->second];
			_pilesoilinfo->PileTopZ_InCurrentSoilLayer = _itr->first.z / _uorpermeter;
			if (_nextitr == _intersectionmap.end())
			{
				_pilesoilinfo->PileBottomZ_InCurrentSoilLayer = _bottompoint.z/_uorpermeter;
				_pilesoilinfo->PilePieceLength = (_itr->first - _bottompoint).Magnitude() / _uorpermeter;
				_result->Add(_pilesoilinfo);
				break;
			}
			else
			{
				_pilesoilinfo->PileBottomZ_InCurrentSoilLayer = _nextitr->first.z / _uorpermeter;
				_pilesoilinfo->PilePieceLength = (_itr->first - _nextitr->first).Magnitude() / _uorpermeter;
				_result->Add(_pilesoilinfo);
			}			
		}
	}
	return _queryresult;
}

void PDIWT_MS_PiledWharf_CPP::IntersectionPointQuery::FinBSElement(ElementAgendaR _bsagenda)
{
	ScanCriteriaP _sc = ScanCriteria::Create();
	_sc->SetModelRef(ACTIVEMODEL);
	_sc->SetModelSections(DgnModelSections::GraphicElements);
	_sc->AddSingleElementTypeTest(MSElementTypes::BSPLINE_SURFACE_ELM);
	_sc->SetElemRefCallback([](ElementRefP _eleRefp, CallbackArgP _arg, ScanCriteriaP _scp)
	{
		auto _bsvp = static_cast<ElementAgenda*>(_arg);
		_bsvp->push_back(ElemAgendaEntry(_eleRefp,_scp->GetModelRef()));
		return (int)SUCCESS;
	}, &_bsagenda);
	_sc->Scan();
	ScanCriteria::Delete(_sc);
}

void PDIWT_MS_PiledWharf_CPP::IntersectionPointQuery::GetBSGeoPtrFromEditElementHandle(bvector<MSBsplineSurfacePtr>& _bsvector, ElementAgendaCR _bsagenda)
{
	for (auto _bsentry : _bsagenda)
	{
		auto _bshandlerp = dynamic_cast<BSplineSurfaceHandler*>(_bsentry.GetDisplayHandler());
		if (_bshandlerp == nullptr) continue;
		MSBsplineSurfacePtr _sbptr;
		if (_bshandlerp->GetBsplineSurface(_bsentry, _sbptr)) continue;
		_bsvector.push_back(_sbptr);
	}
}

StatusInt PDIWT_MS_PiledWharf_CPP::IntersectionPointQuery::GetBSLayerInfoFromHandle(List<SoilLayerInfo^>^ _layerinfos, ElementAgendaCR _bsagenda)
{
	for (auto _bsentry : _bsagenda)
	{
		SoilLayerInfo^ _layerinfo = gcnew SoilLayerInfo();
		if(FindBSElementAttributes(_bsentry,_layerinfo)) continue;
		_layerinfos->Add(_layerinfo);
	}
	return SUCCESS;
}

StatusInt PDIWT_MS_PiledWharf_CPP::IntersectionPointQuery::FindBSElementAttributes(EditElementHandleR _eeh, SoilLayerInfo^ _layerinfo)
{
	DgnECManagerR _manager = DgnECManager::GetManager();
	SchemaInfo _aglosgeoecsi(SchemaKey(L"AglosGeo", 1, 0), *ISessionMgr::GetActiveDgnFile());
	SchemaInfo _powergeoecsi(SchemaKey(L"PowerGeo", 1, 0), *ISessionMgr::GetActiveDgnFile());
	ECSchemaPtr _aglos_ptr = _manager.LocateSchemaInDgnFile(_aglosgeoecsi, SchemaMatchType::SCHEMAMATCHTYPE_Identical);
	ECSchemaPtr _power_ptr = _manager.LocateSchemaInDgnFile(_powergeoecsi, SchemaMatchType::SCHEMAMATCHTYPE_Identical);
	if ( _aglos_ptr.IsNull() || _power_ptr.IsNull()) return ERROR;

	//赋值 boring
#pragma region Aglos ECSchma
	ECClassP _aglos_aglos_ecclassp = _aglos_ptr->GetClassP(L"AglosGeo");
	DgnElementECInstancePtr _aglos_ec_intsance = _manager.FindInstanceOnElement(_eeh, *_aglos_aglos_ecclassp);
	if (_aglos_ec_intsance.IsNull()) return ERROR;

	bvector<WCharCP> _aglos_accessstr = { L"Qfi",L"Xii",L"PsiSi",L"Xifi",L"Betasi",L"Xifi2",L"Qri" };	WString _strval;
	if (ECObjectsStatus::ECOBJECTS_STATUS_Success == _aglos_ec_intsance->GetValueAsString(_strval, _aglos_accessstr[0], false, 0))
		_layerinfo->Qfi = BeStringUtilities::Wtof(_strval.GetWCharCP());
	if (ECObjectsStatus::ECOBJECTS_STATUS_Success == _aglos_ec_intsance->GetValueAsString(_strval, _aglos_accessstr[1], false, 0))
		_layerinfo->Xii = BeStringUtilities::Wtof(_strval.GetWCharCP());
	if (ECObjectsStatus::ECOBJECTS_STATUS_Success == _aglos_ec_intsance->GetValueAsString(_strval, _aglos_accessstr[2], false, 0))
		_layerinfo->PsiSi = BeStringUtilities::Wtof(_strval.GetWCharCP());
	if (ECObjectsStatus::ECOBJECTS_STATUS_Success == _aglos_ec_intsance->GetValueAsString(_strval, _aglos_accessstr[3], false, 0))
		_layerinfo->Xifi = BeStringUtilities::Wtof(_strval.GetWCharCP());
	if (ECObjectsStatus::ECOBJECTS_STATUS_Success == _aglos_ec_intsance->GetValueAsString(_strval, _aglos_accessstr[4], false, 0))
		_layerinfo->Betasi = BeStringUtilities::Wtof(_strval.GetWCharCP());
	if (ECObjectsStatus::ECOBJECTS_STATUS_Success == _aglos_ec_intsance->GetValueAsString(_strval, _aglos_accessstr[5], false, 0))
		_layerinfo->Xifi2 = BeStringUtilities::Wtof(_strval.GetWCharCP());
	if (ECObjectsStatus::ECOBJECTS_STATUS_Success == _aglos_ec_intsance->GetValueAsString(_strval, _aglos_accessstr[6], false, 0))
		_layerinfo->Qri = BeStringUtilities::Wtof(_strval.GetWCharCP());
#pragma endregion

#pragma region Power ECShma
	ECClassP _power_geobase_ecclassp = _power_ptr->GetClassP(L"GeotechnicalLithology");
	DgnElementECInstancePtr _power_ecgeobase_instance = _manager.FindInstanceOnElement(_eeh, *_power_geobase_ecclassp,true);
	if (_power_ecgeobase_instance.IsNull()) return ERROR;
	if (ECOBJECTS_STATUS_Success == _power_ecgeobase_instance->GetValueAsString(_strval, L"CategoryName", false, 0))
		_layerinfo->SoilLayerTypeName = marshal_as<String^>(_strval.GetWCharCP());
	if (ECOBJECTS_STATUS_Success == _power_ecgeobase_instance->GetValueAsString(_strval, L"UserId", false, 0))
		_layerinfo->SoilLayerNum = marshal_as<String^>(_strval.GetWCharCP());
#pragma endregion

	return SUCCESS;
}

PDIWT_MS_PiledWharf_CPP::QueryResultStatus PDIWT_MS_PiledWharf_CPP::IntersectionPointQuery::GetIntersectionPoints(
	const bvector<MSBsplineSurfacePtr>& _bsvector,
	DPoint3dCR _piletoppt,
	DPoint3dCR _pilebottompt,
	bvector<DPoint3d>& _intersectionpts,
	bvector<int>& _intersectionlayerinfoindex)
{
	_intersectionpts.clear();
	_intersectionlayerinfoindex.clear();

	if (_bsvector.size() == 0) return QueryResultStatus::NotFindSoilLayer;
	DRay3d _pileray = DRay3d::FromOriginAndTarget(_piletoppt, _pilebottompt);
	double _pilelength = (_pilebottompt - _piletoppt).Magnitude();
	bvector<DPoint3d> _tmpintersectionpts;

	int _index = 0;
	for each (auto _bs in _bsvector)
	{
		bvector<DPoint3d> _cl_intersectionpts;
		bvector<double> _rayparamerters;
		bvector<DPoint2d> _surfaceparameters;
		_bs->IntersectRay(_cl_intersectionpts, _rayparamerters, _surfaceparameters, _pileray);
		if (_cl_intersectionpts.size() == 0) continue;
		for (size_t i = 0; i < _cl_intersectionpts.size(); i++)
		{
			if (0 <= _rayparamerters[i] && _rayparamerters[i] <= 1)
			{
				_tmpintersectionpts.push_back(_cl_intersectionpts[i]);
				_intersectionlayerinfoindex.push_back(_index);
			}
		}
		_index++;
	}
	if (_tmpintersectionpts.size() == 0)
	{
		_intersectionlayerinfoindex.clear();
		return QueryResultStatus::NotIntersect;
	}
	_intersectionpts.assign(_tmpintersectionpts.begin(), _tmpintersectionpts.end());
	return QueryResultStatus::Success;
}
