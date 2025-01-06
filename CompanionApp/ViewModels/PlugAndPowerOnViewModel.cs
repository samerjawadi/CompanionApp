using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanionApp.ViewModels
{
    public class PlugAndPowerOnViewModel : BindableBase,IDialogAware
    {
        /// <summary>/// Prism Property/// </summary>
		private bool _loading;

        public bool Loading
        {
            get { return _loading; }
            set { SetProperty(ref _loading, value); }
        }

        public DelegateCommand StartCommand { get; set; }

        public string Title => "";

        public PlugAndPowerOnViewModel()
        {
            StartCommand = new DelegateCommand(Start);
            Loading = false;
        }

        public event Action<IDialogResult> RequestClose;

        public void Start() 
        {
            DialogResult dialogResult = new DialogResult();
            dialogResult.Parameters.Add("ok", true);
            RequestClose(dialogResult);

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

        }
    }
}
