using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace PDIWT_MS_CZ.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
        }

        #region 实例化
        public MainViewModel MainVM => ServiceLocator.Current.GetInstance<MainViewModel>();

        #endregion

        public static void Cleanup()
        {
            //TODO Clear the viewmodels
        }


    }
}
