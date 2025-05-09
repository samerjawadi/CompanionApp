using BehaveProject.Events;
using BehaveProject.Views;
using CompanionApp.Events;
using CompanionApp.Models;
using CompanionApp.Service;
using CompanionApp.Views;
using LearningProject.Models.Events;
using LearningProject.Views;
using MazeProject.Events;
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
using System.IO.Ports;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace CompanionApp.ViewModels
{
    public class MainViewModel : BindableBase
    {

        private DispatcherTimer dispatcherTimer;


        IEventAggregator _eventAggregator;
        IDialogService _dialogService;

        private string sourceFile = string.Empty;

        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand CloseViewCommand { get; set; }
        public DelegateCommand OpenWebSiteCommand { get; set; }
        public DelegateCommand OpenWebGithubCommand { get; set; }

        
            

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

        /// <summary>/// Prism Property/// </summary>
		private bool _showPlugInAnimation;

        public bool ShowPlugInAnimation
        {
            get { return _showPlugInAnimation; }
            set { SetProperty(ref _showPlugInAnimation, value); }
        }

        /// <summary>/// Prism Property/// </summary>
		private Module _selectedModule;

        public Module SelectedModule
        {
            get { return _selectedModule; }
            set { SetProperty(ref _selectedModule, value); }
        }

        public int MyEventHandlerMethod { get; private set; }

        public MainViewModel(IEventAggregator eventAggregator, IDialogService dialogService)
        {
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _eventAggregator.GetEvent<LoadModuleEvent>().Subscribe(LoadModuleMethod);
            IsViewVisiblity = Visibility.Collapsed;

            ShowPlugInAnimation = false;


            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(500);
            dispatcherTimer.Tick += Check;

            CancelCommand = new DelegateCommand(Cancel);
            CloseViewCommand = new DelegateCommand(CloseViewMethod);

            ///
            _eventAggregator.GetEvent<LearnCloseEvent>().Subscribe(CloseViewMethod);
            _eventAggregator.GetEvent<BehaveCloseEvent>().Subscribe(CloseViewMethod);
            _eventAggregator.GetEvent<MazeCloseEvent>().Subscribe(CloseViewMethod);

            OpenWebSiteCommand = new DelegateCommand(OpenWebSiteMethod);
            OpenWebGithubCommand = new DelegateCommand(OpenWebGithubMethod);


            

        }

        private void OpenWebGithubMethod()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = IniSupport.GetGitHubUrl(),
                UseShellExecute = true
            });
        }

        private void OpenWebSiteMethod()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = IniSupport.GetGitHubUrl(),
                UseShellExecute = true
            });
        }

        #region Plug-In Animation Method
        public void Cancel()
        {
            dispatcherTimer.Stop();
            ShowPlugInAnimation = false;

        }

        private async void Check(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {

                try
                {


                    //var drive = DriveInfo.GetDrives().Where(drive => drive.DriveType == DriveType.Removable && drive.IsReady && drive.VolumeLabel.Equals("RPI-RP2", StringComparison.OrdinalIgnoreCase)).First();

                    //if (drive != null)
                    if(true)
                    {
                        List<string> oldComs = new List<string>(SerialPort.GetPortNames());

                        dispatcherTimer.Stop();

                        //string destinationPath = Path.Combine(drive.RootDirectory.FullName, "code.uf2");
                       // File.Copy(sourceFile, destinationPath, overwrite: true);
                        
                        Application.Current.Dispatcher.Invoke(async () =>
                        {

                            await Task.Delay(1000);
                            ShowPlugInAnimation = false;

                            switch (SelectedModule)
                            {
                                case Module.Learn:
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
                                    _eventAggregator.GetEvent<LoadPDFEvent>().Publish("Commande_Boutons.pdf");
                                    break;
                                case Module.Explore:

                                    View = new MazeMainView(_eventAggregator, oldComs);
                                    IsViewVisiblity = Visibility.Visible;
                                    _eventAggregator.GetEvent<ShowSlidingViewEvent>().Publish(true);

                                    break;
                                case Module.Behaviour:
                                    View = new BehaviorMainView(_eventAggregator);
                                    IsViewVisiblity = Visibility.Visible;
                                    _eventAggregator.GetEvent<ShowSlidingViewEvent>().Publish(true);
                                    _eventAggregator.GetEvent<LoadPDFEvent>().Publish("Commande_Boutons.pdf");
                                    break;
                                default:
                                    break;
                            }


                        });
                    }
                }
                catch (Exception)
                {

                }
            });


        }

        #endregion

        private void LoadModuleMethod(Module obj)
        {
            SelectedModule = obj;
            dispatcherTimer.Start();
            ShowPlugInAnimation = true;

            string sourceFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources/u2f");
            switch (obj)
            {
                case Module.Learn:
                    sourceFile = Path.Combine(sourceFolder, "BootLoader_microPython.uf2");

                    break;
                case Module.Explore:

                    sourceFile = Path.Combine(sourceFolder, "MazeCode.uf2");

                    break;
                case Module.Behaviour:
                    sourceFile = Path.Combine(sourceFolder, "Modes.uf2");

                    break;
                default:
                    break;
            }
            
            /*_dialogService.ShowDialog("PlugAndPowerOnView", new DialogParameters
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

                            View = new LearningMainView(_eventAggregator);
                            IsViewVisiblity = Visibility.Visible;
                            _eventAggregator.GetEvent<ShowSlidingViewEvent>().Publish(true);

                            break;
                        case Module.Explore:

                            View = new MazeMainView(_eventAggregator);
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

           */
        }

        private void CloseViewMethod()
        {
            View = null;
            IsViewVisiblity = Visibility.Collapsed;
            _eventAggregator.GetEvent<ShowSlidingViewEvent>().Publish(false);

        }


    }
}
