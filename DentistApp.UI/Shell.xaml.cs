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
using DentistApp.UI.Frames;
using DentistApp.UI.Commands;
using DentistApp.UI.ViewModels;
using Microsoft.Practices.Unity;

namespace DentistApp.UI
{/// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Shell : Window
    {
        IUnityContainer _container;
        public Shell(IUnityContainer container)
        {
            InitializeComponent();
            _container = container;
            Loaded += new RoutedEventHandler(Shell_Loaded);
        }

        void Shell_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = _container.Resolve<IMainFrameViewModel>();
        }

    }
}
