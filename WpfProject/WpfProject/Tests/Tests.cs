using NUnit.Framework;
using System;

namespace WpfProject.Tests {
    [TestFixture]
    public class Tests {
        void Foo1(Action do1, Action do2) {
            do1();
            do2();
        }
        [Test]
        public void CSharpDelegates_Action() {
            bool do1Res = false;
            bool do2Res = false;
            Assert.AreEqual(false, do1Res);
            Assert.AreEqual(false, do2Res);

            Action do1 = () => do1Res = true;
            Foo1(do1, () => do2Res = true);

            Assert.AreEqual(true, do1Res);
            Assert.AreEqual(true, do2Res);
        }

        bool Foo2(Func<bool> do1, Func<bool> do2) {
            return do1() || do2();
        }
        [Test]
        public void CSharpDelegates_Func() {
            int do1Count = 0;
            int do2Count = 0;
            Func<bool> do1 = () => {
                do1Count++;
                return true;
            };
            Func<bool> do2 = () => {
                do2Count++;
                return true;
            };
            var res = Foo2(do1, do2);
            Assert.AreEqual(true, res);
            Assert.AreEqual(1, do1Count);
            Assert.AreEqual(0, do2Count);
        }

        bool Foo3(bool do1, bool do2) {
            return do1 || do2;
        }
        [Test]
        public void CSharpDelegates_Func_WithoutFunc() {
            int do1Count = 0;
            int do2Count = 0;
            Func<bool> do1 = () => {
                do1Count++;
                return true;
            };
            Func<bool> do2 = () => {
                do2Count++;
                return true;
            };
            bool do1Res = do1();
            var res = Foo3(do1Res, do2());
            Assert.AreEqual(true, res);
            Assert.AreEqual(1, do1Count);
            Assert.AreEqual(1, do2Count);
        }


        int Foo4MethodImplRes = 0;
        void Foo4(Action<int, int> method, int p1) {
            method(p1, 4);
        }
        void MethodImpl(int p1, int p2) {
            Foo4MethodImplRes = p1 + p2;
        }
        [Test]
        public void CSharpDelegates_Action_Params() {
            Foo4(MethodImpl, 2);
            Assert.AreEqual(6, Foo4MethodImplRes);

            Action<int, int> myDelegate = (x1, x2) => Foo4MethodImplRes = x1 * x2;
            Foo4(myDelegate, 2);
            Assert.AreEqual(8, Foo4MethodImplRes);

            Foo4((x1, x2) => Foo4MethodImplRes = x1 * x2, 2);
            Assert.AreEqual(8, Foo4MethodImplRes);
        }

        [Test]
        public void CSharpDelegates_Func_Params() {
            Func<int, int, int> myFunc1 = (x1, x2) => x1 * x2;
            Func<int, int, int> myFunc2 = (x1, x2) => { return x1 * x2; };
            Assert.AreEqual(4, myFunc1(2, 2));
            Assert.AreEqual(4, myFunc2(2, 2));

            Func<Func<int, int, int>, Func<int, int, int>, int> myFunc3 = (x1, x2) => x1(2, 2) + x2(3, 3);

            Assert.AreEqual(25, myFunc3((x1, x2) => x1 + 10, (x1, x2) => x2 + 10));
        }
    }
}
