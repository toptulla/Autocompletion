using System;
using System.Text.RegularExpressions;
using Autocompletion.Domain;

namespace Autocompletion
{
    public class ConsoleInput : IInput
    {
        public InputData GetInputData()
        {
            Trie<Domain.Autocompletion> trie = GenerateTrie();
            string[] words = GenerateWords();
            return new InputDataWord
            {
                Words = words,
                Trie = trie
            };
        }

        /// <summary>
        /// Построение Trie-структуры словаря
        /// </summary>
        /// <returns>Trie структура, соответствующая ввходным данным консоли</returns>
        private static Trie<Domain.Autocompletion> GenerateTrie()
        {
            var regN = new Regex(Config.AutocompletionPattern);
            var trie = new Trie<Domain.Autocompletion>();

            while (true)
            {
                string line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    Console.WriteLine("Неправильный формат ввода данных. Не указана строка соответствующая количеству автодополнений.");
                    continue;
                }

                int autocompCount = int.Parse(line);
                if (autocompCount < Config.MinN || autocompCount > Config.MaxN)
                {
                    Console.WriteLine("Число автодополнений должно быть больше или раво {0} и меьше или равно {1}.", Config.MinN, Config.MaxN);
                    continue;
                }

                for (int i = 0; i < autocompCount; i++)
                {
                    line = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        Console.WriteLine("Неправильный формат ввода данных. Не найдена строка автодополнения.");
                        i--;
                        continue;
                    }

                    Match lineMatch = regN.Match(line);
                    if (!lineMatch.Success)
                    {
                        Console.WriteLine("Строка автодополнения не соответсвует шаблону.");
                        i--;
                        continue;
                    }

                    string[] lineSplit = lineMatch.Value.Split(' ');
                    string autoCompletion = lineSplit[0];

                    if (autoCompletion.Length < 1 || autoCompletion.Length > Config.MaxAutocompletionLength)
                    {
                        Console.WriteLine("Неправильная длинна слова автодополнения.");
                        i--;
                        continue;
                    }

                    int frequency = int.Parse(lineSplit[1]);
                    if (frequency < Config.MinFrequency || frequency > Config.MaxFrequency)
                    {
                        Console.WriteLine("Неправильная частота строки автодополнения.");
                        i--;
                        continue;
                    }

                    var autocompletion = new Domain.Autocompletion(autoCompletion, frequency);
                    trie.Insert(autocompletion.Word, autocompletion);
                }

                break;
            }

            return trie;
        }

        /// <summary>
        /// Создание массива хранящего слова для автодополнения
        /// </summary>
        /// <returns>Массив слов требующих автодоаолнения</returns>
        private static string[] GenerateWords()
        {
            var regM = new Regex(Config.WordPatten);
            string[] words;

            while (true)
            {
                string line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    Console.WriteLine("Неправильный формат ввода данных. Не указана строка соответствующая количеству слов для автодополнений.");
                    continue;
                }

                int wordsCount = int.Parse(line);
                if (wordsCount < Config.MinM || wordsCount > Config.MaxM)
                {
                    Console.WriteLine("Число слов должно быть больше или раво {0} и меьше или равно {1}.", Config.MinM, Config.MaxM);
                    continue;
                }

                words = new string[wordsCount];
                for (int i = 0; i < wordsCount; i++)
                {
                    line = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        Console.WriteLine("Неправильный формат ввода данных. Не найдена строка слова для автодополнения.");
                        i--;
                        continue;
                    }

                    Match word = regM.Match(line);
                    if (!word.Success)
                    {
                        Console.WriteLine("Строка слова не соответсвует шаблону.");
                        i--;
                        continue;
                    }

                    if (word.Value.Length < 1 || word.Value.Length > Config.MaxWordLength)
                    {
                        Console.WriteLine("Длинна слова не соответсвует шаблону.");
                        i--;
                        continue;
                    }

                    words[i] = word.Value;
                }

                break;
            }

            return words;
        }
    }
}