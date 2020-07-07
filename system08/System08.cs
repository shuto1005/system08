using System;
using System.Collections.Generic;
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
		}
		public bool Run()
		{
			history = priorityModule.Load();
			return true;
		}

		public void Destruct(int order_end)
		{
			priorityModule.Save(history);
		}
	}
}
