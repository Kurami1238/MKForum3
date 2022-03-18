using MKForum.Managers;
using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MKForum.API
{
    /// <summary>
    /// MugenHandler 的摘要描述
    /// </summary>
    public class MugenHandler : IHttpHandler
    {
        private PostManager _pmgr = new PostManager();
        private List<Post> _postlist = new List<Post>();
        public void ProcessRequest(HttpContext context)
        {
            if (string.Compare("POST", context.Request.HttpMethod) == 0 && string.Compare("PageCount", context.Request.QueryString["Action"], true) == 0)
            {

                string PageIndexs = context.Request.Form["PageIndex"];
                string PageSizes = context.Request.Form["PageSize"];
                string CboardIDs = context.Request.Form["CboardID"];
                int cboardid = Convert.ToInt32(CboardIDs);
                int pagesize = Convert.ToInt32(PageSizes);
                int pageindex = Convert.ToInt32(PageIndexs);
                int totalrow = 0;

                List<Post> postlist = this._pmgr.GetPostList(cboardid, pagesize, pageindex, out totalrow);
                int pagecount = (totalrow / pagesize) + 1;
                _postlist = postlist;
                context.Response.ContentType = "text/plain";
                context.Response.Write(pagecount);
                return;
            }
            if (string.Compare("GET", context.Request.HttpMethod) == 0 && string.Compare("Mugen", context.Request.QueryString["Action"], true) == 0)
            {
                string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(_postlist);
                context.Response.ContentType = "application/json";
                context.Response.Write(jsonText);
                return;
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