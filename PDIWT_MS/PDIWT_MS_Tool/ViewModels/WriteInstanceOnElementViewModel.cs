using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

using BDE = Bentley.DgnPlatformNET.DgnEC;
using BES = Bentley.ECObjects.Schema;

namespace PDIWT_MS_Tool.ViewModels
{
    public class WriteInstanceOnElementViewModel : ViewModelBase
    {
        public  WriteInstanceOnElementViewModel()
        {
            var schemalistviewmodel = new SchemaListViewModel();
            schemalistviewmodel.ExecuteFindSchema();
            ImportedSchemas = schemalistviewmodel.SchemaList;
            ImportedClasses = new ObservableCollection<string>();
            PropsToWrite = new ObservableCollection<PropertyToWrite>();
        }

        private ObservableCollection<string> _ImportedSchemas;
        public ObservableCollection<string> ImportedSchemas
        {
            get { return _ImportedSchemas; }
            set { Set(ref _ImportedSchemas, value); }
        }

        private string _SelectedSchema;
        public string SelectedSchema
        {
            get { return _SelectedSchema; }
            set { Set(ref _SelectedSchema, value); }
        }
        private ObservableCollection<string> _ImportedClasses;
        public ObservableCollection<string> ImportedClasses
        {
            get { return _ImportedClasses; }
            set { Set(ref _ImportedClasses, value); }
        }
        private string _SelectedClass;
        public string SelectedClass
        {
            get { return _SelectedClass; }
            set { Set(ref _SelectedClass, value); }
        }

        private ObservableCollection<PropertyToWrite> _PropsToWrite;
        public ObservableCollection<PropertyToWrite> PropsToWrite
        {
            get { return _PropsToWrite; }
            set { Set(ref _PropsToWrite, value); }
        }

        private RelayCommand _SchemaSelectionChanged;
        public RelayCommand SchemaSelectionChanged => _SchemaSelectionChanged ?? (_SchemaSelectionChanged = new RelayCommand(ExecuteSchemaSelectionChanged));
        public void ExecuteSchemaSelectionChanged()
        {
            ImportedClasses = new ObservableCollection<string>();
            SelectedClass = string.Empty;
            string schemaName;
            int majornum, minornum;
            if (!Extension.Utilities.TryParseSchemaString(SelectedSchema, out schemaName, out majornum, out minornum))
                return;

            BDE.FindInstancesScope scope = BDE.FindInstancesScope.CreateScope(Program.GetActiveDgnFile(), new BDE.FindInstancesScopeOption());
            BES.IECSchema schema = BDE.DgnECManager.Manager.LocateSchemaInScope(scope, schemaName, majornum, minornum, BES.SchemaMatchType.Exact);
            if (schema == null)
                return;
            foreach (BES.IECClass ecClass in schema.GetClasses())
            {
                ImportedClasses.Add(ecClass.Name);
            }
        }
        private RelayCommand _ClassSelectionChanged;
        public RelayCommand ClassSelectionChanged => _ClassSelectionChanged ?? (_ClassSelectionChanged = new RelayCommand(ExecuteClassSelectionChanged));
        public void ExecuteClassSelectionChanged()
        {
            PropsToWrite.Clear();
            if (string.IsNullOrEmpty(SelectedClass))
                return;
            string schemaName;
            int marjornum, minornum;
            if (!Extension.Utilities.TryParseSchemaString(SelectedSchema, out schemaName, out marjornum, out minornum))
                return;
            BDE.FindInstancesScope scope = BDE.FindInstancesScope.CreateScope(Program.GetActiveDgnFile(), new BDE.FindInstancesScopeOption());
            BES.IECSchema schema = BDE.DgnECManager.Manager.LocateSchemaInScope(scope, schemaName, marjornum, minornum, BES.SchemaMatchType.Exact);
            BES.IECClass ecClass = schema.GetClass(SelectedClass);

            foreach (BES.IECProperty ecProp in ecClass.Properties(false))
            {
                PropsToWrite.Add(new PropertyToWrite { PropertyName = ecProp.Name, PropertyType = ecProp.Type.Name, PropertyValue = "" });
            }
        }

        public InstanceToWrite GetInstanceToWrite()
        {
            string schemaName;
            int majornum, minornum;
            if (!Extension.Utilities.TryParseSchemaString(SelectedSchema, out schemaName, out majornum, out minornum))
                throw new InvalidOperationException($"{SelectedSchema}无法转化");
            return new InstanceToWrite() { SchemaName = schemaName, MajorVersion = majornum, MinorVersion = minornum, ClassName = SelectedClass, Properties = PropsToWrite.ToList() };
        }

    }
    public class PropertyToWrite
    {
        public string PropertyName { get; set; }
        public string PropertyType { get; set; }
        public string PropertyValue { get; set; }
        public string GetValueAsString()
        {
            if (null == PropertyValue)
                return string.Empty;
            return PropertyValue;
        }
        public bool GetValueAsBoolean()
        {
            bool outbool;
            if (!bool.TryParse(PropertyValue, out outbool))
                return false;
            else
                return outbool;
        }
        public int GetValueAsInt()
        {
            int outint;
            if (!int.TryParse(PropertyValue, out outint))
                return 0;
            else
                return outint;
        }
        public double GetValueAsDouble()
        {
            double outdouble;
            if (!double.TryParse(PropertyValue, out outdouble))
            {
                return 0.0;
            }
            else
                return outdouble;
        }
    }
    public class InstanceToWrite
    {
        public string SchemaName { get; set; }
        public int MajorVersion { get; set; }
        public int MinorVersion { get; set; }
        public string ClassName { get; set; }
        public List<PropertyToWrite> Properties { get; set; }
        public InstanceToWrite()
        {
            Properties = new List<PropertyToWrite>();
        }
    }
}