using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using OfficeOpenXml;

using PDIWT_MS.Marshal;

using BM = Bentley.MstnPlatformNET;
using BD = Bentley.DgnPlatformNET;
using BG = Bentley.GeometryNET;

namespace PDIWT_MS.Tools
{
    /// <summary>
    /// GetQuantitiesUC.xaml 的交互逻辑
    /// </summary>
    public partial class GetQuantitiesUC : UserControl
    {
        private static BM.WPF.DockableWindow currentControl;
        private static BM.AddIn m_addIn;

        public GetQuantitiesUC(BM.AddIn addIn)
        {
            m_addIn = addIn;
            InitializeComponent();
        }

        internal static void ShowWindow(BM.AddIn addIn)
        {
            if (null != currentControl)
            {
                currentControl.Focus();
                return;
            }

            currentControl = new BM.WPF.DockableWindow();
            currentControl.Attach(addIn, "wpfControl", new System.Drawing.Size(150, 240));
            currentControl.Content = new GetQuantitiesUC(addIn);
            //不能停靠
            currentControl.WindowContent.CanDockHorizontally = false;
            currentControl.WindowContent.CanDockVertically = false;
            currentControl.WindowContent.CanDockInCenter = false;
            currentControl.Title = "GetQuantities";
            currentControl.Show();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            currentControl = null;
        }

        private void bt_output_Click(object sender, RoutedEventArgs e)
        {
            var sfd = new System.Windows.Forms.SaveFileDialog();
            sfd.Title = "输出工程量";
            sfd.Filter = "Excel 文件|*.xlsx";
            sfd.DefaultExt = "xlsx";
            sfd.FileName = "工程量文件.xlsx";
            if (System.Windows.Forms.DialogResult.OK == sfd.ShowDialog())
            {
                //删除存在的文件名
                if (System.IO.File.Exists(sfd.FileName))
                    System.IO.File.Delete(sfd.FileName);


                using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(sfd.FileName)))
                {
                    ExcelWorksheet sheet = package.Workbook.Worksheets.Add("工程量表");
                    sheet.Cells[1, 1, 1, 2].Style.Font.Bold = true;
                    sheet.Cells[1, 1, 1, 2].AutoFitColumns();
                    sheet.Cells[1, 1].Value = "图层名称";
                    sheet.Cells[1, 2].Value = "工程量(m^3)";
                    int row = 2;
                    foreach (var r in result)
                    {
                        sheet.Cells[row, 1].Value = r.LevelName;
                        sheet.Cells[row, 2].Value = Math.Round(r.Quantities, 2);
                        row++;
                    }
                    package.Save();
                }

                MessageBox.Show("工程量文件存放至\n" + sfd.FileName, "工程量输出成功");
            }
            else
            {
                MessageBox.Show("未选择输出文件，输出工程量失败！", "未输出工程量");
            }
        }

        private void tb_Calculation_Click(object sender, RoutedEventArgs e)
        {
            // 需要传入的参数
            double volume, area, closureError, ixy, ixz, iyz;
            BG.DPoint3d centroid, moment, principalMoment, pricipalDirection;
            IntPtr nativeEdp;

            BD.DgnModel activeModel = Program.GetActiveDgnModel();
            double uor_per_meter = activeModel.GetModelInfo().UorPerMeter;
            IntPtr modelPtr = activeModel.GetNative();

            BD.LevelCache levelCache = activeModel.GetLevelCache();
            //存储每层图层对应的工程量
            Dictionary<string, double> volumeByLevel = new Dictionary<string, double>();
            string eleLevelName;

            foreach (var ele in activeModel.GetGraphicElements())
            {
                if (0 != BentleyMarshal.mdlElmdscr_getByElemRef(out nativeEdp, ele.GetNativeElementRef(), modelPtr, 0, IntPtr.Zero))
                {
                    if (0 == BentleyMarshal.mdlMeasure_volumeProperties(out volume, out area, out closureError, out centroid, out moment, out ixy, out ixz, out iyz, out principalMoment, out pricipalDirection, nativeEdp, uor_per_meter / 10000))
                    {
                        eleLevelName = levelCache.GetLevel(ele.LevelId).Name;
                        if (!volumeByLevel.ContainsKey(eleLevelName))
                        {
                            volumeByLevel.Add(eleLevelName, 0);
                        }
                        volumeByLevel[eleLevelName] += volume / Math.Pow(uor_per_meter, 3);
                    }
                }
            }

            foreach (var i in volumeByLevel)
            {
                result.Add(new QuantitiesByLevelResult() { LevelName = i.Key, Quantities = Math.Round(i.Value, 2) });
            }

            lv_output.ItemsSource = result;
        }

        private List<QuantitiesByLevelResult> result = new List<QuantitiesByLevelResult>();
    }

    class QuantitiesByLevelResult : INotifyPropertyChanged
    {
        private string levelName = string.Empty;
        public string LevelName
        {
            get
            {
                return this.levelName;
            }
            set
            {
                if (this.levelName != value)
                {
                    this.levelName = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private double quantities = 0;
        public double Quantities
        {
            get
            {
                return this.quantities;
            }
            set
            {
                if (this.quantities != value)
                {
                    this.quantities = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
