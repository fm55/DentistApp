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
using DentistApp.DAL;
using DentistApp.UI.Commands;
using DentistApp.BL;

namespace DentistApp.UI.UserControls
{
    /// <summary>
    /// Interaction logic for AppointmentUserControl.xaml
    /// </summary>
    public partial class AppointmentUserControl : UserControl
    {
        NoteController NoteController = new NoteController();
       public AppointmentUserControl()
        {
            InitializeComponent();
            MethodDataContext = new NoteViewModel();
        }

       public event EventHandler RaiseClosed;
       public object MethodDataContext { get; set; }
    }
}
