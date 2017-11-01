// 这是主 DLL 文件。

#include "stdafx.h"

#include "PDIWT_MS_CPP.h"

using namespace PDIWT_MS_CPP;
BD::StatusInt CellFunction::AttachLibrary([OutAttribute]String ^% openCellLibName, String ^ intputName, String ^ defaultDir)
{
	BeFileName outCellLibname;
	pin_ptr<const wchar_t> pCellLibPath = PtrToStringChars(intputName);
	pin_ptr<const wchar_t> pDefaultDir = PtrToStringChars(defaultDir);
	int status = mdlCell_attachLibrary(outCellLibname, &BeFileName(pCellLibPath), pDefaultDir, TRUE);
	openCellLibName = gcnew String(outCellLibname);
	return BD::StatusInt(status);
}

unsigned int CellFunction::PlaceCell(ElementProp ^ eleProp)
{
	String^ tempCellName = eleProp->CellName;
	pin_ptr<const WChar> pCellName = PtrToStringChars(tempCellName->Split('(')[0]);
	const DPoint3d rOrigin = DPoint3d::From(eleProp->X, eleProp->Y, eleProp->Z);
	RotMatrix rotMatrix = RotMatrix::FromPrincipleAxisRotations(RotMatrix::FromIdentity(), fc_piover180*eleProp->AngleX, fc_piover180*eleProp->AngleY, fc_piover180*eleProp->AngleZ);
	return mdlCell_placeCell(&rOrigin, nullptr, true, &rotMatrix, nullptr, 0, false, 0, 1, pCellName, nullptr);
}

unsigned int CellFunction::PlaceCell(ArmorCellInfo^ cellInfo)
{
	String^ tempCellName = cellInfo->CellName;
	pin_ptr<const WChar> pCellName = PtrToStringChars(tempCellName);
	DPoint3d origin = M2NDPoint3d(cellInfo->Origin);
	RotMatrix rotMatrix = M2NDRotMatrix(cellInfo->CellTrans);
	return mdlCell_placeCell(&origin, nullptr, true, &rotMatrix, nullptr, 0, false, 0, 1, pCellName, nullptr);
}

DPoint3d CellFunction::M2NDPoint3d(BG::DPoint3d^ point)
{
	return DPoint3d::From(point->X, point->Y, point->Z);
}
RotMatrix CellFunction::M2NDRotMatrix(BG::DMatrix3d^ transform)
{
	BG::DVector3d^ rowx = transform->RowX;
	BG::DVector3d^ rowy = transform->RowY;
	BG::DVector3d^ rowz = transform->RowZ;
	return RotMatrix::FromRowValues(rowx->X, rowx->Y, rowx->Z, rowy->X, rowy->Y, rowy->Z, rowz->X, rowz->Y, rowz->Z);
}

//BD::StatusInt PDIWT_MS_CPP::CellFunction::GetLibraryObject([OutAttribute]BD::DgnFile ^% libraryObject, String ^ librarayName)
//{
//	DgnFileP lib = nullptr;
//	pin_ptr<const wchar_t> plibraryName = PtrToStringChars(librarayName);
//	BD::StatusInt outstatus = mdlCell_getLibraryObject(&lib, plibraryName, false);
//	//libraryObject->
//	return outstatus;
//}

