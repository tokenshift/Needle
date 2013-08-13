using System;
using System.Collections.Generic;

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
                using (_currentLock.Read()) {
                    if (_current != null) {
                        return _current;
                    }
                }

                using (_currentLock.Write()) {
                    return _current ?? (_current = new Kernel());
                }
            }
        }

        private static readonly ReadWriteLock _currentLock = new ReadWriteLock();
        private static Kernel _current;

        private readonly Dictionary<Type, object> _rules = new Dictionary<Type, object>();

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
            var type = typeof (TDependency);
            if (!_rules.ContainsKey(type)) {
                switch (ErrorMode) {
                    case ErrorMode.Exception:
                        throw new MissingDependency(type);
                    case ErrorMode.Null:
                        return default(TDependency);
                }
            }

            var rule = (RuleDefinition<TDependency>) _rules[type];
            if (rule.Constructor == null) {
                throw new RuleException(string.Format("Dependency registration for {0} is incomplete.", type));
            }

            return rule.Constructor();
        }

        /// <summary>
        /// Begin registering an implementation of the specified type.
        /// </summary>
        /// <typeparam name="TDependency">The type to be injected at runtime.</typeparam>
        /// <returns>
        /// A chainable 'for'-clause object. Call "Provide" on this object to register the appropriate implementation.
        /// </returns>
        public IForClause<TDependency> For<TDependency>() {
            var def = new RuleDefinition<TDependency>();
            _rules[typeof (TDependency)] = def;
            return def;
        }
    }
}