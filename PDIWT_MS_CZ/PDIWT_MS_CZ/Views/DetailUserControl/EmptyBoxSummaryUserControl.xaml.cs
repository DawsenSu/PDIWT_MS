using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using PDIWT_MS_CZ.Models;

namespace PDIWT_MS_CZ.Views.DetailUserControl
{
    /// <summary>
    /// EmptyBoxSummaryUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class EmptyBoxSummaryUserControl : UserControl
    {
        public EmptyBoxSummaryUserControl()
        {
            InitializeComponent();
        }

        private void RectEmptyBox_ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            RectEmptyBoxWindow rectEmptyBoxWindow = new RectEmptyBoxWindow()
            {
                DataContext = ((Button)sender).DataContext
            };
            rectEmptyBoxWindow.ShowDialog();
        }

        private void ZPlanEmptyBox_ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ZPlanEmptyBoxWindow zPlanEmptyBoxWindow = new ZPlanEmptyBoxWindow()
            {
                DataContext = ((Button) sender).DataContext
            };
            zPlanEmptyBoxWindow.ShowDialog();
        }

        private void AppendRectEmptyBox_ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var temp = new RectEmptyBox()
            {
                ChamferInfos = new ObservableCollection<EmptyBoxEdgeChameferInfo>()
                {
                    new EmptyBoxEdgeChameferInfo()
                    {
                        IsChamfered = true,
                        EdgeIndicator = 0
                    },
                    new EmptyBoxEdgeChameferInfo()
                    {
                        EdgeIndicator = 1,
                        IsChamfered = true,
                    },
                    new EmptyBoxEdgeChameferInfo()
                    {
                        EdgeIndicator = 2,
                        IsChamfered = false,
                    },
                    new EmptyBoxEdgeChameferInfo()
                    {
                        IsChamfered = false,
                        EdgeIndicator = 3
                    },
                    new EmptyBoxEdgeChameferInfo()
                    {
                        IsChamfered = true,
                        EdgeIndicator = 4

                    },
                    new EmptyBoxEdgeChameferInfo()
                    {
                        IsChamfered = false,
                        EdgeIndicator = 5

                    },
                    new EmptyBoxEdgeChameferInfo()
                    {
                        IsChamfered = false,
                        EdgeIndicator = 6

                    },
                    new EmptyBoxEdgeChameferInfo()
                    {
                        IsChamfered = true,
                        EdgeIndicator = 7

                    },
                    new EmptyBoxEdgeChameferInfo()
                    {
                        IsChamfered = true,
                        EdgeIndicator = 8

                    },
                    new EmptyBoxEdgeChameferInfo()
                    {
                        IsChamfered = true,
                        EdgeIndicator = 9

                    },
                    new EmptyBoxEdgeChameferInfo()
                    {
                        IsChamfered = true,
                        EdgeIndicator = 10

                    },
                    new EmptyBoxEdgeChameferInfo()
                    {
                        IsChamfered = true,
                        EdgeIndicator = 11
                    }
                }
            };
            ((LockHeadParameters)EmptyBoxSummaryUC.DataContext).LH_EmptyRectBoxs.Add(temp);
        }

        private void AppendZPlanEmptyBox_ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ((LockHeadParameters)EmptyBoxSummaryUC.DataContext).LH_EmptyZPlanBoxs.Add(new ZPlanEmptyBox()
            {
                ZPlanInfos = new ObservableCollection<ZPlanInfo>()
                {
                    new ZPlanInfo()
                    {
                        X=0,Y=0, BoxEdgeChamferInfo =  new EmptyBoxEdgeChameferInfo()
                    }
                }
            });
        }

        private void DeleteZPlanEmptyBox_ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (ListBox_ZPlanEmpty.SelectedItem != null)
            {

                ((LockHeadParameters) EmptyBoxSummaryUC.DataContext).LH_EmptyZPlanBoxs.Remove(
                    (ZPlanEmptyBox)ListBox_ZPlanEmpty.SelectedItem);
            }
        }

        private void DeleteRectEmptyBox_ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (ListBox_RectEmpty.SelectedItem != null)
            {
                ((LockHeadParameters)EmptyBoxSummaryUC.DataContext).LH_EmptyRectBoxs.Remove(
                    (RectEmptyBox)ListBox_RectEmpty.SelectedItem);
            }
        }
    }
}
