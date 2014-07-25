#region  Using
using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MarvellousWorks.PracticalPattern.FlyweightPattern.ObjectPool;
using MarvellousWorks.PracticalPattern.FlyweightPattern.ObjectPool.Configuration;
using MarvellousWorks.PracticalPattern.FlyweightPattern.ObjectPool.Listener;
#endregion
namespace FlyweightPattern.Test.ObjectPool
{
    [TestClass]
    public class TestObjectPool
    {
        /// <summary>
        /// ���Զ���ضԾ������͵�����
        /// </summary>
        [TestMethod]
        public void TestCapacity()
        {
            PoolListener<AdvancedCalculator> listener = new PoolListener<AdvancedCalculator>();
            AdvancedCalculator obj1 = listener.Acquire();
            obj1.Activate();
            Assert.IsNotNull(obj1);
            AdvancedCalculator obj2 = listener.Acquire();
            obj2.Activate();
            Assert.IsNotNull(obj2);
            AdvancedCalculator obj3 = listener.Acquire();   // ���ˣ��޷���������
            Assert.IsNull(obj3);
        }

        /// <summary>
        /// ���Զ���ص����û���
        /// </summary>
        [TestMethod]
        public void TestResue()
        {
            PoolListener<AdvancedCalculator> listener = new PoolListener<AdvancedCalculator>();
            AdvancedCalculator obj1 = listener.Acquire();
            obj1.Activate();
            AdvancedCalculator obj2 = listener.Acquire();
            obj2.Activate();
            obj1.Deactivate();
            AdvancedCalculator obj3 = listener.Acquire();
            obj3.Activate();
            Assert.IsNotNull(obj3);
            Assert.AreEqual<string>(obj1.Guid, obj3.Guid);
            Assert.AreEqual<int>(obj3.Multiple(2, 2), 2 * 2);
        }

        
        /// <summary>
        /// ���Ի�����Զ���ʱ����
        /// </summary>
        [TestMethod]
        public void TestTimeout()
        {
            PoolListener<AdvancedCalculator> listener = new PoolListener<AdvancedCalculator>();
            AdvancedCalculator obj1 = listener.Acquire();
            string guid = obj1.Guid;
            // 5000 > timeout
            System.Threading.Thread.Sleep(5000); 
            obj1 = listener.Acquire();
            Assert.AreNotEqual<string>(guid, obj1.Guid);
        }

        /// <summary>
        /// ������ϵͳ����
        /// </summary>
        [TestMethod]
        public void TestMultipleTypes()
        {
            // ��һ�������ԣ������������2
            PoolListener<AdvancedCalculator> l1 = new PoolListener<AdvancedCalculator>();
            AdvancedCalculator advanced = l1.Acquire();
            Assert.IsNotNull(advanced);
                            
            // �ڶ��������ԣ������������2
            PoolListener<SimpleCalculator> l2 = new PoolListener<SimpleCalculator>();
            SimpleCalculator simple = l2.Acquire();
            simple.Activate();
            Assert.IsNotNull(simple);
        }
    }
}
