using System;

namespace Needle {
    /// <summary>
    /// The dependency injection context.
    /// </summary>
    public class Kernel {
        /// <summary>
        /// Gets a singleton injection kernel.
        /// </summary>
        /// <remarks>
        /// This kernel is created lazily; only use this
        /// if you don't want to define your own context.
        /// </remarks>
        public static Kernel Current {
            get {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Configures how the kernel will deal with missing dependencies.
        /// </summary>
        public ErrorMode ErrorMode { get; set; }

        /// <summary>
        /// Get the registered implementation of the specified dependency.
        /// </summary>
        /// <typeparam name="TDependency">The dependency to be fulfilled.</typeparam>
        /// <returns>The registered implementation of the dependency.</returns>
        public TDependency Get<TDependency>() {
            throw new NotImplementedException();
        }

        #region Rule Definition

        /// <summary>
        /// Begin registering an implementation of the specified type.
        /// </summary>
        /// <typeparam name="TDependency">The type to be injected at runtime.</typeparam>
        /// <returns>
        /// A chainable 'for'-clause object. Call "Provide" on this object to register the appropriate implementation.
        /// </returns>
        public IForClause<TDependency> For<TDependency>() {
            throw new NotImplementedException();
        }

        #endregion
    }
}