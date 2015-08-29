using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace WKT.Common.Utils
{
    /// <summary>
    /// 支持使用Http Keep-live功能的连接复用请求对像
    /// 可以减少客户端到服务器的连接次数
    /// 该对像创建的Request和Response都不是线程安全的
    /// </summary>
    public class QuickWebRequest
    {
        private QuickWebResponse response;
        private bool KeepAlive = true;
        private Uri refuri = null;

        public WebHeaderCollection Headers;
        public string Header;
        public Uri RequestUri;
        public string Method = "GET";
       

        /// <summary>
        /// 设置引用的URL
        /// </summary>
        public Uri RefererUri {
            get {
                return refuri;
            }
            set {

                refuri = value;
                if (refuri != null)
                {
                    Headers["Referer"] = refuri.AbsoluteUri;
                } else {
                    if ( Headers["Referer"] != null )
                    {
                        Headers.Remove("Referer");
                    }
                }
            }
        }

        /// <summary>
        /// 构造函数用来创建一个新的web请求对象
        /// </summary>
        /// <param name="refuri">页面的引用页的Url</param>
        /// <param name="uri">要下载的页面的Url</param>
        /// <param name="bKeepAlive"></param>
        private QuickWebRequest( Uri uri,Uri refuri, bool bKeepAlive)
		{
			Headers = new WebHeaderCollection();
			RequestUri = uri;
            this.refuri = refuri;
            Headers["Accept"] = "*/*";
            Headers["Accept-Language"] = "zh-cn";
            Headers["UA-CPU"] = "x86";
            Headers["User-Agent"] = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
			Headers["Host"] = uri.Host;
			KeepAlive = bKeepAlive;
            if (KeepAlive)
            {
                Headers["Connection"] = "Keep-Alive";
            }

            RefererUri = refuri;
			
		}

        public static QuickWebRequest Create(Uri uri,Uri refuri, QuickWebRequest AliveRequest, bool bKeepAlive)
		{
			if( bKeepAlive &&
				AliveRequest != null &&
				AliveRequest.response != null &&
				AliveRequest.response.IsKeepLive && 
				AliveRequest.response.SocketConnected && 
				AliveRequest.RequestUri.Host == uri.Host)
			{
				AliveRequest.RequestUri = uri;
                AliveRequest.RefererUri = refuri;

				return AliveRequest;
			}

            //如果原socket还在连接状态,但是主机名发生了变化，导致连接不能复用，需要关闭原socket
            if (AliveRequest != null &&
                AliveRequest.response != null &&
                AliveRequest.response.IsKeepLive &&
                AliveRequest.response.SocketConnected &&
                AliveRequest.RequestUri != null &&
                AliveRequest.RequestUri.Host != uri.Host) {
                    AliveRequest.response.Close();
            }

            return new QuickWebRequest(uri, refuri, bKeepAlive);
		}

        public QuickWebResponse GetResponse(int Timeout)
		{
            if (response == null || !response.SocketConnected)
            {
                response = new QuickWebResponse(Timeout);
                response.ConnectAndGetHeader(this);
            }
            else
            {
                response.ConnectAndGetHeader(this);
            }

			return response;
		}
    }
}
