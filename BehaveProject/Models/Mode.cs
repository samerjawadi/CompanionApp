using BehaveProject.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BehaveProject.Models
{
    public class Mode : BindableBase
    {
		public DelegateCommand SelectedCommand { get; set; }

		IEventAggregator _eventAggregator;
		/// <summary>/// Prism Property/// </summary>
		private string _name;

		public string Name
		{
			get { return _name; }
			set { SetProperty(ref _name, value); }
		}
		/// <summary>/// Prism Property/// </summary>
		private bool _isSelected;

		public bool IsSelected
		{
			get { return _isSelected; }
			set { SetProperty(ref _isSelected, value); }
		}

		/// <summary>/// Prism Property/// </summary>
		private SolidColorBrush _color;

		public SolidColorBrush Color
        {
			get { return _color; }
			set { SetProperty(ref _color, value); }
		}

		public Mode(IEventAggregator eventAggregator, string name, SolidColorBrush color, bool isSelected = false)
        {
            _eventAggregator= eventAggregator;
            IsSelected = isSelected;
            Name = name;
			Color = color;
			SelectedCommand = new DelegateCommand(()=> _eventAggregator.GetEvent<SelectedModeEvent>().Publish(this));
        }
    }
}
