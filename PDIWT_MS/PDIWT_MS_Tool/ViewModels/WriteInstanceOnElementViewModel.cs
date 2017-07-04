using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;

using BDE = Bentley.DgnPlatformNET.DgnEC;
using BES = Bentley.ECObjects.Schema;

namespace PDIWT_MS_Tool.ViewModels
{
    public class WriteInstanceOnElementViewModel : ViewModelBase
    {
        public ObservableCollection<string> ImportedSchemas
        {
            get { return GetProperty(() => ImportedSchemas); }
            set { SetProperty(() => ImportedSchemas, value); }
        }
        public string SelectedSchema
        {
            get { return GetProperty(() => SelectedSchema); }
            set { SetProperty(() => SelectedSchema, value); }
        }
        public ObservableCollection<string> ImportedClasses
        {
            get { return GetProperty(() => ImportedClasses); }
            set { SetProperty(() => ImportedClasses, value); }
        }
        public string SelectedClass
        {
            get { return GetProperty(() => SelectedClass); }
            set { SetProperty(() => SelectedClass, value); }
        }
        public ObservableCollection<PropertyToWrite> PropsToWrite
        {
            get { return GetProperty(() => PropsToWrite); }
            set { SetProperty(() => PropsToWrite, value); }
        }

        [Command]
        public void SchemaSelectionChanged()
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
        [Command]
        public void ClassSelectionChanged()
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
        protected override void OnInitializeInRuntime()
        {
            var schemalistviewmodel = new SchemaListViewModel();
            schemalistviewmodel.FindSchema();
            ImportedSchemas = schemalistviewmodel.SchemaList;
            ImportedClasses = new ObservableCollection<string>();
            PropsToWrite = new ObservableCollection<PropertyToWrite>();
            base.OnInitializeInRuntime();

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