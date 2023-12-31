﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleShipClient
{
    class SocketClient
    {
        public Socket socket;
        byte[] bytes;
        public SocketClient(string AddressIP)
        {
            IPEndPoint serverRemoteEP = new IPEndPoint(IPAddress.Parse(AddressIP), 11000);

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Connect(serverRemoteEP);
            }

            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
                throw;
            }
        }
        public string Receive()
        {
            bytes = new byte[1024];
            int bytesRec = 0;
            string answer = string.Empty;
            try
            {
                while (!answer.Contains("<EOF>"))
                {
                    bytesRec = socket.Receive(bytes);
                    answer += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
            return answer;
        }
        public void Send(String data)
        {
            try
            {
                // Encode the data string into a byte array.
                byte[] msg = Encoding.ASCII.GetBytes(data);

                // Send the data through the socket.
                int bytesSent = socket.Send(msg);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
                throw;
            }
        }
        public void Disconnect()
        {
            try
            {
                // Release the socket.
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }

            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
                throw;
            }
        }
    }
}
