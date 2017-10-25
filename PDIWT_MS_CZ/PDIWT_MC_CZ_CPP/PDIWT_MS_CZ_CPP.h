// PDIWT_MC_CZ_CPP.h

#pragma once
#include "Stdafx.h"
#include <cstdlib>
#include <cmath>
#include <PSolid\PSolidCoreAPI.h>
#include <gsl\gsl_roots.h>

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Collections::ObjectModel;
using namespace System::Runtime::InteropServices;
using namespace PDIWT_MS_CZ::Models;


namespace PDIWT_MS_CZ_CPP {

	public ref class LockHeadDrawing
	{

		// TODO:  在此处添加此类的方法。
	public:
		LockHeadDrawing(LockHeadParameters^);
		property LockHeadParameters^ LH_LockHeadParameter;
		StatusInt DoDraw();
	private:
		//Utilities Function
		// 获得相对于定位点的模型uor比例缩放矩阵
		Transform GetModelTransform(DPoint3dCR);
		// 根据定位点和相对位置向量值，获得各点的坐标
		bvector<DPoint3d> GetAddedPointVector(const DPoint3d, const bvector<DPoint3d>&);
		// 将传入的实体进行通过定位点的x平面的镜像复制合并后传出
		StatusInt CloneMirrorSolidAndUnion(ISolidKernelEntityPtr&, DPoint3dCR);
		// 画出切角curveVector进行进行切角合并(分别位于1,2,3,4象限)
		StatusInt DrawRoundChamferCorner(CurveVectorPtr&, DPoint3dCR, double, double);
		StatusInt DebugCurveVector(CurveVectorCR);
		//Draw Function
		StatusInt DrawBaseBoard(ISolidKernelEntityPtr &, DPoint3dCR);
		StatusInt DrawBaseBoard_Cut(ISolidKernelEntityPtr &, DPoint3dCR); // DrawBaseBoar SubFunction
		StatusInt DrawSidePier(ISolidKernelEntityPtr&, DPoint3dCR);
		StatusInt DrawDoorSill(ISolidKernelEntityPtr&, DPoint3dCR);
		StatusInt DrawLocalConcertationCulvert(ISolidKernelEntityPtr&, DPoint3dCR);
		StatusInt DrawWaterDivision(ISolidKernelEntityPtr&, DPoint3dCR);
		StatusInt DrawShortCulvert(ISolidKernelEntityPtr&, DPoint3dCR);
		StatusInt DrawRectEmptyBoxes(bvector<ISolidKernelEntityPtr>&, DPoint3dCR);
		StatusInt DrawZPlanEmptyBox(bvector<ISolidKernelEntityPtr>&, DPoint3dCR);
	};
}
