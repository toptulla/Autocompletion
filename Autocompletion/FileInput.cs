using System;
using System.IO;
using System.Text.RegularExpressions;
using Autocompletion.Domain;

namespace Autocompletion
{
    public class FileInput : IInput
    {
        public InputData GetInputData()
        {
            using (var reader = new StreamReader(Config.File))
            {
                Trie<Domain.Autocompletion> trie = GenerateTrie(reader);
                string[] words = GenerateWords(reader);
                return new InputDataWord
                {
                    Words = words,
                    Trie = trie
                };
            }
        }

        /// <summary>
        /// Построение Trie-структуры словаря
        /// </summary>
        /// <returns>Trie структура, соответствующая ввходным данным консоли</returns>
        private Trie<Domain.Autocompletion> GenerateTrie(StreamReader reader)
        {
            var regN = new Regex(Config.AutocompletionPattern);
            var trie = new Trie<Domain.Autocompletion>();

            string line = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
                throw new Exception("Неправильный формат файа с данными. Первая строка должна содержать число, соответствующее количеству автодополнений.");

            int n = int.Parse(line);
            if (n < Config.MinN || n > Config.MaxN)
                throw new Exception(string.Format("Число автодополнений должно быть больше или раво {0} и меьше или равно {1}.", Config.MinN, Config.MaxN));

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
                if (autoCompletion.Length < 1 || autoCompletion.Length > Config.MaxAutocompletionLength)
                    throw new Exception("Неправильная длинна слова автодополнения.");

                int frequency = int.Parse(lineSplit[1]);
                if (frequency < Config.MinFrequency || frequency > Config.MaxFrequency)
                    throw new Exception("Неправильная частота строки автодополнения.");

                var autocompletion = new Domain.Autocompletion(autoCompletion, frequency);
                trie.Insert(autocompletion.Word, autocompletion);
            }

            return trie;
        }

        /// <summary>
        /// Создание массива хранящего слова для автодополнения
        /// </summary>
        /// <returns>Массив слов требующих автодоаолнения</returns>
        private string[] GenerateWords(StreamReader reader)
        {
            var regM = new Regex(Config.WordPatten);

            string line = reader.ReadLine();
            if (string.IsNullOrEmpty(line))
                throw new Exception("Неправильный формат файа с данными. Не найдена строка, соответствующая количеству входных слов для автодополнений.");

            int m = int.Parse(line);
            if (m < Config.MinM || m > Config.MaxM)
                throw new Exception(string.Format("Число слов для автодополнений должно быть больше или раво {0} и меьше или равно {1}.", Config.MinM, Config.MaxM));

            var words = new string[m];
            for (int i = 0; i < m; i++)
            {
                line = reader.ReadLine();
                if (string.IsNullOrEmpty(line))
                    throw new Exception("Неправильный формат ввода данных. Не найдена строка слова для автодополнения.");

                Match word = regM.Match(line);
                if (!word.Success)
                    throw new Exception("Строка слова не соответсвует шаблону.");

                if (word.Value.Length < 1 || word.Value.Length > Config.MaxWordLength)
                    throw new Exception("Длинна слова не соответсвует шаблону.");

                words[i] = word.Value;
            }

            return words;
        }
    }
}