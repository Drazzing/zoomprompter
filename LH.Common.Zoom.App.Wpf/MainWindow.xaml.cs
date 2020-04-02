// Copyright (c) Shawn Rosewarne. All rights reserved.
// This code is licensed under MIT license (see LICENSE.md for details)
using System;
using System.Diagnostics;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
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
                var url = Encoding.ASCII.GetString(data);
                var resMsg = "Message received";
                byte[] bytes = Encoding.ASCII.GetBytes(resMsg);
                udpServer.Send(bytes, bytes.Length, remoteEP); // reply back

                Thread confirmThread = new Thread(() => StartupMessageUdp(url));
                confirmThread.Start();
            }

        }


        private void StartupMessageUdp(string zoomUrl)
        {
            _soundStarted = false;
            Thread soundThread = new Thread(() => PlaySoundTimer());
            soundThread.Start();

            Dispatcher.Invoke(new Action(() =>
            {
                this.labelZoomRequest.Content = zoomUrl;
                this.Show();
            }));

        }
        private static void StartZoomUrl(string url)
        {
            var psi = new ProcessStartInfo("iexplore.exe")
            {
                Arguments = url
            };
            Process.Start(psi);
        }

        public class AutoClosingMessageBox
        {
            System.Threading.Timer _timeoutTimer;
            string _caption;
            AutoClosingMessageBox(string text, string caption, int timeout, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage)
            {
                _caption = caption;
                _timeoutTimer = new System.Threading.Timer(OnTimerElapsed,
                    null, timeout, System.Threading.Timeout.Infinite);
                MessageBox.Show(text, caption, messageBoxButton, messageBoxImage);
            }

            public static void Show(string text, string caption, int timeout, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage)
            {
                new AutoClosingMessageBox(text, caption, timeout, messageBoxButton, messageBoxImage);
            }

            void OnTimerElapsed(object state)
            {
                IntPtr mbWnd = FindWindow(null, _caption);
                if (mbWnd != IntPtr.Zero)
                    SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                _timeoutTimer.Dispose();
            }
            const int WM_CLOSE = 0x0010;
            [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
            static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
            [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
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
        }

        private void buttonDecline_Click(object sender, RoutedEventArgs e)
        {
            labelZoomRequest.Content = string.Empty;
            this.Hide();
            _soundStarted = false;

        }


    }
}
