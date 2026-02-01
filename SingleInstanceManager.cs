///// manages app as a whole so that only one instance of the application can be open at a time. Has 

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ScreenGrab
{
    public static class SingleInstanceManager
    {
        // private variables
        private static Mutex? _mutex;                                         // controls mutex for single instance
        private const string MutexName = "Global\\ScreenGrab_SingleInstance";
        private const string EventName = "Global\\ScreenGrab_ShowDriverEvent"; // event for signaling
        private static EventWaitHandle? _showDriverEvent;                     // event handle for IPC
        private static Thread? _monitorThread;                                // background thread to monitor signals
        private static bool _isMonitoring = false;                            // flag to control monitoring
        private const int SW_RESTORE = 9;                                     // Win32 constant for restoring a minimized window

        // Callback delegate for when show signal is received
        public static event Action? OnShowDriverRequested;

        // Win32 API for bringing existing window to foreground
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        // Win32 API for showing proper window
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        // == checks if instance is already running. brings to forground if returns false, meaning another instance is present. return true when there is not another instance, allowing app start == //
        public static bool EnsureSingleInstance()
        {
            _mutex = new Mutex(true, MutexName, out bool createdNew);
            if (!createdNew)
            {
                // Another instance exists - try to bring it to front or signal it to show
                BringExistingInstanceToFront();
                SignalExistingInstance();
                return false;
            }
            return true;
        }

        // == starts monitoring for show driver signals from other instances == //
        public static void StartMonitoring()
        {
            if (_isMonitoring) return;

            _isMonitoring = true;
            _showDriverEvent = new EventWaitHandle(false, EventResetMode.AutoReset, EventName);

            _monitorThread = new Thread(() =>
            {
                while (_isMonitoring)
                {
                    try
                    {
                        // Wait for signal from another instance (with timeout to allow clean shutdown)
                        if (_showDriverEvent.WaitOne(500))
                        {
                            // Signal received - invoke the callback on main thread
                            OnShowDriverRequested?.Invoke();
                        }
                    }
                    catch
                    {
                        // Ignore errors during shutdown
                    }
                }
            })
            {
                IsBackground = true,
                Name = "SingleInstanceMonitor"
            };

            _monitorThread.Start();
        }

        // == stops monitoring and cleans up resources == //
        public static void StopMonitoring()
        {
            _isMonitoring = false;
            _monitorThread?.Join(1000); // wait up to 1 second for thread to finish
            _showDriverEvent?.Close();
            _showDriverEvent = null;
        }

        // == brings existing instance to front if it has a visible window == //
        private static void BringExistingInstanceToFront()
        {
            var currentProcess = Process.GetCurrentProcess();
            var processes = Process.GetProcessesByName(currentProcess.ProcessName);

            foreach (var process in processes)
            {
                if (process.Id != currentProcess.Id && process.MainWindowHandle != IntPtr.Zero)
                {
                    ShowWindow(process.MainWindowHandle, SW_RESTORE);
                    SetForegroundWindow(process.MainWindowHandle);
                    break;
                }
            }
        }

        // == signals the existing instance to show its Driver form via named event == //
        private static void SignalExistingInstance()
        {
            try
            {
                // Open the existing event and signal it
                using (var existingEvent = EventWaitHandle.OpenExisting(EventName))
                {
                    existingEvent.Set(); // Signal the existing instance
                }
            }
            catch
            {
                // Event doesn't exist yet or access denied - ignore
            }
        }

        // == determines if app was launced at OS startup              == //
        // == returns true if launched at startup (minimized to tray)  == //
        // == return false if launched manually (show the driver form) == //
        public static bool IsLaunchedOnStartup()
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Any(arg => arg.Equals("--startup", StringComparison.OrdinalIgnoreCase) ||
                                arg.Equals("/startup", StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            // == if command line fails but OS was booted less then three min ago, still assume startup == //
            var uptime = TimeSpan.FromMilliseconds(Environment.TickCount64);
            if (uptime.TotalMinutes < 3)
            {
                return true;
            }

            return false;
        }

        // == release mutex == //
        public static void Release()
        {
            StopMonitoring();

            if (_mutex != null)
            {
                _mutex.ReleaseMutex();
                _mutex.Dispose();
                _mutex = null;
            }
        }
    }
}
