namespace Autocompletion.Domain
{
    public class InputData
    {
        /// <summary>
        /// Trie-структура представляющая входные данные словаря
        /// </summary>
        public Trie<Autocompletion> Trie { get; set; }
    }
}