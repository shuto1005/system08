using System;
using System.Collections.Generic;

namespace system08
{

	public partial class Class1
	{
		public Class1()
		{
		}
		public void Save(List<wdata> data)
		{

		}
    }

    /// <summary>
    /// A unit of data to hold window information.
    /// </summary>
    public class wdata
    {
        public int id;
        public int priority;
        public string productName;
        public IntPtr hwnd;

        /// <summary>
        /// hwnd may be Zero when loading from data files.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="priority"></param>
        /// <param name="productName"></param>
        /// <param name="hwnd"></param>
        public wdata(int id, int priority, string productName, IntPtr hwnd)
        {
            this.id = id;
            this.priority = priority;
            this.productName = productName;
            this.hwnd = hwnd;
        }
    }
    partial class UIModule
    {
        /// <summary>
        /// ファイルに保存された情報
        /// </summary>
        public List<wdata> history = new List<wdata>();
        /// <summary>
        /// システムが実際に管理しているウィンドウのコレクション
        /// </summary>
        public ObservableCollection<wdata> managedData = new ObservableCollection<wdata>();
    }
}
