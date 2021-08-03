using FTPPMAC.Controller;
using FTPPMAC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPPMAC.Action
{
    public class GetDataPmacAction
    {
        public List<DataFTPModel> GetData(string channelid,DateTime start, DateTime end)
        {
            PMAC_Controller pmac = new PMAC_Controller();
            Log_Controller log = new Log_Controller();

            List<DataFTPModel> list = new List<DataFTPModel>();

            try
            {
                int interval = pmac.Rate(channelid);

                DateTime temp = start;

                while(temp <= end)
                {
                    double? value = pmac.GetValue(channelid, temp);

                    if(value != null)
                    {
                        DataFTPModel el = new DataFTPModel();

                        string year = temp.Year.ToString();
                        string month = temp.Month < 10 ? $"0{temp.Month}" : temp.Month.ToString();
                        string day = temp.Day < 10 ? $"0{temp.Day}" : temp.Day.ToString();
                        string hour = temp.Hour < 10 ? $"0{temp.Hour}" : temp.Hour.ToString();
                        string minute = temp.Minute < 10 ? $"0{temp.Minute}" : temp.Minute.ToString();
                        string second = temp.Second < 10 ? $"0{temp.Second}" : temp.Second.ToString();

                        el.Time = $"{year}{month}{day}{hour}{minute}{second}";
                        el.Value = value.ToString();
                        el.Status = "00";
                        el.Unit = "m3/h";

                        list.Add(el);
                    }

                   temp = temp.AddSeconds(interval);
                }

            }
            catch(Exception ex)
            {
                log.WriteLog(ex.Message, "Error", true);
            }

            return list;
        }
    }
}
