using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MKForum.Helpers
{
    public class Logger
    {
        //private static string _savePath = Path.GetFullPath(@"\Logs\Log.log");
        private const string _savePath = "\\Logs\\Log.log";
        private const string _savePathx = "\\Logs";


        //記錄錯誤
        public static void WriteLog(string moduleName, Exception ex)
        {
            string content =
$@"------------------
{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}
                {moduleName}
                {ex.ToString()}
 ----------------------
";
            if (!Directory.Exists(_savePath)) // 假如資料夾不存在，先建立
                Directory.CreateDirectory(_savePathx);
            File.AppendAllText(Logger._savePath, content);
        }

    }
}