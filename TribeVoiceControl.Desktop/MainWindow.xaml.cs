using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Speech.Recognition;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TribeVoiceControl.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);

        private bool _running;

        public MainWindow()
        {
            InitializeComponent();

            _running = true;
        }

        void SpeechRecognizedHandler(object? sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result != null)
            {
                Console.WriteLine($"Recognised: {e.Result.Text}");

                var args = e.Result.Text.Split(' ');

                switch (args[0].ToUpper())
                {
                    case "EXIT":
                        _running = false;
                        break;
                    case "TRIBE":
                        HandleTribe(args);
                        break;
                }
            }
        }

        void HandleTribe(string[] args)
        {
            switch (args[1].ToLower())
            {
                case "SCENE":
                    HandleSceneChange(args);
                    break;
            }
        }

        void HandleSceneChange(string[] args)
        {
            var scene = int.Parse(args[2]);

            ChangeScene(scene);
        }

        void ChangeScene(int scene)
        {
            var p = Process.GetProcessesByName("TribeXR").FirstOrDefault();
            p.WaitForInputIdle();
            var pointer = p.MainWindowHandle;
            SetForegroundWindow(pointer);
            switch (scene)
            {
                case 0:
                    SendKeys.Send(" ");
                    break;
                default:
                    SendKeys.Send($"{scene}");
                    break;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SpeechRecognizer recognizer = new SpeechRecognizer();

            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(SpeechRecognizedHandler);
        }
    }
}
