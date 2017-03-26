/*--------------------------------------------------------------------------------------+
|
|     $Source: MstnExamples/Elements/ManagedToolsExample/PlaceGroupedHoleForm.cs $
|
|  $Copyright: (c) 2015 Bentley Systems, Incorporated. All rights reserved. $
|
+--------------------------------------------------------------------------------------*/

using System.Windows.Forms;

namespace ManagedToolsExample
{
    /*=================================================================================**//**
    * @bsiclass                                                               Bentley Systems
+===============+===============+===============+===============+===============+======*/
    public partial class PlaceGroupedHoleForm : Form
    {
        private static bool s_addFillColor;
        private static int s_numberOfHoles;

        /*---------------------------------------------------------------------------------**//**
        * Constructor
        * @bsimethod                                                              Bentley Systems
/*--------------+---------------+---------------+---------------+---------------+------*/
        public PlaceGroupedHoleForm()
        {
            InitializeComponent();
            CmbNoOfHoles.SelectedIndex = 0;
            s_addFillColor = true;
            s_numberOfHoles = 1;
        }

        /*---------------------------------------------------------------------------------**//**
        * Whether to add fill color or not.
        * @bsimethod                                                              Bentley Systems
/*--------------+---------------+---------------+---------------+---------------+------*/
        public static bool GetAddFillColor()
        {
            return s_addFillColor;
        }

        /*---------------------------------------------------------------------------------**//**
        * Returns number of holes selected by the user.
        * @bsimethod                                                              Bentley Systems
/*--------------+---------------+---------------+---------------+---------------+------*/
        public static int GetNumberOfHoles()
        {
            return s_numberOfHoles;
        }

        /*---------------------------------------------------------------------------------**//**
        * Set number of holes from user latest selection.
        * @bsimethod                                                              Bentley Systems
/*--------------+---------------+---------------+---------------+---------------+------*/
        private void CmbNoOfHoles_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            s_numberOfHoles = int.Parse(CmbNoOfHoles.SelectedItem.ToString());
        }

        /*---------------------------------------------------------------------------------**//**
        * Add fill color or not.
        * @bsimethod                                                              Bentley Systems
/*--------------+---------------+---------------+---------------+---------------+------*/
        private void ChkAddFill_CheckedChanged(object sender, System.EventArgs e)
        {
            s_addFillColor = ChkAddFill.Checked;
        }
    }
}
