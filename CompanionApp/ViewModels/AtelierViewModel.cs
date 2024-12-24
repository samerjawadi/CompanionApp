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
    public class AtelierViewModel : BindableBase
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
        private ObservableCollection<CarthaModule> _atelier;

        public ObservableCollection<CarthaModule> Atelier
        {
            get { return _atelier; }
            set { SetProperty(ref _atelier, value); }
        }


        public AtelierViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<MenuSelectionChangedEvent>().Subscribe(
                (Section selectedSection) =>
                {
                    IsVisible = selectedSection == Section.Ateliers ? Visibility.Visible : Visibility.Collapsed;
                }
            );
            Atelier = new ObservableCollection<CarthaModule>();
            string folderPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "ModulesImages");

            Atelier.Add(new CarthaModule("Atelier 1", $"{folderPath}/mod.png", "______________",_eventAggregator));

        }

    }
}
