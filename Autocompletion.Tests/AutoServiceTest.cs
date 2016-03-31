using Autocompletion.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Autocompletion.Tests
{
    [TestClass]
    public class AutoServiceTest
    {
        [TestMethod]
        public void AutocompletionOrderTest()
        {
            var trie = new Trie<Domain.Autocompletion>();
            var autocompletions = new[]
            {
                new Domain.Autocompletion("karetchi", 10),
                new Domain.Autocompletion("sakura", 3),
                new Domain.Autocompletion("korosu", 11),
                new Domain.Autocompletion("kare", 13),
                new Domain.Autocompletion("kanojo", 10)
            };
            foreach (var autocompletion in autocompletions)
                trie.Insert(autocompletion.Word, autocompletion);
            var service = new AutoService(trie);

            Domain.Autocompletion[] actual = service.Autocompletion("ka");

            var expected = new[]
            {
                new Domain.Autocompletion("kare", 13),
                new Domain.Autocompletion("kanojo", 10),
                new Domain.Autocompletion("karetchi", 10)
            };
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}