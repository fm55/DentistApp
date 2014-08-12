using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using System.Windows;

namespace DentistApp.UI.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public bool ShouldDelete()
        {
            if (MessageBoxResult.No == MessageBox.Show("Are you sure?", "Confirm", MessageBoxButton.YesNo)) return false;
            return true;
        }
    }
}

