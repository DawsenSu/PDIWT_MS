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

		// TODO:  �ڴ˴���Ӵ���ķ�����
	public:
		LockHeadDrawing(LockHeadParameters^);
		property LockHeadParameters^ LH_LockHeadParameter;
		StatusInt DoDraw();
	private:
		//Utilities Function
		// �������ڶ�λ���ģ��uor�������ž���
		Transform GetModelTransform(DPoint3dCR);
		// ���ݶ�λ������λ������ֵ����ø��������
		bvector<DPoint3d> GetAddedPointVector(const DPoint3d, const bvector<DPoint3d>&);
		// �������ʵ�����ͨ����λ���xƽ��ľ����ƺϲ��󴫳�
		StatusInt CloneMirrorSolidAndUnion(ISolidKernelEntityPtr&, DPoint3dCR);
		// �����н�curveVector���н����нǺϲ�(�ֱ�λ��1,2,3,4����)
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
