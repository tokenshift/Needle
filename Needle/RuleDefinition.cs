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
            Constructor = () => new InstanceProvider().Get<TImplementation>(mode);
        }
    }
}