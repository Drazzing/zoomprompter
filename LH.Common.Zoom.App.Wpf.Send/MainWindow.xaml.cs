// Copyright (c) Shawn Rosewarne. All rights reserved.
// This code is licensed under MIT license (see LICENSE.md for details)

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LH.Common.Zoom.App.Wpf.Send
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<Client> Clients = new List<Client>();

        public MainWindow()
        {
            InitializeComponent();
            LoadClients();
            dgClient.ItemsSource = Clients;
        }


        public void LoadClients()
        {
            using (StreamReader r = new StreamReader("Clients.json"))
            {
                string json = r.ReadToEnd();
                Clients = JsonConvert.DeserializeObject<List<Client>>(json);
            }
        }
        private void dgClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dgClient_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgClient.SelectedItem != null)
            {

                if (string.IsNullOrEmpty(textBox.Text) || !textBox.Text.StartsWith(ConfigurationManager.AppSettings["zoomUrlCheck"].ToString()))
                {
                    MessageBox.Show(ConfigurationManager.AppSettings["zoomUrlCheckError"].ToString(), "Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var client = (Client)dgClient.SelectedItem;
                var result = MessageBox.Show(String.Format(ConfigurationManager.AppSettings["confirmation"].ToString(), client.Description), "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (result.ToString() == "Yes")
                {
                    var msg = textBox.Text;
                    if (!string.IsNullOrEmpty(textFrom.Text))
                    {
                        msg = msg + "|" + textFrom.Text.Trim();

                    }
                    try
                    {
                        var msgRet = Udp.UDP.SendUdp(client.Name, 11000, msg);
                        MessageBox.Show(msgRet);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
            }
        }
    }






    public class Client
    {
        public string Name { get; set; }
        public string Description { get; set; }

    }
}

