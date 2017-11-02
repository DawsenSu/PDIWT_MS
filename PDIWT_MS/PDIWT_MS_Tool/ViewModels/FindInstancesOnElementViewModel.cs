using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace PDIWT_MS_Tool.ViewModels
{
    public class FindInstancesOnElementViewModel : ViewModelBase
    {
        private string _WindowTitle;
        public string WindowTitle
        {
            get { return _WindowTitle; }
            set { Set(ref _WindowTitle, value); }
        }

        private ObservableCollection<Instance> _InstancesInfo;
        public ObservableCollection<Instance> InstancesInfo
        {
            get { return _InstancesInfo; }
            set { Set(ref _InstancesInfo, value); }
        }
        public FindInstancesOnElementViewModel()
        {
            WindowTitle = string.Empty;
            InstancesInfo = new ObservableCollection<Instance>();
        }
    }
}