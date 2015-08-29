using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;

using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using WKT.Model;
using WKT.Model.Enum;
using WKT.Common.Xml;
using WKT.Common.Utils;
using WKT.Config;
using WKT.Facade.Service.Interface;
using WKT.Facade.Service.Wrapper;
using WKT.Log;
using WKT.Common.Data;
using System.Threading;
using DotMaysWind.Office;
using System.Configuration;

namespace HanFang360.InterfaceService.Controllers
{
    public class UploadController : Controller
    {
        public ActionResult Index(string path, string fileName, string browser = "IE")
        {

            ViewBag.Path = path;
            ViewBag.FileName = fileName;
            //ViewBag.Brower = Request.QueryString["Browser"].ToString();
            ViewBag.Brower = browser;
            return View();
        }

        #region SaveFile 保存文件 限制为40MB
        [HttpPost]
        public ActionResult Save()
        {
            return SaveFile(false);
        }

        [HttpPost]
        public ActionResult SaveDetail()
        {
            return SaveFile(true);
        }

        private JsonResult SaveFile(bool IsDetail)
        {

            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files[0];
                if (file.ContentLength > 1024 * 40960)
                    return Json(new { result = "max", msg = "上传文件太大，请确保文件小于40M！" });
                if (file.ContentLength < 0)
                    return Json(new { result = "empty", msg = "上传文件不允许为空！" });
                string path = ("/UploadFile/" + @Request["folder"] + "/").Replace("//", "/");
                string uploadPath = GetUploadPath(path);
                string filename = Path.ChangeExtension(Path.GetRandomFileName(), Path.GetExtension(file.FileName));
                string BaseFileName = Path.GetFileNameWithoutExtension(file.FileName);//原始文件名
                string url = Url.Content(path + filename);
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                if (Path.GetExtension(filename).ToLower() == ".asp" || Path.GetExtension(filename).ToLower() == ".aspx" || Path.GetExtension(filename).ToLower() == ".php" || Path.GetExtension(filename).ToLower() == ".exe" || Path.GetExtension(filename).ToLower() == ".dll" || Path.GetExtension(filename).ToLower() == ".asa" || Path.GetExtension(filename).ToLower() == ".cgi")
                {
                    return Json(new { result = "empty", msg = "上传文件的格式不正确！" });
                }
                else
                {
                    file.SaveAs(uploadPath + filename);
                }

                if (!IsDetail)
                {
                    return Json(new { result = "success", url = url, filename = BaseFileName });
                }
                else
                {
                    return Json(new { result = "success", url = url, filename = BaseFileName, size = ((Decimal)file.ContentLength) / 1024, ext = Path.GetExtension(file.FileName) });
                }
            }
            return Json(new { result = "empty", msg = "上传文件不允许为空！" });
        } 
        #endregion

        #region 获取完整路径
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
        #endregion
        
