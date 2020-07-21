// C2 優先度処理部でウィンドウの有効・無効を判定する処理を実装
// -----
// AL18052 坂本 達哉

using System;
using System.Collections.Generic;
using system08;
using System.Runtime.InteropServices;

namespace system08
{
	partial class PriorityModule
	{
		/// <summary>
		///ウィンドウが現在でも有効か、無効かを判定する
		/// <param name="hWnd">調べるウィンドウのハンドル</param>
		/// <returns>有効なら【true】無効なら【false】 as bool.</returns>
		/// </summary>
		public bool CheckWindow(IntPtr hWnd)
		{
			return IsWindow(hWnd);
		}
		[DllImport("user32.dll")]
		static extern bool IsWindow(IntPtr hWnd);
	}
}