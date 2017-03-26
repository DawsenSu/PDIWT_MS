/*--------------------------------------------------------------------------------------+
|
|     $Source: MstnExamples/Elements/ManagedToolsExample/GroupedHoleHelper.cs $
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
    class GroupedHoleHelper
    {

        /*---------------------------------------------------------------------------------**//**
        * Calculates points from start and opposite points and creates a shape
        * @bsimethod                                                              Bentley Systems
/*--------------+---------------+---------------+---------------+---------------+------*/
        private static ShapeElement CreateShapeElement(DgnModel model, DPoint3d start, DPoint3d opposite)
        {
            DPoint3d[] points = new DPoint3d[5];
            points[0] = points[1] = points[3] = points[4] = start;
            points[2] = opposite;

            points[1].X = opposite.X;
            points[3].Y = opposite.Y;

            ShapeElement shape = new ShapeElement(model, null, points);

            return shape;
        }

        /*---------------------------------------------------------------------------------**//**
        * Calculates start and opposite points of holes and creates them.
        * @bsimethod                                                              Bentley Systems
/*--------------+---------------+---------------+---------------+---------------+------*/
        private static void PopulateHolesAgenda(out ElementAgenda holes, DPoint3d start, DPoint3d opposite, DgnModel model)
        {
            holes = new ElementAgenda();

            //How many holes are required by the user. Possible options are only 1, 2 and 4.
            int numHoles = PlaceGroupedHoleForm.GetNumberOfHoles();

            System.Collections.Generic.List<DPoint3d> holeStarts = new List<DPoint3d>();
            System.Collections.Generic.List<DPoint3d> holeEnds = new List<DPoint3d>();

            if (1 == numHoles)           //One hole in the middle of the shape.
            {
                double xFactor = (opposite.X - start.X) / 3;
                double yFactor = (start.Y - opposite.Y) / 3;

                holeStarts.Add(new DPoint3d(start.X + xFactor, start.Y - yFactor, start.Z));
                holeEnds.Add(new DPoint3d(opposite.X - xFactor, opposite.Y + yFactor, opposite.Z));
            }
            else if (2 == numHoles)      //Two holes.
            {
                double xFactor = (opposite.X - start.X) / 5;
                double yFactor = (start.Y - opposite.Y) / 5;

                holeStarts.Add(new DPoint3d(start.X + xFactor, start.Y - yFactor, start.Z));
                holeEnds.Add(new DPoint3d(start.X + (2 * xFactor), opposite.Y + yFactor, opposite.Z));

                holeStarts.Add(new DPoint3d(opposite.X - (2 * xFactor), start.Y - yFactor, start.Z));
                holeEnds.Add(new DPoint3d(opposite.X - xFactor, opposite.Y + yFactor, opposite.Z));
            }
            else //if(4 == numHoles)    Four holes.
            {
                double xFactor = (opposite.X - start.X) / 5;
                double yFactor = (start.Y - opposite.Y) / 5;

                holeStarts.Add(new DPoint3d(start.X + xFactor, start.Y - yFactor, start.Z));
                holeEnds.Add(new DPoint3d(start.X + (2 * xFactor), start.Y - (2 * yFactor), opposite.Z));

                holeStarts.Add(new DPoint3d(opposite.X - (2 * xFactor), start.Y - yFactor, start.Z));
                holeEnds.Add(new DPoint3d(opposite.X - xFactor, start.Y - (2 * yFactor), opposite.Z));

                holeStarts.Add(new DPoint3d(start.X + xFactor, opposite.Y + (2 * yFactor), start.Z));
                holeEnds.Add(new DPoint3d(start.X + (2 * xFactor), opposite.Y + yFactor, opposite.Z));

                holeStarts.Add(new DPoint3d(opposite.X - (2 * xFactor), opposite.Y + (2 * yFactor), start.Z));
                holeEnds.Add(new DPoint3d(opposite.X - xFactor, opposite.Y + yFactor, opposite.Z));
            }

            //Create hole elements and populate the agenda.
            for (int i = 0; i < numHoles; i++)
            {
                ShapeElement holeShape = CreateShapeElement(model, holeStarts[i], holeEnds[i]);
                holes.Insert(holeShape, true);
            }
        }

        /*---------------------------------------------------------------------------------**//**
        * Create Grouped hole element.
        * @bsimethod                                                              Bentley Systems
/*--------------+---------------+---------------+---------------+---------------+------*/
        public static Element CreateElement(DPoint3d start, DPoint3d opposite)
        {
            DgnModel model = Bentley.MstnPlatformNET.Session.Instance.GetActiveDgnModel();

            //Create Solid shape element from actual start and opposite points.
            ShapeElement solidShape = CreateShapeElement(model, start, opposite);

            //Get holes for grouped hole element.
            ElementAgenda holes;
            PopulateHolesAgenda(out holes, start, opposite, model);

            //Create actual grouped hole element with given solid and holes.
            GroupedHoleElement groupedHoleElement = new GroupedHoleElement(model, solidShape, holes);

            //Set line color.
            ElementPropertiesSetter pSetter = new ElementPropertiesSetter();
            pSetter.SetColor((uint)Bentley.MstnPlatformNET.Settings.GetActiveColor().Index);
            pSetter.Apply(groupedHoleElement);

            //Add fill color.
            if (PlaceGroupedHoleForm.GetAddFillColor())
                groupedHoleElement.AddSolidFill((uint)Bentley.MstnPlatformNET.Settings.GetActiveFillColor().Index, false);

            return groupedHoleElement;
        }
    }
}
