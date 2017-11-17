using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace PDIWT_MS_PiledWharf.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<DrawPileAxisViewModel>();
        }
        public MainViewModel MainVM => ServiceLocator.Current.GetInstance<MainViewModel>();

        public DrawPileAxisViewModel DrawPileAxisVM => ServiceLocator.Current.GetInstance<DrawPileAxisViewModel>();
        public static void Cleanup()
        {

        }
    }
}
