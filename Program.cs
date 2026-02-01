namespace ScreenGrab
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // == force only one insdtance of app to run, if app is launched and instance is running bring it to foreground == //
            if (!SingleInstanceManager.EnsureSingleInstance())
            {
                return;
            }

            try
            {
                ApplicationConfiguration.Initialize();                                // Initialize application configuration
                bool showFormOnLaunch = !SingleInstanceManager.IsLaunchedOnStartup(); // Determine if the form should be shown on launch based on startup status
                Application.Run(new Driver(showFormOnLaunch));                        // Run the main application form (Driver) with the specified visibility
            }
            finally
            {
                SingleInstanceManager.Release(); // Ensure the single instance mutex is released when the application exits
            }
        }
    }
}