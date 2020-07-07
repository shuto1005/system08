using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Controls;

namespace system08
{
	public　partial class UIModule
	{

		private void OnVolumeChanged(object sender, EventArgs e)
		{
			var sl = sender as Slider;
			if (sl == null)
				return;

			string str = sl.Name.Remove(0, 6);


			int num;
			if (int.TryParse(str, out num) == false)
				return;


			IntPtr hWnd = managedData[num].hwnd;
			double volume = sl.Value;

			// ウィンドウハンドルからプロセスIDを取得
			int processId;
			GetWindowThreadProcessId(hWnd, out processId);


			AudioModule.SetVolume(processId, volume);
		}

		//以下：変数・関数の宣言

		[DllImport("user32")]
		private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

	}
}
