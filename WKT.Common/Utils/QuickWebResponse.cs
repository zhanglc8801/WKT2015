using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using System.Net.Sockets;
using System.Net;
using System.IO;

namespace WKT.Common.Utils
{
    public class QuickWebResponse
    {
        /// <summary>
        /// 最大下载的文件的大小，目前定义为10M
        /// </summary>
        public const int MAX_DOWNLOAD_LENGTH = 10 * 1024 * 1024;


        /// <summary>
        /// 向服务器端发送请求后，ResponseUri有可能会发生变化,如果发生了变化则得不到具体的内容，需要重新读取新的URL
        /// </summary>
        public Uri RedirectUri = null;

        public Uri RequestUri = null;
        /// <summary>
        /// 客户端设置KeepAlive后，服务器有可能不支持，发送请求后这里会获得服务器端是否支持KeepAlive
        /// </summary>
        private bool KeepAlive;

        /// <summary>
        /// 发送请求后服务器响应的是否支持KeepLive
        /// </summary>
        public bool IsKeepLive {
            get {
                return KeepAlive;
            }
        }

        /// <summary>
        /// 服务器端返回的ContentType
        /// </summary>
        private string ContentType;

        /// <summary>
        /// Mime内容的字符串
        /// </summary>
        public string ContentTypeStr {
            get {
                return ContentType;
            }
        }

        /// <summary>
        /// 服务器端返回的ContentLength
        /// </summary>
        private  int ContentLength;

        /// <summary>
        /// 本次读取的有效内容长度
        /// </summary>
        public int ContentLen {
            get {
                return ContentLength;
            }
        }

        private WebHeaderCollection Headers;
        private string Header;

        /// <summary>
        /// 服务器返回的Http头信息
        /// </summary>
        public String ResponseHeader {
            get {
                return Header;
            }
        }

        private string m_version;

        /// <summary>
        /// HTTP版本号
        /// </summary>
        public string Version {
            get {
                return m_version;
            }
        }

        private int m_httpCode;
        /// <summary>
        /// Http返回代码
        /// </summary>
        public int HttpCode {
            get {
                return m_httpCode;
            }
        }

        private string m_httpstatus;
        /// <summary>
        /// Http状态字符串
        /// </summary>
        public string HttpStatus {
            get {
                return m_httpstatus;
            }
        }

        private Socket socket;

        /// <summary>
        /// 以秒为单位的超时时间
        /// </summary>
        private int Timeout = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TimeoutSeconds">发送和接收的超时时间秒数</param>
        public QuickWebResponse(int TimeoutSeconds)
		{
            Timeout = TimeoutSeconds;
            RedirectUri = null;
            RequestUri = null;
		}

        /// <summary>
        /// 是否已发送了请求
        /// </summary>
        private bool isRequestSend = false;

        /// <summary>
        /// 是否已读取了包头
        /// </summary>
        private bool isHeaderParsed = false;

        /// <summary>  
        /// 判断是否是Ip地址  
        /// </summary>  
        /// <param name="str1"></param>  
        /// <returns></returns>  
        private bool IsIPAddress(string str1)
        {
            if (str1 == null || str1 == string.Empty || str1.Length < 7 || str1.Length > 15) return false;
            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";
            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str1);
        } 


