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


		public UIModule()
		{
			priorityModule = new PriorityModule();
			managedData = new ObservableCollection<wdata>();
			Run();
		}
		public bool Run()
		{
			history = priorityModule.Load();
			List<wdata> list = GetWindowsWithHistory();

			for (int i = 0; i < list.Count; i++)
			{
				managedData.Add(list[i]);
			}
			return true;
		}

		public void Destruct(int order_end)
		{
			priorityModule.Save(history);
			Release();
		}
	}
}
