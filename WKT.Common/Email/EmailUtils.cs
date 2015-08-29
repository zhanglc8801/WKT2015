using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;

namespace WKT.Common.Email
{
    public class EmailUtils
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="SmtpServer">SMTP服务器</param>
        /// <param name="SmtpPort">SMTP端口号</param>
        /// <param name="AccountMail">邮件账号</param>
        /// <param name="AccountPwd">邮件密码</param>
        /// <param name="AccountName">发件人称呼</param>
        /// <param name="To">收件人,支持群发，多个地址之间用半角逗号分开</param>
        /// <param name="CC">抄送,支持群发，多个地址之间用半角逗号分开</param>
        /// <param name="Bcc">密送，,支持群发，多个地址之间用半角逗号分开</param>
        /// <param name="Subject">主题</param>
        /// <param name="Body">邮件内容</param>
        /// <param name="attachmentList">附件列表，物理路径</param>
        /// <param name="Priority">优先级 1=low 2=normal 3=high</param>
        /// <param name="IsUseCredentials">是否需要身份认证</param>
        /// <param name="MainEncoding">邮件编码 gb2312,utf-8</param>
        /// <param name="isHtml">邮件内容是否包含html</param>
        /// <param name="IsSSL">服务器是否启用SSL加密</param>
        /// <returns></returns>
        public static bool SendMail(string SmtpServer,int SmtpPort,string AccountMail,string AccountPwd,string AccountName,string To,string CC,string Bcc,
            string Subject, string Body, IList<string> attachmentList, int Priority, bool IsUseCredentials, string MainEncoding, bool isHtml,bool IsSSL)
        {          
            SmtpClient smtp = new SmtpClient();
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = IsSSL;//服务器是否启用SSL加密
            smtp.Host = SmtpServer;
            smtp.Port = SmtpPort;
            smtp.UseDefaultCredentials = IsUseCredentials;// 需要身份认证
            if (IsUseCredentials)
            {
                //如果需要认证，则用下面的方式
                smtp.Credentials = new NetworkCredential(AccountMail, AccountPwd);
            }
            
            MailMessage mm = new MailMessage();
            if (Priority == 1)
            {
                mm.Priority = MailPriority.Low;
            }
            else if (Priority == 2)
            {
                mm.Priority = MailPriority.Normal;
            }
            else if (Priority == 3)
            {
                mm.Priority = MailPriority.High;
            }
            else
            {
                mm.Priority = MailPriority.Normal;
            }

            string FromAccount = "";
            if (AccountMail.Contains("@"))
            {
                FromAccount = AccountMail;
            }
            else
            {
                if (AccountName.Contains("@"))
                {
                    FromAccount = AccountName;
                }
                else
                {
                    FromAccount = AccountMail + "@" + SmtpServer.ToLower().Replace("mail.","");
                }
            }
            //mm.From = new MailAddress(FromAccount, AccountName, Encoding.GetEncoding(MainEncoding));
            mm.From = new MailAddress(FromAccount);
            if (!string.IsNullOrWhiteSpace(CC))
            {               
                var arry = CC.Split('，');
                foreach (var item in arry)
                {
                    mm.CC.Add(item);
                }
            }
            if (!string.IsNullOrWhiteSpace(Bcc))
            {              
                var arry = Bcc.Split('，');
                foreach (var item in arry)
                {
                    mm.Bcc.Add(item);
                }
            }
            //mm.Sender = new MailAddress(FromAccount, AccountName, Encoding.GetEncoding(MainEncoding));
            // 邮件接受者支持群发，多个地址之间用 半角逗号 分开
            if(!string.IsNullOrWhiteSpace(To))
            {
                var arry=To.Split('，');
                foreach (var item in arry)
                {
                    mm.To.Add(item);
                }
            }
            mm.Subject = Subject; //邮件标题
            mm.SubjectEncoding = Encoding.GetEncoding(MainEncoding);
            mm.IsBodyHtml = isHtml; //邮件正文是否是HTML格式
            mm.BodyEncoding = Encoding.GetEncoding(MainEncoding);
            mm.Body = Body;
            if (attachmentList != null)
            {
                // 附件
                foreach (string attachment in attachmentList)
                {
                    mm.Attachments.Add(new Attachment(attachment));
                }
            }
            try
            {
                smtp.Send(mm);
            }
            catch
            {

            }
            finally
            {
                mm.Dispose();
                smtp.Dispose();
            }
            return true;
        }


        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="SmtpServer"></param>
        /// <param name="SmtpPort"></param>
        /// <param name="AccountMail"></param>
        /// <param name="AccountPwd"></param>
        /// <param name="FromName"></param>
        /// <param name="To"></param>
        /// <param name="CC"></param>
        /// <param name="Bcc"></param>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        /// <param name="attachmentList"></param>
        /// <param name="Priority"></param>
        /// <param name="IsUseCredentials"></param>
        /// <param name="MainEncoding"></param>
        /// <param name="isHtml"></param>
        /// <param name="IsSSL"></param>
        /// <returns></returns>
        public static bool SendMailEx(string SmtpServer, int SmtpPort, string AccountMail, string AccountPwd, string FromName, string To, string CC, string Bcc, string Subject, string Body, IList<string> attachmentList, int Priority, bool IsUseCredentials, string MainEncoding, bool isHtml, bool IsSSL)
        {

            //过滤掉邮件帐号中的@以及域名
            if (AccountMail.Contains("@"))
            {
                AccountMail = AccountMail.Substring(0, AccountMail.IndexOf("@"));
            }
            //获取发件人邮箱
            string FromMail = string.Empty;
            if (SmtpServer.Contains(';'))
            {
                string[] SmtpServers = SmtpServer.Split(';');
                SmtpServer = SmtpServers[0];
                FromName = AccountMail + "@" + SmtpServers[1].Substring(SmtpServers[1].IndexOf('.') + 1, SmtpServers[1].Length - SmtpServers[1].IndexOf('.') - 1);
            }
            else
            {
                FromMail = AccountMail + "@" + SmtpServer.Substring(SmtpServer.IndexOf('.') + 1, SmtpServer.Length - SmtpServer.IndexOf('.') - 1);
            }

            string ToName = To.Substring(0, To.IndexOf("@"));

            Chilkat.MailMan mailman = new Chilkat.MailMan();
            bool success = mailman.UnlockComponent("MAILT34MB34N_6ADE5E140UIY");
            if (success != true)
            {
                return false;
            }
            //设置SMTP服务器
            mailman.SmtpHost = SmtpServer;
            mailman.SmtpSsl = IsSSL;
            mailman.SmtpPort = SmtpPort;

            //设置SMTP服务器登录信息
            mailman.SmtpUsername = AccountMail;
            mailman.SmtpPassword = AccountPwd;

            //创建一个Email对象
            Chilkat.Email email = new Chilkat.Email();
            email.Subject = Subject;
            email.Body = Body;
            email.SetHtmlBody(Body);
            if (attachmentList != null)
            {
                // 附件
                foreach (string attachment in attachmentList)
                {
                    email.AddFileAttachment(attachment);
                }
            }
            email.From = "" + FromName + " <" + FromMail + ">";
            email.AddTo("" + ToName + "", To);

            success = mailman.SendEmail(email);
            mailman.CloseSmtpConnection();
            if (success != true)
            {
                return false;
            }
            return success;


        }


    }
}
