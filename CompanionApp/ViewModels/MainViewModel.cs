using CompanionApp.Events;
using CompanionApp.Models;
using CompanionApp.Views;
using LearningProject.Views;
using MazeProject.Models;
using MazeProject.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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
            _eventAggregator.GetEvent<LoadModuleEvent>().Subscribe(LoadModuleMethod);
            IsViewVisiblity = Visibility.Collapsed;
            CloseViewCommand = new DelegateCommand(CloseViewMethod);
        }

        private void LoadModuleMethod(Module obj)
        {
            string sourceFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CarthaSoft");
            string sourceFile = Path.Combine(sourceFolder, "CarthaSoft.html");
            Process.Start(new ProcessStartInfo
            {
                FileName = sourceFile,
                UseShellExecute = true // Required for opening files in the default application
            });

            View = new LearningMainView(_eventAggregator);
            IsViewVisiblity = Visibility.Visible;
            _eventAggregator.GetEvent<ShowSlidingViewEvent>().Publish(true);

            return;
            _dialogService.ShowDialog("PlugAndPowerOnView", new DialogParameters
            {
                {"module",obj}
            }, result =>
            {
                if (result.Parameters.Count > 0)
                {
                    switch(obj)
                    {
                        case Module.Learn:
                            string sourceFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CarthaSoft");
                            string sourceFile = Path.Combine(sourceFolder, "CarthaSoft.html");
                            Process.Start(new ProcessStartInfo
                            {
                                FileName = sourceFile,
                                UseShellExecute = true // Required for opening files in the default application
                            });

                            View = new MazeMainView();
                            IsViewVisiblity = Visibility.Visible;
                            _eventAggregator.GetEvent<ShowSlidingViewEvent>().Publish(true);

                            break;
                        case Module.Explore:

                            View = new MazeMainView();
                            IsViewVisiblity = Visibility.Visible;
                            _eventAggregator.GetEvent<ShowSlidingViewEvent>().Publish(true); 

                            break;
                        case Module.Behaviour:
                            break;
                        default:
                            break;
                    }


                }



            }, "PlugAndPowerOnShell");
        }

        private void CloseViewMethod()
        {
            View = null;
            IsViewVisiblity = Visibility.Collapsed;
            _eventAggregator.GetEvent<ShowSlidingViewEvent>().Publish(false);

        }


    }
}
