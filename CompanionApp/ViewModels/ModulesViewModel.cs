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

        /// <summary>/// Prism Property/// </summary>
        private ObservableCollection<CarthaModule> _modules;

        public ObservableCollection<CarthaModule> Modules
        {
            get { return _modules; }
            set { SetProperty(ref _modules, value); }
        }


        public ModulesViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<MenuSelectionChangedEvent>().Subscribe(
                (Section selectedSection) =>
                {
                    IsVisible = selectedSection == Section.Modules ? Visibility.Visible : Visibility.Collapsed;
                }
            );
            Modules = new ObservableCollection<CarthaModule>();
            string folderPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "ModulesImages");

            Modules.Add(new CarthaModule("Module 1",$"{folderPath}/mod.png","Apprendre à programmer"));

        }
    }
}
