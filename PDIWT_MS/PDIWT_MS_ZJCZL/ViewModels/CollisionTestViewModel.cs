using System;
using System.Windows;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.InteropServices;

using BG = Bentley.GeometryNET;
using BDE = Bentley.DgnPlatformNET.Elements;
using BD = Bentley.DgnPlatformNET;
using BM = Bentley.MstnPlatformNET;
using BCOM = Bentley.Interop.MicroStationDGN;

using MSE = MathNet.Spatial.Euclidean;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;

namespace PDIWT_MS_ZJCZL.ViewModels
{
    using Models;
    using Models.Piles;

    public class CollisionTestViewModel : ViewModelBase
    {
        BCOM.Application app = Program.COM_App;
        public CollisionTestViewModel(ObservableCollection<PileBase> piles)
        {
            Piles = piles;
            CollisionResult = new ObservableCollection<CollisionInfo>();
        }
        public ObservableCollection<PileBase> Piles
        {
            get { return GetProperty(() => Piles); }
            set { SetProperty(() => Piles, value); }
        }
        public double MinDistance
        {
            get { return GetProperty(() => MinDistance); }
            set { SetProperty(() => MinDistance, value); }
        }
        public ObservableCollection<CollisionInfo> CollisionResult
        {
            get { return GetProperty(() => CollisionResult); }
            set { SetProperty(() => CollisionResult, value); }
        }
        [Command]
        public void ConductTest()
        {
            CollisionResult = GetCollisionInfoList(MinDistance);
        }

        [Command]
        public void DrawCollisionIndicator()
        {
            var newlevelhandle = AddLevelByName("Collision");
            var levelsetter = new BD.ElementPropertiesSetter();
            levelsetter.SetLevel(newlevelhandle.LevelId);
            levelsetter.SetColor(6);
            levelsetter.SetThickness(10, true);

            var activemodel = Program.GetActiveDgnModel();

            foreach (var collision in CollisionResult)
            {
                var line = new BDE.LineElement(activemodel, null, new BG.DSegment3d(collision.PileACollisionPoint, collision.PileBCollisionPoint));
                levelsetter.Apply(line);
                line.AddToModel();
            }
            BM.SettingsActivator.SetActiveLevel(newlevelhandle.LevelId);
            System.Windows.Forms.MessageBox.Show("碰撞交线已绘制在图层[Collision]中");
        }
        [Command]
        public void Test()
        {
            BG.DSegment3d lineAB = new Bentley.GeometryNET.DSegment3d(0, 0, 5, 0, 0, 10);
            BG.DSegment3d lineCD = new Bentley.GeometryNET.DSegment3d(0, 0, -5, 0, 0, -10);
            BG.DSegment3d outline;
            double fa, fb;
            BG.DSegment3d.ClosestApproachSegment(lineAB, lineCD, out outline, out fa, out fb);
            //MinDistance = 1;
        }
        public bool CanDrawCollisionIndicator() => CollisionResult.Count > 0;
        public List<long> GetPileAxisLineIds()
        {
            var idlist = new List<long>();
            foreach (var pile in Piles)
                idlist.Add(pile.PileId);
            return idlist;
        }
        public ObservableCollection<CollisionInfo> GetCollisionInfoList(double minallowdistance = 0/*unit mm*/)
        {
            //double uor_per_meter = Program.GetActiveDgnModel().GetModelInfo().UorPerMeter;
            double uor_per_master = Program.GetActiveDgnModel().GetModelInfo().UorPerMaster;
            //double uor_per_storage = Program.GetActiveDgnModel().GetModelInfo().UorPerStorage;
            //double uor_per_sub = Program.GetActiveDgnModel().GetModelInfo().UorPerSub;
            var collisionInfoList = new ObservableCollection<CollisionInfo>();
            var pileidlist = GetPileAxisLineIds();
            var collisiontestpilelist = new List<CollisionTestPileInfo>();

            var activemodel = Program.GetActiveDgnModel();

            foreach (var id in pileidlist)
            {
                var ele = activemodel.FindElementById((BD.ElementId)id);
                if (ele != null && ele.ElementType == BD.MSElementType.Line)
                {
                    var blockdatareader = ele.GetLinkage((ushort)BD.ElementLinkageId.String);
                    string elename = string.Empty;
                    if (blockdatareader != null)
                        elename = blockdatareader.ReadString();
                    else
                        elename = ele.Description + " ID:" + ele.ElementId.ToString();

                    var comele = app.ActiveModelReference.GetElementByID64(id).AsLineElement();
                    collisiontestpilelist.Add(new CollisionTestPileInfo { PileCode = elename, PileAxisSegement = new BG.DSegment3d(comele.StartPoint.Point3dToDPoint3d(uor_per_master), comele.EndPoint.Point3dToDPoint3d(uor_per_master)) });
                }
            }
            int pileaxiscount = collisiontestpilelist.Count;
            for (int i = 0; i < pileaxiscount - 1; i++)
            {
                for (int j = i + 1; j < pileaxiscount; j++)
                {
                    if (i == 58 && j == 59)
                        activemodel.FindElementById((BD.ElementId)(ulong)1);
                    BG.DSegment3d minsegement = GetMinDistanceBetweenToLine(collisiontestpilelist[i].PileAxisSegement, collisiontestpilelist[j].PileAxisSegement);
                    if (minsegement.Length / 10 <= minallowdistance)
                        collisionInfoList.Add(new CollisionInfo { PileAName = collisiontestpilelist[i].PileCode, PileBName = collisiontestpilelist[j].PileCode, PileACollisionPoint = minsegement.StartPoint, PileBCollisionPoint = minsegement.EndPoint, MinDistance = minsegement.Length / 10 });
                }
            }
            //完成在一个图层上绘制最短距离
            //if (collisionInfoList.Count != 0) return true; else return false;
            return collisionInfoList;
        }

