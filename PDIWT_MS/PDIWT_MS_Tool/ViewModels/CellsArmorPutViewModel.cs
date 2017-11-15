using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
//using System.Windows.Forms;
using Microsoft.Win32;
using PDIWT_MS_Tool.Properties;
using System.Linq;
using System.Windows.Media;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

using Bentley.EC.Persistence.Query;
using BD = Bentley.DgnPlatformNET;
using BM = Bentley.MstnPlatformNET;
using BG = Bentley.GeometryNET;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

using PDIWT_MS_CPP;
using PDIWT_MS_Tool.Models;

namespace PDIWT_MS_Tool.ViewModels
{
    public class CellsArmorPutViewModel : ViewModelBase
    {
        private string _CellLibPath;
        public string CellLibPath
        {
            get { return _CellLibPath; }
            set { Set(ref _CellLibPath, value); }
        }
        private ObservableCollection<string> _CellNames;
        public ObservableCollection<string> CellNames
        {
            get { return _CellNames; }
            set { Set(ref _CellNames, value); }
        }

        private string _SelectCellName;
        public string SelectCellName
        {
            get { return _SelectCellName; }
            set { Set(ref _SelectCellName, value); }
        }
        private double _UAxisOffset;
        public double UAxisOffset
        {
            get { return _UAxisOffset; }
            set { Set(ref _UAxisOffset, value); }
        }
        private double _VAxisOffset;
        public double VAxisOffset
        {
            get { return _VAxisOffset; }
            set { Set(ref _VAxisOffset, value); }
        }

        private string _MasterUnitTooltip;
        public string MasterUnitTooltip
        {
            get { return _MasterUnitTooltip; }
            set { Set(ref _MasterUnitTooltip, value); }
        }

        private bool _IsOutRectangle;
        public bool IsOutRectangle
        {
            get { return _IsOutRectangle; }
            set { Set(ref _IsOutRectangle, value); }
        }

        public CellsArmorPutViewModel()
        {
            CellLibPath = string.Empty;
            CellNames = new ObservableCollection<string>();
            UAxisOffset = VAxisOffset = 0;
            MasterUnitTooltip = string.Empty;
            IsOutRectangle = true;
            mc = BM.MessageCenter.Instance;
            cellDgnFile = null;
            celluorMeter = celluor = 0;
            dgnuorMeter = Program.GetActiveDgnModel().GetModelInfo().UorPerMeter;
            //dgnuorMaster = Program.GetActiveDgnModel().GetModelInfo().UorPerMaster;
        }

