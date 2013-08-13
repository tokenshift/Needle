using System;

namespace Needle {
    /// <summary>
    /// Strongly-typed WeakReference.
    /// </summary>
    internal class Weak<TValue> {
        private readonly WeakReference _ref;

        /// <summary>
        /// Checks whether the referenced object is still
        /// available.
        /// </summary>
        public bool IsAlive {
            get {
                return _ref.IsAlive;
            }
        }

        /// <summary>
        /// Gets the referenced object, if it is
        /// still available.
        /// </summary>
        public TValue Value {
            get {
                if (!_ref.IsAlive) {
                    throw new ObjectDisposedException(string.Format("Weak reference of {0} expired.", typeof (TValue)));
                }

                return (TValue) _ref.Target;
            }
        }

        public Weak(TValue value) {
            _ref = new WeakReference(value);
        }
    }
}