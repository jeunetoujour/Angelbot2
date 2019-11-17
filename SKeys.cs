
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace AngelBot
{
    class SKeys
    {
        [DllImport("User32.Dll", EntryPoint = "PostMessageA")]
        public static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        static extern short MapVirtualKey(int wCode, int wMapType);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, uint lParam);
        [DllImport("user32.dll")]
        private static extern byte VkKeyScan(char ch);

        private IntPtr proc;

        public SKeys(IntPtr process)
        {
            proc = process;
        }

        ///summary>
        /// Virtual Messages
        /// </summary>
        public enum WMessages : int
        {
            WM_LBUTTONDOWN = 0x201, //Left mousebutton down
            WM_LBUTTONUP = 0x202,  //Left mousebutton up
            WM_LBUTTONDBLCLK = 0x203, //Left mousebutton doubleclick
            WM_RBUTTONDOWN = 0x204, //Right mousebutton down
            WM_RBUTTONUP = 0x205,   //Right mousebutton up
            WM_RBUTTONDBLCLK = 0x206, //Right mousebutton doubleclick
            WM_SYSKEY = 0x105,
            WM_KEYDOWN = 0x100,  //Key down
            WM_KEYUP = 0x101,   //Key up
        }

        /// <summary>
        /// Virtual Keys
        /// </summary>
        public enum VKeys : int
        {
            VK_LBUTTON = 0x01,   //Left mouse button
            VK_RBUTTON = 0x02,   //Right mouse button
            VK_CANCEL = 0x03,   //Control-break processing
            VK_MBUTTON = 0x04,   //Middle mouse button (three-button mouse)
            VK_BACK = 0x08,   //BACKSPACE key
            VK_TAB = 0x09,   //TAB key
            VK_CLEAR = 0x0C,   //CLEAR key
            VK_RETURN = 0x0D,   //ENTER key
            VK_SHIFT = 0x10,   //SHIFT key
            VK_CONTROL = 0x11,   //CTRL key
            VK_MENU = 0x12,   //ALT key
            VK_PAUSE = 0x13,   //PAUSE key
            VK_CAPITAL = 0x14,   //CAPS LOCK key
            VK_ESCAPE = 0x1B,   //ESC key
            VK_SPACE = 0x20,   //SPACEBAR
            VK_PRIOR = 0x21,   //PAGE UP key
            VK_NEXT = 0x22,   //PAGE DOWN key
            VK_END = 0x23,   //END key
            VK_HOME = 0x24,   //HOME key
            VK_LEFT = 0x25,   //LEFT ARROW key
            VK_UP = 0x26,   //UP ARROW key
            VK_RIGHT = 0x27,   //RIGHT ARROW key
            VK_DOWN = 0x28,   //DOWN ARROW key
            VK_SELECT = 0x29,   //SELECT key
            VK_PRINT = 0x2A,   //PRINT key
            VK_EXECUTE = 0x2B,   //EXECUTE key
            VK_SNAPSHOT = 0x2C,   //PRINT SCREEN key
            VK_INSERT = 0x2D,   //INS key
            VK_DELETE = 0x2E,   //DEL key
            VK_HELP = 0x2F,   //HELP key
            VK_0 = 0x30,   //0 key
            VK_1 = 0x31,   //1 key
            VK_2 = 0x32,   //2 key
            VK_3 = 0x33,   //3 key
            VK_4 = 0x34,   //4 key
            VK_5 = 0x35,   //5 key
            VK_6 = 0x36,    //6 key
            VK_7 = 0x37,    //7 key
            VK_8 = 0x38,   //8 key
            VK_9 = 0x39,    //9 key
            VK_A = 0x41,   //A key
            VK_B = 0x42,   //B key
            VK_C = 0x43,   //C key
            VK_D = 0x44,   //D key
            VK_E = 0x45,   //E key
            VK_F = 0x46,   //F key
            VK_G = 0x47,   //G key
            VK_H = 0x48,   //H key
            VK_I = 0x49,    //I key
            VK_J = 0x4A,   //J key
            VK_K = 0x4B,   //K key
            VK_L = 0x4C,   //L key
            VK_M = 0x4D,   //M key
            VK_N = 0x4E,    //N key
            VK_O = 0x4F,   //O key
            VK_P = 0x50,    //P key
            VK_Q = 0x51,   //Q key
            VK_R = 0x52,   //R key
            VK_S = 0x53,   //S key
            VK_T = 0x54,   //T key
            VK_U = 0x55,   //U key
            VK_V = 0x56,   //V key
            VK_W = 0x57,   //W key
            VK_X = 0x58,   //X key
            VK_Y = 0x59,   //Y key
            VK_Z = 0x5A,    //Z key
            VK_NUMPAD0 = 0x60,   //Numeric keypad 0 key
            VK_NUMPAD1 = 0x61,   //Numeric keypad 1 key
            VK_NUMPAD2 = 0x62,   //Numeric keypad 2 key
            VK_NUMPAD3 = 0x63,   //Numeric keypad 3 key
            VK_NUMPAD4 = 0x64,   //Numeric keypad 4 key
            VK_NUMPAD5 = 0x65,   //Numeric keypad 5 key
            VK_NUMPAD6 = 0x66,   //Numeric keypad 6 key
            VK_NUMPAD7 = 0x67,   //Numeric keypad 7 key
            VK_NUMPAD8 = 0x68,   //Numeric keypad 8 key
            VK_NUMPAD9 = 0x69,   //Numeric keypad 9 key
            VK_SEPARATOR = 0x6C,   //Separator key
            VK_SUBTRACT = 0x6D,   //Subtract key
            VK_DECIMAL = 0x6E,   //Decimal key
            VK_DIVIDE = 0x6F,   //Divide key
            VK_F1 = 0x70,   //F1 key
            VK_F2 = 0x71,   //F2 key
            VK_F3 = 0x72,   //F3 key
            VK_F4 = 0x73,   //F4 key
            VK_F5 = 0x74,   //F5 key
            VK_F6 = 0x75,   //F6 key
            VK_F7 = 0x76,   //F7 key
            VK_F8 = 0x77,   //F8 key
            VK_F9 = 0x78,   //F9 key
            VK_F10 = 0x79,   //F10 key
            VK_F11 = 0x7A,   //F11 key
            VK_F12 = 0x7B,   //F12 key
            VK_SCROLL = 0x91,   //SCROLL LOCK key
            VK_LSHIFT = 0xA0,   //Left SHIFT key
            VK_RSHIFT = 0xA1,   //Right SHIFT key
            VK_LCONTROL = 0xA2,   //Left CONTROL key
            VK_RCONTROL = 0xA3,    //Right CONTROL key
            VK_LMENU = 0xA4,      //Left MENU key
            VK_RMENU = 0xA5,   //Right MENU key
            VK_PLAY = 0xFA,   //Play key
            VK_ZOOM = 0xFB, //Zoom key
        }

        /// <summary>
        /// Sends a message to the specified handle
        /// </summary>
        public void _SendMessage(IntPtr handle, int Msg, int wParam, int lParam)
        {
            //SendMessage(handle, Msg, wParam, lParam);
        }

        /// <summary>
        /// MakeLParam Macro
        /// </summary>
        public int MakeLParam(int LoWord, int HiWord)
        {
            return ((HiWord << 16) | (LoWord & 0xffff));
        }


        public void ControlClickWindow(string button, int x, int y, bool doubleklick)
        {
            IntPtr hWnd = proc;
            int LParam = MakeLParam(x, y);

            int btnDown = 0;
            int btnUp = 0;

            if (button == "left")
            {
                btnDown = (int)WMessages.WM_LBUTTONDOWN;
                btnUp = (int)WMessages.WM_LBUTTONUP;
            }

            if (button == "right")
            {
                btnDown = (int)WMessages.WM_RBUTTONDOWN;
                btnUp = (int)WMessages.WM_RBUTTONUP;
            }


            if (doubleklick == true)
            {
                _SendMessage(hWnd, btnDown, 0, LParam);
                _SendMessage(hWnd, btnUp, 0, LParam);
                _SendMessage(hWnd, btnDown, 0, LParam);
                _SendMessage(hWnd, btnUp, 0, LParam);
            }

            if (doubleklick == false)
            {
                _SendMessage(hWnd, (int)WMessages.WM_LBUTTONDOWN, 0, LParam);
                System.Threading.Thread.Sleep(2000);
                _SendMessage(hWnd, (int)WMessages.WM_LBUTTONUP, 0, LParam);
                //_SendMessage(hWnd, (int)WMessages.WM_RBUTTONDOWN, 0, LParam);
                //_SendMessage(hWnd, btnUp, 0, LParam);
            }

        }


        public void SendMessage1(string message)
        {
            for (int i = 0; i < message.Length; i++)
            {
                PostMessage(proc, KeyConstants.WM_KEYDOWN, VkKeyScan(message[i]), 0);
            }
        }

        public void SendLine(string message)
        {
            for (int i = 0; i < message.Length; i++)
            {
                PostMessage(proc, KeyConstants.WM_KEYDOWN, VkKeyScan(message[i]), 0);
            }
            System.Threading.Thread.Sleep(message.Length * 70);
            PostMessage(proc, KeyConstants.WM_KEYDOWN, KeyConstants.VK_RETURN, 0);
            System.Threading.Thread.Sleep(50);
        }

        public void SendKey(int keyCode)
        {

            SendMessage(proc, KeyConstants.WM_KEYDOWN, KeyConstants.VK_MENU, GetLParam(0, VKeys.VK_MENU, 0, 0, 0, 0));
            System.Threading.Thread.Sleep(50);
            SendMessage(proc, KeyConstants.WM_KEYDOWN, KeyConstants.VK_6, GetLParam(0, VKeys.VK_6, 0, 0, 0, 0));
            //SendMessage(proc, KeyConstants.WM_CHAR, 0x7, GDownLParam);
            System.Threading.Thread.Sleep(50);
            SendMessage(proc, KeyConstants.WM_KEYUP, KeyConstants.VK_6, GetLParam(0, VKeys.VK_6, 0, 0, 0, 0));
            System.Threading.Thread.Sleep(50);
            SendMessage(proc, KeyConstants.WM_KEYUP, KeyConstants.VK_MENU, GetLParam(0, VKeys.VK_MENU, 0, 0, 0, 0));

            //PostMessage(proc, KeyConstants.WM_KEYDOWN, keyCode, 0);
            System.Threading.Thread.Sleep(50);
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
        public void SendUp()
        {
            PostMessage(proc, 0x102, KeyConstants.WM_LBUTTONDOWN, 0);
            PostMessage(proc, 0x102, KeyConstants.WM_RBUTTONDOWN, 0);
            System.Threading.Thread.Sleep(800);
            PostMessage(proc, 0x102, KeyConstants.WM_LBUTTONUP, 0);
            PostMessage(proc, 0x102, KeyConstants.WM_RBUTTONUP, 0);
        }



    }

    public class KeyConstants
    {
        public const int WM_LBUTTONDOWN = 0x201; //UINT?
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_LBUTTONDBLCLK = 0x203;
        public const int WM_RBUTTONDOWN = 0x204;
        public const int WM_RBUTTONUP = 0x205;
        public const int WM_RBUTTONDBLCLK = 0x206;
        public const int WM_CHAR = 0x102;
        public const int WM_KEYDOWN = 0x100;
        public const int WM_KEYUP = 0x101;
        public const int VK_0 = 0x30;
        public const int VK_1 = 0x31;
        public const int VK_2 = 0x32;
        public const int VK_3 = 0x33;
        public const int VK_4 = 0x34;
        public const int VK_5 = 0x35;
        public const int VK_6 = 0x36;
        public const int VK_7 = 0x37;
        public const int VK_8 = 0x38;
        public const int VK_9 = 0x39;
        public const int VK_A = 0x41;
        public const int VK_B = 0x42;
        public const int VK_C = 0x43;
        public const int VK_D = 0x44;
        public const int VK_E = 0x45;
        public const int VK_F = 0x46;
        public const int VK_G = 0x47;
        public const int VK_H = 0x48;
        public const int VK_I = 0x49;
        public const int VK_J = 0x4A;
        public const int VK_K = 0x4B;
        public const int VK_L = 0x4C;
        public const int VK_M = 0x4D;
        public const int VK_N = 0x4E;
        public const int VK_O = 0x4F;
        public const int VK_P = 0x50;
        public const int VK_Q = 0x51;
        public const int VK_R = 0x52;
        public const int VK_S = 0x53;
        public const int VK_T = 0x54;
        public const int VK_U = 0x55;
        public const int VK_V = 0x56;
        public const int VK_W = 0x57;
        public const int VK_X = 0x58;
        public const int VK_Y = 0x59;
        public const int VK_Z = 0x5A;
        public const int VK_ADD = 0x6B;
        public const int VK_ATTN = 0xF6;
        public const int VK_BACK = 0x8;
        public const int VK_CANCEL = 0x3;
        public const int VK_CAPITAL = 0x14;
        public const int VK_CLEAR = 0xC;
        public const int VK_CONTROL = 0x11;
        public const int VK_CRSEL = 0xF7;
        public const int VK_DECIMAL = 0x6E;
        public const int VK_DELETE = 0x2E;
        public const int VK_DIVIDE = 0x6F;
        public const int VK_DOWN = 0x28;
        public const int VK_END = 0x23;
        public const int VK_EREOF = 0xF9;
        public const int VK_ESCAPE = 0x1B;
        public const int VK_EXECUTE = 0x2B;
        public const int VK_EXSEL = 0xF8;
        public const int VK_F1 = 0x70;
        public const int VK_F10 = 0x79;
        public const int VK_F11 = 0x7A;
        public const int VK_F12 = 0x7B;
        public const int VK_F13 = 0x7C;
        public const int VK_F14 = 0x7D;
        public const int VK_F15 = 0x7E;
        public const int VK_F16 = 0x7F;
        public const int VK_F17 = 0x80;
        public const int VK_F18 = 0x81;
        public const int VK_F19 = 0x82;
        public const int VK_F2 = 0x71;
        public const int VK_F20 = 0x83;
        public const int VK_F21 = 0x84;
        public const int VK_F22 = 0x85;
        public const int VK_F23 = 0x86;
        public const int VK_F24 = 0x87;
        public const int VK_F3 = 0x72;
        public const int VK_F4 = 0x73;
        public const int VK_F5 = 0x74;
        public const int VK_F6 = 0x75;
        public const int VK_F7 = 0x76;
        public const int VK_F8 = 0x77;
        public const int VK_F9 = 0x78;
        public const int VK_HELP = 0x2F;
        public const int VK_HOME = 0x24;
        public const int VK_INSERT = 0x2D;
        public const int VK_LBUTTON = 0x1;
        public const int VK_LCONTROL = 0xA2;
        public const int VK_LEFT = 0x25;
        public const int VK_LMENU = 0xA4;
        public const int VK_LSHIFT = 0xA0;
        public const int VK_MBUTTON = 0x4;
        public const int VK_MENU = 0x12;
        public const int VK_MULTIPLY = 0x6A;
        public const int VK_NEXT = 0x22;
        public const int VK_NONAME = 0xFC;
        public const int VK_NUMLOCK = 0x90;
        public const int VK_NUMPAD0 = 0x60;
        public const int VK_NUMPAD1 = 0x61;
        public const int VK_NUMPAD2 = 0x62;
        public const int VK_NUMPAD3 = 0x63;
        public const int VK_NUMPAD4 = 0x64;
        public const int VK_NUMPAD5 = 0x65;
        public const int VK_NUMPAD6 = 0x66;
        public const int VK_NUMPAD7 = 0x67;
        public const int VK_NUMPAD8 = 0x68;
        public const int VK_NUMPAD9 = 0x69;
        public const int VK_OEM_CLEAR = 0xFE;
        public const int VK_PA1 = 0xFD;
        public const int VK_PAUSE = 0x13;
        public const int VK_PLAY = 0xFA;
        public const int VK_PRINT = 0x2A;
        public const int VK_PRIOR = 0x21;
        public const int VK_PROCESSKEY = 0xE5;
        public const int VK_RBUTTON = 0x2;
        public const int VK_RCONTROL = 0xA3;
        public const int VK_RETURN = 0xD;
        public const int VK_RIGHT = 0x27;
        public const int VK_RMENU = 0xA5;
        public const int VK_RSHIFT = 0xA1;
        public const int VK_SCROLL = 0x91;
        public const int VK_SELECT = 0x29;
        public const int VK_SEPARATOR = 0x6C;
        public const int VK_SHIFT = 0x10;
        public const int VK_SNAPSHOT = 0x2C;
        public const int VK_SPACE = 0x20;
        public const int VK_SUBTRACT = 0x6D;
        public const int VK_TAB = 0x9;
        public const int VK_UP = 0x26;
        public const int VK_ZOOM = 0xFB;
    }
}
