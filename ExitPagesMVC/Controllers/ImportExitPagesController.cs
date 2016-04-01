using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExitPagesMVC.Models;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace ExitPagesMVC.Controllers
{
    /// <summary>
    /// -This MVC Controller Imports Google Analytics Exit Pages data, that has yet to be imported into the Database.
    /// -It checks what has already been imported by referencing the view "vw_FilesNotYetLoaded.
    /// -You manually insert the file path and file name into the table JPStarter.dbo.LoadedFiles to generate a Primary Key id #.
    /// -The method will reference the file path and ID # to be inserted into the table by using a Tuple.
    /// -TextFieldParser inserts all strings one at a time, while checking for quotes on each comma.
    /// </summary>
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
                } // End of loop
                foreach (Tuple<string, int> fileinfo in filenameid)
                {
                    ExitPgImport(fileinfo.Item1, fileinfo.Item2);
                } // End of loop
            } // End of using statement
        }
        public static void ExitPgImport(string filename, int loadedFile_id)
        {
            using (LINQtoSQLexitpgDataContext db = new LINQtoSQLexitpgDataContext())
            {
                TextFieldParser parser = new TextFieldParser(filename);
                parser.SetDelimiters(",");
                parser.HasFieldsEnclosedInQuotes = true;

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();

                    if (fields[0].IndexOf("/").Equals(0))
                    {
                        StagingExitPage keyword = new StagingExitPage();
                        keyword.Page = fields[0];
                        keyword.Exits = fields[1];
                        keyword.PageViews = fields[2];
                        keyword.ExitRate = fields[3];
                        keyword.LoadedFile_id = loadedFile_id;

                        db.StagingExitPages.InsertOnSubmit(keyword);
                    }
                } // End of While loop
                db.SubmitChanges();
                parser.Close();
            } // End of Using statement      
         }
    }        
}
