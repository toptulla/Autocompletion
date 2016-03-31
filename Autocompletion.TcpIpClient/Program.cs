using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Autocompletion.TcpIpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string serverHost = args[0];
                int port = int.Parse(args[1]);

                WorkWithServerAsync(serverHost, port).Wait();
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

        private async static Task WorkWithServerAsync(string serverHost, int port)
        {
            while (true)
            {
                using (var client = new TcpClient(serverHost, port))
                {
                    using (var stream = client.GetStream())
                    {
                        Console.Write("get ");
                        string prefix = Console.ReadLine();
                        string command = string.Format("get <{0}>", prefix);

                        using (var sr = new StreamReader(stream, Encoding.ASCII))
                        using (var sw = new StreamWriter(stream, Encoding.ASCII))
                        {
                            await sw.WriteLineAsync(command);
                            await sw.FlushAsync();

                            while (!sr.EndOfStream)
                            {
                                string s = await sr.ReadLineAsync();
                                Console.WriteLine(s);
                            }
                            Console.WriteLine();
                        }
                    }
                }
            }
        }
    }
}