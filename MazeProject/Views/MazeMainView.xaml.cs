using MazeProject.Events;
using Prism.Events;
using System.Windows;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace MazeProject.Views
{
    /// <summary>
    /// Interaction logic for MazeMainView
    /// </summary>
    public partial class MazeMainView : UserControl
    {
        IEventAggregator _eventAggregator;
        public MazeMainView(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            _eventAggregator = eventAggregator;
        }

        private void MyImage_Loaded(object sender, RoutedEventArgs e)
        {
            SetImageSize();
        }

        private void MyImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetImageSize();
        }

        private void SetImageSize()
        {
            if (MyImage != null)
            {
                _eventAggregator.GetEvent<ImageSizeChangedEvent>().Publish(new double[] { MyImage.ActualWidth, MyImage.ActualHeight });
            }
        }
    }
}
