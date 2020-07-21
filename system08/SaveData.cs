using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Text;
using System.Windows;
using System.Security;

namespace system08
{

	public partial class PriorityModule
	{
        /// <summary>
        /// データを ./data.txt として上書き保存
        /// </summary>
        /// <param name="data"></param>
		public void Save(List<wdata> data)
		{
            try
            {
                string serialized = JsonSerializer.Serialize(data);
                // 上書き保存
                StreamWriter writer = new StreamWriter(@".\data.txt", false, Encoding.GetEncoding("UTF-8"));
                writer.Write(serialized);
                writer.Close();
            } catch(UnauthorizedAccessException e)
            {
                MessageBox.Show("保存データのアクセスが拒否されました\n" + e.Message);
            } catch(SecurityException e)
            {
                MessageBox.Show("呼び出し元に、必要なアクセス許可がありません\n" + e.Message);
            } catch(Exception e)
            {
                MessageBox.Show("エラーが発生しました\n" + e.Message);
            }
        }
    }

    /// <summary>
    /// A unit of data to hold window information.
    /// </summary>
    public class wdata
    {
        // Set as properties, not as member variables to bind data and serialize.
        public int id { get; set; }
        public int priority { get; set; }
        public string productName { get; set; }
        [JsonIgnore]
        public IntPtr hwnd { get; set; } = IntPtr.Zero;
        [JsonIgnore]
        public double volume { get; set; } = 0;

        /// <summary>
        /// ウィンドウ情報のコンストラクタ
        /// </summary>
        public wdata()
        {
            this.id = 0;
            this.priority = 0;
            this.productName = "";
            this.hwnd = IntPtr.Zero;
            this.volume = -1;
        }

        /// <summary>
        /// ウィンドウ情報のコンストラクタ
        /// </summary>
        /// <param name="id"></param>
        /// <param name="priority"></param>
        /// <param name="productName"></param>
        /// <param name="hwnd"></param>
        public wdata(int id, int priority, string productName, IntPtr hwnd, double volume)
        {
            this.id = id;
            this.priority = priority;
            this.productName = productName;
            this.hwnd = hwnd;
            this.volume = volume;
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
