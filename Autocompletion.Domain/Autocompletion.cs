using System;

namespace Autocompletion.Domain
{
    /// <summary>
    /// Автодополнение
    /// </summary>
    public class Autocompletion : IEquatable<Autocompletion>
    {
        /// <summary>
        /// Слово - текст автодополнения
        /// </summary>
        public string Word { get; private set; }
        /// <summary>
        /// Частота
        /// </summary>
        public int Frequency { get; private set; }

        public Autocompletion(string word, int frequency)
        {
            Word = word;
            Frequency = frequency;
        }

        public bool Equals(Autocompletion other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return string.Equals(Word, other.Word) && Frequency == other.Frequency;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((Autocompletion)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Word != null ? Word.GetHashCode() : 0) * 397) ^ Frequency;
            }
        }
    }
}