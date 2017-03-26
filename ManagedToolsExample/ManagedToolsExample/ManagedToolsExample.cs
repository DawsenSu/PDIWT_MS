/*--------------------------------------------------------------------------------------+
|
|     $Source: MstnExamples/Elements/ManagedToolsExample/ManagedToolsExample.cs $
|
|  $Copyright: (c) 2015 Bentley Systems, Incorporated. All rights reserved. $
|
+--------------------------------------------------------------------------------------*/

/*--------------------------------------------------------------------------------------+
*
*   This example demonstrates DgnPrimitiveTool and DgnElementSetTool for placing
*   primitive elements and modifying elements respectively.
*
*   This example provides two tools, one for placing a Grouped Hole element and
*   the other for modifying the Grouped Hole element. Modification is only
*   restricted to fill color of the element. If the element has a fill color, the
*   the tool will remove that fill, otherwise it will add a fill color to the element.
*   
*   Below are the key-ins provided by this example
 *   
 *   TOOLSEXAMPLE PLACE GROUPEDHOLE: Places a Grouped Hole element with options coming
 *   from PlaceGroupedHoleForm. The options available are
 *   Add Fill Color: If this checkbox is ON, a fill will be added to the element.
 *   Number of holes: How many hole do you need in the Grouped Hole element.
 *   
 *   TOOLSEXAMPLE MODIFY GROUPEDHOLE: Adds a fill color to the element if it is not filled
 *   and removes the fill if the element already has a fill.
+--------------------------------------------------------------------------------------*/

using Bentley.DgnPlatformNET;
using Bentley.MstnPlatform;
using System.Windows.Forms;

namespace ManagedToolsExample
{

    /*=================================================================================**//**
    * @bsiclass                                                               Bentley Systems
+===============+===============+===============+===============+===============+======*/
    [Bentley.MstnPlatformNET.AddInAttribute(MdlTaskID = "ManagedToolsExample")]
    public sealed class ManagedToolsExample : Bentley.MstnPlatformNET.AddIn
    {

        private static ManagedToolsExample s_anagedToolsExampleAddin = null;

        /*---------------------------------------------------------------------------------**//**
        * Constructor.
        * @bsimethod                                                              Bentley Systems
/*--------------+---------------+---------------+---------------+---------------+------*/
        public ManagedToolsExample(System.IntPtr mdlDesc)
            : base(mdlDesc)
        {
            s_anagedToolsExampleAddin = this;
        }

        /*---------------------------------------------------------------------------------**//**
        * Required Run method of AddIn class.
        * @bsimethod                                                              Bentley Systems
/*--------------+---------------+---------------+---------------+---------------+------*/
        protected override int Run(string[] commandLine)
        {
            return 0;
        }

        /*---------------------------------------------------------------------------------**//**
        * Returns an instance of the Addin class.
        * @bsimethod                                                              Bentley Systems
/*--------------+---------------+---------------+---------------+---------------+------*/
        internal static ManagedToolsExample Instance()
        {
            return s_anagedToolsExampleAddin;
        }

        /*---------------------------------------------------------------------------------**//**
        * Starts place grouped hole element tool.
        * @bsimethod                                                              Bentley Systems
/*--------------+---------------+---------------+---------------+---------------+------*/
        internal void StartPlaceGroupedHoleTool()
        {
            PlaceGroupedHoleTool.InstallNewInstance();
        }

        /*---------------------------------------------------------------------------------**//**
        * Starts modify grouped hole element tool.
        * @bsimethod                                                              Bentley Systems
/*--------------+---------------+---------------+---------------+---------------+------*/
        internal void StartModifyGroupedHoleTool()
        {
            ModifyGroupedHoleTool.InstallNewInstance();
        }
    }
}
