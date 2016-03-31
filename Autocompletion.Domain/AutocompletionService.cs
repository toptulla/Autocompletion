namespace Autocompletion.Domain
{
    /// <summary>
    /// Базовый класс сервисов автоподстановки
    /// </summary>
    public abstract class AutocompletionService
    {
        public Trie<Autocompletion> Trie { get; private set; }

        protected AutocompletionService(Trie<Autocompletion> trie)
        {
            Trie = trie;
        }

        /// <summary>
        /// Возвращает массив автоподстанововк по префиксу
        /// </summary>
        /// <param name="prefixKey">Префиксный ключ</param>
        /// <returns>Массив автоподстанововк</returns>
        public abstract Autocompletion[] Autocompletion(string prefixKey);
    }
}