        #region CheckAuthorFileAjax
        [HttpPost]
        public JsonResult CheckAuthorFileAjax(long cid, long AuthorID)
        {
            HttpCookie cookie = Request.Cookies["WKT_SSO.CN"];
            IFlowFacadeService flowService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>(new ResolverOverride[0]);
            CirculationEntity ce = new CirculationEntity
            {
                CID = cid,
                JournalID = SiteConfig.SiteID,
                GroupID = 2
            };
            IList<FlowLogInfoEntity> flowLogList = flowService.GetFlowLog(ce);
            if (flowLogList.Count == 1)//如果是新稿件
            {
                if (flowLogList[0].FigurePath != "" && flowLogList[0].OtherPath != "")
                    return base.Json(new { flag = "19" });
                else if (flowLogList[0].FigurePath != "")
                    return base.Json(new { flag = "12" });
                else if (flowLogList[0].OtherPath != "")
                    return base.Json(new { flag = "13" });
                else
                    return base.Json(new { flag = "10" });

            }
            else //如果不是新稿件
            {
                if (flowLogList[0].CPath != "")//如果存在修改稿
                {
                    #region 判断最新的附件/介绍信
                    if (flowLogList[0].FigurePath != "" && flowLogList[0].OtherPath != "")
                        return base.Json(new { flag = "29" });
                    else if (flowLogList[0].FigurePath != "")
                        return base.Json(new { flag = "22" });
                    else if (flowLogList[0].OtherPath != "")
                        return base.Json(new { flag = "23" });
                    //else
                    //    return base.Json(new { flag = "20" });
                    #endregion

                    #region 判断原始的附件/介绍信
                    if (flowLogList[flowLogList.Count - 1].FigurePath != "" && flowLogList[flowLogList.Count - 1].OtherPath != "")
                        return base.Json(new { flag = "39" });
                    else if (flowLogList[flowLogList.Count - 1].FigurePath != "")
                        return base.Json(new { flag = "32" });
                    else if (flowLogList[flowLogList.Count - 1].OtherPath != "")
                        return base.Json(new { flag = "33" });
                    else
                        return base.Json(new { flag = "30" });
                    #endregion

                }
                else//如果不存在修改稿
                {
                    #region 判断最新的附件/介绍信
                    if (flowLogList[0].FigurePath != "" && flowLogList[0].OtherPath != "")
                        return base.Json(new { flag = "49" });
                    else if (flowLogList[0].FigurePath != "")
                        return base.Json(new { flag = "42" });
                    else if (flowLogList[0].OtherPath != "")
                        return base.Json(new { flag = "43" });
                    //else
                    //    return base.Json(new { flag = "40" });
                    #endregion

                    #region 判断原始的附件/介绍信
                    if (flowLogList[flowLogList.Count - 1].FigurePath != "" && flowLogList[flowLogList.Count - 1].OtherPath != "")
                        return base.Json(new { flag = "59" });
                    else if (flowLogList[flowLogList.Count - 1].FigurePath != "")
                        return base.Json(new { flag = "52" });
                    else if (flowLogList[flowLogList.Count - 1].OtherPath != "")
                        return base.Json(new { flag = "53" });
                    else
                        return base.Json(new { flag = "50" });
                    #endregion
                }

            }
        } 
        #endregion

        #region DownLoadFile 下载文件：仅根据文件路径
        public ActionResult DownLoadFile(string path, string fileName)
        {
            return down(path, fileName);
        }
        private ActionResult down(string path, string fileName)
        {
            string downPath = GetUploadPath(path);
            if (!System.IO.File.Exists(downPath))
                return Content("文件不存在！");
            if (CheckImage(downPath))
            {
                return File(GetImageByte(downPath), @"image/jpeg");
            }
            if (!fileName.Contains("."))
                fileName += Path.GetExtension(downPath);
            HttpResponseBase response = this.HttpContext.Response;
            HttpServerUtilityBase server = this.HttpContext.Server;
            response.Clear();
            response.Buffer = true;
            if (Request.Browser.Browser == "Firefox")
            {
                Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            }
            else
            {
                Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
            }
            response.ContentType = CheckImage(downPath) ? "image/*" : "application/octet-stream";
            //response.AppendHeader("Content-Length", "attachment;filename=" + fileName);
            response.TransmitFile(downPath);
            response.Flush();
            response.Close();
            return null;
        } 
        #endregion

