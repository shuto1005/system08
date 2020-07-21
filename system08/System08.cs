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


		public UIModule()　//システム起動
		{
			priorityModule = new PriorityModule();
			managedData = new ObservableCollection<wdata>();
			Run();
		}
		public bool Run() //Run変数はシステムの起動するための変数

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

		public void Destruct(int order_end)　//Destruct変数は前回システムを使用した際のデータを保存する役割

		{
			priorityModule.Save(history);
			Release();
		}
	}
}
