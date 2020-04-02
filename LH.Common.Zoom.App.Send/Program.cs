// Copyright (c) Shawn Rosewarne. All rights reserved.
// This code is licensed under MIT license (see LICENSE.md for details)

using System;
namespace LH.Common.Zoom.App.Send
{
    class Program
    {
        static void Main(string[] args)
        {
            var msg = "https://lh-org.zoom.us/11111";
            var client = "TEST";
            var msgRet = Udp.UDP.SendUdp(client, 11000, msg);
            Console.Write("receive data from: " + msgRet);
            Console.Read();
        }



    }
}
