using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using WKT.Model;
using chinadoiIF;
using chinadoiIF.Dto;
using chinadoiIF.WebReference;
using System.Threading;

namespace WKT.Common.Utils
{
    public class DoiUtils
    {
        /// <summary>
        /// 生成DOI注册用XML文件
        /// </summary>
        /// <param name="DoiRegTemplatePath">Doi注册模板路径</param>
        /// <param name="DoiRegFileSavePath">Doi注册文件保存路径</param>
        /// <param name="JournalId">期刊ID(请登录www.chinadoi.cn查询)</param>
        /// <param name="DoiPrefix">Doi前缀</param>
        /// <param name="FullTitle">杂志社名称</param>
        /// <param name="FullTitle_En">杂志社英文名称</param>
        /// <param name="issnStr">国际刊号</param>
        /// <param name="cnStr">国内刊号</param>
        /// <param name="IssueYear">年</param>
        /// <param name="IssueVolume">卷</param>
        /// <param name="IssueIssue">期</param>
        /// <param name="list">期刊列表</param>
        /// <param name="URL">网站首页地址 如:www.xxx.com</param>
        public static void CreateDoiRegisterFile(string DoiRegTemplatePath,string DoiRegFileSavePath, string JournalId, string DoiPrefix, string FullTitle, string FullTitle_En, string issnStr, string cnStr, int IssueYear, int IssueVolume, int IssueIssue, IList<IssueContentEntity> list, string URL)
        {
            XElement root = XElement.Load(DoiRegTemplatePath, LoadOptions.SetLineInfo);//加载XML文档
            XElement journal = root.Element("body").Element("journal");//获取journal节点下的子元素

            #region 刊元数据
            //创建[刊元数据]节点
            XElement journal_metadata = new XElement("journal_metadata");
            //添加期刊ID
            XElement journal_id = new XElement("journal_id", JournalId);
            journal_metadata.Add(journal_id);
            //添加杂志名称
            XElement full_title = new XElement("full_title", FullTitle);
            journal_metadata.Add(full_title);
            //添加杂志名称(英文)
            XElement full_title_en = new XElement("full_title", FullTitle_En);
            journal_metadata.Add(full_title_en);
            //其他
            XElement abbrev_title = new XElement("abbrev_title");
            journal_metadata.Add(abbrev_title);
            //国际刊号
            XElement issn = new XElement("issn", issnStr);
            issn.SetAttributeValue("media_type", "print");
            journal_metadata.Add(issn);
            //国内刊号
            XElement cn = new XElement("cn", cnStr);
            cn.SetAttributeValue("media_type", "print");
            journal_metadata.Add(cn);
            //完成添加[刊元数据]节点
            journal.Add(journal_metadata); 
            #endregion

            #region 期元数据
            //创建[期元数据]节点
            XElement journal_issue = new XElement("journal_issue");
            //添加年节点
            XElement publication_date = new XElement("publication_date");
            publication_date.SetAttributeValue("media_type", "print");
            //年
            XElement year = new XElement("year", IssueYear);
            publication_date.Add(year);
            //月
            XElement month = new XElement("month");
            publication_date.Add(month);
            //日
            XElement day = new XElement("day");
            publication_date.Add(day);
            journal_issue.Add(publication_date);
            //添加卷节点
            XElement journal_volume = new XElement("journal_volume");
            //卷
            XElement volume = new XElement("volume", IssueVolume);
            journal_volume.Add(volume);
            journal_issue.Add(journal_volume);
            //期
            XElement issue = new XElement("issue", IssueIssue);
            journal_issue.Add(issue);
            //othoer
            XElement special_numbering = new XElement("special_numbering");
            journal_issue.Add(special_numbering);
            //完成添加[期元数据]节点
            journal.Add(journal_issue); 
            #endregion

            #region 文章元数据
            for (int i = 0; i < list.Count; i++)
            {
                string doiIssuePrefix = string.Empty;
                if (list[i].Issue < 10)
                    doiIssuePrefix = "0" + list[i].Issue;
                else
                    doiIssuePrefix = list[i].Issue.ToString();

                //创建[文章元数据]节点
                XElement journal_article = new XElement("journal_article");

                XElement titles = new XElement("titles");
                XElement title = new XElement("title", Utils.ClearHtmlNbsp(list[i].Title));
                titles.Add(title);
                XElement subtitle = new XElement("subtitle", Utils.ClearHtmlNbsp(list[i].EnTitle));
                titles.Add(subtitle);
                journal_article.Add(titles);

                XElement contributors = new XElement("contributors");
                XElement person_name1 = new XElement("person_name", Utils.ClearHtmlNbsp(list[i].Authors));
                person_name1.SetAttributeValue("sequence", "first");
                person_name1.SetAttributeValue("contributor_role", "author");
                contributors.Add(person_name1);
                XElement person_name2 = new XElement("person_name");
                person_name2.SetAttributeValue("sequence", "additional");
                person_name2.SetAttributeValue("contributor_role", "author");
                contributors.Add(person_name2);
                XElement organization = new XElement("organization");
                organization.SetAttributeValue("sequence", "additional");
                organization.SetAttributeValue("contributor_role", "author");
                contributors.Add(organization);
                journal_article.Add(contributors);

                XElement publication_date_2 = new XElement("publication_date");
                publication_date_2.SetAttributeValue("media_type", "print");
                XElement year_2 = new XElement("year", list[i].Year);
                publication_date_2.Add(year_2);
                journal_article.Add(publication_date_2);

                XElement publisher_item = new XElement("publisher_item");
                XElement item_number = new XElement("item_number", JournalId + list[i].Year + doiIssuePrefix + i.ToString());//由杂志ID与日期及序号构成，如dizhen201412001
                publisher_item.Add(item_number);
                journal_article.Add(publisher_item);

                XElement keywords = new XElement("keywords", Utils.ClearHtmlNbsp(list[i].Keywords));
                journal_article.Add(keywords);

                XElement Abstract = new XElement("abstract", Utils.ClearHtmlNbsp(list[i].Abstract));
                journal_article.Add(Abstract);


                XElement doi_data = new XElement("doi_data");
                if (list[i].DOI.Length > 5)
                {
                    XElement doi = new XElement("doi", list[i].DOI);//10.11939/j.issn.2072-1439.2014.02.01
                    doi_data.Add(doi);
                }
                else
                {
                    XElement doi = new XElement("doi", DoiPrefix + "/j.issn." + issnStr + "." + list[i].Year + "." + doiIssuePrefix + "." + list[i].ContentID);//10.11939/j.issn.2072-1439.2014.02.01
                    doi_data.Add(doi);
                }

                XElement timestamp = new XElement("timestamp");
                doi_data.Add(timestamp);
                XElement resource = new XElement("resource", "<![CDATA[http://" + URL + "/Magazine/Show?id=" + list[i].ContentID + "]]>");
                doi_data.Add(resource);
                journal_article.Add(doi_data);

                XElement pages = new XElement("pages");
                XElement first_page = new XElement("first_page", list[i].StartPageNum);
                pages.Add(first_page);
                XElement last_page = new XElement("last_page", list[i].EndPageNum);
                pages.Add(last_page);
                journal_article.Add(pages);
                journal.Add(journal_article);
            } 
            #endregion
            //保存DOI注册用XML文件
            root.Save(DoiRegFileSavePath);
        }

