using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BD = Bentley.DgnPlatformNET;
using BDDE = Bentley.DgnPlatformNET.DgnEC;
using BES = Bentley.ECObjects.Schema;
using BEI = Bentley.ECObjects.Instance;
using BDEPQ = Bentley.EC.Persistence.Query;
using BDE = Bentley.DgnPlatformNET.Elements;

namespace PDIWT_MS_Tool.Models
{
    using Views;
    using ViewModels;
    class FindInstancesOnElementTool : BD.DgnElementSetTool
    {
        private FindInstancesOnElementView m_findinstanceOnElement = null;
        private FindInstancesOnElementViewModel m_findinstacneOnElementViewModel = null;
        private FindInstancesOnElementTool() : base()
        {
            ShowInstanceView();
        }

        void ShowInstanceView()
        {
            if (m_findinstanceOnElement == null)
            {
                m_findinstanceOnElement = new FindInstancesOnElementView();
                m_findinstacneOnElementViewModel = new FindInstancesOnElementViewModel();
                m_findinstanceOnElement.DataContext = m_findinstacneOnElementViewModel;
                m_findinstanceOnElement.Show();
            }
            m_findinstanceOnElement.Focus();
        }

        protected override bool OnDataButton(BD.DgnButtonEvent ev)
        {
            BD.HitPath hitPath = DoLocate(ev, true, 1);
            if (null!= hitPath)
            {
                BDDE.DgnECManager manager = BDDE.DgnECManager.Manager;
                BDDE.FindInstancesScope scope = BDDE.FindInstancesScope.CreateScope(hitPath.GetHeadElement(), new BDDE.FindInstancesScopeOption( BDDE.DgnECHostType.Element,true));
                BDEPQ.ECQuery query = new BDEPQ.ECQuery(Extension.Utilities.GetActiveModelAllClasses());
                query.SelectClause.SelectAllProperties = true;
                using (BDDE.DgnECInstanceCollection ecInstances = manager.FindInstances(scope, query))
                {
                    if (ecInstances == null)
                    {
                        return false;
                    }
                    m_findinstacneOnElementViewModel.InstancesInfo.Clear();
                    //ObservableCollection<Instance> instanceCollection = new ObservableCollection<Instance>(); 
                    foreach (var ecin in ecInstances)
                    {
                        foreach (BES.IECProperty itemProp in ecin.ClassDefinition.Properties(false))
                        {
                            var instanceitem = new Instance();
                            instanceitem.ClassName = ecin.ClassDefinition.DisplayLabel;
                            instanceitem.PropertyName = itemProp.Name;
                            string type = itemProp.Type.Name.ToLower();
                            instanceitem.TypeName = type;
                            BEI.IECPropertyValue propvalue = ecin.GetPropertyValue(itemProp.Name);

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
                            m_findinstacneOnElementViewModel.InstancesInfo.Add(instanceitem);
                        }
                    }
                    m_findinstacneOnElementViewModel.WindowTitle = hitPath.GetHeadElement().ElementId.ToString();
                }

            }
            return true;
        }

        public override BD.StatusInt OnElementModify(BDE.Element element)
        {
            return BD.StatusInt.Error;
        }

        protected override void OnRestartTool()
        {
            InstallNewInstance();
        }
        protected override void ExitTool()
        {
            if(null!=m_findinstanceOnElement)
            {
                m_findinstanceOnElement.Close();
                m_findinstanceOnElement = null;
                m_findinstacneOnElementViewModel = null;
            }
            base.ExitTool();
        }

        protected override bool OnResetButton(BD.DgnButtonEvent ev)
        {
            ExitTool();
            return true;
        }
        public static void InstallNewInstance()
        {
            FindInstancesOnElementTool tool = new FindInstancesOnElementTool();
            tool.InstallTool();
        }
    }
}
