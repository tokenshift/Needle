using System;

namespace Needle
{
    /// <summary>
    /// Error raised when a dependency is requested for which
    /// there is no registration.
    /// </summary>
    public class MissingDependency : Exception {
        /// <summary>
        /// The type of the dependency that was requested.
        /// </summary>
        public readonly Type Requested;

        public MissingDependency(Type requested)
            : base(string.Format("No satisfying registration found for dependency {0}.", requested)) {
            Requested = requested;
        }
    }
}