        #region Download 下载文件_并记录下载状态：根据稿件ID_流程ID
        /// <summary>
        /// 文件下载_根据稿件ID_流程ID
        /// </summary>
        /// <param name="cid">稿件ID</param>
        /// <param name="fileName">文件名</param>
        /// <param name="downType1">下载类型1：1=下载CPath;2=下载FigurePath;3=OtherPath</param>
        /// <param name="downType2">下载类型2：1=原稿件(最后一条数据);2=最新稿件(第一条数据);3=过程稿件</param>
        /// <param name="isDown">是否下载过：false=未下载过;true=下载过</param>
        /// <returns></returns>
        public ActionResult Download(long cid, long FlowLogID, string fileName, int downType1, int downType2,bool isDown)
        {
            string filePath = string.Empty;
            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies["WKT_SSO.CN"];
            IFlowFacadeService flowService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>();
            CirculationEntity ce = new CirculationEntity();
            ce.CID = cid;
            ce.JournalID = SiteConfig.SiteID;
            ce.GroupID = 1;
            IList<FlowLogInfoEntity> flowLogList = flowService.GetFlowLog(ce);
            if (cookie != null)
            {
                #region MyRegion
                if (downType2 == 1)//下载原稿件
                {
                    if (downType1 == 1)
                        filePath = GetUploadPath(flowLogList[flowLogList.Count - 1].CPath);
                    if (downType1 == 2)
                        filePath = GetUploadPath(flowLogList[flowLogList.Count - 1].FigurePath);
                    if (downType1 == 3)
                        filePath = GetUploadPath(flowLogList[flowLogList.Count - 1].OtherPath);
                }
                #endregion
                #region MyRegion
                if (downType2 == 2)//下载最新稿件
                {
                    if (downType1 == 1)
                        filePath = GetUploadPath(flowLogList[0].CPath);
                    if (downType1 == 2)
                        filePath = GetUploadPath(flowLogList[0].FigurePath);
                    if (downType1 == 3)
                        filePath = GetUploadPath(flowLogList[flowLogList.Count - 1].OtherPath);
                }
                #endregion
                #region MyRegion
                if (downType2 == 3)//下载过程稿件
                {
                    for (int i = 0; i < flowLogList.Count; i++)
                    {
                        if (flowLogList[i].FlowLogID == FlowLogID)
                        {
                            if (downType1 == 1)
                                filePath = GetUploadPath(flowLogList[i].CPath);
                            if (downType1 == 2)
                                filePath = GetUploadPath(flowLogList[i].FigurePath);
                            if (downType1 == 3)
                                filePath = GetUploadPath(flowLogList[i].OtherPath);
                        }

                    }
                }
                #endregion
                fileName += Path.GetExtension(filePath);
                try
                {
                    FileStream myFile = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    BinaryReader br = new BinaryReader(myFile);
                    try
                    {
                        #region MyRegion
                        Response.AddHeader("Accept-Ranges", "bytes");
                        Response.Buffer = false;
                        long fileLength = myFile.Length;
                        long startBytes = 0;

                        double pack = 10240; //10K bytes
                        //int sleep = 200;   //每秒5次   即5*10K bytes每秒
                        int sleep = (int)Math.Floor(1000 * pack / 204800) + 1;
                        if (Request.Headers["Range"] != null)
                        {
                            Response.StatusCode = 206;
                            string[] range = Request.Headers["Range"].Split(new char[] { '=', '-' });
                            startBytes = Convert.ToInt64(range[1]);
                        }

                        Response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                        if (startBytes != 0)
                        {
                            //Response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength-1, fileLength));
                        }
                        Response.AddHeader("Connection", "Keep-Alive");
                        Response.ContentType = "application/octet-stream";                
                        if (Request.Browser.Browser == "Firefox")
                        {
                            Response.AddHeader("Content-Disposition", "attachment;filename=" +fileName);             
                        }
                        else
                        {
                            Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
                        }
                        
                        br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                        int maxCount = (int)Math.Floor((fileLength - startBytes) / pack) + 1;

                        for (int i = 0; i < maxCount; i++)
                        {
                            if (Response.IsClientConnected)
                            {
                                Response.BinaryWrite(br.ReadBytes(int.Parse(pack.ToString())));
                                Thread.Sleep(sleep);
                            }
                            else
                            {
                                i = maxCount;
                            }
                        }
                        #endregion

                        # region 更新下载状态
                        if (isDown == false)
                        {
                            try
                            {
                                FlowLogQuery query = new FlowLogQuery();
                                query.JournalID = SiteConfig.SiteID;
                                query.FlowLogID = FlowLogID;
                                flowService.UpdateFlowLogIsDown(query);
                            }
                            catch (Exception ex)
                            {
                                WKT.Log.LogProvider.Instance.Error("更新审稿日志的下载状态出现异常：" + ex.Message);
                            }
                        }

                        # endregion
                    }
                    catch (Exception ex)
                    {
                        return Content("无法完成下载！详细信息：" + ex.Message);
                    }
                    finally
                    {
                        br.Close();
                        myFile.Close();
                    }
                }
                catch (Exception ex)
                {
                    return Content("无法完成下载！详细信息：" + ex.Message);
                }
                return null;

            }
            else
            {
                string url = ConfigurationManager.AppSettings["RootPath"] + "/User/Login";
                return Content("您无权下载当前文件，请<a href=\""+url+"\">登录</a>后重试。");
            }



        } 
        #endregion

