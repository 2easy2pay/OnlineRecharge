using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineRecharge.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {

        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is UnauthorizedAccessException)
            {
                filterContext.ExceptionHandled = true;
                filterContext.Result = RedirectToAction("Home", "Index");
            }
            WriteLog(filterContext.Exception.Message, filterContext.Exception.StackTrace);
            //
            base.OnException(filterContext);
        }
        public void WriteLog(string Message, string StackTrace)
        {
            string sPath = Server.MapPath("~/Log");
            string fileName = "ErrorLog_" + DateTime.Now.ToString("yyyy-MMM-dd") + ".txt";
            string message = "\r\n-------------------------------------------------------" + DateTime.Now + "---------------------------------------------------------------\r\n"
                            + Message
                            + "\r\n------------- Stack Trace --------------------------------------------------\r\n"
                            + StackTrace;
            if (!Directory.Exists(sPath))
                Directory.CreateDirectory(sPath);
            System.IO.File.AppendAllText(sPath + "/" + fileName, message);
        }

    }
}