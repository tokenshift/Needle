using System;

namespace Needle {
    /// <summary>
    /// The "for" clause of a dependency registration.
    /// </summary>
    /// <typeparam name="TDependency">The type of the dependency being registered.</typeparam>
    public interface IForClause<TDependency> {
        /// <summary>
        /// Registers the specific concrete implementation of the dependency.
        /// </summary>
        /// <typeparam name="TImplementation">The concrete implementation of the dependency.</typeparam>
        /// <remarks>
        /// The implementation must have a public parameterless (default) constructor.
        /// </remarks>
        void Provide<TImplementation>()
            where TImplementation : TDependency, new();

        /// <summary>
        /// Registers the specific concrete implementation of the dependency.
        /// </summary>
        /// <typeparam name="TImplementation">The concrete implementation of the dependency.</typeparam>
        /// <param name="mode">The injection method that will be used for this dependency.</param>
        /// <remarks>
        /// The implementation must have a public parameterless (default) constructor.
        /// </remarks>
        void Provide<TImplementation>(Mode mode)
            where TImplementation : TDependency, new();

        /// <summary>
        /// Registers a custom constructor that will be used to
        /// satisfy the dependency.
        /// </summary>
        /// <param name="constructor">
        /// A custom constructor that will be invoked each time
        /// the dependency is requested.
        /// </param>
        void Provide(Func<TDependency> constructor);
    }
}