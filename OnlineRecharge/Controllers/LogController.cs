using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace OnlineRecharge.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Log/

        public ActionResult Index()
        {

            DirectoryInfo Source = new DirectoryInfo(Server.MapPath("~/Log"));
            DirectoryInfo Destination = new DirectoryInfo(Server.MapPath("~/Log/Old"));

            FileInfo[] Folder1Files = Source.GetFiles();
            if (Folder1Files.Length > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Log/Old/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Log/Old/"));
                }
                foreach (FileInfo aFile in Folder1Files)
                {
                    if (aFile.Name.Contains(DateTime.Now.ToString("yyyy-MMM-dd")))
                    {
                    }
                    else
                    {
                        if (System.IO.File.Exists(Server.MapPath("~/Log//Old/") + aFile.Name))
                        { 
                            //System.IO.File.Delete(Server.MapPath("~/Log//Old") + aFile.Name);
                            Directory.CreateDirectory(Server.MapPath("~/Log/Old/"));
                            aFile.MoveTo(Server.MapPath("~/Log/Old/") + aFile.Name);
                           
                        }
                        else
                        {
                            if (!Directory.Exists(Server.MapPath("~/Log/Old/")))
                            {
                                Directory.CreateDirectory(Server.MapPath("~/Log/Old/"));
                            }

                            aFile.MoveTo(Server.MapPath("~/Log/Old/") + aFile.Name);
                        }
                    }                                      
                }
            }

            var files = Directory.EnumerateFiles(Server.MapPath("~/Log"));
            return View(files);
        }

    }

    public class LogsController : Controller
    {
        //
        // GET: /Log/

       public ActionResult Index()
        {

            DirectoryInfo Source = new DirectoryInfo(Server.MapPath("~/Log"));
            DirectoryInfo Destination = new DirectoryInfo(Server.MapPath("~/Log/Old"));

            FileInfo[] Folder1Files = Source.GetFiles();
            if (Folder1Files.Length > 0)
            {
                if (!Directory.Exists(Server.MapPath("~/Log/Old/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Log/Old/"));
                }
                foreach (FileInfo aFile in Folder1Files)
                {
                    if (aFile.Name.Contains(DateTime.Now.ToString("yyyy-MMM-dd")))
                    {
                    }
                    else
                    {
                        if (System.IO.File.Exists(Server.MapPath("~/Log//Old/") + aFile.Name))
                        { 
                            //System.IO.File.Delete(Server.MapPath("~/Log//Old") + aFile.Name);
                            Directory.CreateDirectory(Server.MapPath("~/Log/Old/"));
                            aFile.MoveTo(Server.MapPath("~/Log/Old/") + aFile.Name);
                           
                        }
                        else
                        {
                            if (!Directory.Exists(Server.MapPath("~/Log/Old/")))
                            {
                                Directory.CreateDirectory(Server.MapPath("~/Log/Old/"));
                            }

                            aFile.MoveTo(Server.MapPath("~/Log/Old/") + aFile.Name);
                        }
                    }                                      
                }
            }

            var files = Directory.EnumerateFiles(Server.MapPath("~/Log"));
            return View(files);
        }

    

    }   
}
