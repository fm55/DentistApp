using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Microsoft.Practices.Prism.Events;

using DentistApp.DAL;
using DentistApp.BL;
using System.Windows.Media;
using DentistApp.UI.Commands;

namespace DentistApp.UI.ViewModels
{
    public class NoteViewModel:BaseViewModel
    {
        private IEventAggregator _eventAggregator;
        Note _SelectedNote;
        NoteController NoteController { get { return new NoteController(); } }
        public string Description { get; set; }
        public int NoteId { get; set; }
        public bool IsReadOnly { get; set; }
        public DateTime DateCreated { get; set; }
        public int? PatientId { get; set; }
        public int? OperationId { get; set; }
        public int? ToothId { get; set; }
        private Note SelectedNote { get {
            return _SelectedNote;
        } set {
            _SelectedNote = value;
        
        } }
        
        public int? AppointmentId { get; set; }
        public ICommand Edit { get{return new DelegateCommand(editNote);}}
        public event EventHandler RaiseClosed;


        public ICommand Save
        {
            get
            {
                return new DelegateCommand((object o) =>
            {
                NoteController.SaveNote(new Note { NoteId = this.NoteId, Description = this.Description }, this.PatientId, this.OperationId, this.ToothId, this.AppointmentId);
                IsReadOnly = true; RaisePropertyChanged("IsReadOnly");
                if (RaiseClosed != null)
                    RaiseClosed(null, null);
            }
                );
            }
        }

        public ICommand Delete { get { return new DelegateCommand((object o) =>{


            if (!ShouldDelete())return; NoteController.Delete(SelectedNote); }); 
        } }


        public NoteViewModel()
        {

        }
        public NoteViewModel(Note n)
        {
            SelectedNote = n;
            Description = n.Description;
            NoteId = n.NoteId;
            PatientId = n.PatientId;
            OperationId = n.OperationId;
            ToothId = n.ToothId;
            AppointmentId = n.AppointmentId;
            DateCreated = n.DateCreated;
            if (NoteId==0)
                IsReadOnly = true;
        }
        public NoteViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }
        private void editNote(object o)
        {
            IsReadOnly = false;
            RaisePropertyChanged("IsReadOnly");
        }

       
    }
}
