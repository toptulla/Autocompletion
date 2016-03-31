using System.Collections.Generic;

namespace Autocompletion.Domain
{
    /// <summary>
    /// Trie-структура
    /// </summary>
    /// <typeparam name="T">Тип значения, хранящегося в узлах структуры</typeparam>
    public class Trie<T>
    {
        private readonly TrieNode<T> _root;

        public Trie()
        {
            _root = new TrieNode<T>();
        }

        /// <summary>
        /// Вставка узла по ключу
        /// </summary>
        /// <param name="key">Ключ узла</param>
        /// <param name="value">Значение</param>
        public void Insert(string key, T value)
        {
            _root.InsertNode(key, 0, value);
        }

        /// <summary>
        /// Возвращает значения всех конечных узлов, производных для узла по ключу
        /// </summary>
        /// <param name="prefixKey">Ключ</param>
        /// <returns></returns>
        public IEnumerable<T> GetPrefixNode(string prefixKey)
        {
            return _root.GetByPrefix(prefixKey);
        }
    }
}