        private RelayCommand _BrowseCellLib;
        public RelayCommand BrowseCellLib => _BrowseCellLib ?? (_BrowseCellLib = new RelayCommand(ExecuteBrowseCellLib));
        public void ExecuteBrowseCellLib()
        {

            OpenFileDialog cellFileDialog = new OpenFileDialog()
            {
                Filter = Resources.CellLibraryFilter,
                Title = "选择Cell库文件",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            try
            {
                if (cellFileDialog.ShowDialog() == true)
                {
                    BD.DgnDocument cellFileDocument = BD.DgnDocument.CreateForLocalFile(cellFileDialog.FileName);
                    cellDgnFile = BD.DgnFile.Create(cellFileDocument, BD.DgnFileOpenMode.ReadOnly).DgnFile;
                    if (cellDgnFile == null)
                    {
                        mc.ShowErrorMessage(Resources.StatusHeader + Resources.ErrorString, Resources.PromptHeader +
                                                                   $"无法读取{cellFileDialog.FileName}的DgnDocument对象", false);
                        return;
                    }
                    BD.StatusInt loadStatusInt;
                    if (BD.DgnFileStatus.Success != cellDgnFile.LoadDgnFile(out loadStatusInt))
                    {
                        mc.ShowErrorMessage(Resources.StatusHeader + Resources.ErrorString,
                            Resources.PromptHeader + "无法载入文件", false);
                        return;
                    }
                    if (cellDgnFile.FillDictionaryModel() != BD.StatusInt.Success)
                    {
                        mc.ShowErrorMessage(Resources.StatusHeader + Resources.ErrorString,
                            Resources.PromptHeader + "填充模型失败", false);
                        return;
                    }
                    CellNames.Clear();
                    foreach (var modelindex in cellDgnFile.GetModelIndexCollection())
                    {
                        if (modelindex.CellPlacementOptions == BD.CellPlacementOptions.CanBePlacedAsCell)
                            CellNames.Add(modelindex.Name);
                    }
                    string filename;
                    if (CellFunction.AttachLibrary(out filename, cellFileDialog.FileName, "") != BD.StatusInt.Success)
                    {
                        mc.ShowErrorMessage(Resources.StatusHeader + Resources.ErrorString,
                            Resources.PromptHeader + "附加模型失败", false);
                        return;
                    }
                    mc.ShowInfoMessage(Resources.StatusHeader + Resources.SuccessString,
                        Resources.PromptHeader + $"{cellFileDialog.SafeFileName}已载入!", false);
                    CellLibPath = cellDgnFile.GetFileName();
                    SelectCellName = MasterUnitTooltip = string.Empty;
                    UAxisOffset = UAxisOffset = 0;
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
            }

        }

        private RelayCommand _SelectedCellNameChanged;
        public RelayCommand SelectedCellNameChanged => _SelectedCellNameChanged ?? (_SelectedCellNameChanged = new RelayCommand(ExecuteSelectedCellNameChanged));
        public void ExecuteSelectedCellNameChanged()
        {
            if (cellDgnFile == null)
            {
                mc.ShowErrorMessage("未载入任何cell库文件", "未加载cell库文件", false);
                return;
            }
            BD.StatusInt loadStatusInt;
            BD.DgnModel selecteDgnModel = cellDgnFile.LoadRootModelById(out loadStatusInt,
                cellDgnFile.FindModelIdByName(SelectCellName));
            if (BD.StatusInt.Success != selecteDgnModel.FillSections(BD.DgnModelSections.All))
            {
                mc.ShowErrorMessage($"无法填充{selecteDgnModel}模型", "无法填充模型", false);
                return;
            }
            BG.DRange3d selectedModelDRange3D;
            if (selecteDgnModel.GetRange(out selectedModelDRange3D) != BD.StatusInt.Success)
            {
                mc.ShowErrorMessage($"无法获得{selecteDgnModel}模型的范围", "无法获得模型范围", false);
                return;
            }
            MasterUnitTooltip = $"单位：{GetMasterUnit(selecteDgnModel)}";
            celluor = selecteDgnModel.GetModelInfo().UorPerMaster;
            celluorMeter = selecteDgnModel.GetModelInfo().UorPerMeter;
            UAxisOffset = selectedModelDRange3D.XSize / celluor;
            VAxisOffset = selectedModelDRange3D.YSize / celluor;
        }

        private RelayCommand _PutArmor;
        public RelayCommand PutArmor => _PutArmor ?? (_PutArmor = new RelayCommand(ExecutePutArmor, CanExecutePutArmor));
        public void ExecutePutArmor()
        {
            double tempuor = celluor * dgnuorMeter / celluorMeter;
            PutArmorTool.InstallNewInstance(SelectCellName, UAxisOffset * tempuor, VAxisOffset * tempuor, IsOutRectangle);
        }

        public bool CanExecutePutArmor()
        {
            return !string.IsNullOrEmpty(SelectCellName);
        }

        private string GetMasterUnit(BD.DgnModel inModel)
        {
            var activeModelInfo = inModel.GetModelInfo();
            double UorPerMeter = activeModelInfo.UorPerMeter;
            double UorPerMaster = activeModelInfo.UorPerMaster;
            double TOLERANCE = 1e-8;
            if (Math.Abs(UorPerMaster - UorPerMeter) < TOLERANCE)
                return "m";
            else if (Math.Abs(UorPerMaster - UorPerMeter / 10) < TOLERANCE)
                return "dm";
            else if (Math.Abs(UorPerMaster - UorPerMeter / 100) < TOLERANCE)
                return "cm";
            else if (Math.Abs(UorPerMaster - UorPerMeter / 1000) < TOLERANCE)
                return "mm";
            else if (Math.Abs(UorPerMaster - UorPerMeter * 1e-6) < TOLERANCE)
                return "um";
            else if (Math.Abs(UorPerMaster - UorPerMeter * 1000) < TOLERANCE)
                return "km";
            else
                return "Unknown";
        }

        private double celluor;
        private double celluorMeter;
        private double dgnuorMeter;
        //private double dgnuorMaster;
        private BM.MessageCenter mc;
        private BD.DgnFile cellDgnFile;
    }
}