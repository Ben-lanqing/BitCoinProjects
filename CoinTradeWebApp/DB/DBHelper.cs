using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinTradeWebApp.DB
{
    public class DbHelper
    {
        static string connectionString;

        private static DbHelper _SingletonSecond = null;

        static DbHelper()
        {

            _SingletonSecond = new DbHelper();
        }

        public static DbHelper CreateInstance(string conStr = "./DB/CoinTradeDB.sqlite")
        {
            connectionString = "Data Source =" + conStr;
            return _SingletonSecond;
        }
        public List<order> GetDBOrders(decimal createdate = 0)
        {
            List<order> list = new List<order>();
            try
            {
                using (var db = new CoinTradeContext(connectionString))
                {
                    var orders = db.order.Where(a => a.createdate >= createdate).ToList();
                    if (orders != null && orders.Count() > 0)
                    {
                        list = orders;
                    }
                }
            }
            catch (Exception e)
            {
                throw (e);

            }
            return list;
        }
        public List<error> GetError(decimal createdate = 0)
        {
            List<error> list = new List<error>();
            try
            {
                using (var db = new CoinTradeContext(connectionString))
                {
                    var orders = db.error.Where(a => a.id >= createdate).ToList();
                    if (orders != null && orders.Count() > 0)
                    {
                        list = orders;
                    }
                }
            }
            catch (Exception e)
            {
                throw (e);

            }
            return list;
        }
        public List<report> GetReport(decimal createdate = 0)
        {
            List<report> list = new List<report>();
            try
            {
                using (var db = new CoinTradeContext(connectionString))
                {
                    var orders = db.report.Where(a => a.id >= createdate).OrderByDescending(a => a.id).ToList();
                    if (orders != null && orders.Count() > 0)
                    {
                        var first = orders.FirstOrDefault();
                        string m = first.date?.Remove(0, 11)?.Remove(1, 2);
                        var orders10 = orders.Where(a => a.date?.Remove(0, 11)?.Remove(1, 2) == m).ToList();
                        list = orders.Take(10).ToList();
                        list.AddRange(orders10);
                        list = list.Distinct().ToList();
                    }
                }
            }
            catch (Exception e)
            {
                throw (e);

            }
            return list;
        }
        public report GetReportById(decimal id)
        {
            try
            {
                using (var db = new CoinTradeContext(connectionString))
                {
                    if (id == -1)
                    {
                        return db.report.LastOrDefault();
                    }
                    var report = db.report.FirstOrDefault(a => a.id == id);
                    return report;
                }
            }
            catch (Exception e)
            {
                throw (e);

            }

        }
        public List<report> GetReportDaily()
        {
            List<report> list = new List<report>();
            try
            {
                using (var db = new CoinTradeContext(connectionString))
                {
                    var createdate = (DateTime.UtcNow.AddDays(-14).Ticks - new DateTime(1970, 1, 1).Ticks) / 10000;
                    var orders = db.report.Where(a => a.id >= createdate).OrderByDescending(a => a.id).ToList();
                    if (orders != null && orders.Count() > 0)
                    {
                        orders = orders.Where(a => a.date?.Remove(0, 8)?.Remove(4, 2) == "0000").ToList();
                        if (orders != null && orders.Count() > 0)
                        {
                            list = orders;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw (e);

            }
            return list;
        }
        public List<report> GetReportHours()
        {
            List<report> list = new List<report>();
            try
            {
                using (var db = new CoinTradeContext(connectionString))
                {
                    //long NowTicks = DateTime.Parse(DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd")).Ticks;
                    //var createdate = (NowTicks - new DateTime(1970, 1, 1).Ticks) / 10000;

                    var createdate = (DateTime.UtcNow.AddHours(-24).Ticks - new DateTime(1970, 1, 1).Ticks) / 10000;
                    var orders = db.report.Where(a => a.id >= createdate).OrderByDescending(a => a.id).ToList();
                    if (orders != null && orders.Count() > 0)
                    {
                        orders = orders.Where(a => a.date?.Remove(0, 10)?.Remove(2, 2) == "00").ToList();
                        if (orders != null && orders.Count() > 0)
                        {
                            list = orders;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw (e);

            }
            return list;
        }
        public List<report> GetReports(DateTime startdate)
        {
            List<report> list = new List<report>();
            try
            {
                long ticks = (startdate.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000; ;
                using (var db = new CoinTradeContext(connectionString))
                {
                    var orders = db.report.Where(a => a.id >= ticks).OrderByDescending(a => a.id).ToList();
                    if (orders != null && orders.Count() > 0)
                    {
                        list = orders;
                    }
                }
            }
            catch (Exception e)
            {
                throw (e);
            }
            return list;
        }
        public List<order> GetDBOrders(DateTime opendate)
        {
            List<order> list = new List<order>();
            try
            {
                string str = opendate.ToString("yyyyMMddHHmmss");
                using (var db = new CoinTradeContext(connectionString))
                {
                    var orders = db.order.Where(a => str.CompareTo(a.date) <= 0).ToList();
                    if (orders != null && orders.Count() > 0)
                    {
                        list = orders;
                    }
                }
            }
            catch (Exception e)
            {
                throw (e);

            }
            return list;
        }

    }
}
