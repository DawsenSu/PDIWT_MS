
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace PDIWT_MS_Tool.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                // SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
            }
            else
            {
                // SimpleIoc.Default.Register<IDataService, DataService>();
            }

            SimpleIoc.Default.Register<CellFastPutViewModel>();
            SimpleIoc.Default.Register<CellsArmorPutViewModel>();
            SimpleIoc.Default.Register<ModelCreatorViewModel>();
            SimpleIoc.Default.Register<LevelExportViewModel>();
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public CellFastPutViewModel CellFastPutVM => ServiceLocator.Current.GetInstance<CellFastPutViewModel>();
        public CellsArmorPutViewModel CellSArmorPutVM => ServiceLocator.Current.GetInstance<CellsArmorPutViewModel>();
        public ModelCreatorViewModel ModelCreatorVM => ServiceLocator.Current.GetInstance<ModelCreatorViewModel>();
        public LevelExportViewModel LevelExportVM => ServiceLocator.Current.GetInstance<LevelExportViewModel>();


    }
}