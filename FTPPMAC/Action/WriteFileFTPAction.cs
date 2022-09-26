using FTPPMAC.Controller;
using FTPPMAC.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPPMAC.Action
{
    public class WriteFileFTPAction
    {
        Log_Controller log = new Log_Controller();
        public bool CheckFile(string folder, string fileName)
        {
            try
            {
                string path = Path.Combine(folder, fileName);

                if (File.Exists(path))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                log.WriteLog(ex.Message, "Error", true);

                return false;
            }
        }

        public bool CheckDirectory(string folder )
        {
            try
            {
                if(Directory.Exists(folder))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                log.WriteLog(ex.Message, "Error", true);

                return false;
            }
        }

        public void CreateDirectory(string folder)
        {
            Directory.CreateDirectory(folder);
        }

        public void CreateFile(string folder, string fileName)
        {
            string path = Path.Combine(folder, fileName);
            File.Create(path).Dispose();
        }

        public async Task WriteFileAsync(string folder , string fileName, List<DataFTPModel> list)
        {
            if(!CheckDirectory(folder))
            {
                CreateDirectory(folder);
            }

            if(!CheckFile(folder, fileName))
            {
                CreateFile(folder, fileName);
            }

            string path = Path.Combine(folder, fileName);

            using (StreamWriter file = new StreamWriter(path, append: false))
            {
                foreach(var item in list)
                {
                    string st = $"{item.Time}\t{item.Value}\t{item.Unit}\t{item.Status}{System.Environment.NewLine}";

                    await file.WriteAsync(st);
                }

                file.Close();
            }
        }

        public void WriteFileSync(string folder, string fileName, List<DataFTPModel> list)
        {
            if (!CheckDirectory(folder))
            {
                CreateDirectory(folder);
            }

            if (!CheckFile(folder, fileName))
            {
                CreateFile(folder, fileName);
            }

            string path = Path.Combine(folder, fileName);

            using (StreamWriter file = new StreamWriter(path, append: false))
            {
                foreach (var item in list)
                {
                    string st = $"{item.Time}\t{item.Value}\t{item.Unit}\t{item.Status}{System.Environment.NewLine}";

                    file.Write(st);
                }

                file.Close();
            }
        }

        public void WriteFileSyncByIndex(string folder, string fileName, DataFTPModel item)
        {
            if (!CheckDirectory(folder))
            {
                CreateDirectory(folder);
            }

            if (!CheckFile(folder, fileName))
            {
                CreateFile(folder, fileName);
            }

            string path = Path.Combine(folder, fileName);

            using (StreamWriter file = new StreamWriter(path, append: false))
            {
                
                string st = $"{item.Time}\t{item.Value}\t{item.Unit}\t{item.Status}{System.Environment.NewLine}";

                file.Write(st);
                

                file.Close();
            }
        }
    }
}
