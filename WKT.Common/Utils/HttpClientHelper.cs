using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;

using WKT.Common.Security;

namespace WKT.Common.Utils
{
    public enum MediaTypeHeader
    {
        application_json,
        application_xml
    }

    /// <summary>
    /// HttpClient Warapper
    /// </summary>
    public  class HttpClientHelper
    {
        public MediaTypeHeader mediaType { get; set; }

        private const string RQUESTHEADERTOKENKEY = "Authorization-Token";
        private const string AUTHSITE = "Request-Site";
        private const string AUTHSITEID = "Request-SiteID";

        public HttpClientHelper()
        {
            this.mediaType = MediaTypeHeader.application_json;
        }

        public T Get<T>(string url)
        {
            T returnResult;
            using (var client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = null;
                    client.GetAsync(url).ContinueWith(
                     (requestTask) =>
                     {
                         response = requestTask.Result;

                     }).Wait(60000);
                    if (response.IsSuccessStatusCode)
                    {
                        returnResult = response.Content.ReadAsAsync<T>().Result;
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                        //returnResult = default(T);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    client.Dispose();
                }
            }
            return returnResult;
        }

        public T GetAuth<T>(string url)
        {
            T returnResult;
            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Add(RQUESTHEADERTOKENKEY, RSAClass.Encrypt(GetToken()));
                    client.DefaultRequestHeaders.Add(AUTHSITEID, ConfigurationManager.AppSettings["SiteID"]);
                    client.DefaultRequestHeaders.Add(AUTHSITE, ConfigurationManager.AppSettings["SiteDomain"]);
                    HttpResponseMessage response = null;
                    client.GetAsync(url).ContinueWith(
                     (requestTask) =>
                     {
                         response = requestTask.Result;

                     }).Wait(60000);
                    if (response.IsSuccessStatusCode)
                    {
                        returnResult = response.Content.ReadAsAsync<T>().Result;
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                        //returnResult = default(T);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    client.Dispose();
                }
            }
            return returnResult;
        }

        # region post

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="url"></param>        
        /// <returns></returns>
        public T Post<T>(string url, T objectParam)
        {
            return Post<T, T>(url,objectParam);
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="url"></param>        
        /// <returns></returns>
        public T Post<T,PT>(string url, PT objectParam)
        {
            T returnResult;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = null;
                    client.PostAsJsonAsync(url, objectParam).ContinueWith((requestTask) =>
                    {
                        response = requestTask.Result;
                    }).Wait(600000);

                    if (response.IsSuccessStatusCode)
                    {
                        returnResult = response.Content.ReadAsAsync<T>().Result;
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                        //returnResult = default(T);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    client.Dispose();
                }
            }
            return returnResult;
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="url"></param>        
        /// <returns></returns>
        public T PostAuth<T>(string url, T objectParam)
        {
            return PostAuth<T, T>(url, objectParam);
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="url"></param>        
        /// <returns></returns>
        public T PostAuth<T, PT>(string url, PT objectParam)
        {
            T returnResult;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Add(RQUESTHEADERTOKENKEY, RSAClass.Encrypt(GetToken()));
                    client.DefaultRequestHeaders.Add(AUTHSITEID, ConfigurationManager.AppSettings["SiteID"]);
                    client.DefaultRequestHeaders.Add(AUTHSITE, ConfigurationManager.AppSettings["SiteDomain"]);
                    HttpResponseMessage response = null;
                    client.PostAsJsonAsync(url, objectParam).ContinueWith((requestTask) =>
                    {
                        response = requestTask.Result;
                    }).Wait(60000);

                    if (response.IsSuccessStatusCode)
                    {
                        returnResult = response.Content.ReadAsAsync<T>().Result;
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                        //returnResult = default(T);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    client.Dispose();
                }
            }
            return returnResult;
        }

        /// <summary>
        /// 返回Token
        /// </summary>
        /// <returns></returns>
        private string GetToken()
        {
            return ConfigurationManager.AppSettings["SiteDomain"] + ConfigurationManager.AppSettings["SiteID"] + DateTime.Now.ToString("yyyyMMdd");
        }

        # endregion
    }
}
