using CompanionApp.Events;
using CompanionApp.Models;
using CompanionApp.Models.Classes;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace CompanionApp.ViewModels
{
    public class ModulesViewModel : BindableBase
    {
        IEventAggregator _eventAggregator;
        /// <summary>/// Prism Property/// </summary>
		private Visibility _isVisible;

        public Visibility IsVisible
        {
            get { return _isVisible; }
            set { SetProperty(ref _isVisible, value); }
        }



        public ModulesViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;


        }
    }
}
