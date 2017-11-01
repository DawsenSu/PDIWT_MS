using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using OfficeOpenXml;
using BM = Bentley.MstnPlatformNET;
using PDIWT_MS_Tool.Models;
using PDIWT_MS_Tool.Properties;
namespace PDIWT_MS_Tool.ViewModels
{
    public class ModelCreatorViewModel : ViewModelBase
    {
        private string _LoadInfo;
        public string LoadInfo
        {
            get { return _LoadInfo; }
            set { Set(ref _LoadInfo, value); }
        }

        private string _OutputInfo;
        public string OutputInfo
        {
            get { return _OutputInfo; }
            set { Set(ref _OutputInfo, value); }
        }



        private BM.MessageCenter mc;
        private List<DgnModelInfo> loadedDgnModelInfos;

        public ModelCreatorViewModel()
        {
            LoadInfo = OutputInfo = string.Empty;
            mc = BM.MessageCenter.Instance;
            loadedDgnModelInfos = new List<DgnModelInfo>();
        }

        private RelayCommand _LoadExcel;
        public RelayCommand LoadExcel => _LoadExcel ?? (_LoadExcel = new RelayCommand(ExecuteLoadExcel));
        public void ExecuteLoadExcel()
        {
            OpenFileDialog openExcelFileDialog = new OpenFileDialog()
            {
                Filter = Resources.ExcelFilter,
                Title = "选择输入Execel文件"
            };
            try
            {
                if (openExcelFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var excelFilePath = openExcelFileDialog.FileName;
                    DgnModelCreator modelCreator = new DgnModelCreator();
                    loadedDgnModelInfos.Clear();
                    loadedDgnModelInfos = modelCreator.ReadModelInfoFromExcel(excelFilePath);
                    LoadInfo = openExcelFileDialog.SafeFileName + "载入成功";
                    mc.ShowInfoMessage(LoadInfo,LoadInfo,false);
                }
            }
            catch (Exception e)
            {
                mc.ShowErrorMessage(e.Message,e.Message,true);
            }
        }

        private RelayCommand _CreateModels;
        public RelayCommand CreateModels => _CreateModels ?? (_CreateModels = new RelayCommand(ExecuteCreateModels, CanExecuteCreateModels));
        public void ExecuteCreateModels()
        {
            try
            {
                DgnModelCreator modelCreator = new DgnModelCreator();
                int successAddModelsNum = modelCreator.CreateModels(loadedDgnModelInfos);
                mc.ShowInfoMessage($"成功添加{successAddModelsNum}个模型",$"成功添加{successAddModelsNum}个模型",true);
            }
            catch (Exception e)
            {
                mc.ShowErrorMessage(e.Message,e.Message,true);
            }
        }

        public bool CanExecuteCreateModels()
        {
            return loadedDgnModelInfos.Count != 0;
        }

        private RelayCommand _OutputExcel;
        public RelayCommand OutputExcel => _OutputExcel ?? (_OutputExcel = new RelayCommand(ExecuteOutputExcel));
        public void ExecuteOutputExcel()
        {
            SaveFileDialog sfDialog = new SaveFileDialog() {Filter = Resources.ExcelFilter, Title = "要保存模型信息的Excel文件"};
            try
            {
                if (sfDialog.ShowDialog() == DialogResult.OK)
                {
                    if(File.Exists(sfDialog.FileName)) File.Delete(sfDialog.FileName);
                    DgnModelCreator dgnModelCreator = new DgnModelCreator();
                    dgnModelCreator.OutputModelInfo(sfDialog.FileName);
                    OutputInfo = sfDialog.FileName + "成功";
                }
            }
            catch (Exception e)
            {
                mc.ShowErrorMessage(e.Message, e.Message, true);
                throw;
            }
        }

    }
}