using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LH.Common.Zoom.App.Wpf
{
    public partial class MainWindow : Window
    {
        public bool _soundStarted = false;

        public MainWindow()
        {
            InitializeComponent();
            this.Hide();
            Thread udpThread = new Thread(new ThreadStart(StartUdp));
            udpThread.Start();

        }


        private void StartUdp()
        {
            UdpClient udpServer = new UdpClient(11000);
            while (true)
            {
                var remoteEP = new IPEndPoint(IPAddress.Any, 11000);
                var data = udpServer.Receive(ref remoteEP);
                var msg = Encoding.ASCII.GetString(data);
                var validUrl = msg.StartsWith("https://lh-org");
                var resMsg = validUrl ? "Message received" : "Invalid URL - Please make sure you have correct ZOOM Meeting URL for Lifehouse";
                byte[] bytes = Encoding.ASCII.GetBytes(resMsg);
                udpServer.Send(bytes, bytes.Length, remoteEP); // reply back
                if (validUrl)
                {
                    Thread confirmThread = new Thread(() => StartupMessageUdp(msg));
                    confirmThread.Start();
                }

            }

        }


        private void StartupMessageUdp(string msg)
        {
            _soundStarted = false;
            var vals = msg.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            Thread soundThread = new Thread(() => PlaySoundTimer());
            soundThread.Start();
            Dispatcher.Invoke(new Action(() =>
            {
                this.labelZoomRequest.Content = vals[0];
                this.labelFrom.Content = vals.Count() > 1 ? $"From: {vals[1]}" : string.Empty;
            }));

            FocusWindow();

        }
        private static void StartZoomUrl(string url)
        {
            var psi = new ProcessStartInfo("iexplore.exe")
            {
                Arguments = url
            };
            Process.Start(psi);
        }


        private void buttonAccept_Click(object sender, RoutedEventArgs e)
        {
            StartZoomUrl(labelZoomRequest.Content.ToString());
            labelZoomRequest.Content = string.Empty;
            this.Hide();
            _soundStarted = false;
        }


        protected void PlaySoundTimer()
        {
            _soundStarted = true;

            for (int i = 0; i < 120; i++)
            {
                if (!_soundStarted) { break; }
                SystemSounds.Beep.Play();
                System.Threading.Thread.Sleep(2000);

            }
            Dispatcher.Invoke(new Action(() =>
            {
                this.labelZoomRequest.Content = string.Empty;
                this.Hide();
            }));
        }

        protected void FocusWindow()
        {

            Dispatcher.Invoke(new Action(() =>
            {
                this.Activate();
                this.WindowState = System.Windows.WindowState.Normal;
                this.Topmost = true;
                this.Focus();
                this.Show();
            }));

        }

        private void buttonDecline_Click(object sender, RoutedEventArgs e)
        {
            labelZoomRequest.Content = string.Empty;
            this.Hide();
            _soundStarted = false;

        }


    }
}
