using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using DentistApp.BL;
using DentistApp.DAL;
using DentistApp.UI.Commands;
using System.Windows;
using DentistApp.UI.UserControls;

namespace DentistApp.UI.ViewModels
{
    public class AppointmentUserControlViewModel:BaseViewModel
    {
        public Patient Patient { get; set; }
        AppointmentController AppointmentController {get;set;}

        public AppointmentUserControlViewModel() {
            //AppointmentController = new AppointmentController();
        }

        
    }
}
