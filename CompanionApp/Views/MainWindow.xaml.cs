using DMSkin.WPF;
using System.Windows;

namespace CompanionApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public MainWindow()
        {
            InitializeComponent();

            SetWindowSize();

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

    }
}
