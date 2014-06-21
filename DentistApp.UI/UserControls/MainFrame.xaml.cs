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

namespace DentistApp.UI.UserControls
{
    /// <summary>
    /// Interaction logic for MainFrame.xaml
    /// </summary>
    public partial class MainFrame : UserControl, IView
    {
        public MainFrame(IMainFrameViewModel mainFrameViewModel)
        {
            InitializeComponent();
            DataContext = mainFrameViewModel;
        }

        public IViewModel ViewModel
        {
            get
            {
                return (IMainFrameViewModel)DataContext;
            }
            set
            {
                DataContext = value;
            }
        }
    }
}
