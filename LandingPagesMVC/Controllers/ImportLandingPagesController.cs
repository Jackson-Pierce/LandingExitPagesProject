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
        public ActionResult Index()
        {
            LandingPgFilesToBeImported();

            return View();
        }
        public static void LandingPgFilesToBeImported()
        {
            List<Tuple<string, int>> filenameid = new List<Tuple<string, int>>();
            using (LINQtoSQLlandingpgDataContext db = new LINQtoSQLlandingpgDataContext())
            {
                List<vw_FilesNotYetLoaded> unloadedfiles = db.vw_FilesNotYetLoadeds.ToList();

                foreach (vw_FilesNotYetLoaded fileid in unloadedfiles)
                {
                    if (fileid.filetype.Equals("Landing Page"))
                    {
                        Tuple<string, int> notloaded = new Tuple<string, int>(fileid.filename, fileid.ID);
                        filenameid.Add(notloaded);
                    }
                } // end of loop
                foreach (Tuple<string, int> fileinfo in filenameid)
                {
                    LandingPgImport(fileinfo.Item1, fileinfo.Item2);
                }
            }
        }

        public static void LandingPgImport(string filename, int loadedFile_id)
        {
            using (LINQtoSQLlandingpgDataContext db = new LINQtoSQLlandingpgDataContext())
            {
                List<string> allLines = System.IO.File.ReadAllLines(filename).ToList();
                List<string> linesIneed = allLines.Where(line => line.IndexOf("/").Equals(0)).ToList();

                foreach (string line in linesIneed)
                {
                    string[] info = line.Split(',');
                    if (!info[0].Equals(""))
                    {
                        stagingLandingPage keyword = new stagingLandingPage();
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
                        keyword.LoadedFile_id = loadedFile_id;

                        db.stagingLandingPages.InsertOnSubmit(keyword);
                    }
                } // End of foreach loop
                db.SubmitChanges();
            }
        }
    }
}