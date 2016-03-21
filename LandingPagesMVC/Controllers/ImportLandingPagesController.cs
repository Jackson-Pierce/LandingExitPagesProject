using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using LandingPagesMVC.Models;

namespace LandingPagesMVC.Controllers
{
    public class ImportLandingPagesController : Controller
    {
        // This Class will sort through a directory and import all files that
        // haven't been imported for the Google Analytics Landing Pages.
        public static void LandingPgSetup()
        {
            string filename = (@"C:\IncomingAnalytics\LandingPages\LandingPages03142016pg1.csv");
            ImportLandingPagesController.LandingPgImportBatching(filename);
            string directory = (@"C:\IncomingAnalytics\LandingPages");
            ImportLandingPagesController.LandingPgFilesToBeImported(directory);
        }
        public static void LandingPgImportBatching(string filename)
        {
            using (LINQtoSQLlandingpgDataContext db = new LINQtoSQLlandingpgDataContext())
            {
                List<string> AllLines = System.IO.File.ReadAllLines(filename).
                    Skip(7).
                    ToList();

                int batchsize = 5000;
                for (int i = 0; i <= AllLines.Count / batchsize; i++)
                {
                    var batchitems = AllLines.Skip(i * batchsize).Take(batchsize).ToList();
                    foreach (string line in batchitems)
                    {
                        string[] info = line.Split(',');
                        LandingPagestaging keyword = new LandingPagestaging();
                        keyword.LandingPage = info[0];
                        keyword.Sessions = info[1];
                        keyword.SessionRate = info[2];
                        keyword.NewUsers = info[3];
                        keyword.BounceRate = info[4];
                        keyword.PagesSession = info[5];
                        keyword.AvgSessionDuration = info[6];
                        keyword.Transactions = info[7];
                        keyword.Revenue = info[8];
                        keyword.EcommerceConversionRate = info[9];

                        db.LandingPagestagings.InsertOnSubmit(keyword);
                    } // End of foreach loop
                } // End of For loop
                db.SubmitChanges();
            } // End of Using statement
        }
        public static List<String> LandingPgFilesToBeImported(string directory)
        {
            {
                List<String> filepaths = Directory.GetFiles(directory, "*.csv").ToList();
                using (LINQtoSQLlandingpgDataContext db = new LINQtoSQLlandingpgDataContext())
                {
                    List<LoadedFile> keywords = db.LoadedFiles.ToList();
                    List<String> fileName = new List<string>();

                    foreach (LoadedFile keyword in keywords)
                    {
                        fileName.Add(keyword.filename);
                    }
                    List<String> fileDifference = filepaths.Except(fileName).ToList();

                    return fileDifference;
                } //End of using statement
            }
        }
        public static List<String> LandingPgImportUnimportedFiles(string directory)
        {
            List<string> filenames = LandingPgFilesToBeImported(directory).ToList();
            foreach (string filename in filenames)
            {
                LandingPgImportBatching(filename);
            }
            return null;
        }
    }
}