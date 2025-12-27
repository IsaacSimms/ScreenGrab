///// ties into active window capture and has an on/off toggle in settings /////
///// when true, and an active window capture is taken, open image in this form. Gather system information including:
///// ProcessName, ProcessId, ExcutablePath, FileVersion, and CaptureTime
///// all of that information gets printed in copiable text under the active window screenshot. 

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ScreenGrab
{
    public partial class SystemInfoCapture : Form
    {

        // == required componenets == //
        public Bitmap Screenshot { get; set; }
        public DateTime CaptureTime { get; set; }
        public IntPtr WindowHandle { get; set; }

        // == process information == //
        public string? ProcessName { get; set; }
        public int ProcessId { get; set; }
        public string? ExecutablePath { get; set; }
        public string? FileVersion { get; set; }
        public string? ProductVersion { get; set; }
        public string? WindowTitle { get; set; }

        // == responsiveness (hung app status) == // 
        public bool IsResponing { get; set; }
        public string ResponsivenessStatus => IsResponing ? "Responding" : "Not Responding";

        // == elevation status == //
        public bool IsElevated { get; set; }
        public string ElevationStatus => IsElevated ? "Elevated" : "Not Elevated";

        // == window hierarchy == //
        public IntPtr ParentWindow { get; set; }
        public IntPtr OwnerWindow { get; set; }
        public IntPtr RootWindow { get; set; }
        public IntPtr RootOwnerWindow { get; set; }
        public bool IsTopLevelWindow { get; set; }
        public bool IsChildWindow { get; set; }

        // == WinAPI imports - process info == //
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        // == WinAPI import - responsiveness == //
        [DllImport("user32.dll")]
        private static extern bool IsHungAppWindow(IntPtr hWnd);

        // == WinAPI import - elevation == //
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool GetTokenInformation(IntPtr TokenHandle, int TokenInformationClass, IntPtr TokenInformation, int TokenInformationLength, out int ReturnLength);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, int processId);

        // constant variables for tokening
        private const uint TOKEN_QUERY = 0x0008;
        private const int TokenElevation = 20;
        private const uint PROCESS_QUERY_INFORMATION = 0x1000;

        // ==  WinAPI imports - window hierarchy == //
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetParent(IntPtr hWnd);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetAncestor(IntPtr hWnd, uint gaFlags);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        // GetWindow constants
        private const uint GW_OWNER = 4;

        // GetAncestor constants
        private const uint GA_ROOT = 2;
        private const uint GA_ROOTOWNER = 3;
        private const uint GA_PARENT = 1;

        // GetWindowLong constants
        private const int GWL_STYLE = -16;
        private const int WS_CHILD = 0x40000000;
        private const int WS_POPUP = unchecked((int)0x80000000);

        // == constructor == //
        public SystemInfoCapture()
        {
            InitializeComponent();
        }

        // == constructor for taking HWND and Bitmap screenshot == //
        public SystemInfoCapture(IntPtr hwnd, Bitmap screenshot) : this()
        {
            WindowHandle = hwnd;
            Screenshot = screenshot;
            CaptureTime = DateTime.Now;


            GatherSystemInfo(hwnd);         // gather process information
            GatherResponsivenessInfo(hwnd); // gather responsiveness information
            GatherElevationInfo();          // gather elevation information
            GatherWindowHierarchy(hwnd);    // gather window hierarchy information

            // populate form fields
            populateUI();
        }

        // == gather system information == //
        public void GatherSystemInfo(IntPtr hwnd)
        {
            WindowTitle = GetWindowTitle(hwnd);

            GetWindowThreadProcessId(hwnd, out uint processId);
            ProcessId = (int)processId;

            // get process details
            try
            {
                using Process process = Process.GetProcessById((int)ProcessId);
                ProcessName = process.ProcessName;

                // get executable path
                try
                {
                    ExecutablePath = process.MainModule?.FileName;
                }
                catch
                {
                    ExecutablePath = "N/A"; // access denied or unavailable
                }
                // get file version info
                if (!string.IsNullOrEmpty(ExecutablePath))
                {
                    try
                    {
                        var versionInfo = FileVersionInfo.GetVersionInfo(ExecutablePath);
                        FileVersion = versionInfo.FileVersion ?? "N/A";
                        ProductVersion = versionInfo.ProductVersion ?? "N/A";
                    }
                    catch
                    {
                        FileVersion = "N/A";
                        ProductVersion = "N/A";
                    }
                }
            }
            catch
            {
                ProcessName = "N/A";
            }
        }
        // == gather responsiveness information == //
        private void GatherResponsivenessInfo(IntPtr hwnd)
        {
            try
            {
                bool isHung = IsHungAppWindow(hwnd);
                IsResponing = !isHung;
            }
            catch
            {
                IsResponing = true; // assume responding if response cannot be determined
            }
        }

        // == gather elevation information == //
        private void GatherElevationInfo()
        {
            IsElevated = false;                 // default to not elevated
            IntPtr processHandle = IntPtr.Zero; // initialize process handle
            IntPtr tokenHandle = IntPtr.Zero;   // initialize token handle 

            try
            {
                processHandle = OpenProcess(PROCESS_QUERY_INFORMATION, false, ProcessId);   // open process
                if (processHandle == IntPtr.Zero) return;                                   // unable to open process
                if (!OpenProcessToken(processHandle, TOKEN_QUERY, out tokenHandle)) return; // unable to open process token

                int elevationResultSize = Marshal.SizeOf(typeof(int));                     // size of elevation result
                IntPtr elevationTypePtr = Marshal.AllocHGlobal(elevationResultSize);       // allocate memory for elevation type

                try
                {
                    if (GetTokenInformation(tokenHandle, TokenElevation, elevationTypePtr, elevationResultSize, out _))
                    {
                        IsElevated = Marshal.ReadInt32(elevationTypePtr) != 0; // read elevation status
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(elevationTypePtr); // free allocated memory
                }
            }
            catch
            {
                IsElevated = false; // assume not elevated on error
            }
            finally
            {
                if (tokenHandle != IntPtr.Zero) CloseHandle(tokenHandle);         // close token handle
                if (processHandle != IntPtr.Zero) CloseHandle(processHandle);     // close process handle
            }
        }

        // == gather window hierarchy information == //
        private void GatherWindowHierarchy(IntPtr hwnd)
        {
            try
            {
                ParentWindow = GetParent(hwnd);
                OwnerWindow = GetWindow(hwnd, GW_OWNER);
                RootWindow = GetAncestor(hwnd, GA_ROOT);
                RootOwnerWindow = GetAncestor(hwnd, GA_ROOTOWNER);
                int style = GetWindowLong(hwnd, GWL_STYLE);
                IsChildWindow = (style & WS_CHILD) != 0;
                IsTopLevelWindow = (style & WS_POPUP) != 0 && !IsChildWindow;
            }
            catch
            {
                ParentWindow = IntPtr.Zero;
                OwnerWindow = IntPtr.Zero;
                RootWindow = IntPtr.Zero;
                RootOwnerWindow = IntPtr.Zero;
                IsChildWindow = false;
                IsTopLevelWindow = true;
            }
        }

        // == get window title from HWND == //
        private static string? GetWindowTitle(IntPtr hWnd)
        {
            int length = GetWindowTextLength(hWnd);
            if (length == 0) return null;

            var sb = new System.Text.StringBuilder(length + 1);
            GetWindowText(hWnd, sb, sb.Capacity);
            return sb.ToString();
        }

        // ==  get window title for any HWND (required for hierarchy) == //
        private static string? GetWindowTitleForHandle(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero) return "None";                        // handle is null

            int length = GetWindowTextLength(hWnd);                        // get title length
            if (length == 0) return $"0x{hWnd.ToString("X8")} (No Title)"; // no title
            var sb = new System.Text.StringBuilder(length + 1);            // create string builder
            GetWindowText(hWnd, sb, sb.Capacity);                          // get title
            return $"0x{hWnd.ToString("X8")} \"{sb}\"";                    // return formatted title
        }

        // == formate handle for display == //
        private static string FormatHandle(IntPtr hWnd)
        {
            return hWnd == IntPtr.Zero ? "None" : $"0x{hWnd.ToString("X8")}";
        }

        // == populate UI  == //
        private void populateUI()
        {
            // populate screenshot
            if (Screenshot != null)
            {
                pictureBoxScreenshot.Image = Screenshot;
            }
            // populate text fields
            txtSystemInfo.Text = $"""
                Capture Time: {CaptureTime}
                
                Process Name: {ProcessName}
                Process ID: {ProcessId}
                Executable Path: {ExecutablePath}
                File Version: {FileVersion}
                Product Version: {ProductVersion}
                
                Window Title: {WindowTitle}
                
                Responsiveness: {ResponsivenessStatus}
                
                Elevation Status: {ElevationStatus}
                
                Window Hierarchy:
                  Parent Window: {GetWindowTitleForHandle(ParentWindow)}
                  Owner Window: {GetWindowTitleForHandle(OwnerWindow)}
                  Root Window: {GetWindowTitleForHandle(RootWindow)}
                  Root Owner Window: {GetWindowTitleForHandle(RootOwnerWindow)}
                  Is Top-Level Window: {IsTopLevelWindow}
                  Is Child Window: {IsChildWindow}
                """;
        }
    }
}
