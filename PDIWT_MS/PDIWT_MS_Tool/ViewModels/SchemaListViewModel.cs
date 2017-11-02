using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

using BDEC = Bentley.DgnPlatformNET.DgnEC;

namespace PDIWT_MS_Tool.ViewModels
{
    using Extension;
    public class SchemaListViewModel : ViewModelBase
    {
        public SchemaListViewModel()
        {
            _SchemaList = new ObservableCollection<string>();
        }

        private ObservableCollection<string> _SchemaList;
        public ObservableCollection<string> SchemaList
        {
            get { return _SchemaList; }
            set { Set(ref _SchemaList, value); }
        }

        private RelayCommand _FindSchema;
        public RelayCommand FindSchema => _FindSchema ?? (_FindSchema = new RelayCommand(ExecuteFindSchema));
        public void ExecuteFindSchema()
        {
            BDEC.DgnECManager manager = BDEC.DgnECManager.Manager;
            SchemaList = manager.DiscoverSchemasForModel(Program.GetActiveDgnModel(), BDEC.ReferencedModelScopeOption.All, false).ToObservableCollection();
        }
    }
}