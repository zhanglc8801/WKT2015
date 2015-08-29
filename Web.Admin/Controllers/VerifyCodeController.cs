using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WKT.Common;

namespace Web.Admin.Controllers
{
    public class VerifyCodeController : Controller
    {
        /// <summary>
        /// 验证码
        /// </summary>
        public string VerifyCodeText
        {
            get
            {
                HttpCookie cookie = Request.Cookies[VERFIYCODEKEY];
                if (cookie != null)
                {
                    return cookie.Value.ToString();
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 验证码Key
        /// </summary>
        private const string VERFIYCODEKEY = "WKT_AUTH_VERFIFYCODE";



        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        public FileContentResult VerifyCode()
        {
            ValidateCode vCode = new ValidateCode();
            string randomcode = vCode.CreateRandomCode(4);
            Response.Cookies.Add(new HttpCookie(VERFIYCODEKEY, WKT.Common.Security.DES.Encrypt(randomcode.ToLower())));
            byte[] bytes = vCode.CreateImageToByte(randomcode, false);
            return File(bytes, @"image/gif");
        }
    }
}
