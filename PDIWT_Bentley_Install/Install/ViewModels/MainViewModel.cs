using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;

using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using System.Linq;

namespace Install.ViewModels
{
    public class MainViewModel:ViewModelBase
    {
        public MainViewModel()
        {
            welcomePageViewModel = new WelcomePageViewModel();
            readEulaPageViewModel = new ReadEulaPageViewModel();
            timeConsumePageViewModel = new TimeConsumePageViewModel();
            congratulationPageViewModel = new CongratulationPageViewModel();

            Items = new ObservableCollection<object>()
            {
                welcomePageViewModel,
                readEulaPageViewModel,
                timeConsumePageViewModel,
                congratulationPageViewModel
            };
        }

      public ObservableCollection<object> Items
        {
            get { return GetProperty(() => Items); }
            private set { SetProperty(() => Items, value); }
        }
        public WelcomePageViewModel welcomePageViewModel
        {
            get { return GetProperty(() => welcomePageViewModel); }
            private set { SetProperty(() => welcomePageViewModel, value); }
        }
        public ReadEulaPageViewModel readEulaPageViewModel
        {
            get { return GetProperty(() => readEulaPageViewModel); }
            private set { SetProperty(() => readEulaPageViewModel, value); }
        }
        public TimeConsumePageViewModel timeConsumePageViewModel
        {
            get { return GetProperty(() => timeConsumePageViewModel); }
            set { SetProperty(() => timeConsumePageViewModel, value); }
        }
        public CongratulationPageViewModel congratulationPageViewModel
        {
            get { return GetProperty(() => congratulationPageViewModel); }
            set { SetProperty(() => congratulationPageViewModel, value); }
        }

