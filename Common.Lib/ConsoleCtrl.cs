using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Common.Lib
{
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

        public static void ConsoleHide(string[] args, string namespaceName)
        {
            Console.Title = args.Length > 0 ? args[0] : namespaceName;
            Thread.Sleep(100);
            ConsoleCtrl.HideConsole(Console.Title);
        }
    }
}
