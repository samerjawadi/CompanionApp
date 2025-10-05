using AdvancedProgramming.Events;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AdvancedProgramming.ViewModels
{
    public class AdvancedProgrammingViewModel : BindableBase
    {
        public IEventAggregator eventAggregator { get; set; }

        // Commands
        public DelegateCommand ConnectCommand { get; }
        public DelegateCommand SendCommand { get; }
        public DelegateCommand RunScriptCommand { get; }
        public DelegateCommand StopScriptCommand { get; }
        public DelegateCommand ClearCommand { get; }

        // NEW: Load/Save
        public DelegateCommand LoadFileScriptCommand { get; }
        public DelegateCommand SaveScriptCommand { get; }

        // CLI Output (readonly terminal text)
        private string _cliOutput;
        public string CliOutput
        {
            get => _cliOutput;
            set => SetProperty(ref _cliOutput, value);
        }

        // CLI Command input (single line)
        private string _commandLine;
        public string CommandLine
        {
            get => _commandLine;
            set => SetProperty(ref _commandLine, value);
        }

        // Python script editor text
        private string _pythonScript;
        public string PythonScript
        {
            get => _pythonScript;
            set => SetProperty(ref _pythonScript, value);
        }

        // Track if a script is running
        private bool _isScriptRunning;
        public bool IsScriptRunning
        {
            get => _isScriptRunning;
            set
            {
                SetProperty(ref _isScriptRunning, value);
                RaisePropertyChanged(nameof(CanRunScript));
                RaisePropertyChanged(nameof(CanStopScript));
            }
        }

        // Serial Port
        private SerialPort _serialPort;

        public AdvancedProgrammingViewModel()
        {
            ConnectCommand = new DelegateCommand(ConnectMethod);
            SendCommand = new DelegateCommand(SendMethod, CanSend)
                          .ObservesProperty(() => CommandLine);
            RunScriptCommand = new DelegateCommand(RunScriptViaRawREPL, () => CanRunScript).ObservesProperty(() => IsScriptRunning).ObservesProperty(()=> PythonScript);
            StopScriptCommand = new DelegateCommand(StopScript);
            ClearCommand = new DelegateCommand(ClearMethod);

            // NEW
            LoadFileScriptCommand = new DelegateCommand(LoadFileScript);
            SaveScriptCommand = new DelegateCommand(SaveScript);
        }

        private void ClearMethod()
        {
            CliOutput = "";
        }

        public void Subscribe(IEventAggregator _eventAggregator)
        {
            this.eventAggregator = _eventAggregator;
            eventAggregator.GetEvent<ScriptChangedEvent>().Subscribe((obj) => PythonScript = obj
            );
        }

        /// <summary>
        /// Connect to MicroPython device
        /// </summary>
        private void ConnectMethod()
        {
            try
            {
                _serialPort = new SerialPort
                {
                    PortName = "COM18",
                    BaudRate = 115200,
                    Encoding = Encoding.ASCII,
                    NewLine = "\r\n"
                };
                _serialPort.DataReceived += _serialPort_DataReceived;
                _serialPort.Open();

                AppendCliOutput("Connected to " + _serialPort.PortName);
                _serialPort.WriteLine(""); // Wake up REPL
            }
            catch (Exception ex)
            {
                AppendCliOutput("Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Handles serial port data received
        /// Only appends output from MicroPython, not sent script
        /// </summary>
        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                while (_serialPort.BytesToRead > 0)
                {
                    string line = _serialPort.ReadLine();
                    // Only append actual output, ignore REPL prompts or sent code
                    if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith(">>>") && !line.StartsWith("..."))
                    {
                        AppendCliOutput(line);
                    }

                    // REPL prompt indicates script finished
                    if (line.Trim().EndsWith(">>>"))
                    {
                        //IsScriptRunning = false;
                        AppendCliOutput(line);

                    }
                }
            }
            catch (Exception ex)
            {
                AppendCliOutput("Read error: " + ex.Message);
            }
        }

        /// <summary>
        /// Append text into CLI Output (thread-safe)
        /// </summary>
        private void AppendCliOutput(string text)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                CliOutput += text + Environment.NewLine;
            });
        }

        /// <summary>
        /// Send single-line CLI command
        /// </summary>
        private void SendMethod()
        {
            if (string.IsNullOrWhiteSpace(CommandLine))
                return;

            try
            {
                _serialPort?.WriteLine(CommandLine);
                // Do not show sent command in CLI
                CommandLine = string.Empty;
            }
            catch (Exception ex)
            {
                AppendCliOutput("Send error: " + ex.Message);
            }
        }

        private bool CanSend()
        {
            return !string.IsNullOrWhiteSpace(CommandLine) && _serialPort?.IsOpen == true;
        }

        private bool CanRunScript => !IsScriptRunning && !string.IsNullOrWhiteSpace(PythonScript) && _serialPort?.IsOpen == true;
        private bool CanStopScript => IsScriptRunning && _serialPort?.IsOpen == true;

        /// <summary>
        /// Run script using raw REPL, CLI shows only MicroPython output
        /// </summary>
        private async void RunScriptViaRawREPL()
        {
            if (_serialPort == null || !_serialPort.IsOpen || string.IsNullOrWhiteSpace(PythonScript))
                return;

            try
            {
                IsScriptRunning = true;

                // Enter paste mode
                _serialPort.Write(new byte[] { 0x05 }, 0, 1); // Ctrl+E
                await Task.Delay(100);

                // Send the script itself (no Ctrl+E inside!)
                _serialPort.Write(Encoding.ASCII.GetBytes(PythonScript.Replace("\r\n", "\n") + "\n"), 0,
                                  Encoding.ASCII.GetByteCount(PythonScript.Replace("\r\n", "\n") + "\n"));

                // End paste mode and run
                _serialPort.Write(new byte[] { 0x04 }, 0, 1); // Ctrl+D

            }
            catch (Exception ex)
            {
                AppendCliOutput("Script error: " + ex.Message);
                IsScriptRunning = false;
            }
        }

        /// <summary>
        /// Stop the running script (Ctrl+C)
        /// </summary>
        private void StopScript()
        {
            try
            {
                _serialPort?.Write("\x03"); // Ctrl+C
            }
            catch (Exception ex)
            {
                AppendCliOutput("Stop error: " + ex.Message);
            }
            finally
            {
                IsScriptRunning = false;
            }
        }
        // ================================
        // NEW: Load/Save Implementation
        // ================================

        private void LoadFileScript()
        {
            try
            {
                var dlg = new OpenFileDialog
                {
                    Filter = "Python Files (*.py)|*.py|Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
                };

                if (dlg.ShowDialog() == true)
                {
                    PythonScript = File.ReadAllText(dlg.FileName);
                    eventAggregator.GetEvent<ScriptLoadedEvent>().Publish(PythonScript);
                    //AppendCliOutput("Loaded script: " + dlg.FileName);
                }
            }
            catch (Exception ex)
            {
                AppendCliOutput("Load error: " + ex.Message);
            }
        }

        private void SaveScript()
        {
            try
            {
                var dlg = new SaveFileDialog
                {
                    Filter = "Python Files (*.py)|*.py|Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                    FileName = "script.py"
                };

                if (dlg.ShowDialog() == true)
                {
                    File.WriteAllText(dlg.FileName, PythonScript ?? "");
                    //AppendCliOutput("Saved script: " + dlg.FileName);
                }
            }
            catch (Exception ex)
            {
                AppendCliOutput("Save error: " + ex.Message);
            }
        }
    }
}