        #region DownloadAuthorCID 下载文件：根据稿件ID_作者ID
        /// <summary>
        /// 文件下载_根据稿件ID_作者ID
        /// </summary>
        /// <param name="cid">稿件ID</param>
        /// <param name="AuthorID">作者ID</param>
        /// <param name="fileName">文件名</param>
        /// <param name="downType1">下载类型1：1=下载CPath;2=下载FigurePath;3=OtherPath</param>
        /// <param name="downType2">下载类型2：1=原稿件(最后一条数据);2=最新稿件(第一条数据)</param>
        /// <returns></returns>
        public ActionResult DownloadAuthorCID(long cid, long AuthorID, string fileName, int downType1, int downType2,bool isDown)
        {
            string filePath = string.Empty;
            HttpCookie cookie = Request.Cookies["WKT_SSO.CN"];
            IFlowFacadeService flowService = ServiceContainer.Instance.Container.Resolve<IFlowFacadeService>(new ResolverOverride[0]);
            CirculationEntity ce = new CirculationEntity
            {
                CID = cid,
                JournalID = SiteConfig.SiteID,
                GroupID = 2
            };
            long flowLogID = 0;
            IList<FlowLogInfoEntity> flowLogList = flowService.GetFlowLog(ce);
            if (cookie != null)
            {
                if (downType1 == 1)
                {
                    if (downType2 == 1)
                    {
                        filePath = this.GetUploadPath(flowLogList[flowLogList.Count - 1].CPath);
                        flowLogID = flowLogList[flowLogList.Count - 1].FlowLogID;
                    }
                    if (downType2 == 2)
                    {
                        filePath = this.GetUploadPath(flowLogList[0].CPath);
                        flowLogID = flowLogList[0].FlowLogID;
                    }
                }
                if (downType1 == 2)
                {
                    if (downType2 == 1)
                    {
                        filePath = this.GetUploadPath(flowLogList[flowLogList.Count - 1].FigurePath);
                        flowLogID = flowLogList[flowLogList.Count - 1].FlowLogID;
                    }
                    if (downType2 == 2)
                    {
                        filePath = this.GetUploadPath(flowLogList[0].FigurePath);
                        flowLogID = flowLogList[0].FlowLogID;
                    }
                }
                if (downType1 == 3)
                {
                    if (downType2 == 1)
                    {
                        filePath = this.GetUploadPath(flowLogList[flowLogList.Count - 1].OtherPath);
                        flowLogID = flowLogList[flowLogList.Count - 1].FlowLogID;
                    }
                    if (downType2 == 2)
                    {
                        filePath = this.GetUploadPath(flowLogList[0].OtherPath);
                        flowLogID = flowLogList[0].FlowLogID;
                    }
                }
                fileName = fileName + Path.GetExtension(filePath);
                try
                {
                    FileStream myFile = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    BinaryReader br = new BinaryReader(myFile);
                    try
                    {
                        #region MyRegion
                        Response.AddHeader("Accept-Ranges", "bytes");
                        Response.Buffer = false;
                        long fileLength = myFile.Length;
                        long startBytes = 0;

                        double pack = 10240; //10K bytes
                        //int sleep = 200;   //每秒5次   即5*10K bytes每秒
                        int sleep = (int)Math.Floor(1000 * pack / 204800) + 1;
                        if (Request.Headers["Range"] != null)
                        {
                            Response.StatusCode = 206;
                            string[] range = Request.Headers["Range"].Split(new char[] { '=', '-' });
                            startBytes = Convert.ToInt64(range[1]);
                        }

                        Response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                        if (startBytes != 0)
                        {
                            //Response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength-1, fileLength));
                        }
                        Response.AddHeader("Connection", "Keep-Alive");
                        Response.ContentType = "application/octet-stream";
                        if (Request.Browser.Browser == "Firefox")
                        {
                            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
                        }
                        else
                        {
                            Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
                        }
                        br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                        int maxCount = (int)Math.Floor((fileLength - startBytes) / pack) + 1;

                        for (int i = 0; i < maxCount; i++)
                        {
                            if (Response.IsClientConnected)
                            {
                                Response.BinaryWrite(br.ReadBytes(int.Parse(pack.ToString())));
                                Thread.Sleep(sleep);
                            }
                            else
                            {
                                i = maxCount;
                            }
                        }
                        #endregion

                        # region 更新下载状态
                        if (isDown == false)
                        {
                            try
                            {
                                FlowLogQuery query = new FlowLogQuery();
                                query.JournalID = SiteConfig.SiteID;
                                query.FlowLogID = flowLogID;
                                flowService.UpdateFlowLogIsDown(query);
                            }
                            catch (Exception ex)
                            {
                                WKT.Log.LogProvider.Instance.Error("更新审稿日志的下载状态出现异常：" + ex.Message);
                            }
                        }

                        # endregion
                    }
                    catch (Exception ex)
                    {
                        return Content("无法完成下载！详细信息：" + ex.Message);
                    }
                    finally
                    {
                        br.Close();
                        myFile.Close();
                    }
                }
                catch (Exception ex)
                {

                    return Content("无法完成下载！详细信息：" + ex.Message);
                }
                return null;


            }
            else
                return null;

        } 
        #endregion
        
