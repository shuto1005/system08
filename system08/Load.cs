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
		public List<wdata> Load()
		{
            try
            {

            // Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            using (StreamReader sr = new StreamReader(@".\data.txt"))
                {
                    string line;
                    // Read and display lines from the file until the end of
                    // the file is reached.
                    if((line = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                        List<wdata> history = JsonSerializer.Deserialize<List<wdata>>(line);

                        return history;
		            }    
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }           
            return null;
	    }
    }
}

