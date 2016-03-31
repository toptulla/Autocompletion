using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Autocompletion.Domain
{
    /// <summary>
    /// Узел Trie структуры
    /// </summary>
    /// <typeparam name="T">Тип значения, хранящегося в узле</typeparam>
    public class TrieNode<T> : IEnumerable<T>
    {
        private T _value;
        private bool _isComplite; // Флаг, сингализирующий, что узел является конечным - содержит значение
        private readonly IDictionary<char, TrieNode<T>> _childs;

        public TrieNode()
        {
            _childs = new Dictionary<char, TrieNode<T>>();
        }

        /// <summary>
        /// Добавляет узел по заданному ключу
        /// </summary>
        /// <param name="key">Ключ соответствующий узлу</param>
        /// <param name="position">Текущая позация включе</param>
        /// <param name="value">Значение узла</param>
        public void InsertNode(string key, int position, T value)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            if (position == key.Length)
            {
                _value = value;
                _isComplite = true;
                return;
            }

            char label = key[position];
            if (_childs.ContainsKey(label))
                _childs[label].InsertNode(key, ++position, value);
            else
            {
                var child = new TrieNode<T>();
                _childs.Add(label, child);
                child.InsertNode(key, ++position, value);
            }
        }

        /// <summary>
        /// Возвращает значения всех конечных узлов, производных для узла по ключу
        /// </summary>
        /// <param name="prefixKey">Ключ</param>
        /// <returns></returns>
        public IEnumerable<T> GetByPrefix(string prefixKey)
        {
            if (prefixKey == null)
                throw new ArgumentNullException("prefixKey");

            TrieNode<T> prefixKeyNode = GetPrefixNode(prefixKey, 0);

            if (prefixKeyNode != null)
                foreach (var value in prefixKeyNode)
                    yield return value;
        }

        /// <summary>
        /// Возвращает узел по ключу
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="position">Текущая позиция в ключе</param>
        /// <returns></returns>
        private TrieNode<T> GetPrefixNode(string key, int position)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            if (position == key.Length)
                return this;

            char label = key[position];
            if (_childs.ContainsKey(label))
                return _childs[label].GetPrefixNode(key, ++position);

            return null;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (_isComplite)
                yield return _value;

            foreach (var childPair in _childs)
                foreach (var child in childPair.Value)
                    yield return child;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}