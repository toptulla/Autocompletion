using System;
using System.Collections.Generic;
using System.Linq;
using Autocompletion.Domain;

namespace Autocompletion
{
    /// <summary>
    /// Сервис автодополнений
    /// </summary>
    public class AutoService : AutocompletionService
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
        /// Создание сервиса
        /// </summary>
        /// <param name="trie">Построенная Trie структура</param>
        public AutoService(Trie<Domain.Autocompletion> trie)
            : base(trie)
        {
            _cache = new Dictionary<string, Domain.Autocompletion[]>();
            _aloritm = (key, tr) => tr.GetPrefixNode(key)
                            .OrderByDescending(a => a.Frequency)
                            .ThenBy(a => a.Word)
                            .Take(10);
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

            Domain.Autocompletion[] filtered = _aloritm(prefixKey, Trie).ToArray();
            _cache.Add(prefixKey, filtered);

            return filtered;
        }
    }
}