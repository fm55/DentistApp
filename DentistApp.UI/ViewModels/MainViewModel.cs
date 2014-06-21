using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using DentistApp.DAL;
using DentistApp.BL;
using System.Windows;
using System.Windows.Input;
using DentistApp.UI.Commands;
using DentistApp.Model;
using System.Windows.Controls;
using DentistApp.UI.Frames;
using Microsoft.Practices.Unity;

namespace DentistApp.UI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        IUnityContainer _container;
        public MainViewModel(IUnityContainer container)
        {
            container = _container;
        }

        public Page SelectedPage { get; set; }
        public ICommand Navigate { get { return new DelegateCommand(NavigateCommand, canExecute); } }
        public void NavigateCommand(object o)
        {
            if (o.Equals("Appointments"))
                SelectedPage = new Appointments();

            else if (o.Equals("Operations"))
                SelectedPage = new Operations();

            else SelectedPage = new Patients(_container.Resolve<IPatientViewModel>());
            RaisePropertyChanged("SelectedPage");
        }


        public bool canExecute(object e)
        {
            return true;
        }


    }
}