        public void Cancel(System.ComponentModel.CancelEventArgs arg)
        {
            if (System.Windows.Forms.MessageBox.Show("确定要退出安装程序吗？", "中交水运规划设计院有限公司", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                arg.Cancel = true;
        }
    }

    public abstract class WizardPageBase :ViewModelBase
    {
        public WizardPageBase()
        {
            CanCancel = CanNext = CanBack = true;
            CanFinish = false;
            ShowNext = ShowBack = ShowCancel = true;
            ShowFinish = false;
        }
        public bool CanBack
        {
            get { return GetProperty(() => CanBack); }
            set { SetProperty(() => CanBack, value); }
        }
        public bool CanFinish
        {
            get { return GetProperty(() => CanFinish); }
            set { SetProperty(() => CanFinish, value); }
        }
        public bool CanNext
        {
            get { return GetProperty(() => CanNext); }
            set { SetProperty(() => CanNext, value); }
        }
        public bool CanCancel
        {
            get { return GetProperty(() => CanCancel); }
            set { SetProperty(() => CanCancel, value); }
        }
        public bool ShowBack
        {
            get { return GetProperty(() => ShowBack); }
            set { SetProperty(() => ShowBack, value); }
        }
        public bool ShowCancel
        {
            get { return GetProperty(() => ShowCancel); }
            set { SetProperty(() => ShowCancel, value); }
        }
        public bool ShowFinish
        {
            get { return GetProperty(() => ShowFinish); }
            set { SetProperty(() => ShowFinish, value); }
        }
        public bool ShowNext
        {
            get { return GetProperty(() => ShowNext); }
            set { SetProperty(() => ShowNext, value); }
        }
        public abstract string Description { get; set;}
        public abstract string Header { get;  set; }

    }

    public class WelcomePageViewModel : WizardPageBase
    {
        public WelcomePageViewModel()
        {
            CanBack = false;
            ShowBack = false;
        }

        public override string Description
        {
            get
            {
                return "基于Mircostation CONNECTION Update3";
            }
            set { Description = value; }
        }
        public override string Header
        {
            get
            {
                return "水规院二次开发成果安装程序包";
            }
            set { Header = value; }
        }
    }

    public class ReadEulaPageViewModel : WizardPageBase
    {
        public ReadEulaPageViewModel()
        {
            CanNext = false;
        }
        public override string Description
        {
            get
            {
                return "请仔细阅读以下条款";
            }
            set { Description = value; }
        }

        public override string Header
        {
            get
            {
                return "1. 请阅读仔细阅读以下条款，同意条款并继续";
            }
            set { Header = value; }
        }
        public string Eula { get { return Properties.Resources.policyterm; } }
    }

    public class TimeConsumePageViewModel : WizardPageBase
    {
        public TimeConsumePageViewModel()
        {
            ShowBack = false;
            CanNext = false;
            RegistryKey lmkey = Registry.LocalMachine;
            RegistryKey msappkey = lmkey.OpenSubKey(@"SOFTWARE\Bentley\MicroStation\{39949BF5-7E21-4A7B-A640-6E7199B7D588}");
            MSAppPath = msappkey.GetValue("ProgramPath") as string;
            MSAppPath += @"Mdlapps";
            MSConfigPath = msappkey.GetValue("ConfigurationPath") as string;
            MSConfigPath += @"WorkSpaces";
            MinimumProgress = 0;
            MaximumProgress = dlldire.GetFiles("*", SearchOption.AllDirectories).Count() + msconfigdire.GetFiles("*", SearchOption.AllDirectories).Count();
        }
        string MSAppPath;
        string MSConfigPath;
        readonly DirectoryInfo dlldire = new DirectoryInfo(@".\DllAssem");
        readonly DirectoryInfo msconfigdire = new DirectoryInfo(@".\MSConfig");
        public override string Description
        {
            get
            {
                return "水规院二次开发";
            }
            set { Description = value; }
        }

        public override string Header
        {
            get
            {
                return "2.安装软件中...";
            }
            set { Header = value; }
        }
        public int MaximumProgress
        {
            get { return GetProperty(() => MaximumProgress); }
            set { SetProperty(() => MaximumProgress, value); }
        }
        public int MinimumProgress
        {
            get { return GetProperty(() => MinimumProgress); }
            set { SetProperty(() => MinimumProgress, value); }
        }
        public int Progress
        {
            get { return GetProperty(() => Progress); }
            set { SetProperty(() => Progress, value); }
        }
        public void Clear()
        {
            Progress = 0;
            CanNext = false;
        }
        [AsyncCommand]
        public Task StartProcess()
        {
            Clear();
            return Task.Factory.StartNew(Process);
        }
        void Process()
        {
            var MSConfigDir = new DirectoryInfo(MSConfigPath);
            var MSAppDir = new DirectoryInfo(MSAppPath);
            if (!MSConfigDir.Exists || ! MSAppDir.Exists)
            {
                Header = "未安装Mircostation CONNECT版本，请先安装！";
                return;
            }
            CopyAll(msconfigdire, MSConfigDir);
            CopyAll(dlldire, MSAppDir);
            CanNext = true;
        }
        void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            if (source.FullName.ToLower() == target.FullName.ToLower())
            {
                return;
            }
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }
            foreach (var file in source.GetFiles())
            {
                file.CopyTo(Path.Combine(target.ToString(), file.Name), true);
                Progress++;
            }
            foreach (var dir in source.GetDirectories())
            {
                DirectoryInfo nextdirectoryinfo = target.CreateSubdirectory(dir.Name);
                CopyAll(dir, nextdirectoryinfo);
            }
        }
    }

    public class CongratulationPageViewModel : WizardPageBase
    {
        public CongratulationPageViewModel()
        {
            CanFinish = true;
            CanCancel = false;
            ShowFinish = true;
            ShowNext = ShowCancel = false;
        }
        public override string Description
        {
            get
            {
                return string.Empty;
            }
            set { Description = value; }
        }

        public override string Header
        {
            get
            {
                return string.Empty;
            }
            set { Header = value; }
        }
    }
}