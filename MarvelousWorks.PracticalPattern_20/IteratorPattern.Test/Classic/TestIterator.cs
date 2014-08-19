using System;
using MarvellousWorks.PracticalPattern.IteratorPattern.Classic;
using NUnit.Framework;

namespace MarvellousWorks.PracticalPattern.IteratorPattern.Test.Classic
{
    [TestFixture]
    public class TestIterator
    {
        [Test]
        public void Test()
        {
            IAggregate target = new Aggregate();
            target.Add("A");
            target.Add("B");
            IIterator iterator = target.CreaetIterator();
            Assert.AreEqual("A", iterator.Next());
            Assert.AreEqual("B", iterator.Next());
            try
            {
                iterator.Next();
            }
            catch (Exception exception)
            {
                Assert.IsTrue(exception is IndexOutOfRangeException);
            }
        }
    }
}
