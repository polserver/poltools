using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

namespace POLLaunch.Console
{
    /// <summary>
    ///     Root Console Hook Class
    /// </summary>
    public class MyConsole : Process
    {
        private bool _hooked = false;

        private IntPtr readHndl;

        /// <summary>
        ///     Default Constructor
        /// </summary>
        public MyConsole() { }

        /// <summary>
        ///     Deconstructor
        /// </summary>
        ~MyConsole()
        {
            // Helps if they close POL Launch while it's Converting or POL running.
            if (!this.HasExited)
                this.Kill();
        }

        /// <summary>
        ///     Tells if current Console Object is still Hooked.
        /// </summary>
        /// <returns>Bool</returns>
        public bool IsHooked
        {
            get { return _hooked; }
        }

        /// <summary>
        ///     Returns the Reader Handle for the started Object
        /// </summary>
        public IntPtr GetReadHndl
        {
            get { return readHndl; }
        }

        /// <summary>
        ///     Writes a string of text to the started processes InputStream.
        /// </summary>
        /// <param name="text">Text to send to the Input Stream for the Process</param>
        public bool Write(string text)
        {
            if (text == string.Empty)
                return false;
            foreach (char c in text)
            {
                if (Write(c) == false)
                    return false;
            }
            return true;
        }

        /// <summary>
        ///     Writes a char to the started processes InputStream.
        /// </summary>
        public bool Write(char c, bool Ctrl, bool Shift, bool Alt)
        {
            uint CtrlKeyState = 0;
            #region Apply modifiers
            if (Ctrl)
                CtrlKeyState |= (uint)Win32.ControlKeyState.LEFT_CTRL_PRESSED;
            if (Alt)
                CtrlKeyState |= (uint)Win32.ControlKeyState.LEFT_ALT_PRESSED;
            if (Shift)
                CtrlKeyState |= (uint)Win32.ControlKeyState.SHIFT_PRESSED;
            #endregion

            Win32.INPUT_RECORD[] input = new Win32.INPUT_RECORD[2];
            input[0].EventType = (ushort)Win32.EventTypes.KEY;
            input[0].KeyEvent.bKeyDown = true;
            input[0].KeyEvent.dwControlKeyState = CtrlKeyState;
            input[0].KeyEvent.UnicodeChar = c;
            // input[0].KeyEvent.wVirtualKeyCode = (uint)key;
            input[0].KeyEvent.wRepeatCount = 1;

            input[1].EventType = (ushort)Win32.EventTypes.KEY;
            input[1].KeyEvent.dwControlKeyState = CtrlKeyState;
            input[1].KeyEvent.UnicodeChar = c;
            input[1].KeyEvent.wRepeatCount = 1;
            // input[1].KeyEvent.wVirtualKeyCode = (uint)key;
            input[1].KeyEvent.bKeyDown = false;

            uint events_written;
            return Win32.WriteConsoleInput(this.readHndl, input, (uint)input.Length, out events_written);
        }

        /// <summary>
        ///     Writes a char to the Input Stream for the Hooked Process.
        /// </summary>
        /// <param name="c">Character to send.</param>
        public bool Write(char c)
        {
            return Write(c, false, false, false);
        }

        private bool HookConsole()
        {
            if (_hooked)
                return true;

            _hooked = Win32.AttachConsole((uint)this.Id);
            readHndl = Win32.GetStdHandle((uint)Win32.StdHandles.Input);
            return _hooked;
        }

        /// <summary>
        /// Starts "filename" process at "workingdir" directory.
        /// </summary>
        /// <param name="filename">Full path to program</param>
        /// <param name="workingdir">Full path to working dir</param>
        public new void Start(string filename, string workingdir)
        {
            if (!File.Exists(filename))
                throw new IOException("File not found");

            if (workingdir != String.Empty && Directory.Exists(workingdir))
                this.StartInfo.WorkingDirectory = workingdir;
            else
                this.StartInfo.WorkingDirectory = String.Empty;

            this.StartInfo.FileName = filename;
            this.StartInfo.RedirectStandardOutput = true;
            this.StartInfo.RedirectStandardInput = true;
            this.StartInfo.CreateNoWindow = true;
            this.StartInfo.UseShellExecute = false;

            this.EnableRaisingEvents = true;

            this.OutputDataReceived += new DataReceivedEventHandler(MyConsole_OutputDataReceived);
            this.Exited += new EventHandler(MyConsole_Exited);

            base.Start();

            this.BeginOutputReadLine();
        }

