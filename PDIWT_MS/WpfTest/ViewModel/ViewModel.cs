using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTest.ViewModel
{
    class ViewModel
    {
        public DelegateCommand CopyCmd { get; set; }
        public Model.Model Model { get; set; }

        public ViewModel()
        {
            Model = new WpfTest.Model.Model();
            CopyCmd = new DelegateCommand();
            //关键点，命令模式的应用
            CopyCmd.ExecuteCommand = new Action<object>(Model.Copy);
        }
    }
}
