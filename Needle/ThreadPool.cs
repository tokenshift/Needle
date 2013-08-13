using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Needle {
    /// <summary>
    /// Provides an instance-per-thread of the specified type,
    /// reusing an instance once its thread exits.
    /// </summary>
    internal class ThreadPool<TValue> 
    where TValue : new() {
        private readonly Dictionary<int, WeakReference> _instances = new Dictionary<int, WeakReference>();
        private readonly ReadWriteLock _lock = new ReadWriteLock();
        
        /// <summary>
        /// Gets an instance of the specified type, constructing
        /// one if there isn't one already available.
        /// </summary>
        public TValue GetInstance() {
            var thread = Thread.CurrentThread.ManagedThreadId;

            // Clear out 'dead' instances.
            var dead = new List<int>();
            using (_lock.Read()) {
                dead.AddRange(from entry in _instances where !entry.Value.IsAlive select entry.Key);
            }

            if (dead.Count > 0) {
                using (_lock.Write()) {
                    foreach (var i in dead) {
                        if (_instances.ContainsKey(i) && !_instances[i].IsAlive) {
                            _instances.Remove(i);
                        }
                    }
                }
            }

            // Check for an existing instance.
            using (_lock.Read()) {
                if (_instances.ContainsKey(thread) && _instances[thread].IsAlive) {
                    return (TValue) _instances[thread].Target;
                }
            }

            // Create a new instance.
            using (_lock.Write()) {
                if (_instances.ContainsKey(thread) && _instances[thread].IsAlive) {
                    return (TValue) _instances[thread].Target;
                }
                else {
                    var value = new TValue();
                    _instances[thread] = new WeakReference(value);
                    return value;
                }
            }
        }
    }
}