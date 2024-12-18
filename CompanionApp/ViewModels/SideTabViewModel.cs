using CompanionApp.Events;
using CompanionApp.Models;
using DMSkin.Core.Common;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CompanionApp.ViewModels
{
    public class SideTabViewModel : BindableBase
    {
        IEventAggregator _eventAggregator;
        /// <summary>/// Prism Property/// </summary>
		private Section _currentSection;

        public Section CurrentSection
        {
            get { return _currentSection; }
            set { SetProperty(ref _currentSection, value); }
        }
        public DelegateCommand<object> SelectMenuCommand{ get; set; }

        public SideTabViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            SelectMenuCommand = new DelegateCommand<object>(ExecuteSelectCommand);
            CurrentSection = Section.Presentation;
            _eventAggregator.GetEvent<MenuSelectionChangedEvent>().Publish(CurrentSection); 
        }

        private void ExecuteSelectCommand(object parameter)
        {
            string action = parameter as string;
            switch (action)
            {
                case "Presentation":
                    CurrentSection = Section.Presentation;
                    break;
                case "Modules":
                    CurrentSection = Section.Modules;
                    break;
                case "Atelier":
                    CurrentSection = Section.Ateliers;
                    break;
                case "Documentation":
                    CurrentSection = Section.Documentation;
                    break;
                default:
                    throw new ArgumentException("Unknown command parameter");
            }
            _eventAggregator.GetEvent<MenuSelectionChangedEvent>().Publish(CurrentSection);

        }
    }
}
