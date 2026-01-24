///// ties into active window capture and has an on/off toggle in settings                                            /////
///// when true, and an active window capture is taken, open image in this form. Gather system information including: /////
///// ProcessName, ProcessId, ExcutablePath, FileVersion, and CaptureTime                                             /////
///// all of that information gets printed in copiable text under the active window screenshot.                       /////

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;

namespace ScreenGrab
{
    public partial class SystemInfoCapture : Form
    {

        // == required componenets == //
        public Bitmap? Screenshot { get; set; }
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
        public bool IsResponding { get; set; }
        public string ResponsivenessStatus => IsResponding ? "Responding" : "Not Responding";
        public TimeSpan ProcessUptime { get; set; }
        public string ProcessUpTimeFormatted => FormatUptime(ProcessUptime);

        // == elevation status == //
        public bool IsElevated { get; set; }
        public string ElevationStatus => IsElevated ? "Elevated" : "Not Elevated";
        public string IntegrityLevel { get; set; } = "unknown"; // default to unknown

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
        [DllImport("advapi32.dll")]
        private static extern IntPtr GetSidSubAuthority(IntPtr pSid, uint nSubAuthority);
        [DllImport("advapi32.dll")]
        private static extern IntPtr GetSidSubAuthorityCount(IntPtr pSid);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, int processId);

        // constant variables for tokening
        private const uint TOKEN_QUERY = 0x0008;
        private const int TokenElevation = 20;
        private const int TokenIntegrityLevel = 25;
        private const uint PROCESS_QUERY_INFORMATION = 0x1000;

        // constant variables for integrity level SID
        private const int SECURITY_MANDATORY_UNTRUSTED_RID = 0x0000;
        private const int SECURITY_MANDATORY_LOW_RID = 0x1000;
        private const int SECURITY_MANDATORY_MEDIUM_RID = 0x2000;
        private const int SECURITY_MANDATORY_MEDIUM_PLUS_RID = 0x2100;
        private const int SECURITY_MANDATORY_HIGH_RID = 0x3000;
        private const int SECURITY_MANDATORY_SYSTEM_RID = 0x4000;
        private const int SECURITY_MANDATORY_PROTECTED_PROCESS_RID = 0x5000;

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
            this.KeyDown += SystemInfoCapture_KeyDown; // add keydown event handler
            this.KeyPreview = true;                    // enable key preview for form
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

                // get process uptime
                try
                {
                    ProcessUptime = DateTime.Now - process.StartTime;
                }
                catch
                {
                    ProcessUptime = TimeSpan.Zero; // unable to get uptime
                }
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
        // == helper: format uptime for display == //
        private static string FormatUptime(TimeSpan uptime)
        {
            if (uptime.TotalDays >= 1)
                return $"{(int)uptime.TotalDays}d {uptime.Hours}h {uptime.Minutes}m {uptime.Seconds}s";
            if (uptime.TotalHours >= 1)
                return $"{(int)uptime.TotalHours}h {uptime.Minutes}m {uptime.Seconds}s";
            if (uptime.TotalMinutes >= 1)
                return $"{(int)uptime.TotalMinutes}m {uptime.Seconds}s";
            return $"{uptime.Seconds}s";
        }

