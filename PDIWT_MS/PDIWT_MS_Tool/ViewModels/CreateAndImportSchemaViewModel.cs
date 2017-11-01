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
        public ObservableCollection<SchemaNode> SchemaTreeViewItems
        {
            get { return GetProperty(() => SchemaTreeViewItems); }
            set { SetProperty(() => SchemaTreeViewItems, value); }
        }
        public string SchemaName
        {
            get { return GetProperty(() => SchemaName); }
            set { SetProperty(() => SchemaName, value); }
        }
        public string ClassName
        {
            get { return GetProperty(() => ClassName); }
            set { SetProperty(() => ClassName, value); }
        }
        public string PropertyName
        {
            get { return GetProperty(() => PropertyName); }
            set { SetProperty(() => PropertyName, value); }
        }
        public string SelectedPropertyType
        {
            get { return GetProperty(() => SelectedPropertyType); }
            set { SetProperty(() => SelectedPropertyType, value); }
        }
        public ObservableCollection<string> PropertyTypes
        {
            get { return GetProperty(() => PropertyTypes); }
            set { SetProperty(() => PropertyTypes, value); }
        }
        [Command]
        public void CreateSchema()
        {
            SchemaTreeViewItems.Add(new SchemaNode() { Name = SchemaName, ClassNodes = new ObservableCollection<ClassNode>() });
        }
        public bool CanCreateSchema() => !string.IsNullOrEmpty(SchemaName);
        [Command]
        public void CreateClass()
        {
            SchemaTreeViewItems.Last().ClassNodes.Add(new ClassNode() { Name = ClassName, PropertyNodes = new ObservableCollection<PropertyNode>() });
        }
        public bool CanCreateClass() =>  !string.IsNullOrEmpty(ClassName) && SchemaTreeViewItems.Count > 0 ;
        [Command]
        public void CreateProperty()
        {
            SchemaTreeViewItems.Last().ClassNodes.Last().PropertyNodes.Add(new PropertyNode() { Name = PropertyName, PropertyType = SelectedPropertyType });
        }
        public bool CanCreateProperty() => !string.IsNullOrEmpty(PropertyName) && SchemaTreeViewItems.Count > 0 && SchemaTreeViewItems.Last().ClassNodes.Count > 0;
        [Command]
        public void CreateAndImport()
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
        [Command]
        public void Clear()
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

        protected override void OnInitializeInRuntime()
        {
            base.OnInitializeInRuntime();
            SchemaTreeViewItems = new ObservableCollection<SchemaNode>();
            PropertyTypes = new ObservableCollection<string>() { "String", "Boolean", "Integer", "Double" };
            SelectedPropertyType = PropertyTypes.First();
        }
    }
    public class SchemaNode:BindableBase
    {
        public string Name
        {
            get { return GetProperty(() => Name); }
            set { SetProperty(() => Name, value); }
        }
        public ObservableCollection<ClassNode> ClassNodes
        {
            get { return GetProperty(() => ClassNodes); }
            set { SetProperty(() => ClassNodes, value); }
        }
    }
    public class ClassNode:BindableBase
    {
        public string Name
        {
            get { return GetProperty(() => Name); }
            set { SetProperty(() => Name, value); }
        }
        public ObservableCollection<PropertyNode> PropertyNodes
        {
            get { return GetProperty(() => PropertyNodes); }
            set { SetProperty(() => PropertyNodes, value); }
        }

    }
    public class PropertyNode:BindableBase
    {
        public string Name
        {
            get { return GetProperty(() => Name); }
            set { SetProperty(() => Name, value); }
        }
        public string PropertyType
        {
            get { return GetProperty(() => PropertyType); }
            set { SetProperty(() => PropertyType, value); }
        }
    }
}