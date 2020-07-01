using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Reflection.Metadata;
using system08;

namespace System08
{
	partial class Class1
	{
		/// <summary>
		/// /Listで保持しているウィンドウハンドルが、現在でも有効であるどうか（破棄されていたらfalse）
		/// </summary>
		/// <param name="hWnd"></param>
		/// <returns></returns>
		private bool CheckWindow(IntPtr hWnd)
		{
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
		}
	}
}