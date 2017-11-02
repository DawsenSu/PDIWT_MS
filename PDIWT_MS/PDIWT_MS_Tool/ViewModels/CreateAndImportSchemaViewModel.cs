using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

using BDEC = Bentley.DgnPlatformNET.DgnEC;
using BD = Bentley.DgnPlatformNET;
using Bentley.ECObjects;
using Bentley.ECObjects.Schema;
using Bentley.ECObjects.XML;
using System.Linq;

namespace PDIWT_MS_Tool.ViewModels
{
    public class CreateAndImportSchemaViewModel : ViewModelBase
    {
        public CreateAndImportSchemaViewModel()
        {
            SchemaTreeViewItems = new ObservableCollection<SchemaNode>();
            PropertyTypes = new ObservableCollection<string>() { "String", "Boolean", "Integer", "Double" };
            SelectedPropertyType = PropertyTypes.First();
        }
        private ObservableCollection<SchemaNode> _SchemaTreeViewItems;
        public ObservableCollection<SchemaNode> SchemaTreeViewItems
        {
            get { return _SchemaTreeViewItems; }
            set { Set(ref _SchemaTreeViewItems, value); }
        }

        private string _SchemaName;
        public string SchemaName
        {
            get { return _SchemaName; }
            set { Set(ref _SchemaName, value); }
        }

        private string _ClassName;
        public string ClassName
        {
            get { return _ClassName; }
            set { Set(ref _ClassName, value); }
        }

        private string _PropertyName;
        public string PropertyName
        {
            get { return _PropertyName; }
            set { Set(ref _PropertyName, value); }
        }

        private string _SelectedPropertyType;
        public string SelectedPropertyType
        {
            get { return _SelectedPropertyType; }
            set { Set(ref _SelectedPropertyType, value); }
        }

        private ObservableCollection<string> _PropertyTypes;
        public ObservableCollection<string> PropertyTypes
        {
            get { return _PropertyTypes; }
            set { Set(ref _PropertyTypes, value); }
        }

        private RelayCommand _CreateSchema;
        public RelayCommand CreateSchema => _CreateSchema ?? (_CreateSchema = new RelayCommand(ExecuteCreateSchema,()=> !string.IsNullOrEmpty(SchemaName)));
        public void ExecuteCreateSchema()
        {
            SchemaTreeViewItems.Add(new SchemaNode() { Name = SchemaName, ClassNodes = new ObservableCollection<ClassNode>() });
        }

        private RelayCommand _CreateClass;
        public RelayCommand CreateClass => _CreateClass ?? (_CreateClass = new RelayCommand(ExecuteCreateClass,()=> !string.IsNullOrEmpty(ClassName) && SchemaTreeViewItems.Count > 0));
        public void ExecuteCreateClass()
        {
            SchemaTreeViewItems.Last().ClassNodes.Add(new ClassNode() { Name = ClassName, PropertyNodes = new ObservableCollection<PropertyNode>() });
        }

        private RelayCommand _CreateProperty;
        public RelayCommand CreateProperty => _CreateProperty ?? (_CreateProperty = new RelayCommand(ExecuteCreateProperty, CanCreateProperty));
        public void ExecuteCreateProperty()
        {
            SchemaTreeViewItems.Last().ClassNodes.Last().PropertyNodes.Add(new PropertyNode() { Name = PropertyName, PropertyType = SelectedPropertyType });
        }
        public bool CanCreateProperty() => !string.IsNullOrEmpty(PropertyName) && SchemaTreeViewItems.Count > 0 && SchemaTreeViewItems.Last().ClassNodes.Count > 0;

        private RelayCommand _CreateAndImport;
        public RelayCommand CreateAndImport => _CreateAndImport ?? (_CreateAndImport = new RelayCommand(ExecuteCreateAndImport, CanCreateAndImport));
        public void ExecuteCreateAndImport()
        {
            ECSchema schema = new ECSchema(SchemaTreeViewItems.Last().Name, 1, 0, "PDIWT");
            foreach (var classnode in SchemaTreeViewItems.Last().ClassNodes)
            {
                ECClass someclass = new ECClass(classnode.Name);
                foreach (var propnode in classnode.PropertyNodes)
                {
                    ECProperty someProp = new ECProperty(propnode.Name, GetTypeFromString(propnode.PropertyType));
                    //someProp.
                    someclass.Add(someProp);
                    //someclass.AddProperty();
                }
                schema.AddClass(someclass);
            }
            if (BDEC.DgnECManager.Manager.ImportSchema(schema,Program.GetActiveDgnFile(), new BDEC.ImportSchemaOptions())== BD.SchemaImportStatus.Success)
            {
                MessageBox.Show($"{schema.FullName}导入成功"," 水规院", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public bool CanCreateAndImport() => SchemaTreeViewItems.Count > 0 && SchemaTreeViewItems.Last().ClassNodes.Count > 0 && SchemaTreeViewItems.Last().ClassNodes.Last().PropertyNodes.Count > 0;

        private RelayCommand _Clear;
        public RelayCommand Clear => _Clear ?? (_Clear = new RelayCommand(ExecuteClear));
        public void ExecuteClear()
        {
            if (MessageBox.Show("你确定要丢弃当前的Schema吗？", "水规院", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                SchemaTreeViewItems.Clear();
                SchemaName = string.Empty;
                ClassName = string.Empty;
                PropertyName = string.Empty;
                SelectedPropertyType = PropertyTypes.First();
            }
        }

        IECType GetTypeFromString(string typename)
        {
            switch (typename)
            {
                case "String":
                    return ECObjects.StringType;
                case "Boolean":
                    return ECObjects.BooleanType;
                case "Integer":
                    return ECObjects.IntegerType;
                case "Double":
                    return ECObjects.DoubleType;
            }
            return ECObjects.StringType;
        }
    }

    public class SchemaNode:ObservableObject
    {
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { Set(ref _Name, value); }
        }

        private ObservableCollection<ClassNode> _ClassNodes;
        public ObservableCollection<ClassNode> ClassNodes
        {
            get { return _ClassNodes; }
            set { Set(ref _ClassNodes, value); }
        }
    }
    public class ClassNode: ObservableObject
    {
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { Set(ref _Name, value); }
        }

        private ObservableCollection<PropertyNode> _PropertyNodes;
        public ObservableCollection<PropertyNode> PropertyNodes
        {
            get { return _PropertyNodes; }
            set { Set(ref _PropertyNodes, value); }
        }

    }
    public class PropertyNode:ObservableObject
    {
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { Set(ref _Name, value); }
        }
        private string _PropertyType;
        public string PropertyType
        {
            get { return _PropertyType; }
            set { Set(ref _PropertyType, value); }
        }
    }
}