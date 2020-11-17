using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Networking
{
    class Program
    {

        internal static readonly string IP = "127.0.0.1";
        internal static readonly int Port = 55555;

        static void Main(string[] args)
        {
            // start server
            Task.Run(() => { new Server().Start(); });

            // connect client
            new Client().Connect();

            Console.WriteLine("Bye!");
            Console.ReadLine();
        }

    }
}

