using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Needle.Test {
    [TestClass]
    public class TestInjection {
        [TestMethod]
        public void TestInstancePerThread() {
            var kernel = new Kernel();
            kernel.For<IFoo>().Provide<Foo>(Mode.Thread);
            kernel.For<IBar>().Provide<Bar>(Mode.Singleton);

            IFoo foo1A = null, foo1B = null, foo2 = null;
            IBar bar1 = null, bar2 = null;

            var t1 = new Thread(() => {
                foo1A = kernel.Get<IFoo>();
                foo1B = kernel.Get<IFoo>();
                bar1 = kernel.Get<IBar>();
            });
            var t2 = new Thread(() => {
                foo2 = kernel.Get<IFoo>();
                bar2 = kernel.Get<IBar>();
            });

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Assert.AreSame(foo1A, foo1B);
            Assert.AreNotSame(foo1A, foo2);
            Assert.AreNotSame(foo1B, foo2);
            Assert.AreSame(bar1, bar2);
        }

        [TestMethod]
        public void TestInjectionContext() {
            var kernel1 = Kernel.Current;
            var kernel2 = Kernel.Current;

            Assert.AreSame(kernel1, kernel2);
        }

        [TestMethod]
        public void TestSimpleInjection() {
            var kernel = new Kernel();
            kernel.For<IFoo>().Provide<Foo>();

            var foo = kernel.Get<IFoo>();
            Assert.IsInstanceOfType(foo, typeof (Foo));
        }

        [TestMethod]
        public void TestExceptionMode() {
            var kernel = new Kernel();
            kernel.ErrorMode = ErrorMode.Null;

            var foo = kernel.Get<IFoo>();
            Assert.IsNull(foo);

            kernel.ErrorMode = ErrorMode.Exception;
            Exception e = null;
            try {
                kernel.Get<IFoo>();
            }
            catch (Exception ex) {
                e = ex;
            }

            Assert.IsNotNull(e);
            Assert.IsInstanceOfType(e, typeof (MissingDependency));
        }

        [TestMethod]
        public void TestMultipleDependencies() {
            var kernel = new Kernel();

            kernel.For<IFoo>().Provide<Foo>();
            kernel.For<IBar>().Provide<Bar>();

            var foo = kernel.Get<IFoo>();
            var bar = kernel.Get<IBar>();

            Assert.IsInstanceOfType(foo, typeof (Foo));
            Assert.IsInstanceOfType(bar, typeof (Bar));
        }

        [TestMethod]
        public void TestSingleton() {
            var kernel = new Kernel();

            kernel.For<IFoo>().Provide<Foo>();
            kernel.For<IBar>().Provide<Bar>(Mode.Singleton);

            var foo1 = kernel.Get<IFoo>();
            var foo2 = kernel.Get<IFoo>();
            var bar1 = kernel.Get<IBar>();
            var bar2 = kernel.Get<IBar>();

            Assert.IsInstanceOfType(foo1, typeof (Foo));
            Assert.IsInstanceOfType(foo2, typeof (Foo));
            Assert.IsInstanceOfType(bar1, typeof (Bar));
            Assert.IsInstanceOfType(bar2, typeof (Bar));

            Assert.AreNotSame(foo1, foo2);
            Assert.AreSame(bar1, bar2);
        }

        [TestMethod]
        public void TestPersistent() {
            var kernel = new Kernel();

            kernel.For<IFoo>().Provide<PersistentFoo>(Mode.Singleton);
            kernel.For<IBar>().Provide<PersistentBar>(Mode.Persistent);

            Assert.IsInstanceOfType(kernel.Get<IFoo>(), typeof (PersistentFoo));
            GC.Collect();
            Assert.IsInstanceOfType(kernel.Get<IFoo>(), typeof(PersistentFoo));

            Assert.IsInstanceOfType(kernel.Get<IBar>(), typeof(PersistentBar));
            GC.Collect();
            Assert.IsInstanceOfType(kernel.Get<IBar>(), typeof(PersistentBar));

            Assert.AreEqual(1, PersistentBar.Count);
            Assert.AreEqual(2, PersistentFoo.Count);
        }

        [TestMethod]
        public void TestCustomConstructorInjection() {
            var kernel = new Kernel();

            kernel.For<IHasDependencies>().Provide(() => new HasDependencies(new Foo(), new Bar()));

            var test = kernel.Get<IHasDependencies>();

            Assert.IsInstanceOfType(test, typeof(HasDependencies));
            Assert.IsInstanceOfType(((HasDependencies)test).Foo, typeof(Foo));
            Assert.IsInstanceOfType(((HasDependencies)test).Bar, typeof(Bar));
        }
    }
}