using FTPPMAC.Controller;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPPMAC.Action
{
    public class FileAction
    {
        Log_Controller _log = new Log_Controller();
        public void Copy_Files(string sourcePath, string fileName, string destPath)
        {
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }

            try
            {
                var source = Path.Combine(sourcePath, Path.GetFileName(fileName));

                var a = Path.Combine(destPath, Path.GetFileName(fileName));
                File.Copy(source, a, true);
            }
            catch (Exception ex)
            {
                _log.WriteLog(ex.ToString(), "error on copy file " + fileName, true);
            }
            
        }
    }
}
