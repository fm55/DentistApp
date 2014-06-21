using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;
using System.Windows.Controls;
using DentistApp.UI.Events;
using DentistApp.UI.Frames;
using Microsoft.Practices.Unity;

namespace DentistApp.UI.ViewModels
{
    public class MainFrameViewModel : BaseViewModel, IMainFrameViewModel
    {
        private IEventAggregator _eventAggregator;
        public Page SelectedPage { get; set; }
        public MainFrameViewModel(IUnityContainer unityContainer, IEventAggregator eventAggregator)
        {
            //var vm = new HomepageViewModel(unityContainer, _eventAggregator);
            SelectedPage = new HomePage(unityContainer, eventAggregator);
            RaisePropertyChanged("SelectedPage");
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<SelectedMenuItemEvent>().Subscribe((page) =>
            {
                SelectedPage = page;
                RaisePropertyChanged("SelectedPage");
            });
        }
    }
}
