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
using System.Windows.Shapes;
using DentistApp.UI.Events;
using GenericInteractionRequest.ViewModels;
using DentistApp.UI.ViewModels;
using DentistApp.DAL;

namespace DentistApp.UI
{
    /// <summary>
    /// Interaction logic for PatientDeleteDialogView.xaml
    /// </summary>
    public partial class PatientDeleteInteractionDialogView : PatientDeleteInteractionDialog, IGenericInteractionView<Patient>, IGenericAdapter<Patient>
    {
        private readonly IGenericAdapter<Patient> adapter;

        public PatientDeleteInteractionDialogView()
        {
            this.adapter = new GenericAdapter<Patient>();
            this.DataContext = this.ViewModel;
            InitializeComponent();
        }

        public void SetEntity(Patient entity)
        {
            this.ViewModel.SetEntity(entity);
        }

        public Patient GetEntity()
        {
            return this.ViewModel.GetEntity();
        }

        public IGenericViewModel<Patient> ViewModel
        {
            get { return this.adapter.ViewModel; }
        }
    }
}
