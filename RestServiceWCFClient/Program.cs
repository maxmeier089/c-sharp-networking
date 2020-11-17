using RestServiceWCFContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RestServiceWCFClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                using (ChannelFactory<IMainService> channelFactory = new ChannelFactory<IMainService>("*"))
                {
                    IMainService mainService = channelFactory.CreateChannel();

                    MainData mainDataPost = await mainService.Post(new MainData() { Text = "Hello Service!" });

                    MainData mainDataGet = await mainService.Get(mainDataPost.Id.Value.ToString());

                    Console.WriteLine(mainDataGet.Text);

                    channelFactory.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.ReadLine();
        }
    }
}