        BD.LevelHandle AddLevelByName(string levelname)
        {
            var levelcache = Program.GetActiveDgnFile().GetLevelCache();
            var levelhandle = levelcache.GetLevelByName(levelname);
            if (levelhandle.Status == BD.LevelCacheErrorCode.CannotFindLevel)
                levelhandle = levelcache.CreateLevel(levelname);
            levelcache.Write();
            return levelhandle;
        }

        BG.DSegment3d GetMinDistanceBetweenToLine(BG.DSegment3d lineAB, BG.DSegment3d lineCD)
        {
            //var lineab = lineAB.DSegement3dToLine3D();
            //var linecd = lineCD.DSegement3dToLine3D();
            //var closetline = lineab.ClosestPointsBetween(linecd, true);
            //return new Bentley.GeometryNET.DSegment3d(closetline.Item1.Point3DToDPoint3d(), closetline.Item2.Point3DToDPoint3d());
            BG.DSegment3d crossline;
            double fa, fb;
            bool isCross;
            isCross = BG.DSegment3d.ClosestApproachSegment(lineAB, lineCD, out crossline, out fa, out fb);

            if (0 <= fa && fa <= 1 && 0 <= fb && fb <= 1)
            {
                return new BG.DSegment3d(lineAB.PointAtFraction(fa), lineCD.PointAtFraction(fb));
            }
            else
            {
                //double A_LineCD, B_LineCD, C_LineAB, D_LineAB;
                BG.DPoint3d P_A_LineCD, P_B_LineCD, P_C_LineAB, P_D_LineAB;
                double f;
                lineCD.ClosestFractionAndPoint(lineAB.StartPoint, true, out f, out P_A_LineCD);
                lineCD.ClosestFractionAndPoint(lineAB.EndPoint, true, out f, out P_B_LineCD);
                lineAB.ClosestFractionAndPoint(lineCD.StartPoint, true, out f, out P_C_LineAB);
                lineAB.ClosestFractionAndPoint(lineCD.EndPoint, true, out f, out P_D_LineAB);
                var closedis = new double[]
                {
                    lineAB.StartPoint.Distance(P_A_LineCD),
                    lineAB.EndPoint.Distance(P_B_LineCD),
                    lineCD.StartPoint.Distance(P_C_LineAB),
                    lineCD.EndPoint.Distance(P_D_LineAB)
                };
                BG.DSegment3d minsegement = new Bentley.GeometryNET.DSegment3d();
                switch (closedis.FindIndex(n => n == closedis.Min()))
                {
                    case 0:
                        minsegement = new BG.DSegment3d(lineAB.StartPoint, P_A_LineCD);
                        break;
                    case 1:
                        minsegement = new BG.DSegment3d(lineAB.EndPoint, P_B_LineCD);
                        break;
                    case 2:
                        minsegement = new BG.DSegment3d(P_C_LineAB, lineCD.StartPoint);
                        break;
                    case 3:
                        minsegement = new BG.DSegment3d(P_D_LineAB, lineCD.EndPoint);
                        break;
                }
                return minsegement;
            }
        }
    }


    public class CollisionTestPileInfo
    {
        public string PileCode { get; set; }
        public BG.DSegment3d PileAxisSegement { get; set; }
    }

    public class CollisionInfo
    {
        [DisplayName("桩A")]
        public string PileAName { get; set; }
        [DisplayName("桩B")]
        public string PileBName { get; set; }
        [DisplayName("桩AB最短距离mm")]
        public double MinDistance { get; set; } //unit: mm
        [DisplayName("桩A碰撞点")]
        [Hidden]
        public BG.DPoint3d PileACollisionPoint { get; set; } //uint: nm
        [DisplayName("桩B碰撞点")]
        [Hidden]
        public BG.DPoint3d PileBCollisionPoint { get; set; } //unit: nm
    }
}