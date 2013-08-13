using System;

namespace Needle {
    /// <summary>
    /// Provides instances of a concrete implementation based on
    /// </summary>
    internal class InstanceProvider {
        private static readonly TypeDictionary _singletons = new TypeDictionary();
        private static readonly ReadWriteLock _singletonsLock = new ReadWriteLock();

        private static readonly TypeDictionary _threadSingletons = new TypeDictionary();
        private static readonly ReadWriteLock _threadSingletonsLock = new ReadWriteLock();

        /// <summary>
        /// Gets an instance of the specified concrete implementation
        /// using the specified instantiation mode.
        /// </summary>
        /// <typeparam name="TImplementation">The concrete implementation type.</typeparam>
        /// <param name="mode">The instantiation mode (e.g. instance, singleton, instance-per-thread).</param>
        /// <returns>The concrete implementation.</returns>
        public TImplementation Get<TImplementation>(Mode mode)
            where TImplementation : new() {
            switch (mode) {
                case Mode.Instance:
                    return GetInstance<TImplementation>();
                case Mode.Singleton:
                    return GetSingleton<TImplementation>();
                case Mode.Thread:
                    return GetThreadSingleton<TImplementation>();
                default:
                    throw new ArgumentException(string.Format("Unsupported mode: {0}", mode), "mode");
            }
        }

        /// <summary>
        /// Constructs and returns a new instance of the specified type.
        /// </summary>
        private TImplementation GetInstance<TImplementation>() where TImplementation : new() {
            return new TImplementation();
        }

        /// <summary>
        /// Returns a single shared instance of the specified type.
        /// </summary>
        private TImplementation GetSingleton<TImplementation>() where TImplementation : new() {
            Weak<TImplementation> impl;
            using (_singletonsLock.Read()) {
                if (_singletons.TryGet(out impl) && impl.IsAlive) {
                    return impl.Value;
                }
            }

            using (_singletonsLock.Write()) {
                if (!_singletons.TryGet(out impl) || !impl.IsAlive) {
                    impl = new Weak<TImplementation>(new TImplementation());
                    _singletons.Add(impl);
                }
            }

            return impl.Value;
        }

        /// <summary>
        /// Returns a new instance of the specified type for each thread
        /// that requests one.
        /// </summary>
        private TImplementation GetThreadSingleton<TImplementation>() where TImplementation : new() {
            using (_threadSingletonsLock.Read()) {
                if (_threadSingletons.ContainsKey<ThreadPool<TImplementation>>()) {
                    return _threadSingletons.Get<ThreadPool<TImplementation>>().GetInstance();
                }
            }

            using (_threadSingletonsLock.Write()) {
                if (_threadSingletons.ContainsKey<ThreadPool<TImplementation>>()) {
                    return _threadSingletons.Get<ThreadPool<TImplementation>>().GetInstance();
                }

                _threadSingletons.Add(new ThreadPool<TImplementation>());
                return _threadSingletons.Get<ThreadPool<TImplementation>>().GetInstance();
            }
        }
    }
}