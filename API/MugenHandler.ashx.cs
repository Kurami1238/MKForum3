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
        // 如果要讓API讀session，
        // 讓上面實作 ,System.Web.SessionState.IReadOnlySessionState 的介面，但會讓ajax失去非同步效果
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
                string SortIDs = context.Request.Form["SortID"];
                int cboardid = Convert.ToInt32(CboardIDs);
                int pagesize = Convert.ToInt32(PageSizes);
                int pageindex = Convert.ToInt32(PageIndexs);
                int sortid = Convert.ToInt32(SortIDs);
                int totalrow = 0;
                List<Post> postList = new List<Post>();
                if (sortid == 0)
                    postList = this._pmgr.GetPostList(cboardid, pagesize, pageindex, out totalrow);
                else
                    postList = this._pmgr.GetPostList(cboardid, pagesize, pageindex, sortid, out totalrow);
                int pagecount = (totalrow / pagesize) + 1;
                // 文章若長度大於二十個字，後面則隱藏
                List<Post> seripost = PostContentSeri(postList);
                //分離memberid，然後用memberid查出 memberaccount
                var memberlistwithpoint = postList.Select(p => p.MemberID);
                List<Member> memberlist = new List<Member>();
                foreach (var x in memberlistwithpoint)
                {
                    Member me = this._amgr.GetAccount(x);
                    memberlist.Add(me);
                }
                //合併兩表連接rpt
                var pLML = from p in postList
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
                               LastEditTime = p.LastEditTime.ToString("yyyy/MM/dd tt HH:mm:ss"),
                               PostDate = p.PostDate.ToString("yyyy/MM/dd tt hh:mm:ss"),
                               CoverImage = p.CoverImage,
                               PostView = p.PostView,
                           };
                // 第二次合併把文章內容大於二十字的替換掉
                var pLMLS = from p in pLML
                            join s in seripost on p.PostID equals s.PostID
                            into tempPS
                            select new Temp
                            {
                                PostID = p.PostID,
                                MemberAccount = p.MemberAccount,
                                PostCotent = (tempPS.FirstOrDefault() != null) ? tempPS.FirstOrDefault().PostCotent : p.PostCotent,
                                MemberID = p.MemberID,
                                CboardID = p.CboardID,
                                Title = p.Title,
                                LastEditTime = p.LastEditTime,
                                PostDate = p.PostDate,
                                CoverImage = p.CoverImage,
                                PostView = p.PostView,
                            };
                MugenList<Temp> mugen = new MugenList<Temp>()
                {
                    PageCount = pagecount,
                    SourceList = pLMLS.ToList(),
                };

                string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(mugen);
                context.Response.ContentType = "application/json";
                context.Response.Write(jsonText);
                return;
            }
        }
        private static List<Post> PostContentSeri(List<Post> postList)
        {
            var postcontent = postList.Select((x, index) => { return new { PostCotent = x.PostCotent, PostID = x.PostID }; });
            List<Post> seripost = new List<Post>();
            foreach (var x in postcontent)
            {
                char[] pccarr = x.PostCotent.ToCharArray();
                if (pccarr.Length > 20)
                {
                    string newpc = string.Empty;
                    for (int i = 0; i < 20; i++)
                    {
                        newpc += pccarr[i];
                    }
                    newpc += " ..........(點擊後觀看)";
                    seripost.Add(new Post() { PostID = x.PostID, PostCotent = newpc });
                }
            }

            return seripost;
        }
        public class MugenList<T>
        {
            public int PageCount { get; set; }
            public List<T> SourceList { get; set; }
        }
        public class Temp
        {
            public Guid PostID { get; set; }
            public string MemberAccount { get; set; }
            public string PostCotent { get; set; }
            public Guid MemberID { get; set; }
            public int CboardID { get; set; }
            public string Title { get; set; }
            public string LastEditTime { get; set; }
            public string PostDate { get; set; }
            public string CoverImage { get; set; }
            public int PostView { get; set; }
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