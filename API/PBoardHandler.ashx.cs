using MKForum.Managers;
using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MKForum.API
{
    /// <summary>
    /// PBoardHandler 的摘要描述
    /// </summary>
    public class PBoardHandler : IHttpHandler
    {
        ParentBoardManager _pbmgr = new ParentBoardManager();

        public void ProcessRequest(HttpContext context)
        {
            //取得

            if (string.Compare("GET", context.Request.HttpMethod, true) == 0)
            {
                List<Pboard> PBoardList = this._pbmgr.GetPBoardList();

                string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(PBoardList);

                context.Response.ContentType = "JSON";
                context.Response.Write(jsonText);
                return;
            }

            //更新(上移母版)
            if (string.Compare("POST", context.Request.HttpMethod) == 0 && string.Compare("MoveUp", context.Request.QueryString["Action"], true) == 0)
            {


                //這邊應該要做欄位型別檢查?
                int pboardID = int.Parse(context.Request.Form["PboardID"]);
                int porder = int.Parse(context.Request.Form["Porder"]);

                try
                {
                    //塞進資料庫
                    if (porder >= 2)                //如果他是第一個則不執行
                    {
                        Pboard modelup = new Pboard()
                        {
                            PboardID = pboardID,
                            Porder = porder - 1,      //這組的順序要上移
                        };


                        Pboard getmodelDownID = new Pboard()
                        {
                            Porder = porder - 1,      //當前母版順位的前一筆
                        };
                        int modelDownID = this._pbmgr.GetnPBoardIdFromOrder(getmodelDownID);//取得前一筆母版的ID

                        Pboard modelDown = new Pboard()
                        {
                            PboardID = modelDownID,
                            Porder = porder,      //將這筆的順位塞入前一筆
                        };
                        this._pbmgr.UpdatePBoardOrder(modelup);
                        this._pbmgr.UpdatePBoardOrder(modelDown);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write("OK");
                    }
                    else
                    {
                        context.Response.ContentType = "text/plain";
                        context.Response.Write("STOP");
                    }
                }
                catch
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("error");
                    return;
                }
            }

            //更新(下移母版)
            if (string.Compare("POST", context.Request.HttpMethod) == 0 && string.Compare("MoveDown", context.Request.QueryString["Action"], true) == 0)
            {
                //如果他是第一個則不執行

                //這邊應該要做欄位型別檢查?
                int pboardID = int.Parse(context.Request.Form["PboardID"]);
                int porder = int.Parse(context.Request.Form["Porder"]);

                try
                {
                    //塞進資料庫
                    if (porder < this._pbmgr.GetPBoardLength())                //如果他是最後一個則不執行
                    {
                        Pboard modeldown = new Pboard()
                        {
                            PboardID = pboardID,
                            Porder = porder + 1,      //這組的順序要下移
                        };


                        Pboard getmodelUpID = new Pboard()
                        {
                            Porder = porder + 1,      //當前母版順位的後一筆
                        };
                        int modelUpID = this._pbmgr.GetnPBoardIdFromOrder(getmodelUpID);//取得後一筆母版的ID

                        Pboard modelUp = new Pboard()
                        {
                            PboardID = modelUpID,
                            Porder = porder,      //將這筆的順位塞入前一筆
                        };
                        this._pbmgr.UpdatePBoardOrder(modeldown);
                        this._pbmgr.UpdatePBoardOrder(modelUp);
                        context.Response.ContentType = "text/plain";
                        context.Response.Write("OK");
                    }
                    else
                    {
                        context.Response.ContentType = "text/plain";
                        context.Response.Write("STOP");
                    }
                }
                catch
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("error");
                    return;
                }
            }

            //更新(命名母版)
            if (string.Compare("POST", context.Request.HttpMethod) == 0 && string.Compare("ReName", context.Request.QueryString["Action"], true) == 0)
            {
                //如果他是第一個則不執行

                //這邊應該要做欄位型別檢查?
                int pboardID = int.Parse(context.Request.Form["PboardID"]);
                string pName = context.Request.Form["Pname"];

                try
                {
                    //塞進資料庫

                        Pboard modeldown = new Pboard()
                        {
                            PboardID = pboardID,
                            Pname = pName ,      //這組的順序要下移
                        };

                        this._pbmgr.ReNamePB(modeldown);
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