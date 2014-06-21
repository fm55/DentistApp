//Author: Gerard Castelló Viader
//Date: 10/16/2011

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace DentistApp.UI.ViewModels
{
    public interface IGenericViewModel<T> : IGenericInteractionView<T>, INotifyPropertyChanged
    {
        T Entity { get; set; }
    }
}
