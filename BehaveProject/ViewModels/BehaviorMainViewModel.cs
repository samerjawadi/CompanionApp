using BehaveProject.Events;
using BehaveProject.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace BehaveProject.ViewModels
{
    public class BehaviorMainViewModel : BindableBase
    {
        IEventAggregator _eventAggregator;
        public DelegateCommand CloseViewCommand { get; set; }

        /// <summary>/// Prism Property/// </summary>
		private ObservableCollection<Mode> _modes;

        public ObservableCollection<Mode> Modes
        {
            get { return _modes; }
            set { SetProperty(ref _modes, value); }
        }
        /// <summary>/// Prism Property/// </summary>
		private Mode _selecetdMode;

        public Mode SelecetdMode
        {
            get { return _selecetdMode; }
            set { SetProperty(ref _selecetdMode, value); }
        }

        public BehaviorMainViewModel(IEventAggregator eventAggregator)
        {
            CloseViewCommand = new DelegateCommand(CloseViewMethod);

            _eventAggregator = eventAggregator;
            Modes = new ObservableCollection<Mode>();

            Modes.Add(new Mode(_eventAggregator, "Éviteur d'obstacles", new SolidColorBrush(Color.FromRgb(247, 148, 28)), true)); // #F7941C
            Modes.Add(new Mode(_eventAggregator, "Suiveur ", new SolidColorBrush(Color.FromRgb(0, 255, 0)))); // #00FF00
            Modes.Add(new Mode(_eventAggregator, "Amical", new SolidColorBrush(Color.FromRgb(88, 127, 237)))); // #587fed
            Modes.Add(new Mode(_eventAggregator, "Obéissant", new SolidColorBrush(Color.FromRgb(241, 100, 162)))); // #F164A2
            _eventAggregator.GetEvent<SelectedModeEvent>().Subscribe(SelectedModeMethod);
            SelecetdMode = Modes[0];
        }
        private void CloseViewMethod()
        {
            _eventAggregator.GetEvent<BehaveCloseEvent>().Publish();
        }
        private void SelectedModeMethod(Mode obj)
        {

            foreach(var mode in Modes)
            {
                if(mode.Name == obj.Name)
                {
                    mode.IsSelected = true;
                    SelecetdMode = mode;

                }
                else
                {
                    mode.IsSelected = false;
                }
            }
        }
    }
}
