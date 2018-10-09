using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoinTradeWebApp.Models;
using CoinTradeWebApp.DB;
using Microsoft.Extensions.Options;

namespace CoinTradeWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ConnectionStrings connectionStrings;
        string conStr;
        string User;

        public HomeController(IOptions<ConnectionStrings> option)
        {
            connectionStrings = option.Value;
            conStr = connectionStrings.Ben;
            User = "Ben";
        }


        public IActionResult Index(string user = "Ben")
        {
            if (user != "Ben")
            {
                conStr = connectionStrings.ZZ;
                User = user;
            }
            ViewBag.user = User;
            var t = (DateTime.UtcNow.AddHours(-1).Ticks - new DateTime(1970, 1, 1).Ticks) / 10000;
            var Reports = DbHelper.CreateInstance(conStr).GetReport(t);
            //var Reports = DbHelper.CreateInstance(conStr).GetReportDaily();
            return View(Reports);
        }
        public IActionResult ReportDaily(string user = "Ben")
        {
            if (user != "Ben")
            {
                conStr = connectionStrings.ZZ;
                User = user;
            }
            ViewBag.user = User;
            var Reports = DbHelper.CreateInstance(conStr).GetReportDaily();
            return View(Reports);
        }
        public IActionResult ReportHours(string user = "Ben")
        {
            if (user != "Ben")
            {
                conStr = connectionStrings.ZZ;
                User = user;
            }
            ViewBag.user = User;
            #region OrdersY Orders24 OrdersT Reports24 fees founds
            DateTime yesterday = DateTime.Parse(DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd"));
            //var yd = (yesterday.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000;
            var OrdersY = DbHelper.CreateInstance(conStr).GetDBOrders(yesterday);

            //var date24 = (DateTime.UtcNow.AddHours(-24).Ticks - new DateTime(1970, 1, 1).Ticks) / 10000;
            //var Orders24 = OrdersY.Where(a => a.createdate >= date24).ToList();

            DateTime today = yesterday.AddDays(1);
            string str = today.ToString("yyyyMMddHHmmss");
            //var dateToday = (today.Ticks - new DateTime(1970, 1, 1).Ticks) / 10000;
            var OrdersT = OrdersY.Where(a => str.CompareTo(a.date) <= 0).ToList();

            var bY = OrdersY.Where(a => a.side == "buy");
            var sY = OrdersY.Where(a => a.side == "sell");

            var feesYb = bY.Sum(a => a.fees);
            var feesYs = sY.Sum(a => a.fees);
            var fYb = bY.Sum(a => a.amount * a.price);
            var fYs = sY.Sum(a => a.amount * a.price);

            var bT = OrdersT.Where(a => a.side == "buy");
            var sT = OrdersT.Where(a => a.side == "sell");

            var feesTb = bT.Sum(a => a.fees);
            var feesTs = sT.Sum(a => a.fees);
            var fTb = bT.Sum(a => a.amount * a.price);
            var fTs = sT.Sum(a => a.amount * a.price);

            var Reports24 = DbHelper.CreateInstance(conStr).GetReportHours();


            #endregion

            var first = Reports24.FirstOrDefault();
            var last = Reports24.LastOrDefault();
            ViewBag.earn = first.NetFund - last.NetFund;
            ViewBag.tearn = first.Earn - last.Earn;

            ViewBag.earnTY = fYs - fYb;
            ViewBag.earnTT = fTs - fTb;
            ViewBag.feesY = feesYb * first.TickerPrice + feesYs;
            ViewBag.feesT = feesTb * first.TickerPrice + feesTs;

            return View(Reports24);
        }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Details(string user = "Ben", decimal reportId = -1)
        {
            if (user != "Ben")
            {
                conStr = connectionStrings.ZZ;
                User = user;
            }
            var item = DbHelper.CreateInstance(conStr).GetReportById(reportId);
            item.LogOrders = RemoveIDStr(item.LogOrders);
            item.LogOrders = item.LogOrders.Replace("\r\n", "<br/>");
            item.DBOrdersInfo = item.DBOrdersInfo.Replace("\r\n", "<br/>");
            item.LogOrders = item.LogOrders.Replace(" ", "&nbsp;");
            item.DBOrdersInfo = item.DBOrdersInfo.Replace(" ", "&nbsp;");
            return View(item);
        }

        private string RemoveIDStr(string str)
        {
            int index = str.IndexOf("A:");
            while (index > 0)
            {
                str = str.Remove(index, 17);
                index = str.IndexOf("A:");
            }
            return str;
        }
    }
}