        // == gather responsiveness information == //
        private void GatherResponsivenessInfo(IntPtr hwnd)
        {
            try
            {
                bool isHung = IsHungAppWindow(hwnd);
                IsResponding = !isHung;
            }
            catch
            {
                IsResponding = true; // assume responding if response cannot be determined
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

                // get integrity level
                GetTokenInformation(tokenHandle, TokenIntegrityLevel, IntPtr.Zero, 0, out int integrityLevelSize);
                if (integrityLevelSize > 0)
                {
                    IntPtr integrityLevelPtr = Marshal.AllocHGlobal(integrityLevelSize);
                    try
                    {
                        if (GetTokenInformation(tokenHandle, TokenIntegrityLevel, integrityLevelPtr, integrityLevelSize, out _))
                        {
                            // TOKEN_MANDATORY_LABEL structure: first field is SID_AND_ATTRIBUTES with Sid pointer
                            IntPtr sidPtr = Marshal.ReadIntPtr(integrityLevelPtr);
                            IntPtr subAuthCountPtr = GetSidSubAuthorityCount(sidPtr);
                            int subAuthCount = Marshal.ReadByte(subAuthCountPtr);
                            IntPtr subAuthPtr = GetSidSubAuthority(sidPtr, (uint)(subAuthCount - 1));
                            int integrityRid = Marshal.ReadInt32(subAuthPtr);

                            IntegrityLevel = integrityRid switch
                            {
                                SECURITY_MANDATORY_UNTRUSTED_RID => "Untrusted",
                                SECURITY_MANDATORY_LOW_RID => "Low",
                                SECURITY_MANDATORY_MEDIUM_RID => "Medium",
                                SECURITY_MANDATORY_MEDIUM_PLUS_RID => "Medium+",
                                SECURITY_MANDATORY_HIGH_RID => "High",
                                SECURITY_MANDATORY_SYSTEM_RID => "System",
                                SECURITY_MANDATORY_PROTECTED_PROCESS_RID => "Protected Process",
                                _ => $"Unknown (0x{integrityRid:X4})"
                            };
                        }
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(integrityLevelPtr); // free allocated memory
                    }
                }
            }
            catch
            {
                IsElevated = false;         // assume not elevated on error
                IntegrityLevel = "unknown"; // default to unknown on error
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
            txtSystemInfo.Text = 
                $"═══════════════════════════════════════════════════════════════\r\n" +
                $"                    SYSTEM CAPTURE INFORMATION                  \r\n" +
                $"═══════════════════════════════════════════════════════════════\r\n" +
                $"\r\n" +
                $"  Capture Time: {CaptureTime:yyyy-MM-dd HH:mm:ss.fff}\r\n" +
                $"  Window Handle: 0x{WindowHandle.ToString("X8")}\r\n" +
                $"\r\n" +
                $"───────────────────────────────────────────────────────────────\r\n" +
                $"  PROCESS INFORMATION\r\n" +
                $"───────────────────────────────────────────────────────────────\r\n" +
                $"\r\n" +
                $"  Process Name: {ProcessName ?? "NULL"}\r\n" +
                $"  Process ID (PID): {ProcessId}\r\n" +
                $"  Window Title: {WindowTitle ?? "NULL"}\r\n" +
                $"\r\n" +
                $"───────────────────────────────────────────────────────────────\r\n" +
                $"  EXECUTABLE INFORMATION\r\n" +
                $"───────────────────────────────────────────────────────────────\r\n" +
                $"\r\n" +
                $"  Executable Path: {ExecutablePath ?? "NULL"}\r\n" +
                $"  File Version: {FileVersion ?? "NULL"}\r\n" +
                $"  Product Version: {ProductVersion ?? "NULL"}\r\n" +
                $"\r\n" +
                $"───────────────────────────────────────────────────────────────\r\n" +
                $"  STATUS & ELEVATION\r\n" +
                $"───────────────────────────────────────────────────────────────\r\n" +
                $"\r\n" +
                $"  Responsiveness: {ResponsivenessStatus}\r\n" +
                $"  Process Uptime: {ProcessUpTimeFormatted}\r\n" +
                $"  Elevation: {ElevationStatus}\r\n" +
                $"  Integrity Level: {IntegrityLevel}\r\n" +
                $"\r\n" +
                $"───────────────────────────────────────────────────────────────\r\n" +
                $"  WINDOW HIERARCHY\r\n" +
                $"───────────────────────────────────────────────────────────────\r\n" +
                $"\r\n" +
                $"  Window Type: {(IsTopLevelWindow ? "Top-Level Window" : "Child Window")}\r\n" +
                $"  Parent Window: {FormatHandle(ParentWindow)}\r\n" +
                $"  Owner Window: {FormatHandle(OwnerWindow)}\r\n" +
                $"  Root Window: {FormatHandle(RootWindow)}\r\n" +
                $"  Root Owner: {FormatHandle(RootOwnerWindow)}\r\n" +
                $"\r\n" +
                $"═══════════════════════════════════════════════════════════════";
        }

        // == pull up driver form via button == //
        private void btnHome_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is Driver driverForm)
                {
                    driverForm.Show();
                    driverForm.WindowState = FormWindowState.Normal;
                    driverForm.ShowInTaskbar = true;
                    driverForm.Activate();
                    break;
                }
            }
            this.Close();
        }

        // == open screenshot in editr form == //
        private void btnOpenEditor_Click(object sender, EventArgs e)
        {
            if (Screenshot == null) return;

            // create copy of screenshot (avoids disposal issues)
            Bitmap screenshotCopy = new Bitmap(Screenshot);

            // open editor form with screenshot
            ImageEditorForm editorForm = new ImageEditorForm(screenshotCopy);
            editorForm.Show();
            this.Close();
        }

        // == save screenshot to file == //
        private void btnSaveScreenshot_Click(object sender, EventArgs e)
        {
            if (Screenshot == null) return;
            using SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Title = "Save Screenshot",
                Filter = "PNG Image|*.png|JPEG Image|*.jpg;*.jpeg|Bitmap Image|*.bmp|GIF Image|*.gif",
                DefaultExt = "png",
                FileName = $"Screenshot_{CaptureTime:yyyyMMdd_HHmmss}"
            };
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // determine image format based on file extension
                    var extension = System.IO.Path.GetExtension(saveFileDialog.FileName).ToLower();
                    System.Drawing.Imaging.ImageFormat format = extension switch
                    {
                        ".jpg" or ".jpeg" => System.Drawing.Imaging.ImageFormat.Jpeg,
                        ".bmp"           => System.Drawing.Imaging.ImageFormat.Bmp,
                        ".gif"           => System.Drawing.Imaging.ImageFormat.Gif,
                        _                => System.Drawing.Imaging.ImageFormat.Png,
                    };
                    // save the screenshot
                    Screenshot.Save(saveFileDialog.FileName, format);
                    MessageBox.Show("Screenshot saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to save screenshot. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // == copy screenshot to clipboard == //
        private void btnCopyScreenshot_Click(object sender, EventArgs e)
        {
            if (Screenshot != null)
            {
                Clipboard.SetImage(Screenshot);
            }
            ScreenshotMessageBox.ShowMessage(
                $"Screenshot copied to clipboard.",
                $"ScreenGrab:",
                4000);
        }

        // == export as markdown to folder == //
        private void btnExportMarkdown_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog folderDialog = new FolderBrowserDialog
            {
                Description = "Select Folder to Save Markdown File",
                ShowNewFolderButton = true,
                UseDescriptionForTitle = true
            };
            if (folderDialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                // == builds markdown content == //
                // create timestamped subfolder
                string folderName = $"SystemCapture_{CaptureTime:yyyyMMdd_HHmmss}";
                string exportPath = Path.Combine(folderDialog.SelectedPath, folderName);
                Directory.CreateDirectory(exportPath);

                // save screenshot
                string screenshotFIleName = "screenshot.png";
                string screenshotFilePath = Path.Combine(exportPath, screenshotFIleName);
                Screenshot?.Save(screenshotFilePath, System.Drawing.Imaging.ImageFormat.Png);

                // generate markdown
                string markdownContent = GenerateMarkdownContent(screenshotFIleName);
                string markdownFilePath = Path.Combine(exportPath, "SystemInfo.md");
                File.WriteAllText(markdownFilePath, markdownContent);

                MessageBox.Show($"Markdown file and screenshot saved successfully to:\r\n{exportPath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to export markdown file. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // == generate markdown content == //
        private string GenerateMarkdownContent(string screenshotFileName)
        {
            return $"""
            # System Capture Information

            **Capture Time:** {CaptureTime:yyyy-MM-dd HH:mm:ss.fff}  
            **Window Handle:** `0x{WindowHandle:X8}`

            ## Screenshot

            ![Screenshot]({screenshotFileName})

            ---

            ## Process Information

            | Property         | Value |
            |------------------|-------|
            | Process Name     | {ProcessName ?? "N/A"} |
            | Process ID (PID) | {ProcessId} |
            | Window Title     | {WindowTitle ?? "N/A"} |

            ---

            ## Executable Information

            | Property         | Value |
            |------------------|-------|
            | Executable Path  | `{ExecutablePath ?? "N/A"}` |
            | File Version     | {FileVersion ?? "N/A"} |
            | Product Version  | {ProductVersion ?? "N/A"} |

            ---

            ## Status & Elevation

            | Property         | Value |
            |------------------|-------|
            | Responsiveness   | {ResponsivenessStatus} |
            | Process Uptime   | {ProcessUpTimeFormatted} |
            | Elevation        | {ElevationStatus} |
            | Integrity Level  | {IntegrityLevel} |

            ---

            ## Window Hierarchy

            | Property         | Value |
            |------------------|-------|
            | Window Type      | {(IsTopLevelWindow ? "Top-Level Window" : "Child Window")} |
            | Parent Window    | `{FormatHandle(ParentWindow)}` |
            | Owner Window     | `{FormatHandle(OwnerWindow)}` |
            | Root Window      | `{FormatHandle(RootWindow)}` |
            | Root Owner       | `{FormatHandle(RootOwnerWindow)}` |
            """;
        }
        // == copy system info text to clipboard == //
        private void btnCopyInfo_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtSystemInfo.Text);
            ScreenshotMessageBox.ShowMessage(
                $"System information copied to clipboard.",
                $"ScreenGrab:",
                4000);
        }

       // == print system info on to screenshot == //
       private void btnPrintInfoOnImage_Click(object sender, EventArgs e)
        {
            if (Screenshot == null) return; // ensure screenshot exists

            // create a copy of the screenshot to draw on
            Bitmap annotatedImage = new Bitmap(Screenshot.Width, Screenshot.Height);

            using (Graphics g = Graphics.FromImage(annotatedImage))
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                // draw the original screenshot
                g.DrawImage(Screenshot, 0, 0);

                //use font for overlay text
                using Font font = new Font("Consolas", 7, FontStyle.Regular);               // iniatlizes overlay text font
                using Brush textBrush = new SolidBrush(Color.Yellow);                       // initalizes overlay text brush
                using Brush backgroundBrush = new SolidBrush(Color.FromArgb(180, 0, 0, 0)); // initalizes semi-transparent background

                // measure size of text for proper overlay location
                SizeF textSize = g.MeasureString(txtSystemInfo.Text, font);

                // calculate position relative to screenshot
                float padding = 5; //so that text is not on very edge of screenshot
                float x       = Screenshot.Width - textSize.Width - padding;
                float y       = padding;

                // draw background
                RectangleF backgroundRect = new RectangleF(x - padding, y - padding, textSize.Width + (padding * 2), textSize.Height + (padding * 2));
                g.FillRectangle(backgroundBrush, backgroundRect);

                // draw text
                g.DrawString(txtSystemInfo.Text, font, textBrush, x, y);

                // dispose of previous image
                Screenshot.Dispose();
                Screenshot = annotatedImage;

                // update picture box with new image
                pictureBoxScreenshot.Image = Screenshot;
            }
        }

        // == handle esc key to close form == //
        private void SystemInfoCapture_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}