// PDIWT_MC_CZ_CPP.h

#pragma once
#include "Stdafx.h"
#include <cstdlib>
#include <PSolid\PSolidCoreAPI.h>

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace PDIWT_MS_CZ::Models;


namespace PDIWT_MS_CZ_CPP {

	public ref class LockHeadDrawing
	{

		// TODO:  在此处添加此类的方法。
	public:
		LockHeadDrawing(LockHeadParameters^);
		property LockHeadParameters^ LH_LockHeadParameter;
		void DoDraw();
	private:
		//Utilities Function
		Transform GetModelTransform();
		bvector<DPoint3d> GetAddedPointVector(const DPoint3d, const bvector<DPoint3d>);
		//Draw Function
		StatusInt DrawBaseBoard(EditElementHandleP,DPoint3dCR);
		StatusInt DrawBaseBoard_Cut(EditElementHandleP, DPoint3dCR);
	};
}
