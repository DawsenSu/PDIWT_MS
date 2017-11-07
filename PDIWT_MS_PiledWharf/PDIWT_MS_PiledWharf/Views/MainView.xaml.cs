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
using BMWPF = Bentley.MstnPlatformNET.WPF;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;

namespace PDIWT_MS_PiledWharf.Views
{
    /// <summary>
    /// Interaction logic for ViewMain.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private MainView()
        {
            InitializeComponent();
            var locator = new ViewModels.ViewModelLocator();
            this.DataContext = locator.MainVM;
            m_wpfHelper = new BMWPF.WPFInteropHelper(this);
            m_wpfHelper.Attach(Program.Addin, true, "MainView");

            //ObjectDataProvider _soilinfo_odp = new ObjectDataProvider();
            //_soilinfo_odp.ObjectInstance = this.listbox_piles.SelectedItem;
            //_soilinfo_odp.MethodName = "GetPilePieceInEachSoilLayerInfos";
            //Binding _binding_selecteditem_to_soilinfo = new Binding(".")
            //{
            //    Source = _soilinfo_odp,
            //};

            //BindingOperations.SetBinding(this.datagrid_soilinfo, DataGrid.ItemsSourceProperty, _binding_selecteditem_to_soilinfo);
        }

        static MainView m_windowhost;
        BMWPF.WPFInteropHelper m_wpfHelper;

        public static void ShowWindow()
        {
            if (m_windowhost != null)
            {
                m_windowhost.Focus();
                return;
            }
            m_windowhost = new MainView
            {
                Icon = new BitmapImage(new Uri("pack://application:,,,/PDIWT_MS_PiledWharf;component/Resources/Icons/SoilLayer.ico", UriKind.RelativeOrAbsolute))
            };
            m_windowhost.Show();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            m_wpfHelper.Detach();
            m_wpfHelper.Dispose();
            m_windowhost = null;
        }

    }
}
