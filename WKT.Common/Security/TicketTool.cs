using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.Script.Serialization;

namespace WKT.Common.Security
{
    public class TicketTool
    {
        private const string COOKIEKEY = "WKT_SSO.CN";
        private const string COOKIENAME = "WKTUSERCOOKIE";
        private const string COOKIEPATH = "/";


        public class Team
        {
            public string GroupID { get; set; }
            public string OldGroupID { get; set; }
            public string AuthorID { get; set; }
        } 
        

        /// <summary>
        /// 创建一个票据，放在cookie中
        /// 票据中的数据经过加密，解决了cookie的安全问题。
        /// </summary>
        /// <param name="userID">员工ID</param>
        /// <param name="userData">员工实体类json字符串</param>
        public static void SetCookie(string userID, string userData)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[COOKIEKEY];

            JavaScriptSerializer js = new JavaScriptSerializer();
            Team team = js.Deserialize<Team>(userData);
            string GroupID = team.GroupID;
            string AuthorID = team.AuthorID;

            //防止中文乱码
            userData = System.Web.HttpUtility.UrlEncode(userData);
            //加密
            userData = DESEncrypt.Encrypt(userData);
            string domain = ConfigurationManager.AppSettings["SiteDomain"];

            if (cookie == null)
            {
                cookie = new HttpCookie(COOKIEKEY);
                if (!string.IsNullOrEmpty(domain))
                {
                    cookie.Domain = domain;
                }
                cookie.Path = COOKIEPATH;
                cookie.Values.Add(COOKIENAME, userData);
                cookie.Values.Add("AID", AuthorID);
                cookie.Values.Add("GID", GroupID);
                HttpContext.Current.Response.AppendCookie(cookie);

            }
            else
            {
                if (!string.IsNullOrEmpty(domain))
                {
                    cookie.Domain = domain;
                }
                cookie.Path = COOKIEPATH;

                if (cookie.Values[COOKIENAME] != null)
                {
                    cookie.Values.Set(COOKIENAME, userData);
                }
                else
                {
                    cookie.Values.Add(COOKIENAME, userData);
                }
                HttpContext.Current.Response.SetCookie(cookie);

            }
        }

        /// <summary>
        /// 创建一个票据，放在cookie中
        /// 票据中的数据经过加密，解决了cookie的安全问题。
        /// </summary>
        /// <param name="userID">员工ID</param>
        /// <param name="userData">员工实体类json字符串</param>
        public static void SetCookie(string userID, string userData, bool autologin)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[COOKIEKEY];

            JavaScriptSerializer js = new JavaScriptSerializer();
            Team team = js.Deserialize<Team>(userData);
            string OldGroupID = team.OldGroupID;
            string GroupID = team.GroupID;
            string AuthorID = team.AuthorID;


            //防止中文乱码
            userData = System.Web.HttpUtility.UrlEncode(userData);
            //加密
            userData = DESEncrypt.Encrypt(userData);
            string domain = ConfigurationManager.AppSettings["SiteDomain"];

