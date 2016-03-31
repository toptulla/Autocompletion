using System;
using System.IO;
using System.Text.RegularExpressions;
using Autocompletion.Domain;

namespace Autocompletion.TcpIpServer
{
    class FileInput : IInput
    {
        /// <summary>
        /// Построение Trie-структуры словаря
        /// </summary>
        /// <returns>Trie структура, соответствующая ввходным данным консоли</returns>
        private Trie<Domain.Autocompletion> GenerateTrie(StreamReader reader)
        {
            var regN = new Regex("[a-z]+\\s[1-9][0-9]*");
            var trie = new Trie<Domain.Autocompletion>();

            string line = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
                throw new Exception("Неправильный формат файа с данными. Первая строка должна содержать число, соответствующее количеству автодополнений.");

            int n = int.Parse(line);
            if (n < 1)
                throw new Exception(string.Format("Число автодополнений должно быть больше 0."));

            for (int i = 0; i < n; i++)
            {
                line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    throw new Exception("Неправильный формат файа с данными. Не найдена строка автодополнения.");

                Match lineMatch = regN.Match(line);
                if (!lineMatch.Success)
                    throw new Exception("Строка автодополнения не соответсвует шаблону.");

                string[] lineSplit = line.Split(' ');
                string autoCompletion = lineSplit[0];
                if (autoCompletion.Length < 1)
                    throw new Exception("Неправильная длинна слова автодополнения.");

                int frequency = int.Parse(lineSplit[1]);
                if (frequency < 1)
                    throw new Exception("Неправильная частота строки автодополнения.");

                var autocompletion = new Domain.Autocompletion(autoCompletion, frequency);
                trie.Insert(autocompletion.Word, autocompletion);
            }

            return trie;
        }

        public InputData GetInputData()
        {
            using (var reader = new StreamReader(Config.File))
            {
                return new InputData
                {
                    Trie = GenerateTrie(reader)
                };
            }
        }
    }
}