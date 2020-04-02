// Copyright (c) Shawn Rosewarne. All rights reserved.
// This code is licensed under MIT license (see LICENSE.md for details)

using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LH.Common.Zoom.Udp
{
    public class UDP
    {
        public static string SendUdp(string hostName,int port, string msg)
        {
            var client = new UdpClient();
            IPEndPoint ep = Helpers.GetIPEndPointFromHostName(hostName, port, false);
            client.Client.SendTimeout = 5000;
            client.Client.ReceiveTimeout = 5000;
            client.Connect(ep);
            byte[] bytes = Encoding.ASCII.GetBytes(msg);
            client.Send(bytes, bytes.Length);
            var receivedData = client.Receive(ref ep);
            return Encoding.ASCII.GetString(receivedData);
        }
    }
}
