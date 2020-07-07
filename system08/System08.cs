using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace system08
{
	public class UIModule

	{
		List<wdata> data;

		private PriorityModule priorityModule;


		public UIModule()
		{
			priorityModule = new PriorityModule();
		}
		public bool Run()
		{
			data = Load();
			return true;
		}

		public void Destruct(int order_end)
		{
			Save(data);
		}
	}
}