        /// <summary>
        /// 上传DOI注册文件
        /// </summary>
        /// <param name="RegFilePath"></param>
        /// <param name="userName"></param>
        /// <param name="userPwd"></param>
        /// <param name="oprType"></param>
        /// <returns></returns>
        public static string Upload(string RegFilePath, string userName, string userPwd, int oprType)
        {
            StringBuilder sb = new StringBuilder();
            UploadFile uploadFile = new UploadFile();
            UploadResult result = uploadFile.upload(RegFilePath, userName, userPwd, oprType);
            foreach (String file in result.getNewFileNames())
            {
                sb.AppendLine(file);
            }
            return sb.ToString();
        }

        //获取DOI注册结果
        public static string GetDoiRegResult(String fileName, String userName, String password, String RegResultSavePath)
        {
            StringBuilder sb = new StringBuilder();
            UploadFile uploadFile = new UploadFile();
            DoiRegResult result = uploadFile.getDoiRegResult(fileName, userName, password);
            //保存注册结果
            if (result.state == "已完成")
            {
                XElement root = XElement.Load(result.resultFileUrl.Replace("Successful.log", ""), LoadOptions.SetLineInfo);//加载XML文档
                root.Save(RegResultSavePath);
            }
            return result.state;
        }

        public static string GetDoiRegResult(String fileName, String userName, String password)
        {
            StringBuilder sb = new StringBuilder();
            UploadFile uploadFile = new UploadFile();
            DoiRegResult result = uploadFile.getDoiRegResult(fileName, userName, password);
            return result.state;
        }

    }
}
