using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;

using BDEC = Bentley.DgnPlatformNET.DgnEC;

namespace PDIWT_MS_Tool.ViewModels
{
    using Extension;
    public class SchemaListViewModel : ViewModelBase
    {
        public SchemaListViewModel()
        {
            SchemaList = new ObservableCollection<string>();

        }

        public ObservableCollection<string> SchemaList
        {
            get { return GetProperty(() => SchemaList); }
            set { SetProperty(() => SchemaList, value); }
        }
        [Command]
        public void FindSchema()
        {
            BDEC.DgnECManager manager = BDEC.DgnECManager.Manager;
            SchemaList = manager.DiscoverSchemasForModel(Program.GetActiveDgnModel(), Bentley.DgnPlatformNET.DgnEC.ReferencedModelScopeOption.All, false).ToObservableCollection();
        }

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
        }
    }
}