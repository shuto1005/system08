//制作者：AL18052 坂本達哉

//内部関数：
//  CheckWindow(IntPtr hWnd)
//		【ウィンドウの有効/無効】を調べる時に呼び出す。

//完成

using System;
using System.Collections.Generic;
using system08;
using System.Runtime.InteropServices;

namespace system08
{
	partial class PriorityModule
	{
		/// <summary>
		/// /【引数で渡されたウィンドウハンドル】が、現在でも有効であるどうか
		/// 有効なら【true】を返す
		/// 無効なら【false】を返す
		/// </summary>
		/// <param name="hWnd"></param>
		/// <returns></returns>
		public bool CheckWindow(IntPtr hWnd)
		{
			return IsWindow(hWnd);
			/*
			List<wdata> list = GetWindows();
			bool flag = false;
			for (int i = 0; i < list.Count; ++i)
			{
				if (hWnd == list[i].hwnd)
				{
					flag = true;
					break;
				}
			}
			return flag;
			*/
		}
		[DllImport("user32.dll")]
		static extern bool IsWindow(IntPtr hWnd);
	}
}