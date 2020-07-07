using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace system08
{
    class PriorityProcessingUnit
    {

        const uint SWP_NOSIZE = 0x0001;
        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        static readonly IntPtr HWND_TOP = new IntPtr(0);
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);

        const int MAX_PRIORITY = 100;

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint flags);

        public void assignPriority(int activeWindowPriority, ObservableCollection<wdata> managedData)
        {

            for (int j = MAX_PRIORITY; j >= activeWindowPriority; j--)
            {
                for (int i = 0; i < managedData.Count; i++)
                {

                    if (j == managedData[i].priority)
                    {
                        bool ret = SetWindowPos(managedData[i].hwnd, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOSIZE);
                        if (ret == false)
                        {
                            int errCode = Marshal.GetLastWin32Error();
                            throw new Exception("Win32エラー・コード：" +
                                String.Format("{0:X8}", errCode));

                        }
                    }

                }

            }


            for (int j = activeWindowPriority; j >= 0; j--)
            {
                for (int i = 0; i < managedData.Count; i++)
                {

                    if (j == managedData[i].priority)
                    {
                        bool ret = SetWindowPos(managedData[i].hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE);
                        if (ret == false)
                        {
                            int errCode = Marshal.GetLastWin32Error();
                            throw new Exception("Win32エラー・コード：" +
                                String.Format("{0:X8}", errCode));

                        }
                    }

                }

            }
        }

        /*
        public int Update(int win_num, int priotiry)
        {

            return -1;
        }
        */
    }

    
    partial class UIModule
    {
        public List<wdata> history = new List<wdata>();
        public ObservableCollection<wdata> managedData = new ObservableCollection<wdata>();
    }
}
