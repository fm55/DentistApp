using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DentistApp.DAL;
using Microsoft.Practices.Prism.Events;
using System.Windows.Controls;

namespace DentistApp.UI.Events
{
    public class SearchedPatientsEvent : CompositePresentationEvent<List<Patient>>
    {
    }
}
