using System;

namespace Autocompletion.TcpIpServer
{
    class Program
    {
        static void Main(string[] arg)
        {
            try
            {
                if (arg.Length < 2)
                    return;
                string file = arg[0];
                int portNumber = int.Parse(arg[1]);

                Config.InitConfig(file, portNumber);

                var server = new Server();

                server.RunAsync().Wait();
            }
            catch (AggregateException ae)
            {
                foreach (var innerException in ae.Flatten().InnerExceptions)
                    Console.WriteLine(innerException.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}