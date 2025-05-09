using CompanionApp.Events;
using CompanionApp.Models.Classes;
using CompanionApp.Service;
using DMSkin.Core.MVVM;
using Prism.Events;
using Prism.Mvvm;
using System;

namespace CompanionApp.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        IEventAggregator _eventAggregator;
        public DelegateCommand UpdateCommand { get; set; }
        /// <summary>/// Prism Property/// </summary>
		private bool _isUpToDate;

        public bool IsUpToDate
        {
            get { return _isUpToDate; }
            set { SetProperty(ref _isUpToDate, value); }
        }

        /// <summary>/// Prism Property/// </summary>
        private string _version;

        public string Version
        {
            get { return _version; }
            set { SetProperty(ref _version, value); }
        }

        /// <summary>/// Prism Property/// </summary>
		private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        /// <summary>/// Prism Property/// </summary>
		private string _updateContent;

        public string UpdateContent
        {
            get { return _updateContent; }
            set { SetProperty(ref _updateContent, value); }
        }


        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<NewVersionAvaliableEvent>().Subscribe(NewVersionAvaliableMethod);
            Version = GetVersion();

            Title = $"Companion Application ({Version})";
            IsUpToDate = true;

            UpdateCommand = new DelegateCommand(UpdateMethod);
        }

        private void UpdateMethod(object obj)
        {
            CheckVersion.OpenNewVersion();
        }

        private void NewVersionAvaliableMethod(string version)
        {
            if (!string.IsNullOrEmpty(version))
            {
                UpdateContent = $"Update to {version}";
                IsUpToDate = false;
            }
        }


        public  string GetVersion()
        {
            string version = "";

            string iniFilePath = System.IO.Path.Combine("Resources", "Settings.ini");

            IniFile iniFile = new IniFile(iniFilePath);

            try
            {
                version = iniFile.ReadValue("Settings", "Version");

            }
            catch (Exception)
            {

            }
            return version;
        }
    }
}
