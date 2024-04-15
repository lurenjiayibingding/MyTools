//using System.Diagnostics;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Windows.Forms;

//namespace ToolStorage.Definition
//{
//    public class GlobalKeyboardHook : IDisposable
//    {
//        public delegate int KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam);

//        private static IntPtr hHook = IntPtr.Zero;

//        private const int WH_KEYBOARD_LL = 13;
//        private const int WM_KEYDOWN = 0x0100;

//        private static KeyboardHookProc hookProcDelegate;

//        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
//        private static extern IntPtr GetModuleHandle(string lpModuleName);

//        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
//        private static extern IntPtr SetWindowsHookEx(int idHook, KeyboardHookProc lpfn, IntPtr hMod, uint dwThreadId);

//        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
//        [return: MarshalAs(UnmanagedType.Bool)]
//        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

//        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
//        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

//        [DllImport("user32.dll")]
//        private static extern int ToUnicode(uint wVirtKey, uint wScanCode, byte[] lpKeyState, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff, int cchBuff, uint wFlags);

//        private static int HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
//        {
//            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
//            {
//                int vkCode = Marshal.ReadInt32(lParam);
//                if (vkCode == (int)Keys.ControlKey)
//                {
//                    // 检查是否连续按下Ctrl键
//                    if (IsDoubleCtrl())
//                    {
//                        Console.WriteLine("Double Ctrl detected");
//                        // 在这里添加处理双Ctrl键被按下时的逻辑
//                    }
//                }
//            }
//            return CallNextHookEx(hHook, nCode, wParam, lParam);
//        }

//        private static bool IsDoubleCtrl()
//        {
//            // 实现检查是否连续快速按下Ctrl键的逻辑
//            // 这里仅作为示例，实际情况可能需要基于时间戳或者其他方式记录按键状态
//            // 并不是所有情况都能简单地在这里实现，因为键盘消息队列的顺序和延迟可能会有所不同
//            return false; // 请替换为实际实现
//        }

//        public void InstallHook()
//        {
//            hookProcDelegate = new KeyboardHookProc(HookCallback);
//            using (var currentProcess = Process.GetCurrentProcess())
//            using (var currentModule = currentProcess.MainModule)
//            {
//                hHook = SetWindowsHookEx(WH_KEYBOARD_LL, hookProcDelegate, GetModuleHandle(currentModule.ModuleName), 0);
//                if (hHook == IntPtr.Zero)
//                {
//                    throw new Exception("Could not install keyboard hook");
//                }
//            }
//        }

//        public void UninstallHook()
//        {
//            if (hHook != IntPtr.Zero)
//            {
//                UnhookWindowsHookEx(hHook);
//            }
//        }

//        public void Dispose()
//        {
//            UninstallHook();
//        }
//    }
//}
