using System;
using System.Collections.Generic;

namespace Needle {
    /// <summary>
    /// A single dependency registration.
    /// </summary>
    internal class RuleDefinition<TDependency> : IForClause<TDependency> {
        internal Func<TDependency> Constructor { get; private set; }

        public void Provide<TImplementation>() where TImplementation : TDependency, new() {
            Provide<TImplementation>(Mode.Instance);
        }

        public void Provide<TImplementation>(Mode mode) where TImplementation : TDependency, new() {
            var type = typeof (TImplementation);

            switch (mode) {
                case Mode.Instance:
                    Constructor = () => new TImplementation();
                    break;
                case Mode.Singleton:
                    Constructor = () => {
                        if (!_singletons.ContainsKey(type)) {
                            _singletons[type] = new TImplementation();
                        }
                        return (TDependency) _singletons[type];
                    };
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private static readonly Dictionary<Type, object> _singletons = new Dictionary<Type, object>();
    }
}