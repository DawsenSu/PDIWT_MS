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

        }

        [Command]
        public void FastPut()
        {
            MessageBoxService.ShowMessage($"The Number of ElementPropList is {ElementPropList.Count}", "Element number test");
            BCOM.Application app = PDIWT_MS.Program.COM_App;
            foreach (var ele in ElementPropList)
            {
              Bentley.GeometryNET.DMatrix3d matrix =  Bentley.GeometryNET.DMatrix3d.FromEulerAngles(Bentley.GeometryNET.EulerAngles.FromDegrees(ele.X, ele.Y, ele.Z));
            }
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
            bool DialogResult = OpenFileDialogService.ShowDialog();
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