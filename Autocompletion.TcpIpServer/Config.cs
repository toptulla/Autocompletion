using System;
using System.IO;

namespace Autocompletion.TcpIpServer
{
    static class Config
    {
        public static string File { get; set; }
        public static int PortNumber { get; set; }

        public static void InitConfig(string file, int portNumber)
        {
            var fileInfo = new FileInfo(file);
            if (!fileInfo.Exists)
                throw new Exception("Файл не найден.");
            File = file;

            if (portNumber < 0 || portNumber > 65535)
                throw new Exception("Неправильно указан порт.");
            PortNumber = portNumber;
        }
    }
}