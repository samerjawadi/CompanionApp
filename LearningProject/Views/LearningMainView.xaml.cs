using LearningProject.Models.Events;
using Prism.Events;
using Syncfusion.Windows.PdfViewer;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace LearningProject.Views
{
    /// <summary>
    /// Interaction logic for LearningMainView
    /// </summary>
    public partial class LearningMainView : UserControl
    {
        IEventAggregator _eventAggregator;

        public LearningMainView(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<LoadPDFEvent>().Subscribe(LoadPDFMethod);
            pdfViewer.Loaded += pdfViewer_Loaded;

        }

        private void LoadPDFMethod(string obj)
        {
            String path = AppDomain.CurrentDomain.BaseDirectory + "PDFs/";
            path = path + obj;
            pdfViewer.Load(path);

        }
        private void pdfViewer_Loaded(object sender, RoutedEventArgs e)
        {
            //Get the instance of the toolbar using its template name.
            DocumentToolbar toolbar = pdfViewer.Template.FindName("PART_Toolbar", pdfViewer) as DocumentToolbar;

            (toolbar.Template.FindName("PART_ButtonTextSearch", toolbar) as Button).Visibility = System.Windows.Visibility.Collapsed;
            (toolbar.Template.FindName("PART_ButtonSignature", toolbar) as Button).Visibility = System.Windows.Visibility.Collapsed;
            (toolbar.Template.FindName("PART_FileMenuStack", toolbar) as StackPanel).Visibility = System.Windows.Visibility.Collapsed;
            (toolbar.Template.FindName("PART_Annotations", toolbar) as ToggleButton).Visibility = System.Windows.Visibility.Collapsed;
            (toolbar.Template.FindName("PART_AnnotationToolsSeparator", toolbar) as System.Windows.Shapes.Rectangle).Visibility = System.Windows.Visibility.Collapsed;



            pdfViewer.ThumbnailSettings.IsVisible = false;
            // Hides the bookmark icon. 
            pdfViewer.IsBookmarkEnabled = false;
            // Hides the layer icon. 
            pdfViewer.EnableLayers = false;
            // Hides the organize page icon. 
            pdfViewer.PageOrganizerSettings.IsIconVisible = false;
            // Hides the redaction icon. 
            pdfViewer.EnableRedactionTool = false;
            // Hides the form icon. 
            pdfViewer.FormSettings.IsIconVisible = false;



        }
    }
}
