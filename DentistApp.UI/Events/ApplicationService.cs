using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DentistApp.UI.Events
{
    using Prism = Microsoft.Practices.Prism.Events;
    internal sealed class ApplicationService
    {
        private ApplicationService() { }

        private static readonly ApplicationService _instance = new ApplicationService();

        internal static ApplicationService Instance { get { return _instance; } }

        private Prism.IEventAggregator _eventAggregator;
        internal Prism.IEventAggregator EventAggregator
        {
            get
            {
                if (_eventAggregator == null)
                    _eventAggregator = new Prism.EventAggregator();

                return _eventAggregator;
            }
        }
    }
}
