using System;
using System.Collections.Generic;

namespace Needle {
    /// <summary>
    /// Contains a set of objects keyed by their type.
    /// </summary>
    internal class TypeDictionary {
        private readonly Dictionary<Type, object> _values = new Dictionary<Type, object>();

        /// <summary>
        /// Adds the specified value to the dictionary,
        /// replacing any existing value.
        /// </summary>
        public void Add<TValue>(TValue value) {
            _values[typeof (TValue)] = value;
        }

        /// <summary>
        /// Checks whether a value of the specified type
        /// is found in the dictionary.
        /// </summary>
        public bool ContainsKey<TValue>() {
            return _values.ContainsKey(typeof (TValue));
        }

        /// <summary>
        /// Gets the specified value from the dictionary.
        /// </summary>
        public TValue Get<TValue>() {
            TValue value;
            if (TryGet(out value)) {
                return value;
            }
            else {
                throw new ArgumentException(string.Format("No such value ({0}) exists in the dictionary.", typeof (TValue)));
            }
        }

        /// <summary>
        /// Gets the specified value from the dictionary,
        /// if it is present.
        /// </summary>
        public bool TryGet<TValue>(out TValue value) {
            var type = typeof (TValue);
            if (_values.ContainsKey(type)) {
                value = (TValue) _values[type];
                return true;
            }
            else {
                value = default(TValue);
                return false;
            }
        }
    }
}