//#define DEBUG

using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PDIWT_MS.Tools.Model;

using BM = Bentley.MstnPlatformNET;
using BD = Bentley.DgnPlatformNET;
using BCOM = Bentley.Interop.MicroStationDGN;
using System.Linq;

namespace PDIWT_MS.Tools.ViewModels
{
    public class QuickInsertViewModel : ViewModelBase
    {
        public ObservableCollection<ElementProp> ElementPropList
        {
            get { return GetProperty(() => ElementPropList); }
            set { SetProperty(() => ElementPropList, value); }
        }
        public ObservableCollection<CellName> CellNameList
        {
            get { return GetProperty(() => CellNameList); }
            set { SetProperty(() => CellNameList, value); }
        }
        public string CellLibPath
        {
            get { return GetProperty(() => CellLibPath); }
            set { SetProperty(() => CellLibPath, value); }
        }
        public int ProcessNum
        {
            get { return GetProperty(() => ProcessNum); }
            set { SetProperty(() => ProcessNum, value); }
        }

        public int ProcessMaximun
        {
            get { return GetProperty(() => ProcessMaximun); }
            set { SetProperty(() => ProcessMaximun, value); }
        }

        protected override void OnInitializeInDesignMode()
        {
            base.OnInitializeInDesignMode();
            ElementPropList = new ObservableCollection<ElementProp>
            {
            };
        }

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            CreateLists();
        }

        void CreateLists()
        {
            ElementPropList = new ObservableCollection<ElementProp>();
            //for (int i = 0; i < 20; i++)
            //{
            //    ElementPropList.Add(new ElementProp(i));
            //}
            CellNameList = new ObservableCollection<CellName>();
            //for (int i = 0; i < 3; i++)
            //{
            //    CellNameList.Add(new CellName(i));
            //}
        }

        public virtual IMessageBoxService MessageBoxService { get { return GetService<IMessageBoxService>(); } }
        public virtual IOpenFileDialogService OpenFileDialogService { get { return GetService<IOpenFileDialogService>(); } }

        IGetElementPropFromFile FileData;

        [Command]
        public void ImportFromFile()
        {
            OpenFileDialogService.Filter = "Excel 07-16(*.xlsx)|*.xlsx";
            OpenFileDialogService.Title = "选择导入文件";
            if (OpenFileDialogService.ShowDialog())
            {
                var fileInfo = new System.IO.FileInfo(OpenFileDialogService.File.GetFullName());
                switch (fileInfo.Extension.ToLower())
                {
                    case ".xlsx":
                        FileData = new GetElementPropFromExcel();
                        break;
                    default:
                        break;
                }
                try
                {
                    ElementPropList = FileData.GetElementPropList(fileInfo, CellNameList);
                }
                catch (Exception e)
                {
                    MessageBoxService.ShowMessage($"在加载{fileInfo.FullName}时出现错误！\n" + e.Message, "加载时出现错误");
                }
            }
            ProcessNum = 0; //进度条为0
        }

        [Command]
        public void FastPut()
        {
            ProcessNum = 0;
            ProcessMaximun = ElementPropList.Count;
            //MessageBoxService.ShowMessage($"The Number of ElementPropList is {ElementPropList.Count}", "Element number test");
            BCOM.Application app = PDIWT_MS.Program.COM_App;
            BCOM.Point3d scale = app.Point3dOne();
            BCOM.Matrix3d bmatrix3dx, bmatrix3dy, bmatrix3dz, bmatrix3dall;
            string cellName;
            BCOM.Point3d origin;
            foreach (var ele in ElementPropList)
            {
                cellName = string.Empty;
                foreach (var cn in CellNameList)
                {
                    if (cn.Id == ele.CellNameType)
                    {
                        cellName = cn.Name;
                        break;
                    }
                }
                origin = app.Point3dFromXYZ(ele.X, ele.Y, ele.Z);
                bmatrix3dx = app.Matrix3dFromAxisAndRotationAngle(0, app.Radians(ele.AngelX));
                bmatrix3dy = app.Matrix3dFromAxisAndRotationAngle(1, app.Radians(ele.AngelY));
                bmatrix3dz = app.Matrix3dFromAxisAndRotationAngle(2, app.Radians(ele.AngelZ));
                bmatrix3dall = app.Matrix3dFromMatrix3dTimesMatrix3dTimesMatrix3d(ref bmatrix3dx, ref bmatrix3dy, ref bmatrix3dz);
                if (!string.IsNullOrEmpty(cellName))
                {
                    BCOM.Element element = app.CreateSharedCellElement2(cellName, ref origin, ref scale, true, ref bmatrix3dall);
                    app.ActiveModelReference.AddElement(element);
                }
                ProcessNum++;
                Bentley.UI.Threading.DispatcherHelper.DoEvents();
            }
            MessageBoxService.ShowMessage($"所列单元创建完毕，共{ElementPropList.Count}个。", "创建完成", MessageButton.OK, MessageIcon.Information);
        }

        public bool CanFastPut()
        {
            if (ElementPropList.Count == 0)
                return false;
            else
                return true;
        }

        [Command]
        public void OpenCellLib()
        {
            OpenFileDialogService.Filter = "Cell Library (.cell)|*.cel|Dgn File(.dng)|*.dgn|All Files (*.*)|*.*";
            OpenFileDialogService.Title = "选择Cell库文件";
            string defaultDirec = string.Empty;
#if DEBUG
            defaultDirec = @"D:\项目\BIM实习\梅山二期\建模中间文件\码头\celllib";
#endif
            bool DialogResult = OpenFileDialogService.ShowDialog(defaultDirec);
            if (DialogResult)
            {
                IntPtr filep;
                if (0 == PDIWT_MS.Marshal.BentleyMarshal.mdlCell_getLibraryObject(out filep, OpenFileDialogService.GetFullFileName(), true))
                {
                    CellLibPath = OpenFileDialogService.GetFullFileName();
                    BD.DgnFile dgnFile = BD.DgnFile.GetDgnFile(filep);
                    int modelNum = 0;
                    foreach (var index in dgnFile.GetModelIndexCollection())
                    {
                        if (index.CellPlacementOptions == Bentley.DgnPlatformNET.CellPlacementOptions.CanBePlacedAsCell)
                        {
                            CellNameList.Add(new CellName { Name = index.Name, Id = modelNum++ });
                        }
                    }
                    PDIWT_MS.Program.COM_App.AttachCellLibrary(OpenFileDialogService.File.GetFullName());
                }
            }
        }
    }
}