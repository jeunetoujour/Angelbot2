using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading;

public class TakeOver

{
#region constants

    private int handle;

    private const int INPUT_MOUSE = 0;
    private const int INPUT_KEYBOARD = 1;
    private const int INPUT_HARDWARE = 2;
    private const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
    private const uint KEYEVENTF_KEYUP = 0x0002;
    private const uint KEYEVENTF_UNICODE = 0x0004;
    private const uint KEYEVENTF_SCANCODE = 0x0008;
    private const uint XBUTTON1 = 0x0001;
    private const uint XBUTTON2 = 0x0002;
    private const uint MOUSEEVENTF_MOVE = 0x0001;
    private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
    private const uint MOUSEEVENTF_LEFTUP = 0x0004;
    private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
    private const uint MOUSEEVENTF_RIGHTUP = 0x0010;
    private const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
    private const uint MOUSEEVENTF_MIDDLEUP = 0x0040;
    private const uint MOUSEEVENTF_XDOWN = 0x0080;
    private const uint MOUSEEVENTF_XUP = 0x0100;
    private const uint MOUSEEVENTF_WHEEL = 0x0800;
    private const uint MOUSEEVENTF_VIRTUALDESK = 0x4000;
    private const uint MOUSEEVENTF_ABSOLUTE = 0x8000;

#endregion
#region DLLImports

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern int SendMessage(int hWnd, int msg, int wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    static extern short MapVirtualKey(int wCode, int wMapType);

    [DllImport("user32.dll")]
    private static extern short VkKeyScan(char ch);
    private const int WM_CHAR = 0x0102;

#endregion

#region Structs
    [StructLayout(LayoutKind.Sequential)]
    struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]  
    struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public uint mouseData;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct HARDWAREINPUT
    { 
        public uint uMsg;
        public ushort wParamL;
        public ushort wParamH;
    }

    [StructLayout(LayoutKind.Explicit)]
    struct INPUT 
    { 
        [FieldOffset(0)]
        public int type;
        [FieldOffset(4)]
        public MOUSEINPUT mi;
        [FieldOffset(4)]
        public KEYBDINPUT ki;
        [FieldOffset(4)]
        public HARDWAREINPUT hi;
    }

#endregion


    public TakeOver(int handle)
    {
        this.handle = handle;
    }

    public void SetFocus()
    {
        SetForegroundWindow(new IntPtr(this.handle));
    }

    public void MoveMouse(int x, int y)
    {
        Rectangle screen = Screen.PrimaryScreen.Bounds;
        int x2 = (65535 * x) / screen.Width;
        int y2 = (65535 * y) / screen.Height;

        INPUT[] inp = new INPUT[2];
        inp[0].type = INPUT_MOUSE;
        inp[0].mi = createMouseInput(x2, y2, 0, 0, MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE);
        SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
    }

    private KEYBDINPUT createKeybdInput(short wVK, uint flag)
    { 
        KEYBDINPUT i = new KEYBDINPUT();
        i.wVk = (ushort)wVK;
        i.wScan = 0;
        i.time = 0;
        i.dwExtraInfo = IntPtr.Zero;
        i.dwFlags = flag;
        return i;
    }


      private MOUSEINPUT createMouseInput(int x, int y, uint data, uint t, uint flag)
      {  
          MOUSEINPUT mi = new MOUSEINPUT();
          mi.dx = x;
          mi.dy = y;
          mi.mouseData = data;
          mi.time = t;
          //mi.dwFlags = MOUSEEVENTF_ABSOLUTE| MOUSEEVENTF_MOVE;
          mi.dwFlags = flag;
          return mi; 
      }

    public void MouseLeftClick()
    { 
        INPUT[] inp = new INPUT[2];
        inp[0].type = INPUT_MOUSE;
        inp[0].mi = createMouseInput(0, 0, 0, 0, MOUSEEVENTF_LEFTDOWN);
        inp[1].type = INPUT_MOUSE;
        inp[1].mi = createMouseInput(0, 0, 0, 0, MOUSEEVENTF_LEFTUP);
        SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
    }

    public void MouseBGLeftClick()
    {
        //SendMessage(new IntPtr(this.handle), (int)Message.KEY_DOWN, 0, GetLParam(0, VKeys.KEY_1, 0, 0, 0, 0));
        //SendMessage(new IntPtr(this.handle), (int)Message.KEY_UP, 0, GetLParam(0, VKeys.KEY_1, 0, 0, 0, 0));
    }

