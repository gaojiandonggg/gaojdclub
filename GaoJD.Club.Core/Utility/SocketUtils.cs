using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GaoJD.Club.Core.Utility
{

    public static class SocketUtils
    {
        private static Socket ConnectSocket(string server, int port)
        {
            Socket s = null;
            IPHostEntry hostEntry = null;
            // Get host related information.
            try
            {

                IPAddress[] address = Dns.GetHostAddresses(server);

                Socket tempSocket =
                    new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                tempSocket.Connect(address, port);
                if (tempSocket.Connected)
                {
                    s = tempSocket;
                }

            }
            catch (Exception ex)
            {

                hostEntry = Dns.GetHostEntry(server);

                // Loop through the AddressList to obtain the supported AddressFamily. This is to avoid
                // an exception that occurs when the host IP Address is not compatible with the address family
                // (typical in the IPv6 case).
                foreach (IPAddress address in hostEntry.AddressList)
                {
                    IPEndPoint ipe = new IPEndPoint(address, port);
                    Socket tempSocket =
                        new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                    tempSocket.Connect(ipe);

                    if (tempSocket.Connected)
                    {
                        s = tempSocket;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }

            }
            return s;
        }


        /// <summary>
        /// 单个发送
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        public static void SendMsg(string server, int port, string message)
        {
            Assert.NotNull(server, nameof(server));
            Assert.NotNull(port, nameof(port));
            Assert.NotNull(message, nameof(message));
            Byte[] bytesSent = Encoding.ASCII.GetBytes(message);
            //  Byte[] bytesReceived = new Byte[256];

            // Create a socket connection with the specified server and port.
            using (Socket s = ConnectSocket(server, port))
            {
                // if (s == null)
                //    return ("Connection failed");
                // Send request to the server.
                s.Send(bytesSent, bytesSent.Length, 0);

            }
        }

        /// <summary>
        /// 单个发送
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        public static void SendBatchMsg(string server, int port, IEnumerable<string> message)
        {
            Assert.NotNull(server, nameof(server));
            Assert.NotNull(port, nameof(port));
            Assert.NotNull(message, nameof(message));

            // Create a socket connection with the specified server and port.
            using (Socket s = ConnectSocket(server, port))
            {
                Assert.NotNull(s, nameof(s));
                foreach (var item in message)
                {
                    Byte[] bytesSent = Encoding.ASCII.GetBytes(item);
                    // Send request to the server.
                    s.Send(bytesSent, bytesSent.Length, 0);
                }
            }
        }
    }
}
