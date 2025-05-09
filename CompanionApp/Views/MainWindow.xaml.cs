using CompanionApp.Events;
using CompanionApp.Models.Classes;
using CompanionApp.Service;
using DMSkin.WPF;
using Prism.Events;
using System;
using System.Threading.Tasks;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace CompanionApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {

        IEventAggregator _eventAggregator;

        private Notifier _notifier;

        public MainWindow(IEventAggregator eventAggregator)
        {
            InitializeComponent();

            _eventAggregator = eventAggregator;
            SetWindowSize();
            _notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.BottomRight,
                offsetX: 10,
                offsetY: 10);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromMilliseconds(5000),
                maximumNotificationCount: MaximumNotificationCount.FromCount(5));

                cfg.Dispatcher = Application.Current.Dispatcher;
            });
        }

        private void SetWindowSize()
        {
            // Get screen width and height

            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;

            // Set minimum window width and height to 80% of screen size
            this.MinWidth = screenWidth * 0.8;
            this.MinHeight = screenHeight * 0.8;

            // Optionally, center the window on the screen
            //this.Left = (screenWidth - this.MinWidth) / 2;
            //this.Top = (screenHeight - this.MinHeight) / 2;

        }

        private async Task CheckAndNotifyVersion()
        {

            _notifier.ShowInformation("New Version is Available, please Update!");

        }

        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string version = await CheckVersion.IsUpToDate(GetVersion());
            if (string.IsNullOrEmpty(version)) return;
            _eventAggregator.GetEvent<NewVersionAvaliableEvent>().Publish(version);
            await Task.Delay(5000);
            await CheckAndNotifyVersion();

        }

        public string GetVersion()
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
