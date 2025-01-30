using Prism.Events;
using System.Windows.Controls;

namespace BehaveProject.Views
{
    /// <summary>
    /// Interaction logic for BehaviorMainView
    /// </summary>
    public partial class BehaviorMainView : UserControl
    {
        IEventAggregator _eventAggregator;
        public BehaviorMainView(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            _eventAggregator = eventAggregator;
        }
    }
}
