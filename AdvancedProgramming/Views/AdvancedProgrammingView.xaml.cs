using AdvancedProgramming.Events;
using AdvancedProgramming.ViewModels;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

namespace AdvancedProgramming.Views
{
    public partial class AdvancedProgrammingView : UserControl
    {
        private readonly IEventAggregator _eventAggregator;
        private bool mustUpdate = true;
        private CompletionWindow _completionWindow;

        public AdvancedProgrammingView(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            _eventAggregator = eventAggregator;

            if (DataContext is AdvancedProgrammingViewModel vm)
                vm.Subscribe(_eventAggregator);

            // Subscribe to events
            eventAggregator.GetEvent<ScriptLoadedEvent>().Subscribe(script =>
            {
                mustUpdate = false;
                TextEditor.Text = script;
                mustUpdate = true;
            });

            // Hook Ctrl+Space for autocomplete
            TextEditor.TextArea.KeyDown += TextArea_KeyDown;
            // Hook Enter to replace the current word with selected autocomplete
            TextEditor.TextArea.PreviewKeyDown += TextArea_PreviewKeyDown;
        }

        private void TextEditor_TextChanged(object sender, System.EventArgs e)
        {
            if (_eventAggregator == null) return;

            if ((DataContext as AdvancedProgrammingViewModel).eventAggregator == null)
                (DataContext as AdvancedProgrammingViewModel).Subscribe(_eventAggregator);

            if (mustUpdate)
                _eventAggregator.GetEvent<ScriptChangedEvent>().Publish(TextEditor.Text);
        }

        // Ctrl+Space triggers autocomplete
        private void TextArea_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                ShowCompletion();
                e.Handled = true;
            }
        }

        private void ShowCompletion()
        {
            _completionWindow = new CompletionWindow(TextEditor.TextArea);
            IList<ICompletionData> data = _completionWindow.CompletionList.CompletionData;

            string[] pythonKeywords =
            {
                "def", "class", "import", "from", "as",
                "for", "while", "if", "elif", "else",
                "try", "except", "finally", "with",
                "return", "yield", "pass", "break", "continue",
                "print", "len", "range", "input", "open",
                "True", "False", "None"
            };

            foreach (var kw in pythonKeywords)
                data.Add(new MyCompletionData(kw));

            _completionWindow.Show();
            _completionWindow.Closed += delegate { _completionWindow = null; };
        }

        // Replace current word with selected autocomplete on Enter
        private void TextArea_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (_completionWindow != null && e.Key == Key.Enter)
            {
                // Remove the current word before inserting
                var caretOffset = TextEditor.CaretOffset;
                var start = TextEditor.Text.LastIndexOfAny(new char[] { ' ', '\n', '\r', '\t' }, caretOffset - 1) + 1;
                if (start < 0) start = 0;

                TextEditor.Select(start, caretOffset - start);
                _completionWindow.CompletionList.RequestInsertion(e);
                e.Handled = true;
            }
        }
    }

    // Basic wrapper for completion items
    public class MyCompletionData : ICSharpCode.AvalonEdit.CodeCompletion.ICompletionData
    {
        public MyCompletionData(string text) => Text = text;
        public System.Windows.Media.ImageSource Image => null;
        public string Text { get; }
        public object Content => Text;
        public object Description => "Python keyword: " + Text;
        public double Priority => 0;

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment, Text);
        }
    }
}
