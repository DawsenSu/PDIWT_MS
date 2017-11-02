using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

using Bentley.DgnPlatformNET.DgnEC;
using Bentley.EC.Persistence.Query;
using Bentley.ECObjects.Instance;
using Bentley.ECObjects.Schema;


namespace PDIWT_MS_Tool.ViewModels
{
    public class FindnstancesViewModel : ViewModelBase
    {
        public FindnstancesViewModel()
        {
            _Instances = new ObservableCollection<Instance>();

        }

        private ObservableCollection<Instance> _Instances;
        public ObservableCollection<Instance> Instances
        {
            get { return _Instances; }
            set { Set(ref _Instances, value); }
        }

        private RelayCommand _FindInstances;
        public RelayCommand FindInstances => _FindInstances ?? (_FindInstances = new RelayCommand(ExecuteFindInstances));
        public void ExecuteFindInstances()
        {
            DgnECManager manager = DgnECManager.Manager;
            FindInstancesScope scope = FindInstancesScope.CreateScope(Program.GetActiveDgnFile(), new FindInstancesScopeOption(DgnECHostType.Element, true));
            ECQuery query = new ECQuery(Extension.Utilities.GetActiveModelAllClasses());
            query.SelectClause.SelectAllProperties = true;
            manager.FindInstances(scope, query);
            //每次寻找实例不对啊，需要打开Property窗口后才能正确显示
            using (DgnECInstanceCollection ecInstances = manager.FindInstances(scope, query))
            {
                foreach (var ecin in ecInstances)
                {
                    foreach (IECProperty itemProp in ecin.ClassDefinition.Properties(false))
                    {
                        var instanceitem = new Instance();
                        instanceitem.ClassName = ecin.ClassDefinition.DisplayLabel;
                        instanceitem.PropertyName = itemProp.Name;
                        string type = itemProp.Type.Name.ToLower();
                        instanceitem.TypeName = type;
                        IECPropertyValue propvalue = ecin.GetPropertyValue(itemProp.Name);

                        switch (type)
                        {
                            case "string":
                                instanceitem.PropertyValue = (propvalue != null) ? propvalue.StringValue : "";
                                break;
                            case "boolean":
                                instanceitem.PropertyValue = (propvalue != null) ? propvalue.StringValue : "";
                                break;
                            case "int":
                                instanceitem.PropertyValue = (propvalue != null) ? propvalue.IntValue.ToString() : "";
                                break;
                            case "double":
                                instanceitem.PropertyValue = (propvalue != null) ? propvalue.DoubleValue.ToString() : "";
                                break;
                        }
                        Instances.Add(instanceitem);
                    }
                }
            }
        }

        //IECClass[] GetSearchClasses()
        //{
        //    List<IECClass> classes = new List<IECClass>();
        //    FindInstancesScope scope = FindInstancesScope.CreateScope(Program.GetActiveDgnFile(), new FindInstancesScopeOption());
        //    //ECSchema echmea = new ECSchema();
        //    foreach (string schemastring in DgnECManager.Manager.DiscoverSchemasForModel(Program.GetActiveDgnModel(), ReferencedModelScopeOption.DirectOnly, false))
        //    {
        //        string schemaName;
        //        int majornum, minornum;
        //        if (!Extension.Utilities.TryParseSchemaString(schemastring, out schemaName, out majornum, out minornum))
        //            throw new InvalidOperationException($"{schemastring}不能转换");
        //        IECSchema schema = DgnECManager.Manager.LocateSchemaInScope(scope, schemaName, majornum, minornum, SchemaMatchType.Exact);
        //        var ecclass = schema?.GetClasses();
        //        if (ecclass != null)
        //            classes.AddRange(ecclass);
        //    }
        //    return classes.ToArray();
        //}

    }
    public class Instance
    {
        public string ClassName { get; set; }
        public string PropertyName { get; set; }
        public string TypeName { get; set; }
        public string PropertyValue { get; set; }
    }
}