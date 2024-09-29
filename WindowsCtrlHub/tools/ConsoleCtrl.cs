/*
 * @author: S 2024/9/27 17:08:48
 */

using System;
using System.Runtime.InteropServices;

namespace WindowsCtrlHub.tools
{
    /// <summary>
    /// 调用系统dll
    /// </summary>
    public class ConsoleCtrl
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

        public static void HideConsole(string consoleTitle)
        {
            consoleTitle = string.IsNullOrEmpty(consoleTitle) ? Console.Title : consoleTitle;
            IntPtr hWnd = FindWindow("ConsoleWindowClass", consoleTitle);
            if (hWnd != IntPtr.Zero)
            {
                ShowWindow(hWnd, 0);
            }
        }

        public static void ShowConsole(string consoleTitle)
        {
            consoleTitle = string.IsNullOrEmpty(consoleTitle) ? Console.Title : consoleTitle;
            IntPtr hWnd = FindWindow("ConsoleWindowClass", consoleTitle);
            if (hWnd != IntPtr.Zero)
            {
                ShowWindow(hWnd, 1);
            }
        }

        public static void MiniConsole(string consoleTitle)
        {
            consoleTitle = string.IsNullOrEmpty(consoleTitle) ? Console.Title : consoleTitle;
            IntPtr hWnd = FindWindow("ConsoleWindowClass", consoleTitle);
            if (hWnd != IntPtr.Zero)
            {
                ShowWindow(hWnd, 2);
            }
        }
    }
}
