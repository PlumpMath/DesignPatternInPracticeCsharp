
using MarvellousWorks.PracticalPattern.IteratorPattern.Pure;
using NUnit.Framework;

namespace MarvellousWorks.PracticalPattern.IteratorPattern.Test.Pure
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
            int i = 0;
            foreach (string message in target)
                i++;
            Assert.AreEqual(2, i);
        }
    }
}