        /// <summary>
        /// 发送连接，并且接收包头分析,只能在连接正常并且keepLive,并获取了包头的情况下调用。
        /// 并且不能调用两次
        /// </summary>
        /// <param name="request"></param>
		public void ConnectAndGetHeader(QuickWebRequest request)
		{
            isRequestSend = false;
            RequestUri = request.RequestUri;
            RedirectUri = null;
			socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress[] addrlist = null;
            if (IsIPAddress(RequestUri.Host))
            {
                addrlist = new IPAddress[1];
                addrlist[0] = IPAddress.Parse(RequestUri.Host);
            }
            else
            {
                addrlist = Dns.GetHostEntry(RequestUri.Host).AddressList;
            }
            IPEndPoint remoteEP = null;
            foreach (IPAddress ipaddr in addrlist)
            {
                try
                {
                    remoteEP = new IPEndPoint( ipaddr, RequestUri.Port );
                    if (this.Timeout > 0)
                    {
                        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, Timeout * 1000);
                        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, Timeout * 1000);
                    }
                    socket.Connect(remoteEP);
                    break;
                }
                catch
                {
                    remoteEP = null;
                }
            }

            //发送请求

            request.Header = request.Method + " " + RequestUri.PathAndQuery + " HTTP/1.1\r\n" + request.Headers;
            socket.Send(Encoding.ASCII.GetBytes(request.Header));
            isRequestSend = true;

            //接收包头
            ReceiveHeader();  
		}

        /// <summary>
        /// 从socket中获取一个Http块的大小
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        private int GetCunkHead(Socket socket,bool isStart){
            //skip\r\n
            byte[] RecvBuffer = new byte[32];
            if (!isStart && socket.Receive(RecvBuffer, 0, 2, SocketFlags.None) != 2)
            {
                return 0;
            }

            MemoryStream streamOut = new MemoryStream(64);
            int nBytes, nTotalBytes = 0;
            while ((nBytes = socket.Receive(RecvBuffer, 1, SocketFlags.None)) == 1)
            {
                streamOut.Write( RecvBuffer, 0, nBytes );
                nTotalBytes++;

                byte[] barray = streamOut.ToArray();
                if ( barray.Length >=2 && 
                     barray[barray.Length - 1] == '\n' && barray[barray.Length - 2] == '\r') {
                         break;
                }
            }
            byte[] chunkArray = streamOut.ToArray();
            streamOut.Close();
            if (chunkArray !=null  && chunkArray.Length > 2)
            {
                string strChunkSize = System.Text.Encoding.ASCII.GetString(chunkArray, 0, chunkArray.Length - 2);
                //内容长度是16位的
                int chunk_size = Convert.ToInt32(strChunkSize,16); ;
                return chunk_size;
            }
            else {
                return 0;
            }

        }

        /// <summary>
        /// 获取响应的内容，如果没有内容则返回null
        /// </summary>
        /// <returns></returns>
        public byte[] ReadResponse(){
                if (!isRequestSend) {
                    throw new Exception("还没有发送请求，无法继续从连接中读取数据!");
                }

                if (!isHeaderParsed)
                {
                    throw new Exception("还没有读取Http包头，无法继续从连接中读取数据!");
                }

                MemoryStream streamOut = new MemoryStream(10240);
                BinaryWriter writer = new BinaryWriter(streamOut);
                byte[] RecvBuffer = new byte[10240];
                int nBytes, nTotalBytes = 0;
                byte[] retbytes = null;
                if ( !string.IsNullOrEmpty(this.Headers["Content-Length"]) && 
                     Convert.ToInt32(this.Headers["Content-Length"] ) > 0 )
                {
                    try
                    {
                        //使用chunked方式向用户发送的字符串
                        if (this.Headers["Transfer-Encoding"] != null && this.Headers["Transfer-Encoding"].ToLower() == "chunked")
                        {
                            int chunk_size = 0;
                            chunk_size = GetCunkHead(socket, true);
                            while (this.SocketConnected && chunk_size > 0)
                            {
                                if (RecvBuffer.Length < chunk_size)
                                {
                                    RecvBuffer = new byte[chunk_size];
                                }

                                //读取chunk块
                                while ((nBytes = socket.Receive(RecvBuffer, 0, chunk_size, SocketFlags.None)) > 0)
                                {
                                    writer.Write(RecvBuffer, 0, nBytes);
                                    if (nBytes != chunk_size)
                                    {
                                        chunk_size = chunk_size - nBytes;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                //读取chunk头
                                chunk_size = GetCunkHead(socket, false);
                            }
                        }
                        else
                        {
                            while (this.SocketConnected && (nBytes = socket.Receive(RecvBuffer, 0, 10240, SocketFlags.None)) > 0)
                            {

                                nTotalBytes += nBytes;

                                writer.Write(RecvBuffer, 0, nBytes);

                                //对于有内容长度的服务器的处理
                                if (ContentLength > 0 && nTotalBytes >= ContentLength)
                                    break;

                                //KeepAlive &&
                                //     (ContentLength > 0 && 

                                //如果内容超过指定大小，则结束下载，并且该连接已经不可重用
                                if (nTotalBytes >= MAX_DOWNLOAD_LENGTH)
                                {
                                    KeepAlive = false;
                                    break;
                                }
                            }
                        }

                        retbytes = streamOut.ToArray();
                    }
                    catch (Exception ex)
                    {
                        KeepAlive = false;
                        if (streamOut == null || streamOut.Length == 0)
                        {
                            throw ex;
                        }
                    }
                    finally
                    {
                        if (writer != null)
                            writer.Close();

                        if (streamOut != null)
                            streamOut.Close();

                        if (!KeepAlive)
                        {
                            // close response
                            Close();
                        }
                    }
                }

                isRequestSend = false;
                isHeaderParsed = false;
                return retbytes;
        }

        /// <summary>
        /// 当前socket是否还在连接状态
        /// </summary>
        public bool SocketConnected {
            get {
                return (socket != null && socket.Connected);
            }
        }
 
		private void ReceiveHeader()
		{
            RedirectUri = null;
            isHeaderParsed = false;
			Header = "";
            m_httpstatus = string.Empty;
            m_version = string.Empty;
            m_httpCode = -1;
			Headers = new WebHeaderCollection();

			byte[] bytes = new byte[10];
			while(socket.Receive(bytes, 0, 1, SocketFlags.None) > 0)
			{
				Header += Encoding.ASCII.GetString(bytes, 0, 1);
				if(bytes[0] == '\n' && Header.EndsWith("\r\n\r\n"))
					break;

                //防止读取的http头过大,超过100k的头明显有问题
                if (Header.Length > (100 * 1024)) {
                    break;
                }
			}

            //读取第一行\r\n的http状态代码和字符串 HTTP/1.0 200 OK\r\n
            StringBuilder sbhttp = new StringBuilder();
            int curindex = 0;
            for (; curindex < Header.Length; curindex++)
            {
                if (Header[curindex] == ' ' || Header[curindex] == '\r')
                {
                    curindex++;
                    break;
                }

                sbhttp.Append(Header[curindex]);
            }

            this.m_version = sbhttp.ToString();
            sbhttp.Remove(0, sbhttp.Length);
            for (; curindex < Header.Length; curindex++)
            {
                if (Header[curindex] == ' ' || Header[curindex] == '\r')
                {
                    curindex++;
                    break;
                }

                sbhttp.Append(Header[curindex]);
            }
            this.m_httpCode = Convert.ToInt32(sbhttp.ToString());

            sbhttp.Remove(0, sbhttp.Length);
            for (; curindex < Header.Length; curindex++)
            {
                if (Header[curindex] == ' ' || Header[curindex] == '\r')
                {
                    break;
                }

                sbhttp.Append(Header[curindex]);
            }
            this.m_httpstatus = sbhttp.ToString();


            //获取Http头
			MatchCollection matches = new Regex("[^\r\n]+").Matches(Header.TrimEnd('\r', '\n'));
            int mcount = 0;
			for(int n = 1; n < matches.Count; n++)
			{
				string[] strItem = matches[n].Value.Split(new char[] { ':' }, 2);
				if(strItem.Length > 0)
					Headers[strItem[0].Trim()] = strItem[1].Trim();

                mcount++;
                if (mcount > TextHelper.MAX_MATCH_COUNT)
                {
                    break;
                } 
			}

			//检查重定向问题
            if ( matches.Count > 0 && 
                 ( matches[0].Value.IndexOf(" 302 ") != -1 || matches[0].Value.IndexOf(" 301 ") != -1) && 
                 Headers["Location"] != null )
            {
                   //从Http头中检查是否已经用 "location" 重定向到了新的地址
                    try
                    {
                        if ( Headers["Location"].StartsWith("http://") || Headers["Location"].StartsWith(RequestUri.Host) )
                        {
                            RedirectUri = new Uri( Headers["Location"] );
                        }
                        else
                        {
                            RedirectUri = new Uri( RequestUri, Headers["Location"] );
                        }
                    }
                    catch
                    {
                        RedirectUri = new Uri( RequestUri, Headers["Location"] );
                    }
            }


			ContentType = Headers["Content-Type"];

            if ( Headers["Content-Length"] != null )
            {
                ContentLength = int.Parse( Headers["Content-Length"] );
            }

			KeepAlive = (Headers["Connection"] != null &&  Headers["Connection"].ToLower() == "keep-alive") ||
						(Headers["Proxy-Connection"] != null && Headers["Proxy-Connection"].ToLower() == "keep-alive");

            //如果重定向到了别的主机,则不重用当前链接
            if ( RedirectUri != null && RedirectUri.Host != RequestUri.Host ) {
                 KeepAlive = false;
            }
            isHeaderParsed = true;
		}

        /// <summary>
        /// 在Keep-live的情况下，如果应用程序没有退出，或者下载的主机没有改变，就不需要调用Close,使用原有的
        /// tcp连接可以重用已经建立的
        /// </summary>
        public void Close()
        {
            if ( SocketConnected )
            {
                socket.Close();
                socket = null;
            }
        }
    }
}
