using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PDIWT_MS.Tools.View
{
    /// <summary>
    /// Interaction logic for View1.xaml
    /// </summary>
    public partial class QuickInsertView : UserControl
    {
        public QuickInsertView()
        {
            InitializeComponent();
            DataContext = new PDIWT_MS.Tools.ViewModels.QuickInsertViewModel();
        }

        private void TableView_ValidateRow(object sender, DevExpress.Xpf.Grid.GridRowValidationEventArgs e)
        {
            if (e.Row == null) return;
            if (e.RowHandle == DevExpress.Xpf.Grid.GridControl.NewItemRowHandle)
            {
                e.IsValid =true;
                e.Handled = true;
            }
        }

        private void TableView_InvalidRowException(object sender, DevExpress.Xpf.Grid.InvalidRowExceptionEventArgs e)
        {
            if (e.RowHandle == DevExpress.Xpf.Grid.GridControl.NewItemRowHandle)
            {
                e.ErrorText = "请输入插入单元名称!";
                e.WindowCaption = "未输入单元名称";
            }
        }

        private void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            double AngelValue;
            if (e.Value==null)
            {
                e.IsValid = false;
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = "不能为空";
                return;
            }
            if(!double.TryParse(e.Value.ToString(),out AngelValue))
            {
                e.IsValid = false;
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = "Angel必须为数字";
                return;
            }
            if (AngelValue >360 || AngelValue <-360)
            {
                e.IsValid = false;
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = "Angel的值必须在[-360,360]之间";
                e.Handled = true;
            }
        }

        private void GridColumn_Validate_XYZ(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            double XYZ;
            if (e.Value == null)
            {
                e.IsValid = false;
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = "不能为空";
                return;
            }
            if (!double.TryParse(e.Value.ToString(), out XYZ))
            {
                e.IsValid = false;
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = "XYZ坐标必须为数字";
                e.Handled = true;
            }
        }
    }
}
