//制作者：AL18052 坂本達哉

//内部関数：
//  List<wdata> GetWindows()
//		ウィンドウを読み込む時に呼び出す
//  bool EnumWindowCallBack(IntPtr hWnd, IntPtr lparam)
//      【EnumWindows関数】のコールバック関数

//完成

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using system08;

namespace system08
{
    partial class PriorityModule
    {
        // 【GetWindows関数】の戻り値で利用する変数
        private List<wdata> m_GetWindows_List;
        //private int count = 0;

        /// <summary>
        /// ウィンドウを読み込む【EnumWindows関数】を呼び出すための関数
        /// List型の変数【m_GetWindow_List】を返す
        /// </summary>
        /// <returns name="m_GetWindows_List"></returns>
        public List<wdata> GetWindows()
        {
            //count = 0;
            m_GetWindows_List = new List<wdata>();
            //ウィンドウを列挙する
            EnumWindows(new EnumWindowsDelegate(EnumWindowCallBack), IntPtr.Zero);
            return m_GetWindows_List;
        }



        /// <summary>
        /// EnumWindows関数のコールバック関数（必須）
        /// 全てのウィンドウを読み込み、フィルタを通して必要なウィンドウを識別する。
        /// 正しく読み込んだウィンドウは、List型の変数 m_GetWindow_List へ格納する。
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lparam"></param>
        /// <returns></returns>
        private bool EnumWindowCallBack(IntPtr hWnd, IntPtr lparam)
        {
            //ウィンドウが可視状態かどうかを取得する
            if (!IsWindowVisible(hWnd))
                return true;

            //ウィンドウがバックグラウンド状態かどうか（UDPアプリ）
            DwmGetWindowAttribute(hWnd, DWMWINDOWATTRIBUTE.Cloaked, out var isCloaked, Marshal.SizeOf(typeof(bool)));
            if (isCloaked)
                return true;

            //ウィンドウのタイトルの長さを取得する
            int textLen = GetWindowTextLength(hWnd);
            if (textLen == 0)
                return true;

            //最小化なら読み込まない
            if (IsIconic(hWnd))
                return true;

            //ウィンドウのタイトルを取得する
            StringBuilder tsb = new StringBuilder(textLen + 1);
            GetWindowText(hWnd, tsb, tsb.Capacity);

            //ウィンドウのクラス名を取得する
            StringBuilder csb = new StringBuilder(256);
            GetClassName(hWnd, csb, csb.Capacity);

            //【Progman】というソフトを除外
            if (csb.ToString() == "Progman")
                return true;

            // ウィンドウハンドルからプロセスIDを取得
            int processId;
            GetWindowThreadProcessId(hWnd, out processId);

            // プロセスIDからProcessクラスのインスタンスを取得
            Process p = Process.GetProcessById(processId);


            /*/結果をLogに表示する（以下のデータはウィンドウを開閉する度に値が変わる）
            ++count;
            System.Diagnostics.Trace.WriteLine(count + "番目");
            System.Diagnostics.Trace.WriteLine("HWND:" + hWnd.ToString());
            System.Diagnostics.Trace.WriteLine("タイトル:" + tsb.ToString());
            System.Diagnostics.Trace.WriteLine("クラス名:" + csb.ToString());
            System.Diagnostics.Trace.WriteLine("プロセス名:" + p.ToString());
            System.Diagnostics.Trace.WriteLine("プロセスID:" + processId.ToString());
            //*/


            //ハッシュ法で【wdataクラスのid】を作成する
            //クラス名から２桁の整数を取得する
            string str = p.ToString().Replace("System.Diagnostics.Process", "");
            int i, sum = 0;
            for (i = 0; i < csb.ToString().Length; ++i)
            {
                char ch = csb.ToString()[i];
                sum += ch;
            }
            int csb_num = sum % 100 * 100;
            
            //プロセス名から２桁の整数を取得する
            sum = 0;
            for (i = 0; i < str.ToString().Length; ++i)
            {
                char ch = str.ToString()[i];
                sum += ch;
            }
            int str_num = sum % 100;

            //【num】の範囲は【0000～9999】上位2桁：クラス名  下位2桁：プロセス名
            int num = csb_num + str_num;
            if (num < 0)
                num = -num;

            //GetWindows関数の戻り値：List<wdata>へデータを追加
            wdata wd = new wdata(num, -1, tsb.ToString(), hWnd, -1);
            m_GetWindows_List.Add(wd);

            //true：次のウィンドウを列挙 <-> false：停止
            return true;
        }


        //以下：変数・関数の宣言

        public delegate bool EnumWindowsDelegate(IntPtr hWnd, IntPtr lparam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool EnumWindows(EnumWindowsDelegate lpEnumFunc, IntPtr lparam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

        [DllImport("user32")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32")]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("user32")]
        private static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32")]
        private static extern bool IsZoomed(IntPtr hWnd);

        [DllImport("user32")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private enum DWMWINDOWATTRIBUTE : uint
        {
            NCRenderingEnabled = 1,
            NCRenderingPolicy,
            TransitionsForceDisabled,
            AllowNCPaint,
            CaptionButtonBounds,
            NonClientRtlLayout,
            ForceIconicRepresentation,
            Flip3DPolicy,
            ExtendedFrameBounds,
            HasIconicBitmap,
            DisallowPeek,
            ExcludedFromPeek,
            Cloak,
            Cloaked,
            FreezeRepresentation
        }
        [DllImport("dwmapi.dll")]
        static extern int DwmGetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE dwAttribute, out bool pvAttribute, int cbAttribute);

        [DllImport("user32.dll")]
        static extern IntPtr FindWindow(string IpClassName, string IpWindowName);

        
        //SetWindowPos()のuFlagsを設定する定数
        private const int SWP_NOSIZE = 0x0001; //サイズを維持
        private const int SWP_NOMOVE = 0x0002; //位置を維持
        private const int SWP_NOZORDER = 0x0004; //Zオーダーを維持 //TopMostと併用不可
        private const int SWP_NOACTIVATE = 0x0010; //アクティブにしない
        private const int SWP_SHOWWINDOW = 0x0040; //ウィンドウを表示
        private const int SWP_NOSENDCHANGING = 0x0400; //ウィンドウがWM_WINDOWPOSCHANGINGメッセージを受信しないようにする
        //アクティブにするには一度最上位にする必要がある？？
        private const int SWP_NOOWNERZORDER = 0x0200; //子ウィンドウ内でZオーダーを変更できる（タブとか？）

        private const int HWND_TOPMOST = -1; //最前列グループ
        private const int HWND_NOTOPMOST = -2; //最前列グループ解除
        private const int HWND_TOP = 0; //前列グループの中で一番前
        private const int HWND_BOTTOM = 1; //前列グループの中で一番後ろ

        private const int SW_RESTORE = 9; //最小化,最大化を解除//IsIconic,IsZoomedと組み合わせる
        private const int SW_SHOW = 5;
    }
}