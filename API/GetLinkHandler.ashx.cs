using MKForum.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MKForum.API
{
    /// <summary>
    /// GetLinkHandler 的摘要描述
    /// </summary>
    public class GetLinkHandler : IHttpHandler
    {
        private PostManager _pmgr = new PostManager();

        public void ProcessRequest(HttpContext context)
        {
            if (string.Compare("POST", context.Request.HttpMethod) == 0 && string.Compare("Link", context.Request.QueryString["Action"], true) == 0)
            { 
                string memberIDs = context.Request.Form["MemberID"];
                string imagelink = string.Empty;

                Guid memberID;
                if (Guid.TryParse(memberIDs, out memberID))
                {
                    imagelink = this._pmgr.GetImage(memberID);
                }
                if (!string.IsNullOrEmpty(imagelink))
                {
                    string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(imagelink);
                    context.Response.ContentType = "application/json";
                    context.Response.Write(jsonText);
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