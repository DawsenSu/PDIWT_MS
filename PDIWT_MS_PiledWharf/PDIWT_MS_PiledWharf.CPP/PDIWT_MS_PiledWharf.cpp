// PDIWT_MS_PiledWharf.CPP.cpp: 定义 DLL 应用程序的导出函数。
//

#include "stdafx.h"
#include "PDIWT_MS_PiledWharf.h"

void PDIWT_MS_PiledWharf_CPP::IntersectionPointQuery::FinBSElement(bvector<MSBsplineSurfacePtr>& _bsvector)
{
	ScanCriteriaP _sc = new ScanCriteria();
	_sc->SetModelRef(ACTIVEMODEL);
	_sc->SetModelSections(DgnModelSections::GraphicElements);
	_sc->AddSingleElementTypeTest(MSElementTypes::BSPLINE_SURFACE_ELM);
	_sc->SetElemRefCallback([](ElementRefP _eleRefp, CallbackArgP _arg, ScanCriteriaP _scp)
	{
		auto _bsvp = static_cast<bvector<MSBsplineSurfacePtr>*>(_arg);
		EditElementHandle _handle(_eleRefp, _scp->GetModelRef());
		auto _bshandlerp = dynamic_cast<BSplineSurfaceHandler*>(_handle.GetDisplayHandler());
		if (_bshandlerp == nullptr) return (int)ERROR;
		MSBsplineSurfacePtr _sbptr;
		if (_bshandlerp->GetBsplineSurface(_handle, _sbptr)) return (int)ERROR;
		_bsvp->push_back(_sbptr);
		return (int)SUCCESS;
	}, 
		&_bsvector );
}

