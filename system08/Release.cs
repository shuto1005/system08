﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using system08;

namespace system08
{
    partial class UIModule
    {
        public bool Release()
        {
            List<wdata> w_list = managedData.ToList();
            w_list.Sort((a, b) => a.priority - b.priority);
            int i;
            for (i = 0; i < w_list.Count; ++i)
            {
                IntPtr hWnd = w_list[i].hwnd;
                if (priorityModule.CheckWindow(hWnd))
                    SetWindowPos(hWnd, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
            }
            return true;
        }
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOACTIVATE = 0x0010;
        private const int HWND_TOPMOST = -1;
        private const int HWND_NOTOPMOST = -2;
    }
}