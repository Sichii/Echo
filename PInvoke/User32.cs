using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DAWindower
{
    internal static class User32
    {
        #region GetWindow
        [DllImport("user32.dll")]
        internal static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);
        [DllImport("user32.dll")]
        internal static extern bool GetClientRect(IntPtr hWnd, ref Rect rectangle);
        [DllImport("user32.dll")]
        internal static extern WindowStyleFlags GetWindowLong(IntPtr hWnd, WindowFlags nIndex);
        [DllImport("user32.dll")]
        internal static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        internal static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        #endregion

        #region SetWindow
        [DllImport("user32.dll")]
        internal static extern int SetWindowText(IntPtr hWnd, string text);
        [DllImport("user32.dll")]
        internal static extern int SetWindowLong(IntPtr hWnd, WindowFlags nIndex, WindowStyleFlags dwNewLong);
        [DllImport("user32.dll")]
        internal static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);
        [DllImport("User32.dll")]
        internal static extern int SetForegroundWindow(int hWnd);
        #endregion

        #region WindowMethods
        [DllImport("user32.dll")]
        internal static extern bool ShowWindowAsync(IntPtr hWnd, ShowWindowFlags nCmdShow);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ShowWindow(IntPtr hWnd, ShowWindowFlags nCmdShow);
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hwnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsWindowVisible(IntPtr hWnd);
        #endregion

        #region Hooking
        [DllImport("user32.dll")]
        internal static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        internal static extern int MapVirtualKey(int uCode, int uMapType);
        [DllImport("user32.dll")]
        internal static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32.dll")]
        internal static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        [DllImport("user32.dll")]
        internal static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        internal static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        internal static extern uint MapVirtualKeyEx(uint uCode, uint uMapType, IntPtr dwhkl);
        #endregion
    }
}
