//Author: Gerard Castelló Viader
//Date: 10/16/2011

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DentistApp.UI.ViewModels;

namespace GenericInteractionRequest.ViewModels
{
    public interface IGenericAdapter<T>
    {
        IGenericViewModel<T> ViewModel { get; }
    }
}
