using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace MKForum
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
        }
        private object logLock = new object();
        protected void Application_Error(object sender, EventArgs e)
        {
            //Exception objErr = Server.GetLastError().GetBaseException();
            //string err = "Error Caught in Application_Error event/n" +
            //              "Error in: " + Request.Url.ToString() +
            //              "/nError Message:" + objErr.Message.ToString() +
            //               "/nStack Trace:" + objErr.StackTrace.ToString();
            //EventLog.WriteEntry("MKForum", err, EventLogEntryType.Error);
            //Server.ClearError();
            //additional actions...
            var error = HttpContext.Current.Error;
            lock (logLock)
            {
                System.IO.File.AppendAllText("\\logs\\log.log", error.ToString());
            }
        }
    }
}