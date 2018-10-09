using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoinTradeWebApp.DB
{
    public class CoinTradeContext : DbContext
    {
        string connectionString = "Data Source=./DB/CoinTradeDB.sqlite";
        public DbSet<report> report { get; set; }
        public DbSet<error> error { get; set; }
        public DbSet<order> order { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(connectionString);
        }
        public CoinTradeContext(string conStr)
        {
            connectionString = conStr;
        }

    }
    public class report
    {
        public decimal id { get; set; }
        public string type { get; set; }
        public string date { get; set; }
        public string runningTime { get; set; }
        public Nullable<decimal> OpenPrice { get; set; }
        public Nullable<decimal> TickerPrice { get; set; }
        public Nullable<decimal> OpenFund { get; set; }
        public Nullable<decimal> NetFund { get; set; }
        public Nullable<decimal> TradeQty { get; set; }
        public Nullable<decimal> SpanPrice { get; set; }
        public Nullable<decimal> OrderQty { get; set; }
        public Nullable<decimal> Earn { get; set; }
        public Nullable<decimal> RateYear { get; set; }
        public Nullable<System.DateTime> Open_Time { get; set; }
        public Nullable<decimal> HResetCount { get; set; }
        public Nullable<decimal> DealCount { get; set; }
        public Nullable<decimal> ResetCount { get; set; }
        public Nullable<decimal> RealEarn { get; set; }
        public Nullable<decimal> HDealCount { get; set; }
        public string LogOrders { get; set; }
        public string DBOrdersInfo { get; set; }
    }
    public class error
    {
        public decimal id { get; set; }
        public string errtitle { get; set; }
        public string date { get; set; }
        public string errmessage { get; set; }
        public string errtext { get; set; }
    }
    public class order
    {
        public string orderid { get; set; }
        public Nullable<decimal> price { get; set; }
        public Nullable<decimal> amount { get; set; }
        public Nullable<decimal> createdate { get; set; }
        public string status { get; set; }
        public string symbol { get; set; }
        public string side { get; set; }
        public Nullable<decimal> fees { get; set; }
        public string type { get; set; }
        public string platform { get; set; }
        public string date { get; set; }
    }

}
