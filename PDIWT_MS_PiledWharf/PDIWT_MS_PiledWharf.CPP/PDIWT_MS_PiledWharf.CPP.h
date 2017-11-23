// PDIWT_MS_PiledWharf.CPP.h

#pragma once

#include "stdafx.h"
#include <cstdlib>
#include <cmath>
#include <PSolid\PSolidCoreAPI.h>

#include <vcclr.h>
#include <msclr\marshal.h>

#include <algorithm>
#include <regex>

using namespace System;
using namespace System::Text;
using namespace System::Collections;
using namespace System::Collections::ObjectModel;
using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;
using namespace PDIWT_MS_PiledWharf::Models;
using namespace PDIWT_MS_PiledWharf::Models::Soil;
using namespace msclr::interop;

namespace PDIWT_MS_PiledWharf_CPP {

	public enum class QueryResultStatus
	{
		Success,
		NotFindSoilLayer,
		NotIntersect,
		Error
	};

	struct DPoint3dZGreater
	{
		bool operator()(const DPoint3d& k1, const DPoint3d& k2)
		{
			return k1.z > k2.z;
		}
	};

	public class IntersectionPointQuery
	{
	public:		
		String^ GetNumber();
		QueryResultStatus GetIntersectionPointsAndLayerInfo(ObservableCollection<PilePieceInSoilLayerInfo^>^% _result, DPoint3dCR _toppoint, DPoint3dCR _bottompoint);
	private:
		void FinBSElement(ElementAgendaR _bsagenda);
		void GetBSGeoPtrFromEditElementHandle(bvector<MSBsplineSurfacePtr>& _bsvector, ElementAgendaCR _bsagenda);
		StatusInt GetBSLayerInfoFromHandle(List<SoilLayerInfo^>^ _layerinfo, ElementAgendaCR _bsagenda);
		StatusInt FindBSElementAttributes(EditElementHandleR _eeh, SoilLayerInfo^ _layerinfo);
		QueryResultStatus GetIntersectionPoints(const bvector<MSBsplineSurfacePtr>& _bsvector, DPoint3dCR _piletoppt, DPoint3dCR _pilebottompt, bvector<DPoint3d>& _intersectionpts,bvector<int>& _intersectionlayerinfoindex);
	};


	public ref class PointQuery
	{
	private:
		QueryResultStatus _queryresult;
	public:
		ObservableCollection<PilePieceInSoilLayerInfo^>^ GetPilePieceSoilLayerInfos(Bentley::GeometryNET::DPoint3d^ _piletop, Bentley::GeometryNET::DPoint3d^ _pilebottom)
		{
			IntersectionPointQuery _test;
			DPoint3d _native_piletop = DPoint3d::From(_piletop->X, _piletop->Y, _piletop->Z);
			DPoint3d _native_pilebottom = DPoint3d::From(_pilebottom->X, _pilebottom->Y, _pilebottom->Z);
			ObservableCollection<PilePieceInSoilLayerInfo^>^ _result = gcnew ObservableCollection<PilePieceInSoilLayerInfo ^>();
			_queryresult = _test.GetIntersectionPointsAndLayerInfo(_result, _native_piletop, _native_pilebottom);
			return _result;
		}
		String^ GetInfo()
		{
			IntersectionPointQuery _test;
			ObservableCollection<PilePieceInSoilLayerInfo^>^ _result = gcnew ObservableCollection<PilePieceInSoilLayerInfo ^>();
			_queryresult = _test.GetIntersectionPointsAndLayerInfo(_result, DPoint3d::From(0,0,0), DPoint3d::From(0,0,-1e8));
			//StringBuilder^ _sb = gcnew StringBuilder();
			//for each(auto _pile in _result)
			//{
			//	_sb->Append(String::Format("Name:{0} topz:{1} bottomz:{2} length:{3}\n", _pile->CurrentSoilLayerInfo->SoilLayerNum, _pile->PileTopZ_InCurrentSoilLayer, _pile->PileBottomZ_InCurrentSoilLayer, _pile->PilePieceLength));
			//}
			return _result->Count.ToString();
		}
		property QueryResultStatus QueryResult
		{
			QueryResultStatus get()
			{
				return _queryresult;
			}
		}
	};

	public enum class PDIWTSchemaImportSatutes
	{
		Failed_ReadxmlString,
		Failed_UpdateSchema,
		Failed_ImportSchema,
		Success_UpdateSchema,
		Success_ImportSchema
	};

	public ref class SchmemaHelper
	{
	public:
		PDIWTSchemaImportSatutes ImportPDIWTSchema(String^ xmlstring)
		{
			DgnECManagerR _manager = DgnECManager::GetManager();
			pin_ptr<const wchar_t> _xmlstr = PtrToStringChars(xmlstring);
			ECSchemaPtr _pdiwtschema;
			if (SchemaReadStatus::SCHEMA_READ_STATUS_Success != _manager.ReadSchemaFromXmlString(_pdiwtschema, _xmlstr, ISessionMgr::GetActiveDgnFile()))
				return PDIWTSchemaImportSatutes::Failed_ReadxmlString;
			auto _importresult = _manager.ImportSchema(*_pdiwtschema, *ISessionMgr::GetActiveDgnFile());
			if (SchemaImportStatus::SCHEMAIMPORT_SchemaAlreadyStoredInFile == _importresult)
			{
				if (SchemaUpdateStatus::SCHEMAUPDATE_Success == _manager.UpdateSchema(*_pdiwtschema, *ISessionMgr::GetActiveDgnFile()))
					return PDIWTSchemaImportSatutes::Success_UpdateSchema;
				else
					return PDIWTSchemaImportSatutes::Failed_UpdateSchema;
			}
			if (SchemaImportStatus::SCHEMAIMPORT_Success != _importresult)
				return PDIWTSchemaImportSatutes::Failed_ImportSchema;			 
			return PDIWTSchemaImportSatutes::Success_ImportSchema;
		}
	};
}
