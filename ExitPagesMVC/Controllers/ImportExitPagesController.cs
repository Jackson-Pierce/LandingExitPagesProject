using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExitPagesMVC.Models;
using System.IO;

namespace ExitPagesMVC.Controllers
{
    //Notes for Monday:
    // 1. Need to write querys for view of LoadingFiles, so that I can use it to see un-Imported files.
    // 2. Try to write most of the code in 1 or 2 methods in MVC.


    public class ImportExitPagesController : Controller
    {
        public ActionResult Index()
        {
            string filename = (@"C:\IncomingAnalytics\ExitPages\ExitPages03142016pg1.csv");
            string directory = (@"C:\IncomingAnalytics\ExitPages");

            ImportExitPagesController.ExitPgFilesToBeImported(directory);
            ImportExitPagesController.ExitPgImport(filename);
            var importmethod = ExitPgImportUnimportedFiles(directory);

            return View(importmethod);
        }
        public static List<String> ExitPgImportUnimportedFiles(string directory)
        {
            List<string> filenames = ExitPgFilesToBeImported(directory).ToList();
            foreach (string filename in filenames)
            {
                ExitPgImport(filename);
            }
            return null;
        }
        // This Class will sort through a directory and import all files that
        // haven't been imported for the Google Analytics Exit Pages.
        public static void ExitPgImport(string filename)
        {
            using (LINQtoSQLexitpgDataContext db = new LINQtoSQLexitpgDataContext())
            {
                //List<string> someLines = System.IO.File.ReadAllLines(filename).
                //    Skip(7).
                //    ToList();
                //List<string> AllLines = someLines.Take(someLines.Count() - 5).ToList();

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

                        db.StagingExitPages.InsertOnSubmit(keyword);
                    }
                    } // End of foreach loop              
                db.SubmitChanges();
            } // End of Using statement
        }
        public static List<String> ExitPgFilesToBeImported(string directory)
        {
            List<String> filepaths = Directory.GetFiles(directory, "*.csv").ToList();
            using (LINQtoSQLexitpgDataContext db = new LINQtoSQLexitpgDataContext())
            {
                // Changing this list object to correct table
                List<LoadedFile> keywords = db.LoadedFiles.ToList();
                List<String> fileName = new List<string>();

                // NEED TO SET UP "Warehouse data" FROM CONFLUENCE PAGE 
                // This will give me the pages that will still need to be imported for this loop
                // Will need to change "StagingExitPage" EVERYWHERE to the table that has all the files that have already been loaded.

                foreach (LoadedFile keyword in keywords)
                {
                   fileName.Add(keyword.filename);
                }
                List<String> fileDifference = filepaths.Except(fileName).ToList();

                return fileDifference;    
            }
        }

    }
}