    public void oldMouseBGLeftClick()
    {
        //SendMessage(new IntPtr(this.handle), (int)Message.LBUTTONDOWN, 0, GetLParam(0,VKeys.KEY_LBUTTON,0,0,0,0));
        //SendMessage(new IntPtr(this.handle), (int)Message.LBUTTONUP, 0, GetLParam(0, VKeys.KEY_LBUTTON, 0, 0, 0, 0));
        //SendMessage(hWnd, btnDown, 0, LParam);
        //SendMessage(hWnd, btnUp, 0, LParam); 
    }
    public void MouseLeftDown()
    {
        INPUT[] inp = new INPUT[1];
        inp[0].type = INPUT_MOUSE;
        inp[0].mi = createMouseInput(0, 0, 0, 0, MOUSEEVENTF_LEFTDOWN);
        SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
    }
    public void MouseLeftUp()
    {
        INPUT[] inp = new INPUT[1];
        inp[0].type = INPUT_MOUSE;
        inp[0].mi = createMouseInput(0, 0, 0, 0, MOUSEEVENTF_LEFTUP);
        SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
    }


    public void MouseRightClick()
    {
        INPUT[] inp = new INPUT[2];
        inp[0].type = INPUT_MOUSE;
        inp[0].mi = createMouseInput(0, 0, 0, 0, MOUSEEVENTF_RIGHTDOWN);
        inp[1].type = INPUT_MOUSE;
        inp[1].mi = createMouseInput(0, 0, 0, 0, MOUSEEVENTF_RIGHTUP);
        SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
    }
    public void MouseRightDown()
    {
        INPUT[] inp = new INPUT[1];
        inp[0].type = INPUT_MOUSE;
        inp[0].mi = createMouseInput(0, 0, 0, 0, MOUSEEVENTF_RIGHTDOWN);
        SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
    }
    public void MouseRightUp()
    {
        INPUT[] inp = new INPUT[1];
        inp[0].type = INPUT_MOUSE;
        inp[0].mi = createMouseInput(0, 0, 0, 0, MOUSEEVENTF_RIGHTUP);
        SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
    }
    public enum WMessages : int
    {
        WM_LBUTTONDOWN = 0x201, //Left mousebutton down
        WM_LBUTTONUP = 0x202,  //Left mousebutton up
        WM_LBUTTONDBLCLK = 0x203, //Left mousebutton doubleclick
        WM_RBUTTONDOWN = 0x204, //Right mousebutton down
        WM_RBUTTONUP = 0x205,   //Right mousebutton up
        WM_RBUTTONDBLCLK = 0x206, //Right mousebutton doubleclick
        WM_KEYDOWN = 0x100,  //Key down
        WM_KEYUP = 0x101,   //Key up
    } 
    public void MouseMiddleClick()
    {
        INPUT[] inp = new INPUT[2];
        inp[0].type = INPUT_MOUSE;
        inp[0].mi = createMouseInput(0, 0, 0, 0, MOUSEEVENTF_MIDDLEDOWN);
        inp[1].type = INPUT_MOUSE;
        inp[1].mi = createMouseInput(0, 0, 0, 0, MOUSEEVENTF_MIDDLEUP);
        SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
    }
    
    
    

    //this uses sendinput rather than sendmessage, it will only send the actual key press not with shift or alt just key press
    public void KeypressString(string txt)
    {
        short c;
        INPUT[] inp;
        if (txt == null || txt.Length == 0)
            return;
        inp = new INPUT[2];
        for (int i = 0; i < txt.Length; i++)
        { 
            c = VkKeyScan(txt[i]);
            inp[0].type = INPUT_KEYBOARD;
            inp[0].ki = createKeybdInput(c, 0);
            inp[1].type = INPUT_KEYBOARD;
            inp[1].ki = createKeybdInput(c, KEYEVENTF_KEYUP);
            SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
        } 
    }

