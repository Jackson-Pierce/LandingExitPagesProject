using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LandingPagesMVC.Models;
using System.IO;

namespace LandingPagesMVC.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }
        public FileContentResult CampaignReport()
        {
            List<string> reports = new List<string>();
            using (LINQtoSQLlandingpgDataContext db = new LINQtoSQLlandingpgDataContext())
            {
                List<vwCampaignWeekReportLandingPage> columns = db.vwCampaignWeekReportLandingPages.ToList();

                reports.Add
                ("SumOfSessions" +
                ",NewSessions" +
                ",SumOfNewUsers" +
                ",SumOfTransactions" +
                ",Bounces" +
                ",PagesViewed" +
                ",Campaign" +
                ",Loadedfile_id" +
                ",Campaignid"
                );

                foreach (vwCampaignWeekReportLandingPage column in columns)
                {
                    var arr1 = new List<String>
                      {
                        column.SumOfSessions.ToString(),
                        column.NewSessions.ToString(),
                        column.SumOfNewUsers.ToString(),
                        column.SumOfTransactions.ToString(),
                        column.Bounces.ToString(),
                        column.PagesViewed.ToString(),
                        column.campaign.ToString(),
                        column.Loadedfile_id.ToString(),
                        column.campaignid
                      };
                    reports.Add(String.Join(",", arr1));
                }

                byte[] reportbytes = reports.SelectMany(s => System.Text.Encoding.UTF8.GetBytes(s)).ToArray();
                return File(reportbytes, "text/csv", "CampaignReports.csv");
            } //End of using statement
        }
        public FileContentResult NetsuiteReport()
        {
            List<string> reports = new List<string>();
            using (LINQtoSQLlandingpgDataContext db = new LINQtoSQLlandingpgDataContext())
            {
                List<vwNetsuiteWeekReportLandingPage> columns = db.vwNetsuiteWeekReportLandingPages.ToList();

                reports.Add
                ("SumOfSessions" +
                ",NewSessions" +
                ",SumOfNewUsers" +
                ",SumOfTransactions" +
                ",Bounces" +
                ",PagesViewed" +
                ",Manufacturer" +
                ",CleanSKU" +
                ",Category" +
                ",DisplayName" +
                ",Loadedfile_id" +
                ",netsuiteid"
                );

                foreach (vwNetsuiteWeekReportLandingPage column in columns)
                {
                    var arr1 = new List<String>
                      {
                        column.SumOfSessions.ToString(),
                        column.NewSessions.ToString(),
                        column.SumOfNewUsers.ToString(),
                        column.SumOfTransactions.ToString(),
                        column.Bounces.ToString(),
                        column.PagesViewed.ToString(),
                        column.Manufacturer.ToString(),
                        column.CleanSKU.ToString(),
                        column.Category.ToString(),
                        column.DisplayName.ToString(),
                        column.Loadedfile_id.ToString(),
                        column.netsuiteid
                      };
                    reports.Add(String.Join(",", arr1));
                }

                byte[] reportbytes = reports.SelectMany(s => System.Text.Encoding.UTF8.GetBytes(s)).ToArray();
                return File(reportbytes, "text/csv", "NetsuiteReports.csv");
            } //End of using statement
        }
    }
}