            if (cookie == null)
            {
                cookie = new HttpCookie(COOKIEKEY);
                if (!string.IsNullOrEmpty(domain))
                {
                    cookie.Domain = domain;
                }
                cookie.Path = COOKIEPATH;
                cookie.Values.Add(COOKIENAME, userData);
                if (autologin)
                {
                    cookie.Expires = DateTime.Now.AddDays(30);
                }
                HttpContext.Current.Response.AppendCookie(cookie);

            }
            else
            {
                if (!string.IsNullOrEmpty(domain))
                {
                    cookie.Domain = domain;
                }
                cookie.Path = COOKIEPATH;
                if (cookie.Values[COOKIENAME] != null)
                {
                    cookie.Values.Set(COOKIENAME, userData);
                }
                else
                {
                    cookie.Values.Add(COOKIENAME, userData);
                }
                if (autologin)
                {
                    cookie.Expires = DateTime.Now.AddDays(30);
                }
                HttpContext.Current.Response.SetCookie(cookie);

            }
        }

        # region bak

        ///// <summary>
        ///// 创建一个票据，放在cookie中
        ///// 票据中的数据经过加密，解决了cookie的安全问题。
        ///// </summary>
        ///// <param name="userID">员工ID</param>
        ///// <param name="userData">员工实体类json字符串</param>
        //public static void SetCookie(string userID, string userData)
        //{
        //    string temp = HttpContext.Current.Request.Url.ToString();
        //    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userID, DateTime.Now, DateTime.Now.AddHours(8), false, userData, FormsAuthentication.FormsCookiePath);
        //    string encTicket = FormsAuthentication.Encrypt(ticket);
        //    HttpCookie newCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
        //    newCookie.HttpOnly = true;
        //    newCookie.Path = "/";
        //    string domain = ConfigurationManager.AppSettings["SiteDomain"];
        //    if (!string.IsNullOrEmpty(domain))
        //    {
        //        newCookie.Domain = domain;
        //    }
        //    HttpContext.Current.Response.Cookies.Add(newCookie);
        //}

       

        ///// <summary>
        ///// 创建一个票据，放在cookie中
        ///// 票据中的数据经过加密，解决了cookie的安全问题。
        ///// </summary>
        ///// <param name="userID">员工ID</param>
        ///// <param name="userData">员工实体类json字符串</param>
        //public static void SetCookie(string userID, string userData,bool autologin)
        //{
        //    int days = 30;
        //    string temp = HttpContext.Current.Request.Url.ToString();
        //    FormsAuthenticationTicket ticket = null;
        //    if (autologin)
        //    {
        //        ticket = new FormsAuthenticationTicket(1, userID, DateTime.Now, DateTime.Now.AddDays(days), true, userData, FormsAuthentication.FormsCookiePath);
        //    }
        //    else
        //    {
        //        ticket = new FormsAuthenticationTicket(1, userID, DateTime.Now, DateTime.Now.AddHours(8), false, userData, FormsAuthentication.FormsCookiePath);
        //    }
        //    string encTicket = FormsAuthentication.Encrypt(ticket);
        //    HttpCookie newCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
        //    newCookie.HttpOnly = true;
        //    newCookie.Path = "/";
        //    string domain = ConfigurationManager.AppSettings["SiteDomain"];
        //    if (!string.IsNullOrEmpty(domain))
        //    {
        //        newCookie.Domain = domain;
        //    }
        //    if (autologin)
        //    {
        //        newCookie.Expires = DateTime.Now.AddDays(days);// 保留两周
        //    }
        //    HttpContext.Current.Response.Cookies.Add(newCookie);
        //}

        # endregion

        /// <summary>
        /// 修改Cookie的值
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userData"></param>
        public static void UpdateCookie(string userID, string userData)
        {
            SetCookie(userID,userData);
        }

        /// <summary>
        /// 通过此法判断登录
        /// </summary>
        /// <returns>已登录返回true</returns>
        public static bool IsLogin()
        {
            //if (HttpContext.Current == null)
            //{
            //    return false;
            //}
            //else
            //{
            //    return HttpContext.Current.User.Identity.IsAuthenticated;
            //}
            HttpCookie cookie = HttpContext.Current.Request.Cookies[COOKIEKEY];
            if (cookie != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        public static void Logout()
        {
            //FormsAuthentication.SignOut();
            HttpCookie cookie = HttpContext.Current.Request.Cookies[COOKIEKEY];
            if (cookie != null)
            {
                string domain = ConfigurationManager.AppSettings["SiteDomain"];
                cookie.Values.Clear();
                if (!string.IsNullOrEmpty(domain))
                {
                    cookie.Domain = domain;
                }
                cookie.Path = COOKIEPATH;
                cookie.Expires = DateTime.Now.AddDays(-31);
                HttpContext.Current.Response.SetCookie(cookie);
            }
        }
        ///// <summary>
        ///// 取得登录员工ID
        ///// </summary>
        ///// <returns></returns>
        //public static string GetUserID()
        //{
        //    if (HttpContext.Current == null)
        //    {
        //        return "";
        //    }
        //    return HttpContext.Current.User.Identity.Name;
        //}
        /// <summary>
        /// 取得票据中数据
        /// </summary>
        /// <returns></returns>
        public static string GetUserData()
        {
            //if (HttpContext.Current == null)
            //{
            //    return "";
            //}
            //return (HttpContext.Current.User.Identity as FormsIdentity).Ticket.UserData;

            HttpCookie cookie = HttpContext.Current.Request.Cookies[COOKIEKEY];
            if (cookie != null)
            {
                string _value = cookie.Values.Get(COOKIENAME);
                if (!string.IsNullOrEmpty(_value))
                {
                    //解密
                    _value = DESEncrypt.Decrypt(_value);
                    //防止中文乱码
                    _value = System.Web.HttpUtility.UrlDecode(_value);
                    return _value;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
    }
}
