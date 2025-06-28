namespace SharedLib.Helpers
{
    /// <summary>
    /// Helper for garbage collection.
    /// </summary>
    public static class GCHelper
    {
        /// <summary>
        /// Collects all garbage and waits for finalizers to complete.
        /// </summary>
        public static void FullCollect()
        {
            GC.Collect(GC.MaxGeneration);
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
