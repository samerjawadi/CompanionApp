using MazeProject.Events;
using MazeProject.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MazeProject.ViewModels
{
    public class MazeMainViewModel : BindableBase
    {
        private IDialogService _dialogService;
        IEventAggregator _eventAggregator;
        /// <summary>/// Prism Property/// </summary>
		private Robot _playingRobot;

        public Robot PlayingRobot
        {
            get { return _playingRobot; }
            set { SetProperty(ref _playingRobot, value); }
        }

        /// <summary>/// Prism Property/// </summary>
		private Map _map;

        public Map Map
        {
            get { return _map; }
            set { SetProperty(ref _map, value); }
        }
        public DelegateCommand GoForwardCommand { get; set; }
        public DelegateCommand GoBackwardCommand { get; set; }
        public DelegateCommand GoLeftCommand { get; set; }
        public DelegateCommand GoRightCommand { get; set; }
        public DelegateCommand SelecetMapCommand { get; set; }

        public DelegateCommand CloseViewCommand { get; set; }


        public MazeMainViewModel(IDialogService dialogService,IEventAggregator eventAggregator)
        {
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
            PlayingRobot = new Robot();
            Map = new Map();

            GoForwardCommand = new DelegateCommand(GoForwardMethod);
            GoBackwardCommand = new DelegateCommand(GoBackwardMethod);
            GoLeftCommand = new DelegateCommand(GoLeftMethod);
            GoRightCommand = new DelegateCommand(GoRightMethod);
            SelecetMapCommand = new DelegateCommand(SelecetMapMethod);


            CloseViewCommand = new DelegateCommand(CloseViewMethod);



        }

        private void CloseViewMethod()
        {
            _eventAggregator.GetEvent<MazeCloseEvent>().Publish();
        }
        private void SelecetMapMethod()
        {

            _dialogService.ShowDialog("SelectMapImageView", new DialogParameters
            {

            }, result =>
            {
                if (result.Parameters.Count > 0)
                {
                    Map.Update(result.Parameters.GetValue<string>("image"), result.Parameters.GetValue<int>("RowNumber"), result.Parameters.GetValue<int>("ColumnsNumber"), result.Parameters.GetValue<int>("Id"));
                    PlayingRobot = new Robot();

                }


            }, "SelectMapImageShell");


        }
        private void GoForwardMethod()
        {

            if ((PlayingRobot.CurrentStep + 1) > PlayingRobot.MaxSteps) return;
            if(Map.GoForwardMethod())
            {
                PlayingRobot.GoForwardMethod();
            }
            

        }
        private void GoBackwardMethod()
        {
            if ((PlayingRobot.CurrentStep + 1) > PlayingRobot.MaxSteps) return;
            if (Map.GoBackwardMethod())
            {
                PlayingRobot.GoBackwardMethod();
            }

        }
        private void GoLeftMethod()
        {
            if ((PlayingRobot.CurrentStep + 1) > PlayingRobot.MaxSteps) return;
            if (Map.GoLeftMethod())
            {
                PlayingRobot.GoLeftMethod();
            }

        }
        private void GoRightMethod()
        {
            if ((PlayingRobot.CurrentStep + 1) > PlayingRobot.MaxSteps) return;
            if (Map.GoRightMethod())
            {
                PlayingRobot.GoRightMethod();
            }
        }
    }
}
