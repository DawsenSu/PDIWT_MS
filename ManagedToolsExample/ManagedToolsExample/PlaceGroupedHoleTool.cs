/*--------------------------------------------------------------------------------------+
|
|     $Source: MstnExamples/Elements/ManagedToolsExample/PlaceGroupedHoleTool.cs $
|
|  $Copyright: (c) 2015 Bentley Systems, Incorporated. All rights reserved. $
|
+--------------------------------------------------------------------------------------*/

using System.Collections.Generic;

using Bentley.GeometryNET;
using Bentley.DgnPlatformNET;
using Bentley.DgnPlatformNET.Elements;
using Bentley.MstnPlatformNET;
using System.Windows.Forms;

namespace ManagedToolsExample
{
    /*=================================================================================**//**
    * @bsiclass                                                               Bentley Systems
+===============+===============+===============+===============+===============+======*/
    class PlaceGroupedHoleTool : DgnPrimitiveTool
    {

        private List<DPoint3d> m_points;

        private static PlaceGroupedHoleForm m_groupHoleForm = null;

        /*---------------------------------------------------------------------------------**//**
        * Constructor.
        * @bsimethod                                                              Bentley Systems
/*--------------+---------------+---------------+---------------+---------------+------*/
        public PlaceGroupedHoleTool(int toolId, int prompt) : base(toolId, prompt)
        {
            m_points = new List<DPoint3d>();

            //Load options form.
            if (null == m_groupHoleForm)
            {
                m_groupHoleForm = new PlaceGroupedHoleForm();
                m_groupHoleForm.Show();
            }
        }

        /*---------------------------------------------------------------------------------**//**
        * Restart tool.
        * @bsimethod                                                              Bentley Systems
/*--------------+---------------+---------------+---------------+---------------+------*/
        protected override void OnRestartTool()
        {
            InstallNewInstance();
        }

        /*---------------------------------------------------------------------------------**//**
        * Exit tool.
        * @bsimethod                                                              Bentley Systems
/*--------------+---------------+---------------+---------------+---------------+------*/
        protected override void ExitTool()
        {
            if (null != m_groupHoleForm)
            {
                m_groupHoleForm.Close();
                m_groupHoleForm = null;
            }
            base.ExitTool();
        }

        /*---------------------------------------------------------------------------------**//**
        * Enable snapping.
        * @bsimethod                                                              Bentley Systems
/*--------------+---------------+---------------+---------------+---------------+------*/
        protected override void OnPostInstall()
        {
            AccuSnap.SnapEnabled = true;
            base.OnPostInstall();
        }

        /*---------------------------------------------------------------------------------**//**
        * Get start and opposite points from data buttons in this method.
/// Start dynamics on first data point.
/// Place the grouped hole on second data point.
        * @bsimethod                                                              Bentley Systems
/*--------------+---------------+---------------+---------------+---------------+------*/
        protected override bool OnDataButton(DgnButtonEvent ev)
        {
            if (0 == m_points.Count)
                BeginDynamics();

            m_points.Add(ev.Point); // Save current data point location.

            if (m_points.Count < 2)
                return false;

            Element element = GroupedHoleHelper.CreateElement(m_points[0], m_points[1]);
            if (null != element)
                element.AddToModel(); // Add new element to active model.

            base.OnReinitialize();

            return true;
        }

        /*---------------------------------------------------------------------------------**//**
        * Temporarly show how the element will look like.
        * @bsimethod                                                              Bentley Systems
/*--------------+---------------+---------------+---------------+---------------+------*/
        protected override void OnDynamicFrame(DgnButtonEvent ev)
        {
            Element element = GroupedHoleHelper.CreateElement(m_points[0], ev.Point);
            if (null == element)
                return;

            RedrawElems redrawElems = new RedrawElems();
            redrawElems.SetDynamicsViewsFromActiveViewSet(Session.GetActiveViewport());
            redrawElems.DrawMode = DgnDrawMode.TempDraw;
            redrawElems.DrawPurpose = DrawPurpose.Dynamics;

            redrawElems.DoRedraw(element);
        }

        /*---------------------------------------------------------------------------------**//**
        * Restart the tool on reset.
        * @bsimethod                                                              Bentley Systems
/*--------------+---------------+---------------+---------------+---------------+------*/
        protected override bool OnResetButton(DgnButtonEvent ev)
        {
            ExitTool();
            return true;
        }

        /*---------------------------------------------------------------------------------**//**
        * Static method to initialize this tool from any class.
        * @bsimethod                                                              Bentley Systems
/*--------------+---------------+---------------+---------------+---------------+------*/
        public static void InstallNewInstance()
        {
            PlaceGroupedHoleTool groupedHoleTool = new PlaceGroupedHoleTool(0, 0);
            groupedHoleTool.InstallTool();
        }
    }
}
