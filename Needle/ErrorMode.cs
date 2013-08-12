namespace Needle {
    /// <summary>
    /// Defines how the injection context deals with errors.
    /// </summary>
    public enum ErrorMode {
        /// <summary>
        /// Throw an exception when no registration is found for a dependency.
        /// This is the default.
        /// </summary>
        Exception,

        /// <summary>
        /// Return null when no registration is found for a dependency.
        /// </summary>
        Null
    }
}