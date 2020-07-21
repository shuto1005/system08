// C2 優先度処理部で実装する関数の実装
// -----
// AL18073 鄭秀煥


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Linq;

namespace system08
{
    partial class PriorityModule
    {

        const int MAX_PRIORITY = 100;///優先度の上限


        /// <summary>
        /// user32.dllのSetWindowPos関数を利用し、managedDataに登録されているウィンドウを描画する
        /// <param name"=”hWnd”>優先度を変更したウィンドウのプロセスID</param>
        /// <param name"=”activeWindowPriority”>優先度を変更したウィンドウの優先度値</param>
        /// <param name"=”managedData”>ウィンドウ情報wdataのコレクション</param>
        /// <returns>なし</returns>
        /// </summary>

        public void assignPriority(IntPtr hWnd, int activeWindowPriority, ObservableCollection<wdata> managedData)
        {
            List<wdata> w_data = managedData.ToList<wdata>();
            w_data.Sort((a, b) => a.priority - b.priority);//w_data内のwdataを優先度の昇順にソート
            if (hWnd == IntPtr.Zero)//初期化時or更新ボタンクリック時
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
            else//優先度変更時
            {
                for (int i = 0; i < w_data.Count; ++i)//ソートされているため、優先度の低いウィンドウを描画した後、より優先度の高いウィンドウが描画される
                {
                    if (w_data[i].priority > activeWindowPriority)//activeWindowPriorityよりも優先度が高いウィンドウはTOPMOST(非アクティブでも常に前面に表示される)に設定
                    {
                        bool ret = SetWindowPos(w_data[i].hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_SHOWWINDOW | SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
                        if (ret == false)
                        {
                            int errCode = Marshal.GetLastWin32Error();
                            throw new Exception("Win32エラー・コード：" + String.Format("{0:X8}", errCode));
                        }
                        System.Diagnostics.Trace.WriteLine("HWND:" + w_data[i].hwnd.ToString());
                    }
                    else//優先度がactiveWindowPriority以下のウィンドウはNOTOPMOST(一般的なウィンドウの状態)に設定
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
                SetWindowPos(hWnd, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_SHOWWINDOW | SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);//優先度を変更したウィンドウはNOTOPMOSTの中で最前面に描画
            }
            SetWindowPos(this_window_hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_SHOWWINDOW | SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);//本システムのUIウィンドウを最前面に描画
            if (IsZoomed(this_window_hwnd))//本システムのUIウィンドウが全画面表示になっている場合は全画面表示を解除する
                ShowWindow(this_window_hwnd, SW_RESTORE);
        }
    }

}
