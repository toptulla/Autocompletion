using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Autocompletion.Domain;

namespace Autocompletion.TcpIpServer
{
    /// <summary>
    /// Сервер автодополнений
    /// </summary>
    internal class Server
    {
        private readonly AutoService _service;
        private readonly TcpListener _tcpListener;

        public Server()
        {
            _service = CreateAutoService();
            _tcpListener = new TcpListener(IPAddress.Any, 11000);
        }

        private AutoService CreateAutoService()
        {
            var input = new FileInput();
            InputData inputData = input.GetInputData();
            return new AutoService(inputData.Trie);
        }

        /// <summary>
        /// Запуск сервера
        /// </summary>
        /// <returns></returns>
        public async Task RunAsync()
        {
            _tcpListener.Start();
            Console.WriteLine("Started...");
            await WorkWithClientAsync();
        }

        /// <summary>
        /// Обслуживание клиентских пдключений
        /// </summary>
        /// <returns></returns>
        private async Task WorkWithClientAsync()
        {
            while (true)
            {
                // Ожидание подключения клиента и неблокирующая обработка запроса
                TcpClient client = await _tcpListener.AcceptTcpClientAsync();
                ReadClientInputAsync(client);
            }
        }

        /// <summary>
        /// Обработка запроса клиента
        /// </summary>
        /// <param name="client">Клиент</param>
        /// <returns></returns>
        private async Task ReadClientInputAsync(TcpClient client)
        {
            using (var stream = client.GetStream())
            {
                using (var sr = new StreamReader(stream, Encoding.ASCII))
                using (var sw = new StreamWriter(stream, Encoding.ASCII))
                {
                    // Чтение запроса
                    string command = await sr.ReadLineAsync();
                    int startIndex = command.IndexOf('<');
                    int endIndex = command.IndexOf('>');
                    string prefix = command.Substring(startIndex + 1, endIndex - startIndex - 1);

                    // Обработка данныхзапроса
                    Domain.Autocompletion[] autocompletions = await Task.Run(() => _service.Autocompletion(prefix));

                    // Запись ответа
                    foreach (var autocompletion in autocompletions)
                    {
                        await sw.WriteLineAsync(autocompletion.Word);
                        await sw.FlushAsync();
                    }
                }
            }
        }
    }
}