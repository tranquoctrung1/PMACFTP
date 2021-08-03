using FTPPMAC.Action;
using FTPPMAC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPPMAC.Controller
{
    public class MainController
    {

        public void Main()
        {

            FileAction file = new FileAction();
            WriteFileLastTimeUpdateAction writeFileLastTimeUpdateAction = new WriteFileLastTimeUpdateAction();
            Log_Controller log = new Log_Controller();
            WriteFileFTPAction writeFileFTPAction = new WriteFileFTPAction();
            GetDataPmacAction getDataPmacAction = new GetDataPmacAction();
            UploadFileFTPAction uploadFileFTPAction = new UploadFileFTPAction();

            try
            {
                // copy file
                string source = "C:/PMAC/DATA/";
                string fileName = "0005_02.dat";
                string des = "C:/PMAC/FTP/";

                string fileltu = "lasttimeupdate.txt";

                string channelid = "0005_02";

                file.Copy_Files(source, fileName, des);

                // run file with time
                DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour -1 , 0, 0);
                //DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

                string s = writeFileLastTimeUpdateAction.ReadFileSync(fileltu);

                if (s != "" && s != null)
                {
                    string[] sp = s.Split(new char[] { ',' }, StringSplitOptions.None);

                    start = new DateTime(int.Parse(sp[0]), int.Parse(sp[1]), int.Parse(sp[2]), int.Parse(sp[3]), 0, 0);

                }

                DateTime end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
                //DateTime end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 0, 0);
                // get data
                List<DataFTPModel> list = getDataPmacAction.GetData(channelid, start, end);

                if(list.Count > 0)
                {
                    string desFTP = "FTP";
                    string fileNameFTP = "SL_NhaMayNuocThanhPho1_TramNuocTho_LuuLuong_";

                    string year = end.Year.ToString();
                    string month = end.Month < 10 ? $"0{end.Month}" : end.Month.ToString();
                    string day = end.Day < 10 ? $"0{end.Day}" : end.Day.ToString();
                    string hour = end.Hour < 10 ? $"0{end.Hour}" : end.Hour.ToString();
                    string minute = "00";
                    string second = "00";

                    string timeString = $"{year}{month}{day}{hour}{minute}{second}";

                    fileNameFTP += timeString + ".txt";

                    writeFileFTPAction.WriteFileSync(desFTP, fileNameFTP, list);

                    writeFileLastTimeUpdateAction.WriteFileSync(fileltu, end);

                    uploadFileFTPAction.Upload(desFTP, fileNameFTP);

                }
                
            }
            catch (Exception ex)
            {
                log.WriteLog(ex.Message, "Error", true);
            }

        }

    }
}
