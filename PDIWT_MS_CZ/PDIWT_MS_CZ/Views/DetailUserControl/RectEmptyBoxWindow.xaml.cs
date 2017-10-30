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
using GalaSoft.MvvmLight.Messaging;

namespace PDIWT_MS_CZ.Views.DetailUserControl
{
    /// <summary>
    /// EmptyRectBoxUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class RectEmptyBoxWindow : Window
    {
        public RectEmptyBoxWindow()
        {
            InitializeComponent();
            this.AddHandler(System.Windows.Controls.Primitives.TextBoxBase.TextChangedEvent, new TextChangedEventHandler(TextBoxes_Changed), true);
            this.AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, new RoutedEventHandler(Button_click), true);
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
