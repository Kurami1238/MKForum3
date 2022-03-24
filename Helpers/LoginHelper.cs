using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Security.Principal;


namespace MKForum.Helpers
{
    public class LoginHelper
    {
        public void Login(string account, string userID)
        {
            bool isPersistance = false;
            TimeSpan timeOut = new TimeSpan(3, 0, 0);
            FormsAuthentication.SetAuthCookie(account, false);
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket
            (
                1,                      //版本
                account,                //帳號
                DateTime.Now,           //發證時間
                DateTime.Now.Add(timeOut),  //逾期時間
                isPersistance,
                userID
            );

            // 設定目前登入者至 Cookie
            string cookieName = FormsAuthentication.FormsCookieName;
            string encryptedText = FormsAuthentication.Encrypt(ticket);
            HttpCookie loginCookie = new HttpCookie(cookieName, encryptedText);
            loginCookie.HttpOnly = true;
            loginCookie.Expires = DateTime.Now.Add(timeOut);
            HttpContext.Current.Request.Cookies.Add(loginCookie);

            // 設定目前登入者至 Current User
            FormsIdentity identity = new FormsIdentity(ticket);
            GenericPrincipal gp = new GenericPrincipal(identity, new string[] { });
            HttpContext.Current.User = gp;
        }

        public string[] GetIdentityData()
        {
            var identity = HttpContext.Current.User.Identity as FormsIdentity; //取ID
            var UserID = identity.Ticket.UserData;
            string UserAccount = HttpContext.Current.User.Identity.Name; //取Account
            string[] userinfo = new string[] { UserAccount, UserID };
            return userinfo;
        }


        public void Logout()
        {
            HttpContext.Current.Session.Abandon();
            FormsAuthentication.SignOut();
        }

        public bool dentityLogined()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }




    }
}