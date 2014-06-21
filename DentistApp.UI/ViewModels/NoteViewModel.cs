using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Commands;
using DentistApp.DAL;
using DentistApp.BL;

namespace DentistApp.UI.ViewModels
{
    public class NoteViewModel:BaseViewModel
    {
        private IEventAggregator _eventAggregator;
        NoteController NoteController { get { return new NoteController(); } }
        public string Description { get; set; }
        public int NoteId { get; set; }
        public bool IsReadOnly { get; set; }
        public DateTime DateCreated { get; set; }
        public ICommand Edit { get{return new DelegateCommand(editNote);}}
        public ICommand Save { get { return new DelegateCommand(() => { NoteController.SaveNote(new Note { NoteId = this.NoteId, Description = this.Description }); }); } }

        private Note SelectedNote { get; set; }
        public ICommand Delete { get { return new DelegateCommand(() => {if (ShouldDelete())return; NoteController.Delete(SelectedNote); }); } }
        public NoteViewModel(Note n)
        {
            SelectedNote = n;
            Description = n.Description;
            NoteId = n.NoteId;
            DateCreated = n.DateCreated;
            IsReadOnly = true;
        }
        public NoteViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        private void editNote()
        {
            IsReadOnly = false;
            RaisePropertyChanged("IsReadOnly");
        }
    }
}
