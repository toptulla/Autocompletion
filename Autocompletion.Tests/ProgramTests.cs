using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Autocompletion.Tests
{
    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public void RunFileInputPerfomance_10Seconds_Test()
        {
            var sw = Stopwatch.StartNew();

            Program.Run(new FileInput());
            sw.Stop();

            Assert.IsTrue(10d > sw.Elapsed.TotalSeconds);
        }
    }
}