using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Networking
{
    public class Server
    {

        public void Start()
        {
            TcpListener server = null;

            try
            {
                // listen
                server = new TcpListener(IPAddress.Parse(Program.IP), Program.Port);
                server.Start();
                Console.WriteLine("Server: Waiting for a connection... ");

                // accept
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Server: Connected!");

                // read & write
                NetworkStream stream = client.GetStream();
                byte[] bytes = new byte[256];
                string data = null;
                int i;

                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("Server: Received: {0}", data);
                    byte[] msg = Encoding.ASCII.GetBytes(data);
                    stream.Write(msg, 0, msg.Length);
                    Console.WriteLine("Server: Sent: {0}", data);
                }

                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Server: Exception: {0}", ex);
            }
            finally
            {
                server.Stop();
            }
        }

    }
}
