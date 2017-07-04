using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BD = Bentley.DgnPlatformNET;
using BDEC = Bentley.DgnPlatformNET.DgnEC;
using BES = Bentley.ECObjects.Schema;
using BEI = Bentley.ECObjects.Instance;
using BDE = Bentley.DgnPlatformNET.Elements;

namespace PDIWT_MS_Tool.Models
{
    using Bentley.MstnPlatformNET;
    using ViewModels;
    using Views;
    class WriteInstanceOnElementTool : BD.DgnElementSetTool
    {
        public WriteInstanceOnElementTool(int toolID, int toolName) : base()
        {
            WriteInstanceOnElementView.ShowWindow(Program.Addin);
        }
        MessageCenter mc = MessageCenter.Instance;
        protected override void OnRestartTool()
        {
            InstallNewInstance();
        }
        protected override bool OnDataButton(BD.DgnButtonEvent ev)
        {
            BD.HitPath hitPath = DoLocate(ev, true, 1);
            if (null!= hitPath)
            {
                InstanceToWrite instanceInfo = (WriteInstanceOnElementView.Instance.DataContext as WriteInstanceOnElementViewModel).GetInstanceToWrite();
                if (string.IsNullOrEmpty(instanceInfo.SchemaName)||string.IsNullOrEmpty(instanceInfo.ClassName) || instanceInfo.Properties.Count == 0)
                {
                    mc.StatusPrompt = "请先选择要附加的class";
                    return false;
                }
                BDEC.FindInstancesScope scope = BDEC.FindInstancesScope.CreateScope(Program.GetActiveDgnFile(), new BDEC.FindInstancesScopeOption());
                BES.IECSchema schema = BDEC.DgnECManager.Manager.LocateSchemaInScope(scope, instanceInfo.SchemaName, instanceInfo.MajorVersion, instanceInfo.MinorVersion, BES.SchemaMatchType.Exact);
                BDE.Element ele = hitPath.GetHeadElement();

                BES.ECClass class1 = schema.GetClass(instanceInfo.ClassName) as BES.ECClass;
                BDEC.DgnECInstanceEnabler instanceEnabler = BDEC.DgnECManager.Manager.ObtainInstanceEnabler(Program.GetActiveDgnFile(), class1);
                BEI.StandaloneECDInstance instance = instanceEnabler.SharedWipInstance;
                foreach (var pInfo in instanceInfo.Properties)
                {
                    switch (pInfo.PropertyType.ToLower())
                    {
                        case "string":
                            instance.MemoryBuffer.SetStringValue(pInfo.PropertyName, -1, pInfo.GetValueAsString());
                            break;
                        case "boolean":
                            instance.MemoryBuffer.SetBooleanValue(pInfo.PropertyName, -1, pInfo.GetValueAsBoolean());
                            break;
                        case "int":
                            instance.MemoryBuffer.SetIntegerValue(pInfo.PropertyName, -1, pInfo.GetValueAsInt());
                            break;
                        case "double":
                            instance.MemoryBuffer.SetDoubleValue(pInfo.PropertyName, -1, pInfo.GetValueAsDouble());
                            break;
                    }
                }
                instanceEnabler.CreateInstanceOnElement(ele, instance, false);
                mc.StatusPrompt = "附加完成";
                mc.StatusMessage = $"将{instanceInfo.ClassName}附加到ID：{ele.ElementId}物体上";
            }

            return true;
        }
        public override BD.StatusInt OnElementModify(BDE.Element element)
        {
            return BD.StatusInt.Error;
        }
        protected override bool OnResetButton(BD.DgnButtonEvent ev)
        {
            ExitTool();
            return true;
        }
        protected override void ExitTool()
        {
            WriteInstanceOnElementView.CloseWindow();
            base.ExitTool();
        }

        public void InstallNewInstance()
        {
            WriteInstanceOnElementTool tool = new WriteInstanceOnElementTool(0,0); // tool的名称无法修改
            tool.InstallTool();
            mc.StatusCommand = "选择物体添加属性";
            mc.StatusPrompt = "表示物体";
        }

    }
}
