/*--------------------------------------------------------------------------------------+
|
|     $Source: MstnExamples/Elements/ManagedToolsExample/ModifyGroupedHoleTool.cs $
|
|  $Copyright: (c) 2015 Bentley Systems, Incorporated. All rights reserved. $
|
+--------------------------------------------------------------------------------------*/

using Bentley.DgnPlatformNET;
using Bentley.DgnPlatformNET.Elements;
using Bentley.GeometryNET;
using System.Collections.Generic;
namespace ManagedToolsExample
{

    /*=================================================================================**//**
    * @bsiclass                                                               Bentley Systems
+===============+===============+===============+===============+===============+======*/
    class ModifyGroupedHoleTool : DgnElementSetTool
    {

        /*---------------------------------------------------------------------------------**//**
        * Constructor
        * @bsimethod                                                              Bentley Systems
/*--------------+---------------+---------------+---------------+---------------+------*/
        private ModifyGroupedHoleTool() : base()
        {
        }

        /*---------------------------------------------------------------------------------**//**
        * Actual modification on first data button if it lies on GroupedHole element.
        * @bsimethod                                                              Bentley Systems
/*--------------+---------------+---------------+---------------+---------------+------*/
        protected override bool OnDataButton(Bentley.DgnPlatformNET.DgnButtonEvent ev)
        {
            //Locate an element.
            Bentley.DgnPlatformNET.HitPath hitPath = DoLocate(ev, true, 1);

            //If an element is located and the element is a grouped hole.
            //If element has a solid fill, remove it.
            //Else add a solid fill.
            if (null != hitPath)
            {
                int numHoles = PlaceGroupedHoleForm.GetNumberOfHoles();
                Bentley.DgnPlatformNET.Elements.Element element = hitPath.GetHeadElement();

                if (element is GroupedHoleElement)
                {
                    GroupedHoleElement groupedHoleElement = element as GroupedHoleElement;

                    uint fillColor;
                    bool alwaysFilled;
                    if (groupedHoleElement.GetSolidFill(out fillColor, out alwaysFilled))
                    {
                        groupedHoleElement.RemoveAreaFill();
                    }
                    else
                    {
                        groupedHoleElement.AddSolidFill((uint)Bentley.MstnPlatformNET.Settings.GetActiveFillColor().Index, false);
                    }

                    //Element must be replaced in model.
                    groupedHoleElement.ReplaceInModel(groupedHoleElement);
                }
            }

            return true;
        }

        /*---------------------------------------------------------------------------------**//**
        * All modification is done in OnDataButton method, just return Error from this method.
        * @bsimethod                                                              Bentley Systems
/*--------------+---------------+---------------+---------------+---------------+------*/
        public override Bentley.DgnPlatformNET.StatusInt OnElementModify(Bentley.DgnPlatformNET.Elements.Element element)
        {
            return Bentley.DgnPlatformNET.StatusInt.Error;
        }

        /*---------------------------------------------------------------------------------**//**
        * Restart tool
        * @bsimethod                                                              Bentley Systems
/*--------------+---------------+---------------+---------------+---------------+------*/
        protected override void OnRestartTool()
        {
            InstallNewInstance();
        }

        /*---------------------------------------------------------------------------------**//**
        * Static method to initialize this tool from any class.
        * @bsimethod                                                              Bentley Systems
/*--------------+---------------+---------------+---------------+---------------+------*/
        public static void InstallNewInstance()
        {
            ModifyGroupedHoleTool modifyTool = new ModifyGroupedHoleTool();
            modifyTool.InstallTool();
        }
    }
}