        /// <summary>
        /// Starts "filename" process at "workingdir" directory using "args" arguments.
        /// </summary>
        /// <param name="filename">Full path to program</param>
        /// <param name="workingdir">Full path to working dir</param>
        /// <param name="args">Arguments to send to the command line.</param>
        public void Start(string filename, string workingdir, string args)
        {
            if (!File.Exists(filename))
                throw new IOException("File not found");

            if (workingdir != String.Empty && Directory.Exists(workingdir))
                this.StartInfo.WorkingDirectory = workingdir;
            else
                this.StartInfo.WorkingDirectory = String.Empty;

            this.StartInfo.Arguments = args;
            this.StartInfo.FileName = filename;
            this.StartInfo.RedirectStandardOutput = true;
            this.StartInfo.RedirectStandardInput = true;
            this.StartInfo.CreateNoWindow = true;
            this.StartInfo.UseShellExecute = false;

            this.EnableRaisingEvents = true;

            this.OutputDataReceived += new DataReceivedEventHandler(MyConsole_OutputDataReceived);
            this.Exited += new EventHandler(MyConsole_Exited);

            base.Start();

            this.BeginOutputReadLine();
        }

        void MyConsole_Exited(object sender, EventArgs e)
        {
            Win32.FreeConsole();
            this.Close();
        }
        private void MyConsole_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (_hooked)
                return;
            else
                HookConsole();
        }

    }

    class Win32
    {
        [DllImport("kernel32.dll")]
        public static extern bool GenerateConsoleCtrlEvent(uint dwCtrlEvent, uint dwProcessGroupId);
        [DllImport("kernel32.dll")]
        public static extern uint GetProcessId(IntPtr Process);
        [DllImport("kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetStdHandle(uint nStdHandle);
        [DllImport("kernel32.dll")]
        public static extern bool AttachConsole(uint dwProcessId);
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        [DllImport("kernel32.dll")]
        public static extern bool WriteConsoleInput(IntPtr hConsoleInput, INPUT_RECORD[] lpBuffer, uint nLength, out uint lpNumberOfEventsWritten);


        public enum StdHandles : uint
        {
            Input = 0xFFFFFFF6,
            Output = 0xFFFFFFF5,
            Error = 0xFFFFFFF4
        }


        public enum CtrlTypes : uint
        {
            CTRL_C = 0,
            CTRL_BREAK,
            CTRL_CLOSE,
            CTRL_LOGOFF = 5,
            CTRL_SHUTDOWN
        }

        public enum EventTypes : uint
        {
            KEY = 0x0001,
            MOUSE = 0x0002,
            WINDOW_BUFFER_SIZE = 0x0004,
            MENU = 0x0008,
            FOCUS = 0x0010
        }

        public enum ControlKeyState : uint
        {
            CAPSLOCK_ON = 0x0080,
            ENHANCED_KEY = 0x0100,
            LEFT_ALT_PRESSED = 0x0002,
            LEFT_CTRL_PRESSED = 0x0008,
            NUMLOCK_ON = 0x0020,
            RIGH_ALT_PRESSED = 0x0001,
            RIGHT_CTRL_PRESSED = 0x0004,
            SCROLLLOCK_ON = 0x0040,
            SHIFT_PRESSED = 0x0010
        }


        //This is a union, but I only set KEY_EVENT to test
        [StructLayout(LayoutKind.Explicit)]
        public struct INPUT_RECORD
        {
            [FieldOffset(0)]
            public ushort EventType;
            [FieldOffset(4)]
            public KEY_EVENT_RECORD KeyEvent;
        }

        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
        public struct KEY_EVENT_RECORD
        {
            [FieldOffset(0), MarshalAs(UnmanagedType.Bool)]
            public bool bKeyDown;
            [FieldOffset(4), MarshalAs(UnmanagedType.U2)]
            public ushort wRepeatCount;
            [FieldOffset(6), MarshalAs(UnmanagedType.U2)]
            public VirtualKeys wVirtualKeyCode;
            [FieldOffset(8), MarshalAs(UnmanagedType.U2)]
            public ushort wVirtualScanCode;
            [FieldOffset(10)]
            public char UnicodeChar;
            [FieldOffset(12), MarshalAs(UnmanagedType.U4)]
            public uint dwControlKeyState;
        }
        #region VirtualKeys Enum
        /// <summary>
        /// Enumeration for virtual keys.
        /// </summary>
        public enum VirtualKeys
            : ushort
        {
            /// <summary></summary>
            LeftButton = 0x01,
            /// <summary></summary>
            RightButton = 0x02,
            /// <summary></summary>
            Cancel = 0x03,
            /// <summary></summary>
            MiddleButton = 0x04,
            /// <summary></summary>
            ExtraButton1 = 0x05,
            /// <summary></summary>
            ExtraButton2 = 0x06,
            /// <summary></summary>
            Back = 0x08,
            /// <summary></summary>
            Tab = 0x09,
            /// <summary></summary>
            Clear = 0x0C,
            /// <summary></summary>
            Return = 0x0D,
            /// <summary></summary>
            Shift = 0x10,
            /// <summary></summary>
            Control = 0x11,
            /// <summary></summary>
            Menu = 0x12,
            /// <summary></summary>
            Pause = 0x13,
            /// <summary></summary>
            Kana = 0x15,
            /// <summary></summary>
            Hangeul = 0x15,
            /// <summary></summary>
            Hangul = 0x15,
            /// <summary></summary>
            Junja = 0x17,
            /// <summary></summary>
            Final = 0x18,
            /// <summary></summary>
            Hanja = 0x19,
            /// <summary></summary>
            Kanji = 0x19,
            /// <summary></summary>
            Escape = 0x1B,
            /// <summary></summary>
            Convert = 0x1C,
            /// <summary></summary>
            NonConvert = 0x1D,
            /// <summary></summary>
            Accept = 0x1E,
            /// <summary></summary>
            ModeChange = 0x1F,
            /// <summary></summary>
            Space = 0x20,
            /// <summary></summary>
            Prior = 0x21,
            /// <summary></summary>
            Next = 0x22,
            /// <summary></summary>
            End = 0x23,
            /// <summary></summary>
            Home = 0x24,
            /// <summary></summary>
            Left = 0x25,
            /// <summary></summary>
            Up = 0x26,
            /// <summary></summary>
            Right = 0x27,
            /// <summary></summary>
            Down = 0x28,
            /// <summary></summary>
            Select = 0x29,
            /// <summary></summary>
            Print = 0x2A,
            /// <summary></summary>
            Execute = 0x2B,
            /// <summary></summary>
            Snapshot = 0x2C,
            /// <summary></summary>
            Insert = 0x2D,
            /// <summary></summary>
            Delete = 0x2E,
            /// <summary></summary>
            Help = 0x2F,
            /// <summary></summary>
            N0 = 0x30,
            /// <summary></summary>
            N1 = 0x31,
            /// <summary></summary>
            N2 = 0x32,
            /// <summary></summary>
            N3 = 0x33,
            /// <summary></summary>
            N4 = 0x34,
            /// <summary></summary>
            N5 = 0x35,
            /// <summary></summary>
            N6 = 0x36,
            /// <summary></summary>
            N7 = 0x37,
            /// <summary></summary>
            N8 = 0x38,
            /// <summary></summary>
            N9 = 0x39,
            /// <summary></summary>
            A = 0x41,
            /// <summary></summary>
            B = 0x42,
            /// <summary></summary>
            C = 0x43,
            /// <summary></summary>
            D = 0x44,
            /// <summary></summary>
            E = 0x45,
            /// <summary></summary>
            F = 0x46,
            /// <summary></summary>
            G = 0x47,
            /// <summary></summary>
            H = 0x48,
            /// <summary></summary>
            I = 0x49,
            /// <summary></summary>
            J = 0x4A,
            /// <summary></summary>
            K = 0x4B,
            /// <summary></summary>
            L = 0x4C,
            /// <summary></summary>
            M = 0x4D,
            /// <summary></summary>
            N = 0x4E,
            /// <summary></summary>
            O = 0x4F,
            /// <summary></summary>
            P = 0x50,
            /// <summary></summary>
            Q = 0x51,
            /// <summary></summary>
            R = 0x52,
            /// <summary></summary>
            S = 0x53,
            /// <summary></summary>
            T = 0x54,
            /// <summary></summary>
            U = 0x55,
            /// <summary></summary>
            V = 0x56,
            /// <summary></summary>
            W = 0x57,
            /// <summary></summary>
            X = 0x58,
            /// <summary></summary>
            Y = 0x59,
            /// <summary></summary>
            Z = 0x5A,
            /// <summary></summary>
            LeftWindows = 0x5B,
            /// <summary></summary>
            RightWindows = 0x5C,
            /// <summary></summary>
            Application = 0x5D,
            /// <summary></summary>
            Sleep = 0x5F,
            /// <summary></summary>
            Numpad0 = 0x60,
            /// <summary></summary>
            Numpad1 = 0x61,
            /// <summary></summary>
            Numpad2 = 0x62,
            /// <summary></summary>
            Numpad3 = 0x63,
            /// <summary></summary>
            Numpad4 = 0x64,
            /// <summary></summary>
            Numpad5 = 0x65,
            /// <summary></summary>
            Numpad6 = 0x66,
            /// <summary></summary>
            Numpad7 = 0x67,
            /// <summary></summary>
            Numpad8 = 0x68,
            /// <summary></summary>
            Numpad9 = 0x69,
            /// <summary></summary>
            Multiply = 0x6A,
            /// <summary></summary>
            Add = 0x6B,
            /// <summary></summary>
            Separator = 0x6C,
            /// <summary></summary>
            Subtract = 0x6D,
            /// <summary></summary>
            Decimal = 0x6E,
            /// <summary></summary>
            Divide = 0x6F,
            /// <summary></summary>
            F1 = 0x70,
            /// <summary></summary>
            F2 = 0x71,
            /// <summary></summary>
            F3 = 0x72,
            /// <summary></summary>
            F4 = 0x73,
            /// <summary></summary>
            F5 = 0x74,
            /// <summary></summary>
            F6 = 0x75,
            /// <summary></summary>
            F7 = 0x76,
            /// <summary></summary>
            F8 = 0x77,
            /// <summary></summary>
            F9 = 0x78,
            /// <summary></summary>
            F10 = 0x79,
            /// <summary></summary>
            F11 = 0x7A,
            /// <summary></summary>
            F12 = 0x7B,
            /// <summary></summary>
            F13 = 0x7C,
            /// <summary></summary>
            F14 = 0x7D,
            /// <summary></summary>
            F15 = 0x7E,
            /// <summary></summary>
            F16 = 0x7F,
            /// <summary></summary>
            F17 = 0x80,
            /// <summary></summary>
            F18 = 0x81,
            /// <summary></summary>
            F19 = 0x82,
            /// <summary></summary>
            F20 = 0x83,
            /// <summary></summary>
            F21 = 0x84,
            /// <summary></summary>
            F22 = 0x85,
            /// <summary></summary>
            F23 = 0x86,
            /// <summary></summary>
            F24 = 0x87,
            /// <summary></summary>
            NumLock = 0x90,
            /// <summary></summary>
            ScrollLock = 0x91,
            /// <summary></summary>
            NEC_Equal = 0x92,
            /// <summary></summary>
            Fujitsu_Jisho = 0x92,
            /// <summary></summary>
            Fujitsu_Masshou = 0x93,
            /// <summary></summary>
            Fujitsu_Touroku = 0x94,
            /// <summary></summary>
            Fujitsu_Loya = 0x95,
            /// <summary></summary>
            Fujitsu_Roya = 0x96,
            /// <summary></summary>
            LeftShift = 0xA0,
            /// <summary></summary>
            RightShift = 0xA1,
            /// <summary></summary>
            LeftControl = 0xA2,
            /// <summary></summary>
            RightControl = 0xA3,
            /// <summary></summary>
            LeftMenu = 0xA4,
            /// <summary></summary>
            RightMenu = 0xA5,
            /// <summary></summary>
            BrowserBack = 0xA6,
            /// <summary></summary>
            BrowserForward = 0xA7,
            /// <summary></summary>
            BrowserRefresh = 0xA8,
            /// <summary></summary>
            BrowserStop = 0xA9,
            /// <summary></summary>
            BrowserSearch = 0xAA,
            /// <summary></summary>
            BrowserFavorites = 0xAB,
            /// <summary></summary>
            BrowserHome = 0xAC,
            /// <summary></summary>
            VolumeMute = 0xAD,
            /// <summary></summary>
            VolumeDown = 0xAE,
            /// <summary></summary>
            VolumeUp = 0xAF,
            /// <summary></summary>
            MediaNextTrack = 0xB0,
            /// <summary></summary>
            MediaPrevTrack = 0xB1,
            /// <summary></summary>
            MediaStop = 0xB2,
            /// <summary></summary>
            MediaPlayPause = 0xB3,
            /// <summary></summary>
            LaunchMail = 0xB4,
            /// <summary></summary>
            LaunchMediaSelect = 0xB5,
            /// <summary></summary>
            LaunchApplication1 = 0xB6,
            /// <summary></summary>
            LaunchApplication2 = 0xB7,
            /// <summary></summary>
            OEM1 = 0xBA,
            /// <summary></summary>
            OEMPlus = 0xBB,
            /// <summary></summary>
            OEMComma = 0xBC,
            /// <summary></summary>
            OEMMinus = 0xBD,
            /// <summary></summary>
            OEMPeriod = 0xBE,
            /// <summary></summary>
            OEM2 = 0xBF,
            /// <summary></summary>
            OEM3 = 0xC0,
            /// <summary></summary>
            OEM4 = 0xDB,
            /// <summary></summary>
            OEM5 = 0xDC,
            /// <summary></summary>
            OEM6 = 0xDD,
            /// <summary></summary>
            OEM7 = 0xDE,
            /// <summary></summary>
            OEM8 = 0xDF,
            /// <summary></summary>
            OEMAX = 0xE1,
            /// <summary></summary>
            OEM102 = 0xE2,
            /// <summary></summary>
            ICOHelp = 0xE3,
            /// <summary></summary>
            ICO00 = 0xE4,
            /// <summary></summary>
            ProcessKey = 0xE5,
            /// <summary></summary>
            ICOClear = 0xE6,
            /// <summary></summary>
            Packet = 0xE7,
            /// <summary></summary>
            OEMReset = 0xE9,
            /// <summary></summary>
            OEMJump = 0xEA,
            /// <summary></summary>
            OEMPA1 = 0xEB,
            /// <summary></summary>
            OEMPA2 = 0xEC,
            /// <summary></summary>
            OEMPA3 = 0xED,
            /// <summary></summary>
            OEMWSCtrl = 0xEE,
            /// <summary></summary>
            OEMCUSel = 0xEF,
            /// <summary></summary>
            OEMATTN = 0xF0,
            /// <summary></summary>
            OEMFinish = 0xF1,
            /// <summary></summary>
            OEMCopy = 0xF2,
            /// <summary></summary>
            OEMAuto = 0xF3,
            /// <summary></summary>
            OEMENLW = 0xF4,
            /// <summary></summary>
            OEMBackTab = 0xF5,
            /// <summary></summary>
            ATTN = 0xF6,
            /// <summary></summary>
            CRSel = 0xF7,
            /// <summary></summary>
            EXSel = 0xF8,
            /// <summary></summary>
            EREOF = 0xF9,
            /// <summary></summary>
            Play = 0xFA,
            /// <summary></summary>
            Zoom = 0xFB,
            /// <summary></summary>
            Noname = 0xFC,
            /// <summary></summary>
            PA1 = 0xFD,
            /// <summary></summary>
            OEMClear = 0xFE
        }
        #endregion
    }
}
