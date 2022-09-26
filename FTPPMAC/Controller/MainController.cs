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
            GetIndexLoggerAction getIndexLoggerAction = new GetIndexLoggerAction();

            try
            {
                //// copy file
                //string source = "C:/PMAC/DATA/";
                //string fileName = "1001_02.dat";
                //string des = "C:/PMAC/FTP/";

                //file.Copy_Files(source, fileName, des);

                string channelid = "1001_02";

                // run file with time
                DateTime start = DateTime.Now;
                start = start.AddDays(-1);
                start = new DateTime(start.Year, start.Month, start.Day, 0, 0, 0);

                DateTime end = DateTime.Now;
                end = new DateTime(end.Year, end.Month, end.Day, 0, 0, 0);

                DataLoggerModel indexStart = getIndexLoggerAction.GetIndexLogger(channelid, start);
                DataLoggerModel indexEnd = getIndexLoggerAction.GetIndexLogger(channelid, end);

                DataLoggerModel dataIndex = new DataLoggerModel();

                dataIndex.TimeStamp = start;
                dataIndex.Value = (indexEnd.Value ?? 0) - (indexStart.Value ?? 0);
               
                if(dataIndex != null)
                {
                    string desFTP = "FTP";
                    string fileNameFTP = "DN_NHAMAYNUOCBOOTHUDUC_TRAMBOM_LUULUONG_";

                    string year = start.Year.ToString();
                    string month = start.Month < 10 ? $"0{start.Month}" : start.Month.ToString();
                    string day = start.Day < 10 ? $"0{start.Day}" : start.Day.ToString();
                    string hour = start.Hour < 10 ? $"0{start.Hour}" : start.Hour.ToString();
                    string minute = "00";
                    string second = "00";

                    string timeString = $"{year}{month}{day}{hour}{minute}{second}";

                    fileNameFTP += timeString + ".txt";

                    DataFTPModel data = new DataFTPModel();
                    data.Time = timeString;
                    data.Value = dataIndex.Value.ToString();
                    data.Unit = "m3";
                    data.Status = "00";

                    writeFileFTPAction.WriteFileSyncByIndex(desFTP, fileNameFTP, data);

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
