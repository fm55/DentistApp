using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DentistApp.UI
{
    public interface IView
    {
        IViewModel ViewModel { get; set; }
    }
}
