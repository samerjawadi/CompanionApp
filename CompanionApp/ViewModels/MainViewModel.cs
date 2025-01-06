using CompanionApp.Events;
using CompanionApp.Views;
using MazeProject.Models;
using MazeProject.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CompanionApp.ViewModels
{
    public class MainViewModel : BindableBase
    {
        
        IEventAggregator _eventAggregator;
        IDialogService _dialogService;

        public DelegateCommand CloseViewCommand { get; set; }
        /// <summary>/// Prism Property/// </summary>
		private UserControl _view;

        public UserControl View
        {
            get { return _view; }
            set { SetProperty(ref _view, value); }
        }

        /// <summary>/// Prism Property/// </summary>
		private Visibility _isViewVisiblity;


        public Visibility IsViewVisiblity
        {
            get { return _isViewVisiblity; }
            set { SetProperty(ref _isViewVisiblity, value); }
        }

        public MainViewModel(IEventAggregator eventAggregator, IDialogService dialogService)
        {
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _eventAggregator.GetEvent<LoadMazeAtelierEvent>().Subscribe(LoadMazeAtelierMethod);
            IsViewVisiblity = Visibility.Collapsed;
            CloseViewCommand = new DelegateCommand(CloseViewMethod);
        }

        private void CloseViewMethod()
        {
            View = null;
            IsViewVisiblity = Visibility.Collapsed;
            _eventAggregator.GetEvent<ShowSlidingViewEvent>().Publish(false);

        }

        private void LoadMazeAtelierMethod()
        {


            _dialogService.ShowDialog("PlugAndPowerOnView", new DialogParameters
            {

            }, result =>
            {
                if (result.Parameters.Count > 0)
                {
                    View = new MazeMainView();
                    IsViewVisiblity = Visibility.Visible;
                    _eventAggregator.GetEvent<ShowSlidingViewEvent>().Publish(true);

                }



            }, "PlugAndPowerOnShell");


        }
    }
}
