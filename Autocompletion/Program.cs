using System;
using Autocompletion.Domain;

namespace Autocompletion
{
    public class Program
    {
        static void Main()
        {
            Run(new ConsoleInput());
        }

        /// <summary>
        /// Обрабатывает ввод и выводит автодополнения на консоль
        /// </summary>
        /// <param name="input">Тип ввода ConsoleInput или FileInput</param>
        public static void Run(IInput input)
        {
            try
            {
                Config.InitConfig();

                var inputData = input.GetInputData() as InputDataWord;

                if (inputData != null)
                {
                    var service = new AutoService(inputData.Trie);

                    foreach (var word in inputData.Words)
                        ShowAutocompletions(service.Autocompletion(word));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Вывод автодополнений на консоль
        /// </summary>
        /// <param name="autocompletions">Автодополнения</param>
        private static void ShowAutocompletions(Domain.Autocompletion[] autocompletions)
        {
            if (autocompletions.Length > 0)
            {
                Console.WriteLine();
                foreach (var autocompletion in autocompletions)
                    Console.WriteLine(autocompletion.Word);
            }
        }
    }
}