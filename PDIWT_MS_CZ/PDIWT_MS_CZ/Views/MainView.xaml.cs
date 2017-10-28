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
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using BMWPF = Bentley.MstnPlatformNET.WPF;

namespace PDIWT_MS_CZ.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
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
            this.AddHandler(System.Windows.Controls.Primitives.TextBoxBase.TextChangedEvent, new TextChangedEventHandler(TextBoxes_Changed), true);
            this.AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, new RoutedEventHandler(Button_click), true);
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
            m_windowhost = new MainView()
            {
                Icon =
                    new BitmapImage(
                        new Uri("pack://application:,,,/PDIWT_MS_CZ;component/Image/Icons/Wharf.ico",
                            UriKind.RelativeOrAbsolute))
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

        private void TextBoxes_Changed(object sender, TextChangedEventArgs e) => SendChangedMessage();

        private void Button_click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is CheckBox)
                SendChangedMessage();
        }

        private void SendChangedMessage()
        {
            Messenger.Default.Send<bool>(true, "ParameterChanged");
        }
    }
}
