using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTest.Model
{
    class Model : NotificationObject
    {
        private string _wpf = "WPF";

        public string WPF
        {
            get { return _wpf; }
            set
            {
                if (_wpf != value)
                {
                    _wpf = value;
                    OnPropertyChanged("WPF");
                }
            }
        }

        public void Copy(object obj)
        {
            WPF += " WPF";
        }
    }
}
