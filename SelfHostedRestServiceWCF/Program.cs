using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace SelfHostedRestServiceWCF
{
    class Program
    {
        /// <summary>
        /// Needs to be started as admin!
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                WebServiceHost host = new WebServiceHost(typeof(MainService));

                host.Open();

                Console.WriteLine("Service started!");

                Console.ReadLine();

                host.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine("Bye!");

            Console.ReadLine();
                
        }
    }
}
