using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Prism;
using log4net;
using DentistApp.BL;

namespace DentistApp.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public App()
        {
            Dispatcher.UnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(Dispatcher_UnhandledException);
            
        }

        private void CheckForRequiredData()
        {
            ToothController tc = new ToothController();
            if (tc.List().Count() == 0)
            {
                MessageBox.Show("Please run the first step in the installation guide to load the teeth into the application.");
                Application.Current.Shutdown();
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            log4net.Config.XmlConfigurator.Configure();
            Bootstrapper b = new DentistApp.UI.Bootstrapper();
            b.Run();
            CheckForRequiredData();
            //Log.Info("Hello World");
            base.OnStartup(e);
            Log.Info("Application Starting");
        }


        void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Application encountered an error.  It has been logged in the log file.");
            Log.Error(e.Exception.ToString());
            e.Handled = true;
        }
    }
}
