using MKForum.Managers;
using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MKForum.API
{
    /// <summary>
    /// CBoardHandler 的摘要描述
    /// </summary>
    public class CBoardHandler : IHttpHandler
    {
        CboardManager _cbmgr = new CboardManager();

        public void ProcessRequest(HttpContext context)
        {
            //取得

            if (string.Compare("POST", context.Request.HttpMethod, true) == 0 && string.Compare("GetList", context.Request.QueryString["Action"], true) == 0)
            {
                string pboardID = context.Request.Form["PBid"];


                List<Cboard> cBoardList = this._cbmgr.GetCbFromPB(pboardID);

                string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(cBoardList);

                context.Response.ContentType = "JSON";
                context.Response.Write(jsonText);
                return;
            }

            //更新(命名子版)
            if (string.Compare("POST", context.Request.HttpMethod) == 0 && string.Compare("ReName", context.Request.QueryString["Action"], true) == 0)
            {

                //這邊應該要做欄位型別檢查?
                int cboardID = int.Parse(context.Request.Form["CboardID"]);
                string cName = context.Request.Form["Cname"];
                string cboardCotent = context.Request.Form["CboardCotent"];

                try
                {
                    //塞進資料庫

                    Cboard modelRename = new Cboard()
                    {
                        CboardID = cboardID,
                        Cname = cName,
                        CboardCotent = cboardCotent,
                    };

                    this._cbmgr.ReNameCB(modelRename);
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("OK");

                }
                catch
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("error");
                    return;
                }
            }



        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}