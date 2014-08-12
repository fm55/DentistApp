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

namespace DentistApp.UI.UserControls
{
    /// <summary>
    /// Interaction logic for NoteUserControl.xaml
    /// </summary>
    public partial class CreateNoteUserControl : UserControl
    {
        public event EventHandler RaiseClosed;
        public CreateNoteUserControl()
        {
            var nm = new NoteViewModel();
            nm.RaiseClosed += new EventHandler(nm_RaiseClosed);
            InitializeComponent();
           //this.DataContext = nm;
           
        }

        public CreateNoteUserControl(NoteViewModel nm)
        {
            nm.RaiseClosed += new EventHandler(nm_RaiseClosed);
            InitializeComponent();
            this.DataContext = nm;

        }

        void nm_RaiseClosed(object sender, EventArgs e)
        {
            if (RaiseClosed != null)
                RaiseClosed(null, null);
        }

    }
}
