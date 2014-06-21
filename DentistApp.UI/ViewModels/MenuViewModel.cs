using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using DentistApp.UI.Frames;
using DentistApp.UI.Commands;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Events;
using DentistApp.UI.Events;


namespace DentistApp.UI.ViewModels
{
    public class MenuViewModel:BaseViewModel,IMenuViewModel
    {
        private IUnityContainer _container;
        private IEventAggregator _eventAggregator;
        public Page SelectedPage { get; set; }
        public ICommand Navigate { get { return new DelegateCommand(NavigateCommand, canExecute); } }
        public void NavigateCommand(object o)
        {
            if (o.Equals("Appointments"))
                SelectedPage = new Appointments();

            else if (o.Equals("Operations"))
                SelectedPage = new Operations();

            else if (o.Equals("Home"))
                SelectedPage = new HomePage(_container, _eventAggregator);

            else SelectedPage = new Patients(_container.Resolve<IPatientViewModel>()); ;
            RaisePropertyChanged("SelectedPage");
            _eventAggregator.GetEvent<SelectedMenuItemEvent>().Publish(SelectedPage);           

        }


        public bool canExecute(object e)
        {
            return true;
        }
        public MenuViewModel(IUnityContainer container, IEventAggregator eventAggregator)
        {
            _container = container;
            _eventAggregator = eventAggregator;
        }
    }
}
