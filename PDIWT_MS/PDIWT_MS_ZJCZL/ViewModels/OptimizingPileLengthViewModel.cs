using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;

namespace PDIWT_MS_ZJCZL.ViewModels
{
    using Models;
    using Models.Piles;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    public class OptimizingPileLengthViewModel : ViewModelBase
    {
        public OptimizingPileLengthViewModel(ObservableCollection<PileBase> piles)
        {
            Piles = piles;
            AimBearingForce = 0;
            PileLengthModulus = 0.5;
            OptimizedPileInfos = new ObservableCollection<PileOptimizedInfo>();
        }

        public ObservableCollection<PileBase> Piles
        {
            get { return GetProperty(() => Piles); }
            set { SetProperty(() => Piles, value); }
        }
        public double AimBearingForce
        {
            get { return GetProperty(() => AimBearingForce); }
            set { SetProperty(() => AimBearingForce, value); }
        }
        public double PileLengthModulus
        {
            get { return GetProperty(() => PileLengthModulus); }
            set { SetProperty(() => PileLengthModulus, value); }
        }
        public ObservableCollection<PileOptimizedInfo> OptimizedPileInfos
        {
            get { return GetProperty(() => OptimizedPileInfos); }
            set { SetProperty(() => OptimizedPileInfos, value); }
        }

        [Command]
        public void Optimize()
        {
            try
            {
                OptimizedPileInfos.Clear();
                foreach (var pile in Piles)
                {
                    PileLengthCalculation pilelengthcal = new PileLengthCalculation(pile, AimBearingForce);
                    OptimizedPileInfos.Add(new PileOptimizedInfo()
                    {
                        PileName = pile.PileCode,
                        PileLength = Utilities.CellingWithInterval(pilelengthcal.GetPileLengthByBearingCapacity(), PileLengthModulus)
                    });
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString(),"错误", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

        }

        [Command]
        public void OutPutOptimizedResults()
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "逗号分隔符文件|*.csv";
                sfd.Title = "保存优化桩长结果";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("桩名称,优化后桩长(m)\n");
                    foreach (var info in OptimizedPileInfos)
                    {
                        sb.Append(info.PileName + "," + info.PileLength + "\n");
                    }
                    File.WriteAllText(sfd.FileName, sb.ToString());
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        public bool CanOutPutOptimizedResults() => OptimizedPileInfos.Count > 0;
    }

    public class PileOptimizedInfo
    {
        public string PileName { get; set; }
        public double PileLength { get; set; }        
    }
}