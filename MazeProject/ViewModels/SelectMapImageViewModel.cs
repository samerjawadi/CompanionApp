using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MazeProject.ViewModels
{
    public class SelectMapImageViewModel : BindableBase, IDialogAware
    {
        public DelegateCommand LoadImageCommand { get; set; }
        public DelegateCommand OkCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        /// <summary>/// Prism Property/// </summary>
        private string _imagePath;

        public string ImagePath
        {
            get { return _imagePath; }
            set { SetProperty(ref _imagePath, value); }
        }

        /// <summary>/// Prism Property/// </summary>
		private int _columnsNumber;

        public int ColumnsNumber
        {
            get { return _columnsNumber; }
            set { SetProperty(ref _columnsNumber, value);
                LoadImageCommand = new DelegateCommand(LoadImageMethod);

                CustGrid = new ObservableCollection<CustomCell>();
                for (int i = 0; i < ColumnsNumber; i++)
                {
                    for (int j = 0; j < RowNumber; j++)
                    {
                        CustGrid.Add(new CustomCell()
                        {
                            Width = 400 / (double)ColumnsNumber,
                            Height = 400 / (double)RowNumber,

                        }); ;
                    }

                }
            }
        }
        /// <summary>/// Prism Property/// </summary>
		private int _rowNumber;

        public int RowNumber
        {
            get { return _rowNumber; }
            set { SetProperty(ref _rowNumber, value);
                LoadImageCommand = new DelegateCommand(LoadImageMethod);

                CustGrid = new ObservableCollection<CustomCell>();
                for (int i = 0; i < ColumnsNumber; i++)
                {
                    for (int j = 0; j < RowNumber; j++)
                    {
                        CustGrid.Add(new CustomCell()
                        {
                            Width = 400 / (double)ColumnsNumber,
                            Height = 400 / (double)RowNumber,

                        }); ;
                    }

                }
            }
        }

        /// <summary>/// Prism Property/// </summary>
		private ObservableCollection<CustomCell> _custGrid;

        public ObservableCollection<CustomCell> CustGrid
        {
            get { return _custGrid; }
            set { SetProperty(ref _custGrid, value); }
        }

        public SelectMapImageViewModel()
        {
            LoadImageCommand = new DelegateCommand(LoadImageMethod);
            OkCommand = new DelegateCommand(OkMethod);
            CancelCommand = new DelegateCommand(CancelMethode);

            CustGrid = new ObservableCollection<CustomCell>();
            ImagePath = "";
            RowNumber = 4;
            ColumnsNumber = 4;

        }
        private void OkMethod()
        {

            DialogResult dialogResult = new DialogResult();
            dialogResult.Parameters.Add("image", ImagePath);
            dialogResult.Parameters.Add("RowNumber", RowNumber);
            dialogResult.Parameters.Add("ColumnsNumber", ColumnsNumber);
            RequestClose(dialogResult);

        }
        private void CancelMethode()
        {
            RequestClose(new DialogResult());

        }

        private void LoadImageMethod()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select an Image",
                Filter = "Image Files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg",
                Multiselect = false // Ensure only one file can be selected
            };

            // Show the dialog and process the result
            if (openFileDialog.ShowDialog() == true)
            {
                // Set the selected file path to ImagePath
                ImagePath = openFileDialog.FileName;
            }
        }

        public string Title => "";

        public event Action<IDialogResult> RequestClose;

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
    public class CustomCell : BindableBase
    {
        public DelegateCommand SelectedCommand { get; set; }
        /// <summary>/// Prism Property/// </summary>
		private double _width;

        public double Width
        {
            get { return _width; }
            set { SetProperty(ref _width, value); }
        }

        /// <summary>/// Prism Property/// </summary>
        private double _height;

        public double Height
        {
            get { return _height; }
            set { SetProperty(ref _height, value); }
        }

        /// <summary>/// Prism Property/// </summary>
		private bool _selected;

        public bool Selected
        {
            get { return _selected; }
            set { SetProperty(ref _selected, value); }
        }


        public CustomCell()
        {
            SelectedCommand = new DelegateCommand(SelectedMethod);
        }

        private void SelectedMethod()
        {
            Selected = true;
        }
    }
}
