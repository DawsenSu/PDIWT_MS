using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using OfficeOpenXml;
using BM = Bentley.MstnPlatformNET;
using PDIWT_MS_Tool.Models;
using PDIWT_MS_Tool.Properties;
namespace PDIWT_MS_Tool.ViewModels
{
    public class ModelCreatorViewModel : ViewModelBase
    {
        public string LoadInfo
        {
            get { return GetProperty(() => LoadInfo); }
            set { SetProperty(() => LoadInfo, value); }
        }
        public string OutputInfo
        {
            get { return GetProperty(() => OutputInfo); }
            set { SetProperty(() => OutputInfo, value); }
        }


        private BM.MessageCenter mc;
        private List<DgnModelInfo> loadedDgnModelInfos;

        protected override void OnInitializeInRuntime()
        {
            LoadInfo = OutputInfo = string.Empty;
            mc = BM.MessageCenter.Instance;
            loadedDgnModelInfos = new List<DgnModelInfo>();
            base.OnInitializeInRuntime();
        }

        [Command]
        public void LoadExcel()
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

        [Command]
        public void CreateModels()
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

        public bool CanCreateModels()
        {
            return loadedDgnModelInfos.Count != 0;
        }

        [Command]
        public void OutputExcel()
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