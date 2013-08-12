using System;

namespace Needle {
    /// <summary>
    /// An error that occurs due to an invalid dependency registration.
    /// </summary>
    public class RuleException : Exception {
        public RuleException(string message)
            : base(message) {}
    }
}