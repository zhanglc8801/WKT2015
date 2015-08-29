using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Net;
using System.IO;

using Microsoft.Practices.Unity;

using WKT.Model.FangZ;
using WKT.Model;
using WKT.Common.Utils;
using WKT.Config;
using WKT.Log;
using WKT.Facade.Service.Interface;
using WKT.Facade.Service.Wrapper;
using WKT.Common.Xml;

namespace HanFang360.InterfaceService
{
    /// <summary>
    /// FangzAPI 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class FangzAPI : System.Web.Services.WebService
    {
        # region GetMagazineDoc

        /// <summary>
        /// 获取杂志期刊信息
        /// </summary>
        /// <param name="strManuId"></param>
        /// <param name="strUserID"></param>
        /// <param name="strSafetyCode"></param>
        /// <returns></returns>
        [WebMethod]
        public XmlDocument GetMagazineDoc(string strManuId, string strUserID, string strSafetyCode)
        {
            docs rootDoc = new docs();
            docmodel docEntity = new docmodel();
            try
            {
                // 验证令牌正确性
                TokenQuery tokenQuery = new TokenQuery();
                tokenQuery.JournalID = TypeParse.ToLong(strUserID,0);
                tokenQuery.AuthorID = 0;
                tokenQuery.Token = strSafetyCode;
                tokenQuery.ExpireDate = DateTime.Now.AddDays(-7);
                IAuthorFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
                TokenEntity tokenEntity = authorService.GetToken(tokenQuery);

                if (tokenEntity != null)
                {
                    IAuthorPlatformFacadeService cService = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
                    // 得到稿件路径
                    ContributionInfoQuery cQuery = new ContributionInfoQuery();
                    cQuery.JournalID = SiteConfig.SiteID;
                    cQuery.CID = TypeParse.ToLong(strManuId);
                    ContributionInfoEntity cEntity = cService.GetContributionInfoModel(cQuery);
                    if (cEntity != null)
                    {
                        docEntity.title = cEntity.Title;
                        docEntity.manuid = cEntity.CID.ToString();
                        docEntity.name = SiteConfig.SiteName;
                        docEntity.state = "true";
                        docEntity.author = new authorentity { chinesename = "", englishname = "" };
                        docEntity.url = "http://" + Utils.GetHost() + SiteConfig.RootPath + "/Plugins/Download.aspx?CID=" + cEntity.CID;
                    }
                    else
                    {
                        docEntity.title = "对不起，没有该稿件信息";
                        docEntity.manuid = "0";
                        docEntity.name = SiteConfig.SiteName;
                        docEntity.state = "false";
                        docEntity.url = "";
                    }
                }
                else
                {
                    docEntity.title = "安全码无效，请检查";
                    docEntity.manuid = "0";
                    docEntity.name = "";
                    docEntity.state = "false";
                    docEntity.url = "";
                }
                docEntity.author = new authorentity { chinesename = "", englishname = "" };
                docEntity.date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                rootDoc.doc = docEntity;
            }
            catch (Exception ex)
            {
                LogProvider.Instance.Error("方正获取稿件信息出现异常：" + ex.Message);
            }

            string retrun = XmlUtils.Serialize(rootDoc);
            XmlDocument  xd=new XmlDocument ();
            XmlDeclaration xmldecl;
            xmldecl = xd.CreateXmlDeclaration("1.0","utf-8", null);
            XmlElement root = xd.DocumentElement;
            xd.InsertBefore(xmldecl, root);
            xd.LoadXml(retrun);
            return xd;
        }

        # endregion

        # region 登录返回安全码

        /// <summary>
        /// 登录返回安全码
        /// </summary>
        /// <param name="strMagazineName"></param>
        /// <param name="strUserName"></param>
        /// <param name="strPassword"></param>
        /// <returns></returns>
        [WebMethod]
        public XmlDocument Login(string strMagazineName, string strUserName, string strPassword)
        {
            string returnXml = @"
                                <login>
                                <result>{0}</result>
                                <user_id>{1}</user_id>
                                <safety_code>{2}</safety_code>
                                <magazine_id>{3}</magazine_id>
                                </login>
                                ";

            string strToekCode = RadomCode.GenerateCode(10) + DateTime.Now.Ticks;
            IAuthorFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
            TokenEntity getPwdToken = new TokenEntity();
            getPwdToken.Token = strToekCode;
            getPwdToken.JournalID = SiteConfig.SiteID;
            getPwdToken.Type = 3; // API Token
            getPwdToken.AuthorID = 0;
            authorService.InsertToken(getPwdToken);

            string result = string.Format(returnXml, 0, SiteConfig.SiteID, strToekCode, SiteConfig.SiteID);

            XmlDocument xd = new XmlDocument();
            XmlDeclaration xmldecl;
            xmldecl = xd.CreateXmlDeclaration("1.0", "utf-8", null);
            XmlElement root = xd.DocumentElement;
            xd.InsertBefore(xmldecl, root);
            xd.LoadXml(result);
            return xd;
        }

        # endregion

        # region 获取杂志栏目

        /// <summary>
        /// 获取杂志栏目
        /// </summary>
        /// <param name="strMagazineID"></param>
        /// <param name="strUserID"></param>
        /// <param name="strSafetyCode"></param>
        /// <returns></returns>
        [WebMethod]
        public XmlDocument GetMagazineStruct(string strMagazineID, string strUserID, string strSafetyCode)
        {
            Magazine magazineChannel = new Magazine();
            magazineChannel.magzineId = SiteConfig.SiteID.ToString();
            magazineChannel.magzineName = SiteConfig.SiteName;
            magazineChannel.magazinecolumns = new List<magazinecolumn>();

            IIssueFacadeService service = ServiceContainer.Instance.Container.Resolve<IIssueFacadeService>();
            JournalChannelQuery query = new JournalChannelQuery();
            query.JournalID = SiteConfig.SiteID;
            query.CurrentPage = 1;
            query.PageSize = 500;
            Pager<JournalChannelEntity> pager = service.GetJournalChannelPageList(query);
            if (pager != null)
            {
                foreach (JournalChannelEntity item in pager.ItemList)
                {
                    magazineChannel.magazinecolumns.Add(new magazinecolumn { name = item.ChannelName});
                }
            }
            string retrun = XmlUtils.Serialize(magazineChannel);
            XmlDocument xd = new XmlDocument();
            XmlDeclaration xmldecl;
            xmldecl = xd.CreateXmlDeclaration("1.0", "utf-8", null);
            XmlElement root = xd.DocumentElement;
            xd.InsertBefore(xmldecl, root);
            xd.LoadXml(retrun);
            return xd;
        }

        # endregion

        [WebMethod]
        public string UploadSingleProof(string strManuId, string strUserID, string strSafetyCode, string indexXml)
        {
            return "";
        }

        /// <summary>
        /// 回传排版稿件
        /// </summary>
        /// <param name="strManuId"></param>
        /// <param name="strUserID"></param>
        /// <param name="strSafetyCode"></param>
        /// <param name="indexXml"></param>
        /// <returns></returns>
        [WebMethod]
        public string UploadSingleFile(string strManuId, string strUserID, string strSafetyCode, string indexXml)
        {
            // 验证令牌正确性
            TokenQuery tokenQuery = new TokenQuery();
            tokenQuery.JournalID = TypeParse.ToLong(strUserID, 0);
            tokenQuery.AuthorID = 0;
            tokenQuery.Token = strSafetyCode;
            tokenQuery.ExpireDate = DateTime.Now.AddDays(-7);
            IAuthorFacadeService authorService = ServiceContainer.Instance.Container.Resolve<IAuthorFacadeService>();
            TokenEntity tokenEntity = authorService.GetToken(tokenQuery);

            if (tokenEntity != null)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(indexXml);
                XmlNode node = doc.SelectSingleNode("/doc/file[@type='previewPdf']");
                if (node != null)
                {
                    try
                    {
                        string path = Server.MapPath("/UploadFile/Fangzheng/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string fileName = strManuId + "_single.pdf";
                        using (WebClient wclient = new WebClient())
                        {
                            string savefile = path + fileName;
                            wclient.DownloadFile(node.InnerText, savefile);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogProvider.Instance.Error("下载方正排版后文件异常:" + ex.Message);
                        return "error:" + ex.Message;
                    }
                }
                else
                {
                    return "node null";
                }
            }
            return "ok";
        }

        [WebMethod]
        public void Test()
        {
            string xml = "<doc id=\"39833\" type=\"final\"><file type=\"previewPdf\">http://219.141.190.71:80/Content/Download?content=524f34e9-ecd2-4840-aee1-6a037fdd12d0</file><file type=\"metaInfo\">http://219.141.190.71:80/Content/Download?content=51fabbc1-59b0-4d3c-a165-f73362755141</file></doc>";
            UploadSingleFile("39833", "201212030001", "NG3ATZQPUP635019032407709757", xml);
        }

        /// <summary>
        /// 获取完整路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetUploadPath(string path)
        {
            string folder = SiteConfig.UploadPath;
            string uploadPath;
            if (!string.IsNullOrWhiteSpace(folder))
            {
                uploadPath = folder + path.Replace("/", "\\");
            }
            else
            {
                uploadPath = Server.MapPath(path);
            }
            return uploadPath;
        }
    }
}
