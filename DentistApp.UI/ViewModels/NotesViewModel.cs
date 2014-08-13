using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using DentistApp.DAL;
using DentistApp.BL;
using System.Windows;
using System.Windows.Input;
using DentistApp.UI.Commands;
using DentistApp.Model;
using System.ComponentModel;
using System.Windows.Data;
using DentistApp.UI.UserControls;
using System.Windows.Controls;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Events;
using DentistApp.UI.Events;

namespace DentistApp.UI.ViewModels
{
    public class NotesViewModel : BaseViewModel
    {
        #region Properties
        /////properties////
        //list of appointments
        public ObservableCollection<Note> Notes { get; set; }
        public NoteController NoteController = new NoteController();

        Window noteWindow;
        public NotesViewModel()
        {
            _items = Items;
        }
        public NotesViewModel(IUnityContainer unityContainer, IEventAggregator eventAggregator)
        {
            _items = Items;
        }

        public ICommand CreateNote
        {
            get
            {
                return new DelegateCommand((object o) =>
                {
                    var vm = new CreateNoteUserControl(new NoteViewModel());
                    vm.RaiseClosed += new EventHandler(viewModel_RaisedClosed);
                    noteWindow = new Window
                    {
                        Title = "New Note",
                        Content = vm,
                        SizeToContent = SizeToContent.WidthAndHeight,
                        ResizeMode = ResizeMode.NoResize
                    };

                    noteWindow.ShowDialog();
                    var notes = NoteController.GetNotesForSystem();
                    Notes = new ObservableCollection<Note>(notes.OrderByDescending(d => d.DateCreated));
                    RaisePropertyChanged("Notes");
                },
                (object o) =>
                {
                    return true;
                });
            }
        }
        void viewModel_RaisedClosed(object sender, EventArgs e)
        {
            if (noteWindow != null)
                noteWindow.Close();

            var notes = NoteController.GetNotesForSystem();
            Notes = new ObservableCollection<Note>(notes.OrderByDescending(d => d.DateCreated));
                    RaisePropertyChanged("Notes");
        }

        private ObservableCollection<Note> _items;
        private IUnityContainer unityContainer;
        private IEventAggregator eventAggregator;
        public ObservableCollection<Note> Items
        {
            get
            {
                var notes = NoteController.GetNotesForSystem();
                    Notes = new ObservableCollection<Note>(notes.OrderByDescending(d => d.DateCreated));
                    RaisePropertyChanged("Notes");
                    _items = Notes;

                return _items;
            }
        }

        
        #endregion

        IUnityContainer _container { get; set; }
        IEventAggregator _eventAggregator { get; set; }
       
        
        
        public bool canExecute(object e)
        {
            return true;
        }

        public string Description { get; set; }
        public int NoteId { get; set; }
        public bool IsReadOnly { get; set; }
        public DateTime DateCreated { get; set; }
        public int? PatientId { get; set; }
        public int? OperationId { get; set; }
        public int? ToothId { get; set; }

        public int? AppointmentId { get; set; }
        public event EventHandler RaiseClosed;

        private bool Validate(Note note)
        {
            if (string.IsNullOrWhiteSpace(note.Description))
            {
                MessageBox.Show("Please enter a description for the note");
                return false;
            }
            return true;
        }
        public ICommand Save
        {
            get
            {
                return new DelegateCommand((object o) =>
            {
                var note = o as Note;
                if (!Validate(note)) return;
                NoteController.SaveNote(note);
                IsReadOnly = true; RaisePropertyChanged("IsReadOnly");
                if (RaiseClosed != null)
                    RaiseClosed(null, null);
            }
                );
            }
        }

        private Note SelectedNote { get; set; }
        public ICommand Delete { get { return new DelegateCommand((object o) =>{
            if (!ShouldDelete())return;
            var note = o as Note;
            NoteController.Delete(note);
            Notes = Items;
            RaisePropertyChanged("Notes");
        }); 
        } }

        

    }
}
