// PDIWT_MS_CPP.h

#pragma once

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Collections::ObjectModel;
using namespace System::Runtime::InteropServices;
namespace BD = Bentley::DgnPlatformNET;
namespace BG = Bentley::GeometryNET;
namespace BM = Bentley::MstnPlatformNET;
namespace BECN = Bentley::ECN;
namespace BECO = Bentley::ECObjects;
using namespace PDIWT_MS_Tool::ViewModels;

namespace PDIWT_MS_CPP
{

	public ref class ArmorCellInfo
	{
	public:
		property String^ CellName;
		property BG::DPoint3d^ Origin;
		property BG::DMatrix3d^ CellTrans;
	};

	public ref class CellFunction
	{
	public:
		static BD::StatusInt AttachLibrary([OutAttribute]String^% openCellLibName, String^ intputName, String^ defaultDir);
		//static BD::StatusInt GetLibraryObject([OutAttribute]BD::DgnFile^% libraryObject, String^ librarayName);
		static unsigned int PlaceCell(ElementProp^ eleProp);
		static unsigned int PlaceCell(ArmorCellInfo^ cellInfo);
		static DPoint3d M2NDPoint3d(BG::DPoint3d^ point);
		static RotMatrix M2NDRotMatrix(BG::DMatrix3d^ transform);
	};

}
