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
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;

namespace DentistApp.UI.Frames
{
    /// <summary>
    /// Interaction logic for Operations.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public NotesViewModel MethodDataContext{get;set;}
        public HomePage(IUnityContainer unityContainer, IEventAggregator eventAggregator)
        {
            InitializeComponent();
            this.DataContext = new HomeViewModel(unityContainer, eventAggregator);
            MethodDataContext = new NotesViewModel();
        }
    }
}
