using MKForum.Managers;
using MKForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace MKForum.API
{
    /// <summary>
    /// MugenHandler 的摘要描述
    /// </summary>
    public class MugenHandler : IHttpHandler
    {
        private AccountManager _amgr = new AccountManager();
        private PostManager _pmgr = new PostManager();
        private List<Post> _postlist = new List<Post>();
        public void ProcessRequest(HttpContext context)
        {
            if (string.Compare("POST", context.Request.HttpMethod) == 0 && string.Compare("Mugen", context.Request.QueryString["Action"], true) == 0)
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
                //分離memberid，然後用memberid查出 memberaccount
                var memberlistwithpoint = postlist.Select(p => p.MemberID);
                List<Member> memberlist = new List<Member>();
                foreach (var x in memberlistwithpoint)
                {
                    Member me = this._amgr.GetAccount(x);
                    memberlist.Add(me);
                }
                //合併兩表連接rpt
                //另外一個詭異的問題 IEnumerable 無法轉型成 Generic
                var pLML = from p in postlist
                           join m in memberlist on p.MemberID equals m.MemberID
                           into temppm
                           //from g in temppm.defaultifempty().distinct()
                           select new Temp
                           {
                               PostID = p.PostID,
                               MemberAccount = (temppm.FirstOrDefault() != null) ? temppm.FirstOrDefault().Account : "無",
                               PostCotent = p.PostCotent,
                               MemberID = p.MemberID,
                               CboardID = p.CboardID,
                               Title = p.Title,
                               LastEditTime = p.LastEditTime,
                               PostDate = p.PostDate,
                               CoverImage = p.CoverImage,
                           };

                MugenList<Temp> mugen = new MugenList<Temp>() {
                    PageCount = pagecount,
                    SourceList = pLML.ToList(),
                    //SourceList = postlist,
                }; 

                //string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(mugen);
                context.Response.ContentType = "application/json";
                //context.Response.Write(jsonText);
                return;
            }
        }
        public class MugenList<T>
        {
            public int PageCount { get; set; }
            public List<T> SourceList { get; set; }
        }
        public class Temp
        {
           public Guid  PostID             {get;set;}
           public string  MemberAccount    {get;set;}
           public string  PostCotent       {get;set;}
           public Guid  MemberID           {get;set;}
           public int  CboardID            {get;set;}
           public string  Title            {get;set;}
           public DateTime?  LastEditTime  {get;set;}
           public DateTime  PostDate       {get;set;}
           public string CoverImage { get; set; }
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