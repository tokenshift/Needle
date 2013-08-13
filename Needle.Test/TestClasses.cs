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
}