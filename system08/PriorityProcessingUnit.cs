using System;
using System.Collections.Generic;
using System.Text;

namespace system08
{
    class PriorityProcessingUnit
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int MoveWindow(IntPtr hwnd, int x, int y,int nWidth, int nHeight, int bRepaint);
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public void assignPriority(int window_id, int priority)
        {
            System.Diagnostics.Process p =
            System.Diagnostics.Process.Start("notepad.exe");
            p.WaitForInputIdle();
            MoveWindow(p.MainWindowHandle, 0, 10, 300, 200, 1);
        }

        public int Update(int win_num, int priotiry)
        {

            return -1;
        }
    }

    struct wdata
    {
        String id;
        int priority;
        String productName;
        IntPtr hwnd;
    }
}
