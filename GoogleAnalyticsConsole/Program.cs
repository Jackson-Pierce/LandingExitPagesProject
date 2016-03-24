using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleAnalyticsConsole
{
    class Program
    {
        static void Main(string[] args)
        {

        }
    }
}
//                string directory = (@"C:\IncomingAnalytics\ExitPages\");

//                ExitPgImportUnimportedFiles(directory);

//                ExitPgImportUnimportedFiles(directory);
//            }
//            //May not use ExitPgFilesToBeImported method // directory seems repetitive 
//            public static List<String> ExitPgFilesToBeImported(string directory)
//            {
//                List<String> filepaths = Directory.GetFiles(directory, "*.csv").ToList();
//                using (LINQtoSQLexitpgDataContext db = new LINQtoSQLexitpgDataContext())
//                {
//                    List<vwFilesNotYetLoaded> filesnotloaded = db.vwFilesNotYetLoadeds.ToList();
//                    List<String> fileNames = new List<string>();

//                    foreach (vwFilesNotYetLoaded fileid in filesnotloaded)
//                    {
//                        if (fileid.filename.Contains("ExitPages"))
//                        {
//                            fileNames.Add(fileid.filename);
//                        }
//                    }
//                    List<String> fileDifference = fileNames.Intersect(filepaths).ToList();

//                    return fileDifference;
//                }
//            }
//            public static List<String> ExitPgImportUnimportedFiles(string directory)
//            {
//                List<string> filenames = ExitPgFilesToBeImported(directory).ToList();
//                foreach (string filename in filenames)
//                {
//                    ExitPgImport(filename);
//                }
//                return null;
//            }

//            public static void ExitPgImport(string filename)
//            {
//                using (LINQtoSQLexitpgDataContext db = new LINQtoSQLexitpgDataContext())
//                {
//                    //List<string> someLines = System.IO.File.ReadAllLines(filename).
//                    //    Skip(7).
//                    //    ToList();
//                    //List<string> AllLines = someLines.Take(someLines.Count() - 5).ToList();

//                    List<string> allLines = System.IO.File.ReadAllLines(filename).ToList();

//                    List<string> linesIneed = allLines.Where(line => line.IndexOf("/").Equals(0)).ToList();

//                    foreach (string line in linesIneed)
//                    {
//                        string[] info = line.Split(',');
//                        if (!info[0].Equals(""))
//                        {
//                            StagingExitPage keyword = new StagingExitPage();
//                            keyword.Page = info[0];
//                            keyword.Exits = info[1];
//                            keyword.PageViews = info[2];
//                            keyword.ExitRate = info[3];
//                            keyword.LoadedFile_id =

//                            db.StagingExitPages.InsertOnSubmit(keyword);
//                        }
//                    } // End of foreach loop              
//                    db.SubmitChanges();
//                } // End of Using statement
//            }
//        }
//    }
//}
//    }
//}
