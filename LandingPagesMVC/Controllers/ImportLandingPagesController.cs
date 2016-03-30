using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using LandingPagesMVC.Models;
using Microsoft.VisualBasic.FileIO;

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
                // List<string> allLines = System.IO.File.ReadAllLines(filename).ToList();
                // List<string> linesIneed = allLines.Where(line => line.IndexOf("/").Equals(0)).ToList();
                TextFieldParser parser = new TextFieldParser(filename);
                parser.SetDelimiters(",");
                parser.HasFieldsEnclosedInQuotes = true;
                
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();

                    if (fields[0].IndexOf("/").Equals(0))
                    {
                        stagingLandingPage keyword = new stagingLandingPage();
                        keyword.LandingPage = fields[0];
                        keyword.Sessions = fields[1];
                        keyword.SessionRate = fields[2];
                        keyword.NewUsers = fields[3];
                        keyword.BounceRate = fields[4];
                        keyword.PagesSession = fields[5];
                        keyword.AvgSessionDuration = fields[6];
                        keyword.Transactions = fields[7];
                        keyword.Revenue = fields[8];
                        keyword.EcommerceConversionRate = fields[9];
                        keyword.LoadedFile_id = loadedFile_id;

                        db.stagingLandingPages.InsertOnSubmit(keyword);                       
                    }                
                } // End of While loop
                db.SubmitChanges();
                parser.Close();
            }
        } // end of LandingPgImport
    }
}
