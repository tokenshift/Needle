namespace Needle {
    /// <summary>
    /// The injection mode.
    /// </summary>
    public enum Mode {
        /// <summary>
        /// A new instance of the implementation will be provided
        /// each time the dependency is requested. This is
        /// the default.
        /// </summary>
        Instance,

        /// <summary>
        /// A single instance of the implementation will be shared
        /// by all requestors.
        /// </summary>
        Singleton,

        /// <summary>
        /// A single instance of the implementation will be created
        /// for each thread that requests the dependency.
        /// </summary>
        Thread
    }
}