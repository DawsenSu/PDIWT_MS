using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;

namespace PDIWT_MS_Tool.ViewModels
{
    public class FindInstancesOnElementViewModel : ViewModelBase
    {
        public string WindowTitle
        {
            get { return GetProperty(() => WindowTitle); }
            set { SetProperty(() => WindowTitle, value); }
        }
        public ObservableCollection<Instance> InstancesInfo
        {
            get { return GetProperty(() => InstancesInfo); }
            set { SetProperty(() => InstancesInfo, value); }
        }
        public FindInstancesOnElementViewModel()
        {
            WindowTitle = string.Empty;
            InstancesInfo = new ObservableCollection<Instance>();
        }
    }
}