using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPPMAC.Action
{
    public class WriteFileLastTimeUpdateAction
    {
        public  async Task WriteFileAsync(string fileName, DateTime time)
        {
            if(File.Exists(fileName))
            {
                File.Create(fileName).Dispose();
            }

            using (StreamWriter file = new StreamWriter(fileName, append: false))
            {
                string t = $"{time.Year},{time.Month},{time.Day},{time.Hour}";

                await file.WriteLineAsync(t);

                file.Close();
            }
        }

        public async Task<string> ReadFileAsync(string fileName)
        {
            string s = "";

            if(File.Exists(fileName))
            {
                using (StreamReader file = new StreamReader(fileName))
                {
                    s =  await file.ReadLineAsync();

                    file.Close();
                }
            }
            else
            {
                s = "";
            }

            return s;
                
        }

        public string ReadFileSync(string fileName)
        {
            string s = "";

            if (File.Exists(fileName))
            {
                using (StreamReader file = new StreamReader(fileName))
                {
                    s = file.ReadLine();

                    file.Close();
                }
            }
            else
            {
                s = "";
            }

            return s;
        }

        public void WriteFileSync(string fileName, DateTime time)
        {
            if (File.Exists(fileName))
            {
                File.Create(fileName).Dispose();
            }

            using (StreamWriter file = new StreamWriter(fileName, append: false))
            {
                string t = $"{time.Year},{time.Month},{time.Day},{time.Hour}";

                file.WriteLine(t);

                file.Close();
            }
        }
    }
}
