using CompanionApp.Events;
using CompanionApp.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CompanionApp.ViewModels
{
    public class PresentationViewModel : BindableBase
    {
        IEventAggregator _eventAggregator;
        /// <summary>/// Prism Property/// </summary>
		private Visibility _isVisible;

        public Visibility IsVisible
        {
            get { return _isVisible; }
            set { SetProperty(ref _isVisible, value); }
        }

        public PresentationViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<MenuSelectionChangedEvent>().Subscribe(
                (Section selectedSection) =>
                {
                    IsVisible = selectedSection == Section.Presentation ? Visibility.Visible : Visibility.Collapsed;
                }
            );

        }
    }
}
