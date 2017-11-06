using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using BD = Bentley.DgnPlatformNET;
using BCOM = Bentley.Interop.MicroStationDGN;
using BM = Bentley.MstnPlatformNET;
using BG = Bentley.GeometryNET;
using BDE = Bentley.DgnPlatformNET.Elements;

namespace PDIWT_MS_Tool
{
    public class KeyinCommands
    {
        #region Measure
        public static void LinestringArea(string unparsed)
        {
            double UPM = Program.GetActiveDgnModel().GetModelInfo().UorPerMaster;
            var activemodel = Program.GetActiveDgnModel();
            var elements = Program.GetActiveDgnModel().GetGraphicElements();
            foreach (var ele in elements)
            {
                if (ele.ElementType == BD.MSElementType.LineString)
                {
                    BDE.LineStringElement linestring = ele as BDE.LineStringElement;
                    var vector = linestring.GetCurveVector();
                    Bentley.GeometryNET.DPoint3d start, end;
                    vector.GetStartEnd(out start, out end);
                    if (start.Distance(end) < 0.1 * UPM)
                    {
                        var shapeele = DrawShapeFromLineString(linestring);
                        if (shapeele != null)
                            shapeele.AddToModel();
                    }
                }
                if (ele.ElementType == BD.MSElementType.ComplexString)
                {
                    BDE.ComplexStringElement complexstringele = ele as BDE.ComplexStringElement;
                    var cv = complexstringele.AsCurvePathEdit().GetCurveVector();
                    if (cv.Count > 1)
                    {
                        BG.DPoint3d DpStart1, DpEnd1, DpStart2, DpEnd2;
                        cv.First().GetStartEnd(out DpStart1, out DpEnd1);
                        cv.Last().GetStartEnd(out DpStart2, out DpEnd2);
                        if (DpStart1.Distance(DpEnd2) < 0.1 * UPM)
                        {
                            var pointlist = new List<BG.DPoint3d>();
                            var complexshape = new BDE.ComplexShapeElement(activemodel,null);
                            foreach (var pri in cv)
                            {
                                if (pri.TryGetLineString(pointlist))
                                {
                                    if (pointlist.Count > 2)
                                    {
                                        complexshape.AddComponentElement(new BDE.LineStringElement(activemodel, null, pointlist.ToArray()));
                                    }

                                }
                            }
                            complexshape.AddComponentComplete();
                            complexshape.AddSolidFill(2, true);
                            complexshape.AddToModel();
                        }
                    }
                }
            }
            System.Windows.Forms.MessageBox.Show("Done!");
        }

        public static BDE.ShapeElement DrawShapeFromLineString(BDE.LineStringElement linestringele)
        {
            var pointlist = new List<BG.DPoint3d>();
            var cv = linestringele.GetCurveVector();
            if (cv.Count == 1 && cv.GetAt(0).TryGetLineString(pointlist))
            {
                if (pointlist.Count > 2)
                {
                    var shape = new BDE.ShapeElement(linestringele.DgnModel, null, pointlist.ToArray());
                    shape.AddSolidFill(2, true);
                    return shape;
                }
            }
            return null;
        }

        public static void LinestringUnite(string unparsed)
        {
            BD.ScanCriteria sc = new BD.ScanCriteria();
            sc.SetModelRef(Program.GetActiveDgnModelRef());
            BD.BitMask bm = new Bentley.DgnPlatformNET.BitMask(true);
            bm.Set((uint)BD.MSElementType.LineString);
            bm.Set((uint)BD.MSElementType.ComplexString);
            sc.SetElementTypeTest(bm);

            var cvlist = new List<BG.CurveVector>();
            sc.Scan((ele, model) =>
            {
                if (ele.ElementType == BD.MSElementType.LineString)
                {
                    var stringEle = ele as BDE.LineStringElement;
                    cvlist.Add(stringEle.GetCurveVector());
                }
                if(ele.ElementType == BD.MSElementType.ComplexString)
                {
                    var complexstring = ele as BDE.ComplexStringElement;
                    cvlist.Add(complexstring.AsCurvePathEdit().GetCurveVector());
                }
                return BD.StatusInt.Success;
            });
            var results = new List<BG.CurveVector>();
            if(cvlist.Count > 0)
            {
                for (int i = 0; i < cvlist.Count-1; i++)
                {
                    for (int j = i+1; j < cvlist.Count; j++)
                    {
                        var unitecv = BG.CurveVector.AreaUnion(cvlist[i], cvlist[j]);
                        if (unitecv != null)
                            results.Add(unitecv);
                    }
                }                
            }
        }
        #endregion
        //#region  EC Schema and instance
        public static void CmdFindSchema(string unparsed)
        {
            PDIWT_Encrypt.Entrance.VerifyActivationState(Views.SchemaListView.ShowWindow);
        }

        public static void CmdImportSchema(string unparsed)
        {
            PDIWT_Encrypt.Entrance.VerifyActivationState(Views.CreateAndImportSchemaView.ShowWindow);
        }
        public static void CmdFindAllInstance(string unparsed)
        {
            PDIWT_Encrypt.Entrance.VerifyActivationState(Views.FindnstancesView.ShowWindow);
        }
        public static void CmdWirteInstance(string unparsed)
        {
            PDIWT_Encrypt.Entrance.VerifyActivationState(() =>
            {
                Models.WriteInstanceOnElementTool tool = new Models.WriteInstanceOnElementTool(0, 0);
                tool.InstallNewInstance();
            });

        }
        public static void CmdFindAllOnElement(string unparsed)
        {
            PDIWT_Encrypt.Entrance.VerifyActivationState(Models.FindInstancesOnElementTool.InstallNewInstance);
        }
        //#endregion
        #region Level export
        public static void ExportRGBColorTable(string unparsed)
        {
            PDIWT_Encrypt.Entrance.VerifyActivationState(Views.LevelExportView.ShowWindow);
        }
        #endregion

        #region Cells
        public static void CellsFastPut(string unparsed)
        {
            PDIWT_Encrypt.Entrance.VerifyActivationState(Views.CellFastPutView.ShowWindow);
        }

        public static void CellsArmorPut(string unparsed)
        {
            PDIWT_Encrypt.Entrance.VerifyActivationState(Views.CellsArmorPutView.ShowWindow);
        }
        #endregion

        #region Models

        public static void ModelsCreateFromExcel(string unparsed)
        {
            PDIWT_Encrypt.Entrance.VerifyActivationState(Views.ModelCreatorView.ShowWindow);
        }
        #endregion

    }
}
