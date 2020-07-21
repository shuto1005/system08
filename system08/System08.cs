// C1 UI処理部（起動処理、終了処理）の実装
// -----
// AL18088 野本秀登


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;

namespace system08
{
	public partial class UIModule

	{

		private PriorityModule priorityModule;

		/// <summary>
		/// UIModuleのコンストラクタ
		/// </summary>
		public UIModule()
		{
			// 初期化
			priorityModule = new PriorityModule();
			managedData = new ObservableCollection<wdata>();
			Run();
		}
		 /// <summary>
        /// システムの起動
        /// </summary>
		public bool Run()

		{
			history = priorityModule.Load();
			List<wdata> list = GetWindowsWithHistory();

			for (int i = 0; i < list.Count; i++) //開いているアプリケーションのカウント
			{
				managedData.Add(list[i]);
			}
			try
			{
				priorityModule.assignPriority(IntPtr.Zero, 99, managedData);
			} catch(Exception e)
            {
				System.Windows.MessageBox.Show("コンストラクタでの例外\n" + e.Message);
            }
			return true;
		}
		 /// <summary>
        /// データを保存
        /// </summary>
        /// <param name="order_end">フラグ</param>
		public void Destruct(int order_end)　

		{
			priorityModule.Save(history);
			Release();
		}
	}
}
