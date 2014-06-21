using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using DentistApp.UI.UserControls;
using DentistApp.UI.ViewModels;
using Microsoft.Practices.Prism.Regions;

namespace DentistApp.UI.Modules
{
    public class ModuleAModule:IModule
    {
        IUnityContainer _container;
        IRegionManager _manager;
        public ModuleAModule(IUnityContainer container, IRegionManager manager)
        {
            _container = container;
            _manager = manager;
        }

        public void Initialize()
        {
            _container.RegisterType<Menu>();
            _container.RegisterType<MainFrame>();
            _container.RegisterType<IMenuViewModel, MenuViewModel>();
            _container.RegisterType<IMainFrameViewModel, MainFrameViewModel>();

            _manager.RegisterViewWithRegion("Menu", typeof(Menu));
            _manager.RegisterViewWithRegion("MainFrame", typeof(MainFrame));
            
        }
    }
}
