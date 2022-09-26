using FTPPMAC.ConnectDB;
using FTPPMAC.Controller;
using FTPPMAC.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPPMAC.Action
{
    public class GetIndexLoggerAction
    {
        public DataLoggerModel GetIndexLogger(string channelid ,DateTime start)
        {
            DataLoggerModel result = new DataLoggerModel();
            Connect connect = new Connect();
            Log_Controller log = new Log_Controller();


            try
            {
                string timeStart = $"{start.Month}/{start.Day}/{start.Year} {start.Hour}:{start.Minute}:{start.Second}";

                DateTime end = start.AddHours(1);

                string timeEnd = $"{end.Month}/{end.Day}/{end.Year} {end.Hour}:{end.Minute}:{end.Second}";

                string sqlQuery = $"select top(1) * from t_Index_Logger_{channelid} where TimeStamp between convert(nvarchar, '{timeStart}', 120) and convert(nvarchar, '{timeEnd}', 120) order by timestamp ";

                connect.Connected();

                SqlDataReader reader = connect.Select(sqlQuery);

                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        try
                        {
                            result.TimeStamp = DateTime.Parse(reader["TimeStamp"].ToString());
                        }
                        catch(Exception ex)
                        {
                            result.TimeStamp = null;
                        }
                        try
                        {
                            result.Value = double.Parse(reader["Value"].ToString());
                        }
                        catch(Exception ex)
                        {
                            result.Value = null;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                log.WriteLog(ex.Message, "Error", true);
            }
            finally
            {
                connect.DisConnected();
            }

            return result;
        }
    }
}
