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
        private const int SW_RESTORE = 9;                                     // Win32 constant for restoring a minimized window
        private const int HWND_BROADCAST = 0xffff;                            // broadcast message to all windows
        private const int WM_USER = 0x0400;                                   // base for custom messages
        public const int WM_SHOWDRIVER = WM_USER + 1;                         // custom message to show Driver form

        // Win32 API for bringing existing window to foreground
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        // Win32 API for showing proper window
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        // Win32 API for broadcasting messages to all windows
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);


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

        // == signals the existing instance to show its Driver form via Windows message == //
        private static void SignalExistingInstance()
        {
            // Broadcast custom message to all windows - existing instance will handle it
            SendMessage((IntPtr)HWND_BROADCAST, WM_SHOWDRIVER, IntPtr.Zero, IntPtr.Zero);
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
            if (_mutex != null)
            {
                _mutex.ReleaseMutex();
                _mutex.Dispose();
                _mutex = null;
            }
        }
    }
}
