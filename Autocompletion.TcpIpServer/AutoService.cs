using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Autocompletion.Domain;

namespace Autocompletion.TcpIpServer
{
    /// <summary>
    /// Сервис автодополнений
    /// </summary>
    class AutoService : AutocompletionService
    {
        /// <summary>
        /// Кэш
        /// </summary>
        private readonly Dictionary<string, Domain.Autocompletion[]> _cache;
        /// <summary>
        /// Алгоритм сортировки и выбора автодополнений
        /// </summary>
        private readonly Func<string, Trie<Domain.Autocompletion>, IEnumerable<Domain.Autocompletion>> _aloritm;
        /// <summary>
        /// Объект блокировки при многопоточном обращении к сервису
        /// </summary>
        private readonly object _lock;

        /// <summary>
        /// Создание сервиса
        /// </summary>
        /// <param name="trie">Построенная Trie структура</param>
        public AutoService(Trie<Domain.Autocompletion> trie)
            : base(trie)
        {
            _cache = new Dictionary<string, Domain.Autocompletion[]>();
            _aloritm = (key, tr) => tr.GetPrefixNode(key)
                .OrderByDescending(a => a.Frequency)
                .ThenBy(a => a.Word);
            _lock = new object();
        }

        /// <summary>
        /// Возвращает массив автодополнений в соответствии с префиксом и алгоритмом сервиса
        /// </summary>
        /// <param name="prefixKey">Префикс</param>
        /// <returns>Массив автодополнений</returns>
        public override Domain.Autocompletion[] Autocompletion(string prefixKey)
        {
            if (_cache.ContainsKey(prefixKey))
                return _cache[prefixKey];

            lock (_lock)
            {
                if (_cache.ContainsKey(prefixKey))
                    return _cache[prefixKey];

                Thread.Sleep(10000);

                Domain.Autocompletion[] filtered = _aloritm(prefixKey, Trie).ToArray();
                _cache.Add(prefixKey, filtered);

                return filtered;
            }
        }
    }
}