using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExitPagesMVC.Models;
using System.IO;

namespace ExitPagesMVC.Controllers
{
    //Notes for Thursday:
    // 1. db.vwFilesNotYetLoadeds for some reason is adding everyline
    // 2. make sure the import is getting the right Tuple set (item1 for filename and item2 for FK id)


    public class ImportExitPagesController : Controller
    {
        public ActionResult Index()
        {
            ExitPgFilesToBeImported();

            return View();
        }
        public static void ExitPgFilesToBeImported()
        {
            List<Tuple<string, int>> filenameid = new List<Tuple<string, int>>();
            using (LINQtoSQLexitpgDataContext db = new LINQtoSQLexitpgDataContext())
            {
                List<vw_FilesNotYetLoaded> unloadedfiles = db.vw_FilesNotYetLoadeds.ToList();

                foreach (vw_FilesNotYetLoaded fileid in unloadedfiles)
                {
                    if (fileid.filetype.Equals("Exit Page"))
                    {
                        Tuple<string, int> notloaded = new Tuple<string, int>(fileid.filename , fileid.ID);
                        filenameid.Add(notloaded);
                    }
                } // end of loop
                foreach (Tuple<string, int> fileinfo in filenameid)
                {
                    ExitPgImport(fileinfo.Item1, fileinfo.Item2);
                }
            }
        }    

        public static void ExitPgImport(string filename, int loadedFile_id)
        {
            using (LINQtoSQLexitpgDataContext db = new LINQtoSQLexitpgDataContext())
            {                              
                List<string> allLines = System.IO.File.ReadAllLines(filename).ToList();
                List<string> linesIneed = allLines.Where(line => line.IndexOf("/").Equals(0)).ToList();

                    foreach (string line in linesIneed)
                    {
                        string[] info = line.Split(',');
                        if (!info[0].Equals(""))
                        {
                            StagingExitPage keyword = new StagingExitPage();
                            keyword.Page = info[0];
                            keyword.Exits = info[1];
                            keyword.PageViews = info[2];
                            keyword.ExitRate = info[3];
                            keyword.LoadedFile_id = loadedFile_id;

                            db.StagingExitPages.InsertOnSubmit(keyword);
                        }
                    } // End of foreach loop
                db.SubmitChanges();
            }              
         }
    }        
}
