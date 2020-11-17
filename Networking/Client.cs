using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Networking
{
    class Client
    {
        
        public void Connect()
        {
            TcpClient client = null;

            try
            {
                // connect
                client = new TcpClient(Program.IP, Program.Port);
                NetworkStream stream = client.GetStream();
                Console.WriteLine("Client: Connected!");

                // send
                string helloWorld = "Hello World!";
                Console.WriteLine("Client: Send: " + helloWorld);
                byte[] data = Encoding.ASCII.GetBytes(helloWorld);
                stream.Write(data, 0, data.Length);

                // receive
                byte[] bytes = new byte[client.ReceiveBufferSize];
                int bytesRead = stream.Read(bytes, 0, client.ReceiveBufferSize);
                Console.WriteLine("Client: Received : " + Encoding.ASCII.GetString(bytes, 0, bytesRead));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Client: Exception: {0}", ex);
            }
            finally
            {
                // close
                client.Close();
            }
        }
    }
}
