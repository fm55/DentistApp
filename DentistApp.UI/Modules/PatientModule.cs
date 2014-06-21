using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using DentistApp.UI.UserControls;
using DentistApp.UI.ViewModels;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.Events;
using DentistApp.UI.Events;

namespace DentistApp.UI.Modules
{
    public class PatientModule:IModule
    {
        IUnityContainer _container;
        IRegionManager _manager;
        public PatientModule(IUnityContainer container, IRegionManager manager)
        {
            _container = container;
            _manager = manager;
        }

        public void Initialize()
        { 
            _container.RegisterType<PatientList>();
            _container.RegisterType<PatientDetails>();
            _container.RegisterType<IPatientListViewModel, PatientListViewModel>();
            _container.RegisterType<IPatientDetailsViewModel, PatientDetailsViewModel>();
            _container.RegisterType<IPatientViewModel, PatientViewModel>();

            _manager.RegisterViewWithRegion("PatientList", typeof(PatientList));
            _manager.RegisterViewWithRegion("PatientDetails", typeof(PatientDetails));
            
        }
    }
}
