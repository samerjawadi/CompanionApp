using AdvancedProgramming.Communs;
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
using System.Windows.Media;

namespace AdvancedProgramming.Views
{
    /// <summary>
    /// Interaction logic for AdvancedProgrammingView
    /// </summary>
    public partial class AdvancedProgrammingView : UserControl
    {
        private readonly IEventAggregator _eventAggregator;
        private bool mustUpdate = true;

        // Completion
        private CompletionWindow _completionWindow;

        public AdvancedProgrammingView(IEventAggregator eventAggregator,List<string> oldComs)
        {
            InitializeComponent();

            _eventAggregator = eventAggregator;

            if (DataContext is AdvancedProgrammingViewModel vm)
            {
                vm.Subscribe(_eventAggregator);
                vm.OldCom = oldComs;
                vm.ConnectMethod();
            }

            // Subscribe to events
            eventAggregator.GetEvent<ScriptLoadedEvent>().Subscribe((script) =>
            {
                mustUpdate = false;
                TextEditor.Text = script;
                mustUpdate = true;
            });

            // Hook Ctrl+Space
            TextEditor.TextArea.KeyDown += TextArea_KeyDown;
        }

        private void CliOutputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            textBox?.ScrollToEnd();
        }

        private void TextEditor_TextChanged(object sender, System.EventArgs e)
        {
            if (_eventAggregator == null) return;

            if ((DataContext as AdvancedProgrammingViewModel).eventAggregator == null)
                (DataContext as AdvancedProgrammingViewModel).Subscribe(_eventAggregator);

            if (mustUpdate)
                _eventAggregator.GetEvent<ScriptChangedEvent>().Publish(TextEditor.Text);
        }

        // === Ctrl+Space Trigger ===
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

            // Add Python keywords and builtins
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
    }
    

}