    //this will send a string to the window minimized but it does not support shift'ed or alt'ed characters
    public void SendKeyboardText(string txt)
    {
        foreach (char c in txt)
        {
            SendMessage(handle, WM_CHAR, c, IntPtr.Zero);
            Thread.Sleep(20);
        }
    }
    //this will send a char to a minamized window but does not support shif'ed or alt'd characters or movement characters
    public void SendChar(char c)
    {
        //SendMessage(handle, WM_CHAR, c, IntPtr.Zero);
    }
    public enum Message : int
    {
        /// <summary>Key down</summary>
        KEY_DOWN = (0x0100),
        /// <summary>Key up</summary>
        KEY_UP = (0x0101),
        /// <summary>The character being pressed</summary>
        VM_CHAR = (0x0102),
        /// <summary>An Alt/ctrl/shift + key down message</summary>
        SYSKEYDOWN = (0x0104),
        /// <summary>An Alt/Ctrl/Shift + Key up Message</summary>
        SYSKEYUP = (0x0105),
        /// <summary>An Alt/Ctrl/Shift + Key character Message</summary>
        SYSCHAR = (0x0106),
        /// <summary>Left mousebutton down</summary>
        LBUTTONDOWN = (0x201),
        /// <summary>Left mousebutton up</summary>
        LBUTTONUP = (0x202),
        /// <summary>Left mousebutton double left click</summary>
        LBUTTONDBLCLK = (0x203),
        /// <summary>Right mousebutton down</summary>
        RBUTTONDOWN = (0x204),
        /// <summary>Right mousebutton up</summary>
        RBUTTONUP = (0x205),
        /// <summary>Right mousebutton doubleclick</summary>
        RBUTTONDBLCLK = (0x206)
    }
    public enum MapType : uint
    {
        /// <summary>uCode is a virtual-key code and is translated into a scan code.
        /// If it is a virtual-key code that does not distinguish between left- and
        /// right-hand keys, the left-hand scan code is returned.
        /// If there is no translation, the function returns 0.
        /// </summary>
        /// <remarks></remarks>
        MAPVK_VK_TO_VSC = 0x0,

        /// <summary>uCode is a scan code and is translated into a virtual-key code that
        /// does not distinguish between left- and right-hand keys. If there is no
        /// translation, the function returns 0.
        /// </summary>
        /// <remarks></remarks>
        MAPVK_VSC_TO_VK = 0x1,

        /// <summary>uCode is a virtual-key code and is translated into an unshifted
        /// character value in the low-order word of the return value. Dead keys (diacritics)
        /// are indicated by setting the top bit of the return value. If there is no
        /// translation, the function returns 0.
        /// </summary>
        /// <remarks></remarks>
        MAPVK_VK_TO_CHAR = 0x2,

        /// <summary>Windows NT/2000/XP: uCode is a scan code and is translated into a
        /// virtual-key code that distinguishes between left- and right-hand keys. If
        /// there is no translation, the function returns 0.
        /// </summary>
        /// <remarks></remarks>
        MAPVK_VSC_TO_VK_EX = 0x3,

        /// <summary>Not currently documented
        /// </summary>
        /// <remarks></remarks>
        MAPVK_VK_TO_VSC_EX = 0x4,
    }

