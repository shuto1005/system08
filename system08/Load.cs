using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace system08
{ 
	public partial class PriorityModule
	{
		public List<wdata> Load()
		{
			string deserialized = JsonSerializer.Deserialize(data);

			StreamReader reader = new StreamReader(@".\data.txt", false, Encoding.GetEncoding("UTF-8"));
			reader.Read(serialized);
			reader.Close();

		}
	}
}