        #region 原始稿件及附件下载(作者界面)
        /// <summary>
        /// 原始稿件及附件下载(作者界面)
        /// </summary>
        /// <param name="cid">稿件ID</param>
        /// <param name="downType">下载类型1=稿件 2=附件 3=介绍信</param>
        /// <returns></returns>
        public ActionResult DownloadCID(long AuthorID,long cid, string fileName, int downType)
        {
            long[] CIDs = { cid };
            string filePath = string.Empty;
            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies["WKT_SSO.CN"];

            ContributionInfoQuery query = new ContributionInfoQuery();
            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            query.JournalID = SiteConfig.SiteID;
            query.AuthorID = AuthorID;
            query.CIDs = CIDs;
            query.CurrentPage = 1;
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            query.OrderStr = " AddDate desc";
            Pager<ContributionInfoEntity> pager = service.GetContributionInfoPageList(query);
            if (cookie != null)
            {
                #region MyRegion
                if (downType == 1)
                    filePath = GetUploadPath(pager.ItemList[0].ContributePath);
                if (downType == 2)
                    filePath = GetUploadPath(pager.ItemList[0].FigurePath);
                if (downType == 3)
                    filePath = GetUploadPath(pager.ItemList[0].IntroLetterPath);
                #endregion
                fileName += Path.GetExtension(filePath);
                try
                {
                    FileStream myFile = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    BinaryReader br = new BinaryReader(myFile);
                    try
                    {
                        #region MyRegion
                        Response.AddHeader("Accept-Ranges", "bytes");
                        Response.Buffer = false;
                        long fileLength = myFile.Length;
                        long startBytes = 0;

                        double pack = 10240; //10K bytes
                        //int sleep = 200;   //每秒5次   即5*10K bytes每秒
                        int sleep = (int)Math.Floor(1000 * pack / 204800) + 1;
                        if (Request.Headers["Range"] != null)
                        {
                            Response.StatusCode = 206;
                            string[] range = Request.Headers["Range"].Split(new char[] { '=', '-' });
                            startBytes = Convert.ToInt64(range[1]);
                        }

                        Response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                        if (startBytes != 0)
                        {
                            //Response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength-1, fileLength));
                        }
                        Response.AddHeader("Connection", "Keep-Alive");
                        Response.ContentType = "application/octet-stream";
                        if (Request.Browser.Browser == "Firefox")
                        {
                            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
                        }
                        else
                        {
                            Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
                        }
                        br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                        int maxCount = (int)Math.Floor((fileLength - startBytes) / pack) + 1;

                        for (int i = 0; i < maxCount; i++)
                        {
                            if (Response.IsClientConnected)
                            {
                                Response.BinaryWrite(br.ReadBytes(int.Parse(pack.ToString())));
                                Thread.Sleep(sleep);
                            }
                            else
                            {
                                i = maxCount;
                            }
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        return Content("无法完成下载！详细信息：" + ex.Message);
                    }
                    finally
                    {
                        br.Close();
                        myFile.Close();
                    }
                }
                catch (Exception ex)
                {

                    return Content("无法完成下载！详细信息：" + ex.Message);
                }
                return null;

            }
            else
            {
                return null;
            }
        } 
        #endregion

        #region 原始稿件及附件下载(编辑界面,需传入作者ID)
        /// <summary>
        /// 原始稿件及附件下载(作者界面)
        /// </summary>
        /// <param name="cid">稿件ID</param>
        /// <param name="downType">下载类型1=稿件 2=附件 3=介绍信</param>
        /// <returns></returns>
        public ActionResult DownloadCIDByEditor(long cid, string fileName, int downType,long AuthorID)
        {
            long[] CIDs = { cid };
            string filePath = string.Empty;
            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies["WKT_SSO.CN"];

            ContributionInfoQuery query = new ContributionInfoQuery();
            IAuthorPlatformFacadeService service = ServiceContainer.Instance.Container.Resolve<IAuthorPlatformFacadeService>();
            query.JournalID = SiteConfig.SiteID;
            query.AuthorID = AuthorID;
            query.CIDs = CIDs;
            query.CurrentPage = 1;
            query.PageSize = Convert.ToInt32(Request.Params["pagesize"]);
            query.OrderStr = " AddDate desc";
            Pager<ContributionInfoEntity> pager = service.GetContributionInfoPageList(query);
            if (cookie != null)
            {
                #region MyRegion
                if (downType == 1)
                    filePath = GetUploadPath(pager.ItemList[0].ContributePath);
                if (downType == 2)
                    filePath = GetUploadPath(pager.ItemList[0].FigurePath);
                if (downType == 3)
                    filePath = GetUploadPath(pager.ItemList[0].IntroLetterPath);
                #endregion
                fileName += Path.GetExtension(filePath);
                try
                {
                    FileStream myFile = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    BinaryReader br = new BinaryReader(myFile);
                    try
                    {
                        #region MyRegion
                        Response.AddHeader("Accept-Ranges", "bytes");
                        Response.Buffer = false;
                        long fileLength = myFile.Length;
                        long startBytes = 0;

                        double pack = 10240; //10K bytes
                        //int sleep = 200;   //每秒5次   即5*10K bytes每秒
                        int sleep = (int)Math.Floor(1000 * pack / 204800) + 1;
                        if (Request.Headers["Range"] != null)
                        {
                            Response.StatusCode = 206;
                            string[] range = Request.Headers["Range"].Split(new char[] { '=', '-' });
                            startBytes = Convert.ToInt64(range[1]);
                        }

                        Response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                        if (startBytes != 0)
                        {
                            //Response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength-1, fileLength));
                        }
                        Response.AddHeader("Connection", "Keep-Alive");
                        Response.ContentType = "application/octet-stream";
                        if (Request.Browser.Browser == "Firefox")
                        {
                            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
                        }
                        else
                        {
                            Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
                        }
                        br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                        int maxCount = (int)Math.Floor((fileLength - startBytes) / pack) + 1;

                        for (int i = 0; i < maxCount; i++)
                        {
                            if (Response.IsClientConnected)
                            {
                                Response.BinaryWrite(br.ReadBytes(int.Parse(pack.ToString())));
                                Thread.Sleep(sleep);
                            }
                            else
                            {
                                i = maxCount;
                            }
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        return Content("无法完成下载！详细信息：" + ex.Message);
                    }
                    finally
                    {
                        br.Close();
                        myFile.Close();
                    }
                }
                catch (Exception ex)
                {

                    return Content("无法完成下载！详细信息：" + ex.Message);
                }
                return null;

            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 判断是否为图片
        /// <summary>
        /// 判断是否为图片
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool CheckImage(string path)
        {
            string ext = Path.GetExtension(path).ToLower();
            if (ext.Equals(".jpg") || ext.Equals(".gif") || ext.Equals(".png") || ext.Equals(".bmp"))
            {
                try
                {
                    using (System.Drawing.Image img = System.Drawing.Image.FromFile(path))
                    {
                        img.Dispose();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;
        } 
        #endregion

        #region 把图像数据转换为byte[]
        /// <summary>
        /// 把图像数据转换为byte[]
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        private byte[] GetImageByte(string imagePath)
        {
            using (FileStream files = new FileStream(imagePath, FileMode.Open))
            {
                byte[] imgByte = new byte[files.Length];
                files.Read(imgByte, 0, imgByte.Length);
                files.Close();
                files.Dispose();
                return imgByte;
            }
        }
        #endregion

        #region 获取DOC/DOCX稿件内容
        private IOfficeFile _file;
        /// <summary>
        /// 获取DOC/DOCX稿件内容
        /// </summary>
        /// <param name="filePath">稿件文件路径</param>
        /// <returns></returns>
        [AjaxRequest]
        public JsonResult GetDocContentAjax(String filePath)
        {
            try
            {
                filePath = GetUploadPath(filePath);
                this._file = OfficeFileFactory.CreateOfficeFile(filePath);
                IWordFile wordFile = _file as IWordFile;
                StringReader sr = new StringReader(wordFile.ParagraphText);
                string line;
                int lineIndex = 0;

                string CTitle = string.Empty;//稿件标题
                string CAbstract = string.Empty;//摘要
                string CAbstractEn = string.Empty;//英文摘要
                string CKeywords = string.Empty;//关键词
                //string CReference = string.Empty;//参考文献

                while ((line = sr.ReadLine()) != null)
                {
                    if (lineIndex == 0)
                        CTitle = line;
                    if (line.Contains("摘要"))
                    {
                        string newStr = TextHelper.Replace(line, "摘要", "");
                        newStr = TextHelper.Replace(newStr, "[", "");
                        newStr = TextHelper.Replace(newStr, "]", "");
                        newStr = TextHelper.Replace(newStr, "【", "");
                        newStr = TextHelper.Replace(newStr, "】", "");
                        newStr = TextHelper.Replace(newStr, "：", "");
                        newStr = TextHelper.Replace(newStr, ":", "");
                        CAbstract = newStr;
                    }
                    if (line.Contains("Abstract") || line.Contains("abstract"))
                    {
                        string AbstractStr = TextHelper.Replace(line, "Abstract", "");
                        AbstractStr = TextHelper.Replace(AbstractStr, "abstract", "");
                        AbstractStr = TextHelper.Replace(AbstractStr, "[", "");
                        AbstractStr = TextHelper.Replace(AbstractStr, "]", "");
                        AbstractStr = TextHelper.Replace(AbstractStr, "【", "");
                        AbstractStr = TextHelper.Replace(AbstractStr, "】", "");
                        AbstractStr = TextHelper.Replace(AbstractStr, "：", "");
                        AbstractStr = TextHelper.Replace(AbstractStr, ":", "");
                        CAbstractEn = AbstractStr;
                    }

                    if (line.Contains("关键词"))
                    {
                        string KeyStr = TextHelper.Replace(line, "关键词", "");
                        KeyStr = TextHelper.Replace(KeyStr, " ", "");
                        KeyStr = TextHelper.Replace(KeyStr, "[", "");
                        KeyStr = TextHelper.Replace(KeyStr, "]", "");
                        KeyStr = TextHelper.Replace(KeyStr, "【", "");
                        KeyStr = TextHelper.Replace(KeyStr, "】", "");
                        KeyStr = TextHelper.Replace(KeyStr, "：", "");
                        KeyStr = TextHelper.Replace(KeyStr, ":", "");
                        CKeywords = KeyStr.Replace(",", ";").Replace("，", ";").Replace("；",";");
                    }
                    //if (line.Trim().Contains("参考文献"))
                    //{
                    //    line = sr.ReadLine();
                    //    line += sr.ReadLine() + "\r\n";
                    //    line += sr.ReadLine() + "\r\n";
                    //    line += sr.ReadLine() + "\r\n";
                    //    line += sr.ReadLine() + "\r\n";
                    //    line += sr.ReadLine() + "\r\n";
                    //    line += sr.ReadLine() + "\r\n";
                    //    line += sr.ReadLine() + "\r\n";
                    //    line += sr.ReadLine() + "\r\n";
                    //    line += sr.ReadLine() + "\r\n";
                    //    line += "......";
                    //    CReference = line;

                    //}
                    lineIndex++;

                }
                return base.Json(new { flag=1,Title = CTitle, Abstract = CAbstract, AbstractEn = CAbstractEn, Keywords = CKeywords });

            }
            catch (Exception ex)
            {
                this._file = null;
                return base.Json(new { flag = 0,error=ex.Message });
            }
        } 
        #endregion

        /// <summary>
        /// 字符串保存为Word文件
        /// </summary>
        /// <param name="str">输入字符串</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult SaveToWord(string str, string fileName)
        {
            System.IO.StringWriter sw = new System.IO.StringWriter();
            sw.Write(str);
            Response.Charset = "GB2312";
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8)+".doc");
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            Response.Write(sw.ToString());
            Response.End();
            return null;
        }
        
    }
}
