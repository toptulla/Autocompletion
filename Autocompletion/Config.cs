using System;
using System.Configuration;

namespace Autocompletion
{
    /// <summary>
    /// Настройки приложения
    /// </summary>
    internal static class Config
    {
        public static int MinN { get; private set; }
        public static int MaxN { get; private set; }
        public static int MinM { get; private set; }
        public static int MaxM { get; private set; }
        public static int MaxAutocompletionLength { get; private set; }
        public static int MaxWordLength { get; private set; }
        public static int MinFrequency { get; private set; }
        public static int MaxFrequency { get; private set; }
        public static string AutocompletionPattern { get; private set; }
        public static string WordPatten { get; private set; }
        public static string File { get; private set; }

        public static void InitConfig()
        {
            MinN = int.Parse(ConfigurationManager.AppSettings["minN"]);
            MaxN = int.Parse(ConfigurationManager.AppSettings["maxN"]);
            MinM = int.Parse(ConfigurationManager.AppSettings["minM"]);
            MaxM = int.Parse(ConfigurationManager.AppSettings["maxM"]);
            MaxAutocompletionLength = int.Parse(ConfigurationManager.AppSettings["maxAutocompletionLength"]);
            MaxWordLength = int.Parse(ConfigurationManager.AppSettings["maxWordLength"]);
            MinFrequency = int.Parse(ConfigurationManager.AppSettings["minFrequency"]);
            MaxFrequency = int.Parse(ConfigurationManager.AppSettings["maxFrequency"]);
            AutocompletionPattern = ConfigurationManager.AppSettings["autocompletionPattern"];
            WordPatten = ConfigurationManager.AppSettings["wordPatten"];
            File = Environment.CurrentDirectory + ConfigurationManager.AppSettings["test.in"];
        }
    }
}