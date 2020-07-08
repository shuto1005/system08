using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
		}
		public bool Run()
		{
			history = priorityModule.Load();
			List<wdata> list = priorityModule.GetWindows();
			if(list.Count > 0)
            {

            }
			return true;
		}

		public void Destruct(int order_end)
		{
			priorityModule.Save(history);
		}
	}
}
