using MKForum.Managers;
using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MKForum
{
    /// <summary>
    /// PBoardInMain 的摘要描述
    /// </summary>
    public class PBoardInMain : IHttpHandler
    {
        ParentBoardManager _pbmgr = new ParentBoardManager();
        public void ProcessRequest(HttpContext context)
        {
            //取得

            if (string.Compare("GET", context.Request.HttpMethod, true) == 0 )
            {
                List<Pboard> PBoardList =this._pbmgr.GetListToAPI();

                string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(PBoardList);
                    //SQL改成依照porder做排序?
                
                context.Response.ContentType = "JSON";
                context.Response.Write(jsonText);
                return;
            }

            //更新(上移母版)
            if (string.Compare("POST", context.Request.HttpMethod) == 0 && string.Compare("MoveUp", context.Request.QueryString["Action"], true) == 0)
            {
                //如果他是第一個則不執行

                //這邊應該要做欄位型別檢查?
                int pboardID = int.Parse(context.Request.Form["PboardID"]);
                int porder = int.Parse(context.Request.Form["Porder"]);

                try {
                //塞進資料庫
                if (porder >= 2)
                {
                    Pboard model = new Pboard()
                    {
                        PboardID = pboardID,
                        Porder = porder - 1,      //這組的順序要上移
                    };
                    this._pbmgr.UpdatePBoardOrder(model);


                    Pboard model2 = new Pboard()
                    {
                        Porder = porder - 1,      //當前母版順位的前一筆
                    };
                    int porderID2 =  this._pbmgr.GetnPBoardOrder(model2);//取得前一筆母版的ID

                    Pboard model3 = new Pboard()
                    {
                        PboardID = porderID2,
                        Porder = porder,      //將這筆的順位塞入前一筆
                    };
                    this._pbmgr.UpdatePBoardOrder(model3);
                    }
                    context.Response.ContentType = "text/json";
                    context.Response.Write("OK");
                }
                catch
                {
                    context.Response.ContentType = "text/json";
                    context.Response.Write("error");
                    return;
                }
            }



            //List<Pboard> info = ParentBoardManager.GetPBoardList();

            //string replyText = Newtonsoft.Json.JsonConvert.SerializeObject(info);

            //context.Response.ContentType = "application/json";
            //context.Response.Write(replyText);

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