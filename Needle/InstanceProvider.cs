using System;

namespace Needle {
    /// <summary>
    /// Provides instances of a concrete implementation based on
    /// </summary>
    internal class InstanceProvider {
        private readonly TypeDictionary _singletons = new TypeDictionary();

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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a new instance of the specified type for each thread
        /// that requests one.
        /// </summary>
        private TImplementation GetThreadSingleton<TImplementation>() where TImplementation : new() {
            throw new NotImplementedException();
        }
    }
}