    private static uint GetLParam(Int16 repeatCount, VKeys key, byte extended, byte contextCode, byte previousState, byte transitionState)
    {
        uint lParam = (uint)repeatCount; 
        uint scanCode = (uint)MapVirtualKey((int)key, 0x4);//MapType.MAPVK_VK_TO_VSC_EX);
        lParam += (uint)(scanCode * 0x10000);
        lParam += (uint)((extended) * 0x1000000);
        lParam += (uint)((contextCode * 2) * 0x10000000);
        lParam += (uint)((previousState * 4) * 0x10000000);
        lParam += (uint)((transitionState * 8) * 0x10000000);
        return lParam;
    }
    public enum VKeys : int
    {
        ///<summary>0 key </summary>
        KEY_0 = 0x30,
        ///<summary>1 key </summary>
        KEY_1 = 0x31,
        ///<summary>2 key </summary>
        KEY_2 = 0x32,
        ///<summary>3 key </summary>
        KEY_3 = 0x33,
        ///<summary>4 key </summary>
        KEY_4 = 0x34,
        ///<summary>5 key </summary>
        KEY_5 = 0x35,
        ///<summary>6 key </summary>
        KEY_6 = 0x36,
        ///<summary>7 key </summary>
        KEY_7 = 0x37,
        ///<summary>8 key </summary>
        KEY_8 = 0x38,
        ///<summary>9 key</summary>
        KEY_9 = 0x39,
        ///<summary> - key</summary>
        KEY_MINUS = 0xBD,
        ///<summary> + key</summary>
        KEY_PLUS = 0xBB,
        ///<summary>A key </summary>
        KEY_A = 0x41,
        ///<summary>B key </summary>
        KEY_B = 0x42,
        ///<summary>C key </summary>
        KEY_C = 0x43,
        ///<summary>D key </summary>
        KEY_D = 0x44,
        ///<summary>E key </summary>
        KEY_E = 0x45,
        ///<summary>F key </summary>
        KEY_F = 0x46,
        ///<summary>G key </summary>
        KEY_G = 0x47,
        ///<summary>H key </summary>
        KEY_H = 0x48,
        ///<summary>I key </summary>
        KEY_I = 0x49,
        ///<summary>J key </summary>
        KEY_J = 0x4A,
        ///<summary>K key </summary>
        KEY_K = 0x4B,
        ///<summary>L key </summary>
        KEY_L = 0x4C,
        ///<summary>M key </summary>
        KEY_M = 0x4D,
        ///<summary>N key </summary>
        KEY_N = 0x4E,
        ///<summary>O key </summary>
        KEY_O = 0x4F,
        ///<summary>P key </summary>
        KEY_P = 0x50,
        ///<summary>Q key </summary>
        KEY_Q = 0x51,
        ///<summary>R key </summary>
        KEY_R = 0x52,
        ///<summary>S key </summary>
        KEY_S = 0x53,
        ///<summary>T key </summary>
        KEY_T = 0x54,
        ///<summary>U key </summary>
        KEY_U = 0x55,
        ///<summary>V key </summary>
        KEY_V = 0x56,
        ///<summary>W key </summary>
        KEY_W = 0x57,
        ///<summary>X key </summary>
        KEY_X = 0x58,
        ///<summary>Y key </summary>
        KEY_Y = 0x59,
        ///<summary>Z key </summary>
        KEY_Z = 0x5A,
        ///<summary>Left mouse button </summary>
        KEY_LBUTTON = 0x01,
        ///<summary>Right mouse button </summary>
        KEY_RBUTTON = 0x02,
        ///<summary>Control-break processing </summary>
        KEY_CANCEL = 0x03,
        ///<summary>Middle mouse button (three-button mouse) </summary>
        KEY_MBUTTON = 0x04,
        ///<summary>BACKSPACE key </summary>
        KEY_BACK = 0x08,
        ///<summary>TAB key </summary>
        KEY_TAB = 0x09,
        ///<summary>CLEAR key </summary>
        KEY_CLEAR = 0x0C,
        ///<summary>ENTER key </summary>
        KEY_RETURN = 0x0D,
        ///<summary>SHIFT key </summary>
        KEY_SHIFT = 0x10,
        ///<summary>CTRL key </summary>
        KEY_CONTROL = 0x11,
        ///<summary>ALT key </summary>
        KEY_MENU = 0x12,
        ///<summary>PAUSE key </summary>
        KEY_PAUSE = 0x13,
        ///<summary>CAPS LOCK key </summary>
        KEY_CAPITAL = 0x14,
        ///<summary>ESC key </summary>
        KEY_ESCAPE = 0x1B,
        ///<summary>SPACEBAR </summary>
        KEY_SPACE = 0x20,
        ///<summary>PAGE UP key </summary>
        KEY_PRIOR = 0x21,
        ///<summary>PAGE DOWN key </summary>
        KEY_NEXT = 0x22,
        ///<summary>END key </summary>
        KEY_END = 0x23,
        ///<summary>HOME key </summary>
        KEY_HOME = 0x24,
        ///<summary>LEFT ARROW key </summary>
        KEY_LEFT = 0x25,
        ///<summary>UP ARROW key </summary>
        KEY_UP = 0x26,
        ///<summary>RIGHT ARROW key </summary>
        KEY_RIGHT = 0x27,
        ///<summary>DOWN ARROW key </summary>
        KEY_DOWN = 0x28,
        ///<summary>SELECT key </summary>
        KEY_SELECT = 0x29,
        ///<summary>PRINT key </summary>
        KEY_PRINT = 0x2A,
        ///<summary>EXECUTE key </summary>
        KEY_EXECUTE = 0x2B,
        ///<summary>PRINT SCREEN key </summary>
        KEY_SNAPSHOT = 0x2C,
        ///<summary>INS key </summary>
        KEY_INSERT = 0x2D,
        ///<summary>DEL key </summary>
        KEY_DELETE = 0x2E,
        ///<summary>HELP key </summary>
        KEY_HELP = 0x2F,
        ///<summary>Numeric keypad 0 key </summary>
        KEY_NUMPAD0 = 0x60,
        ///<summary>Numeric keypad 1 key </summary>
        KEY_NUMPAD1 = 0x61,
        ///<summary>Numeric keypad 2 key </summary>
        KEY_NUMPAD2 = 0x62,
        ///<summary>Numeric keypad 3 key </summary>
        KEY_NUMPAD3 = 0x63,
        ///<summary>Numeric keypad 4 key </summary>
        KEY_NUMPAD4 = 0x64,
        ///<summary>Numeric keypad 5 key </summary>
        KEY_NUMPAD5 = 0x65,
        ///<summary>Numeric keypad 6 key </summary>
        KEY_NUMPAD6 = 0x66,
        ///<summary>Numeric keypad 7 key </summary>
        KEY_NUMPAD7 = 0x67,
        ///<summary>Numeric keypad 8 key </summary>
        KEY_NUMPAD8 = 0x68,
        ///<summary>Numeric keypad 9 key </summary>
        KEY_NUMPAD9 = 0x69,
        ///<summary>Separator key </summary>
        KEY_SEPARATOR = 0x6C,
        ///<summary>Subtract key </summary>
        KEY_SUBTRACT = 0x6D,
        ///<summary>Decimal key </summary>
        KEY_DECIMAL = 0x6E,
        ///<summary>Divide key </summary>
        KEY_DIVIDE = 0x6F,
        ///<summary>F1 key </summary>
        KEY_F1 = 0x70,
        ///<summary>F2 key </summary>
        KEY_F2 = 0x71,
        ///<summary>F3 key </summary>
        KEY_F3 = 0x72,
        ///<summary>F4 key </summary>
        KEY_F4 = 0x73,
        ///<summary>F5 key </summary>
        KEY_F5 = 0x74,
        ///<summary>F6 key </summary>
        KEY_F6 = 0x75,
        ///<summary>F7 key </summary>
        KEY_F7 = 0x76,
        ///<summary>F8 key </summary>
        KEY_F8 = 0x77,
        ///<summary>F9 key </summary>
        KEY_F9 = 0x78,
        ///<summary>F10 key </summary>
        KEY_F10 = 0x79,
        ///<summary>F11 key </summary>
        KEY_F11 = 0x7A,
        ///<summary>F12 key </summary>
        KEY_F12 = 0x7B,
        ///<summary>SCROLL LOCK key </summary>
        KEY_SCROLL = 0x91,
        ///<summary>Left SHIFT key </summary>
        KEY_LSHIFT = 0xA0,
        ///<summary>Right SHIFT key </summary>
        KEY_RSHIFT = 0xA1,
        ///<summary>Left CONTROL key </summary>
        KEY_LCONTROL = 0xA2,
        ///<summary>Right CONTROL key </summary>
        KEY_RCONTROL = 0xA3,
        ///<summary>Left MENU key </summary>
        KEY_LMENU = 0xA4,
        ///<summary>Right MENU key </summary>
        KEY_RMENU = 0xA5,
        ///<summary>, key</summary>
        KEY_COMMA = 0xBC,
        ///<summary>. key</summary>
        KEY_PERIOD = 0xBE,
        ///<summary>Play key </summary>
        KEY_PLAY = 0xFA,
        ///<summary>Zoom key </summary>
        KEY_ZOOM = 0xFB,
        NULL = 0x0,
    }
    
    public void PressKey(Keys key)
    { 

        INPUT[] inp;
        inp = new INPUT[2];
        inp[0].type = INPUT_KEYBOARD;
        inp[0].ki = createKeybdInput((short)key, 0);
        inp[1].type = INPUT_KEYBOARD;
        inp[1].ki = createKeybdInput((short)key, KEYEVENTF_KEYUP);
        SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));
        
    }

    // this will work with movment characters but not in the background
    public void PressKeyDown(Keys key)
    {
        INPUT[] inp;
        inp = new INPUT[1];
        inp[0].type = INPUT_KEYBOARD;
        inp[0].ki = createKeybdInput((short)key, 0);
        SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));

   }

    public void PressKeyUp(Keys key)
    {
        INPUT[] inp;
        inp = new INPUT[1];
        inp[0].type = INPUT_KEYBOARD;
        inp[0].ki = createKeybdInput((short)key, KEYEVENTF_KEYUP);
        SendInput((uint)inp.Length, inp, Marshal.SizeOf(inp[0].GetType()));

    }


}


