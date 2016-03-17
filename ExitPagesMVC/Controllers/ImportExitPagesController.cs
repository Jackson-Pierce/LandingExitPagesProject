using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExitPagesMVC.Models;
using System.IO;

namespace ExitPagesMVC.Controllers
{
    public class ImportExitPagesController : Controller
    {
        public static void Setup()
        {
            string filename = (@"C:\Users\j.pierce\JacksonProjects\IncomingAnalytics\ExitPages\ExitPages103162016.csv");
            ImportExitPagesController.ExitPgImportBatching(filename);

            string directory = (@"C:\Users\j.pierce\JacksonProjects\IncomingAnalytics\ExitPages");
            ImportExitPagesController.ExitPgFilesToBeImported(directory);
        }
        public static void ExitPgImportBatching(string filename)
        {
            using (LINQtoSQLexitpgDataContext db = new LINQtoSQLexitpgDataContext())
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
                        StagingExitPage keyword = new StagingExitPage();
                        keyword.Page = info[0];
                        keyword.Exits = info[1];
                        keyword.PageViews = info[2];
                        keyword.ExitRate = info[3];

                        db.StagingExitPages.InsertOnSubmit(keyword);
                    } // End of foreach loop
                } // End of For loop
                db.SubmitChanges();
            } // End of Using statement
        }
        public static List<String> ExitPgFilesToBeImported(string directory)
        {
            List<String> filepaths = Directory.GetFiles(directory, "*.csv").ToList();
            using (LINQtoSQLexitpgDataContext db = new LINQtoSQLexitpgDataContext())
            {
                List<StagingExitPage> keywords = db.StagingExitPages.ToList();
                List<String> fileNames = new List<string>();

                // NEED TO SET UP "Warehouse data" FROM CONFLUENCE PAGE 
                // This will give me the pages that will still need to be imported for this loop
                // Will need to change "StagingExitPage" EVERYWHERE to the table that has all the files that have already been loaded.

                foreach (StagingExitPage keyword in keywords)
                {
                   // fileNames.Add(keyword.filename);
                }
                List<String> fileDifference = filepaths.Except(fileNames).ToList();

                return fileDifference;    
            }
        }
        public static List<String> ExitPgImportUnimportedFiles(string directory)
        {
            List<string> filenames = ExitPgFilesToBeImported(directory).ToList();
            foreach (string filename in filenames)
            {
                ExitPgImportBatching(filename);
            }
            return null;
        }
    }
}