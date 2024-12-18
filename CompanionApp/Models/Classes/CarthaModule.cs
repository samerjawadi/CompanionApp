using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CompanionApp.Models.Classes
{
    public class CarthaModule : BindableBase
    {
		/// <summary>/// Prism Property/// </summary>
		private string _name;

		public string Name
		{
			get { return _name; }
			set { SetProperty(ref _name, value); }
		}
		/// <summary>/// Prism Property/// </summary>
		private string _image;

		public string Image
		{
			get { return _image; }
			set { SetProperty(ref _image, value); }
		}

		/// <summary>/// Prism Property/// </summary>
		private string _discription;

		public string Discription
		{
			get { return _discription; }
			set { SetProperty(ref _discription, value); }
		}
		public CarthaModule(string name,string image,string discription)
		{
			this.Name = name;
			this.Image = image;
			this.Discription = discription;
		}

	}
}
