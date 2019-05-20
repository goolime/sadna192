using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sadna192
{
    public class Logger
    {
        private StreamWriter streamWriter;
        private string file_path;

       public Logger(string fileName)
        {
            Console.WriteLine(fileName);
            string dir_path = @"c:\Sadna192 log files";
             file_path = dir_path + "\\" +fileName+ ".txt";
            Console.WriteLine(file_path);
            try
            {
                if (!Directory.Exists(dir_path))
                    Directory.CreateDirectory(dir_path);

                using (streamWriter = new StreamWriter(file_path, true))
                {
                    if (File.Exists(file_path))
                    {
                        streamWriter.WriteLine("----------------- START : " + System.DateTime.Now.ToString() + " ----------------- ");
                    }

                    streamWriter.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void Add(string str)
        {
            Console.WriteLine("writting...{0}" , str);
            using (this.streamWriter = new StreamWriter(this.file_path, true))
            {
                streamWriter.WriteLine(str);
                streamWriter.Close(); 
            }
        }

        public String ToString()
        {
            return "logger : " + file_path; 
        }
    }
}
