using DSCore;
using NUnit.Framework;

namespace DSCoreNodesTests
{
    static class LogicTests
    {
        [Test]
        [Category("UnitTests")]
        public static void Xor()
        {
            Assert.AreEqual(false, Logic.Xor(true, true));
            Assert.AreEqual(true, Logic.Xor(true, false));
            Assert.AreEqual(true, Logic.Xor(false, true));
            Assert.AreEqual(false, Logic.Xor(false, false));
        }
    }
}
