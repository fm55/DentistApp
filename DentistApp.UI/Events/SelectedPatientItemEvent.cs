using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DentistApp.DAL;
using Microsoft.Practices.Prism.Events;

namespace DentistApp.UI.Events
{
    public class SelectedPatientItemEvent:CompositePresentationEvent<Patient>
    {
    }
}
