﻿using CompanionApp.Service;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using System;
using System.IO;
using System.Windows.Threading;
using System.Windows;
using System.Diagnostics;

namespace CompanionApp.ViewModels
{
    public class PlugAndPowerOnViewModel : BindableBase,IDialogAware
    {
        private DispatcherTimer dispatcherTimer;
        private string sourceFile = string.Empty;
        /// <summary>/// Prism Property/// </summary>
		private bool _loading;

        public bool Loading
        {
            get { return _loading; }
            set { SetProperty(ref _loading, value); }
        }

        public DelegateCommand CancelCommand { get; set; }

        public string Title => "";

        public PlugAndPowerOnViewModel()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(500);
            dispatcherTimer.Tick += Check;

            CancelCommand = new DelegateCommand(Cancel);
            Loading = false;
        }

        private async void Check(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {

                try
                {
                    var drive = DriveInfo.GetDrives().Where(drive => drive.DriveType == DriveType.Removable && drive.IsReady && drive.VolumeLabel.Equals("RPI-RP2", StringComparison.OrdinalIgnoreCase)).First();

                    if (drive != null)
                    {
                        dispatcherTimer.Stop();

                        string destinationPath = Path.Combine(drive.RootDirectory.FullName, "code.uf2");

                        File.Copy(sourceFile, destinationPath, overwrite: true);

                        Application.Current.Dispatcher.Invoke(async () =>
                        {

                            await Task.Delay(1000);
                            DialogResult dialogResult = new DialogResult();
                            dialogResult.Parameters.Add("ok", true);
                            RequestClose(dialogResult);

                        });
                    }
                }
                catch (Exception)
                {

                }
            });


        }

        public event Action<IDialogResult> RequestClose;

        public void Cancel() 
        {
            dispatcherTimer.Stop();
            RequestClose(new DialogResult());
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }


        public void OnDialogOpened(IDialogParameters parameters)
        {
            string programmeToLoad = parameters.GetValue<string>("explore");
            string sourceFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources/u2f");

            if (programmeToLoad == "explore")
            {
                sourceFile = Path.Combine(sourceFolder, "BootLoader_microPython.uf2");
            }
            else if (programmeToLoad == "micropython")
            {
                sourceFile = Path.Combine(sourceFolder, "BootLoader_microPython.uf2");

            }
            dispatcherTimer.Start();

        }
    }
}
