using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Text;

namespace system08
{

	public partial class PriorityModule
	{
		public void Save(List<wdata> data)
		{
            string serialized = JsonSerializer.Serialize(data);
            // 上書き保存
            StreamWriter writer = new StreamWriter(@".\data.txt", false, Encoding.GetEncoding("UTF-8"));
            writer.Write(serialized);
            writer.Close();
        }
    }

    /// <summary>
    /// A unit of data to hold window information.
    /// </summary>
    public class wdata
    {
        // Accessor is needed to serialize.
        public int id { get; set; }
        public int priority { get; set; }
        public string productName { get; set; }
        [JsonIgnore]
        public IntPtr hwnd { get; set; }

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
