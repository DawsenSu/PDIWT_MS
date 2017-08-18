using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bentley.Interop.MicroStationDGN;
using BG = Bentley.GeometryNET;
using BD = Bentley.DgnPlatformNET;
using BM = Bentley.MstnPlatformNET;
using BDE = Bentley.DgnPlatformNET.Elements;
 
namespace BentleyTrain01
{
    class KeyinCommands
    {
        public static void Command(string unparsed)
        {

        }

        public static void PrintDictionaryModel(string unparsed)
        {
            var activeDgnFile = Program.ActiveDgnFile;
            var dictModel = activeDgnFile.GetDictionaryModel();
            MessageBox.Show(dictModel.ModelName);
            var modelEleCOl = dictModel.GetElements();
            foreach (var ele in modelEleCOl)
            {
                int dependantNum = ele.GetDependants().Count;
                BM.MessageCenter.Instance.ShowInfoMessage(ele.TypeName,ele.ElementId.ToString()+":"+ ele.ElementType.ToString() + ":" + ele.Description+":Dependants "+ dependantNum,false);
            }
        }

        public static void CreateElementLine(string unparsed)
        {
            BD.DgnModel activeDgnModel = Program.ActiveDgnModel;
            double uorPerMe = activeDgnModel.GetModelInfo().UorPerMaster;
            BDE.LineElement lineElement = new BDE.LineElement(activeDgnModel,null,new BG.DSegment3d(0,0,0,1000*uorPerMe,1000*uorPerMe,1000*uorPerMe));
            lineElement.AddToModel();
        }

        public static void CreateElementUniLine(string unparsed)
        {
            BD.DgnModel activeDgnModel = Program.ActiveDgnModel;
            double uorPerMe = activeDgnModel.GetModelInfo().UorPerMaster;
            var lines = new BG.DPoint3d[]
            {
                new BG.DPoint3d(0, 0),
                new BG.DPoint3d(100*uorPerMe, 0),
                new BG.DPoint3d(100*uorPerMe, 100*uorPerMe),
                new BG.DPoint3d(0, 100*uorPerMe),
            };
            var curvePri = BG.CurvePrimitive.CreateLineString(lines);

            BG.DEllipse3d circle = BG.DEllipse3d.FromCenterRadiusXY(new BG.DPoint3d(50*uorPerMe,50*uorPerMe),10*uorPerMe );
            var curvePriC = BG.CurvePrimitive.CreateArc(circle);

            BG.CurveVector composeCurveVector = new BG.CurveVector(BG.CurveVector.BoundaryType.ParityRegion)
            {
                curvePri,curvePriC
            };
            var ele = BDE.DraftingElementSchema.ToElement(activeDgnModel, composeCurveVector, null);
            ele.AddToModel();
        }
    }
}
