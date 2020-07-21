//W1よりデータを読み込みC1へ値を渡す
// -----
// AL18014 井澤明信 



using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace system08
{ 
	public partial class PriorityModule
	{ 
        /// <summary>
        ///データ読み込み
        /// <returns> history </returns>
        /// </summary>
		public List<wdata> Load()
		{
            try
            {

            using (StreamReader sr = new StreamReader(@".\data.txt"))
                {
                    string line;
                    
                    if((line = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                        //デシリアライズ
                        List<wdata> history = JsonSerializer.Deserialize<List<wdata>>(line);

                        return history;
		            }    
                }
            }
            catch (Exception e)
            {
               
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }           
            return null;
	    }
    }
}

