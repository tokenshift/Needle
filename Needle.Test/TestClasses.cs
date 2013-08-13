namespace Needle.Test {
    public interface IBar {}

    public class Bar : IBar {}

    public class PersistentBar : IBar {
        public static int Count { get; private set; }

        public PersistentBar() {
            ++Count;
        }
    }

    public interface IFoo {}

    public class Foo : IFoo {}

    public class PersistentFoo : IFoo {
        public static int Count { get; private set; }

        public PersistentFoo() {
            ++Count;
        }
    }

    public interface IHasDependencies {}

    public class HasDependencies : IHasDependencies {
        public readonly IFoo Foo;
        public readonly IBar Bar;

        public HasDependencies(IFoo foo, IBar bar) {
            Foo = foo;
            Bar = bar;
        }
    }
}