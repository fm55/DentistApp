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
    /// Interaction logic for NoteUserControl.xaml
    /// </summary>
    public partial class AppointmentNoteUserControl : UserControl
    {
        public bool ShouldDelete()
        {
            if (MessageBoxResult.No == MessageBox.Show("Are you sure?", "Confirm", MessageBoxButton.YesNoCancel)) return false;
            return true;
        }

        NoteController NoteController { get { return new NoteController(); } }

        int note;
        public event EventHandler RaiseClosed;
        public int NoteId
        {
            get
            {
                return note;
            }
            set
            {
                note = value;
            }
        }

        public AppointmentNoteUserControl()
        {
            var nm = new NoteViewModel();
            nm.RaiseClosed += new EventHandler(nm_RaiseClosed);
            InitializeComponent();
            this.MethodDataContext = new NotesViewModel();
            //this.DataContext = nm;
           
        }

        public AppointmentNoteUserControl(NoteViewModel nm)
        {
            nm.RaiseClosed += new EventHandler(nm_RaiseClosed);
            InitializeComponent();
            //this.DataContext = nm;

        }

        void nm_RaiseClosed(object sender, EventArgs e)
        {
            if (RaiseClosed != null)
                RaiseClosed(null, null);
        }

        public ICommand Save
        {
            get
            {
                return new DelegateCommand((object o) =>
                {
                    //NoteController.SaveNote(new Note { NoteId = this.NoteId, Description = this.Description }, this.PatientId, this.OperationId, this.ToothId, this.AppointmentId);
                    if (RaiseClosed != null)
                        RaiseClosed(null, null);
                }
                );
            }
        }

        public ICommand Delete
        {
            get
            {
                return new DelegateCommand((object o) =>
                {
                    if (!ShouldDelete()) return; 
                    //NoteController.Delete(SelectedNote);
                });
            }
        }


        public object MethodDataContext { get; set; }
    }
}
