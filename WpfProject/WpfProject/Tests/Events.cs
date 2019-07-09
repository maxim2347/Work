using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfProject.Tests {
    [TestFixture]
    public class EventTests {
        [Test]
        public void Test() {
            var eventOwner = new EventOwner();

            int count1 = 0;
            eventOwner.Action1 = () => count1++;
            eventOwner.InvokeAction1();
            Assert.AreEqual(1, count1);

            int count2 = 0;
            eventOwner.Action1 = () => count2++;
            eventOwner.InvokeAction1();
            Assert.AreEqual(1, count1);
            Assert.AreEqual(1, count2);
        }

        [Test]
        public void Test2() {
            var eventOwner = new EventOwner();
            eventOwner.Event1 += OnEventOwnerEvent1;
            eventOwner.Event1 += OnEventOwnerEvent1;

            eventOwner.RaiseEvent1();
            Assert.AreEqual(2, EventOwnerEvent1Count);

            eventOwner.Event1 -= OnEventOwnerEvent1;
            eventOwner.RaiseEvent1();
            Assert.AreEqual(3, EventOwnerEvent1Count);

            eventOwner.Event1 -= OnEventOwnerEvent1;
            eventOwner.RaiseEvent1();
            Assert.AreEqual(3, EventOwnerEvent1Count);
        }
        int EventOwnerEvent1Count = 0;
        void OnEventOwnerEvent1(object sender, EventArgs e) {
            EventOwnerEvent1Count++;
        }
    }

    class EventOwner {
        public Action Action1;
        public void InvokeAction1() { Action1?.Invoke(); }

        public event EventHandler Event1;
        public void RaiseEvent1() {
            //if(Event1 != null)
            //    Event1(this, EventArgs.Empty);
            Event1?.Invoke(this, EventArgs.Empty);
        }

        public MyEventHandler MyDelegate { get; set; }
        public event MyEventHandler MyEvent;
    }
    public delegate void MyEventHandler(object x1, object x2, object x3);
    public delegate void MyEventHandler1(object sender, MyEventArgs e);
    public class MyEventArgs : EventArgs {
        public object X1 { get; set; }
        public object X2 { get; set; }
    }
}
