using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace system08
{
    partial class PriorityModule
    {

//        const uint SWP_NOSIZE = 0x0001;
//        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
//        static readonly IntPtr HWND_TOP = new IntPtr(0);
//        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
//        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);

        const int MAX_PRIORITY = 100;

        public void assignPriority(IntPtr hWnd, int activeWindowPriority, ObservableCollection<wdata> managedData)
        {
            /*
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
            //*/
            //*
            List<wdata> w_data = managedData.ToList<wdata>();
            w_data.Sort((a, b) => a.priority - b.priority);
            if (hWnd == IntPtr.Zero)
            {
                for (int i = 0; i < w_data.Count; ++i)
                {
                    bool ret = SetWindowPos(w_data[i].hwnd, HWND_TOP, 0, 0, 0, 0, SWP_SHOWWINDOW | SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
                    if (ret == false)
                    {
                        int errCode = Marshal.GetLastWin32Error();
                        throw new Exception("Win32エラー・コード：" + String.Format("{0:X8}", errCode));
                    }
                }
            }
            else
            {
                for (int i = 0; i < w_data.Count; ++i)
                {
                    if (w_data[i].priority > activeWindowPriority)
                    {
                        bool ret = SetWindowPos(w_data[i].hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_SHOWWINDOW | SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
                        if (ret == false)
                        {
                            int errCode = Marshal.GetLastWin32Error();
                            throw new Exception("Win32エラー・コード：" + String.Format("{0:X8}", errCode));
                        }
                        System.Diagnostics.Trace.WriteLine("HWND:" + w_data[i].hwnd.ToString());
                    }
                    else
                    {
                        if (hWnd != w_data[i].hwnd)
                        {
                            bool ret = SetWindowPos(w_data[i].hwnd, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_SHOWWINDOW | SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
                            if (ret == false)
                            {
                                int errCode = Marshal.GetLastWin32Error();
                                throw new Exception("Win32エラー・コード：" + String.Format("{0:X8}", errCode));
                            }
                            System.Diagnostics.Trace.WriteLine("HWND:" + w_data[i].hwnd.ToString());
                        }
                    }
                }
                //時々、高優先度ウィンドウが前面にならないことがある
                SetWindowPos(hWnd, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_SHOWWINDOW | SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
            }
            SetWindowPos(this_window_hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_SHOWWINDOW | SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
            if (IsZoomed(this_window_hwnd))
                ShowWindow(this_window_hwnd, SW_RESTORE);
            //*/
        }

        /*
        public int Update(int win_num, int priotiry)
        {

            return -1;
        }
        */
    }

}
