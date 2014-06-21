using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DentistApp.UI.ViewModels;

namespace DentistApp.UI.Frames
{
    /// <summary>
    /// Interaction logic for Operations.xaml
    /// </summary>
    public partial class Appointments : Page
    {
        public Appointments()
        {
            InitializeComponent();
            this.DataContext = new AppointmentViewModel();
        }
    }
}
