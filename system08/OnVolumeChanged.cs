//W1よりデータを読み取り音量の値をC3へ渡す
// -----
// AL18014 井澤明信





using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Controls;

namespace system08
{
	public　partial class UIModule
	{

	　 /// <summary>
       /// 音量の読み取り
       /// <param name"="sender”></param>
       /// <param name"="e" ></param>
       /// </summary>
		public void OnVolumeChanged(object sender, EventArgs e)
		{
			var sl = sender as Slider;
			
       　　 //sliderの直後の数字を文字として取得
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
