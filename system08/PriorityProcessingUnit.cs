using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
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

    public class wdata
    {
        public int id;
        public int priority;
        public string productName;
        public IntPtr hwnd;
        public wdata(int id,int priority,string productName,IntPtr hwnd)
        {
            this.id = id;
            this.priority = priority;
            this.productName = productName;
            this.hwnd = hwnd;
        }
    }
    partial class UIModule
    {
        public List<wdata> history = new List<wdata>();
        public ObservableCollection<wdata> managedData = new ObservableCollection<wdata>();
    }
}
