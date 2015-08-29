using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.BLL;
using WKT.BLL.Interface;
using WKT.Service.Interface;
using WKT.Model.Enum;
using WKT.Common.Extension;

namespace WKT.Service
{
    public partial class AuthorDetailService:IAuthorDetailService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private IAuthorDetailBusiness authorDetailBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IAuthorDetailBusiness AuthorDetailBusProvider
        {
            get
            {
                 if(authorDetailBusProvider == null)
                 {
                     authorDetailBusProvider = new AuthorDetailBusiness();//ServiceBusContainer.Instance.Container.Resolve<IAuthorDetailBusiness>();
                 }
                 return authorDetailBusProvider;
            }
            set
            {
              authorDetailBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public AuthorDetailService()
        {
        }
        
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="pKID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public AuthorDetailEntity GetAuthorDetail(Int64 pKID)
        {
           return AuthorDetailBusProvider.GetAuthorDetail( pKID);
        }
        
         /// <summary>
        /// 根据作者编号获取实体
        /// </summary>
        /// <param name="AuthorID"></param>
        /// <returns></returns>
        public AuthorDetailEntity GetAuthorDetailModel(Int64 AuthorID)
        {
            return AuthorDetailBusProvider.GetAuthorDetailModel(AuthorID);
        }
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<AuthorDetailEntity></returns>
        public List<AuthorDetailEntity> GetAuthorDetailList()
        {
            return AuthorDetailBusProvider.GetAuthorDetailList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="authorDetailQuery">AuthorDetailQuery查询实体对象</param>
        /// <returns>List<AuthorDetailEntity></returns>
        public List<AuthorDetailEntity> GetAuthorDetailList(AuthorDetailQuery authorDetailQuery)
        {
            return AuthorDetailBusProvider.GetAuthorDetailList(authorDetailQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询实体对象</param>
        /// <returns>Pager<AuthorDetailEntity></returns>
        public Pager<AuthorDetailEntity> GetAuthorDetailPageList(CommonQuery query)
        {
            return AuthorDetailBusProvider.GetAuthorDetailPageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<AuthorDetailEntity></returns>
        public Pager<AuthorDetailEntity> GetAuthorDetailPageList(QueryBase query)
        {
            return AuthorDetailBusProvider.GetAuthorDetailPageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="authorDetailQuery">AuthorDetailQuery查询实体对象</param>
        /// <returns>Pager<AuthorDetailEntity></returns>
        public Pager<AuthorDetailEntity> GetAuthorDetailPageList(AuthorDetailQuery authorDetailQuery)
        {
            return AuthorDetailBusProvider.GetAuthorDetailPageList(authorDetailQuery);
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="authorDetail">AuthorDetailEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddAuthorDetail(AuthorDetailEntity authorDetail)
        {
            return AuthorDetailBusProvider.AddAuthorDetail(authorDetail);
        }
        
        #endregion
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="authorDetail">AuthorDetailEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        public bool UpdateAuthorDetail(AuthorDetailEntity authorDetail)
        {
            return AuthorDetailBusProvider.UpdateAuthorDetail(authorDetail);
        }

        #endregion
        
        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="pKID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteAuthorDetail(Int64 pKID)
        {
            return AuthorDetailBusProvider.DeleteAuthorDetail( pKID);
        }
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="authorDetail">AuthorDetailEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteAuthorDetail(AuthorDetailEntity authorDetail)
        {
            return AuthorDetailBusProvider.DeleteAuthorDetail(authorDetail);
        }
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="pKID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteAuthorDetail(Int64[] pKID)
        {
            return AuthorDetailBusProvider.BatchDeleteAuthorDetail( pKID);
        }
        
        #endregion
        
        #endregion

        /// <summary>
        /// 保存作者详细信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult Save(AuthorDetailEntity model)
        {
            ExecResult execResult = new ExecResult();
            bool result = false;
            model.AuthorName = model.AuthorName.TextFilter();
            model.EnglishName = model.EnglishName.TextFilter();
            model.Nation = model.Nation.TextFilter();
            model.NativePlace = model.NativePlace.TextFilter();
            model.Province = model.Province.TextFilter();
            model.City = model.City.TextFilter();
            model.Area = model.Area.TextFilter();
            model.IDCard = model.IDCard.TextFilter();
            model.Address = model.Address.TextFilter();
            model.ZipCode = model.ZipCode.TextFilter();
            model.Mobile = model.Mobile.TextFilter();
            model.Tel = model.Tel.TextFilter();
            model.Fax = model.Fax.TextFilter();
            model.Professional = model.Professional.TextFilter();
            model.Job = model.Job.TextFilter();
            model.ResearchTopics = model.ResearchTopics.TextFilter();
            model.WorkUnit = model.WorkUnit.TextFilter();
            model.SectionOffice = model.SectionOffice.TextFilter();
            model.Mentor = model.Mentor.TextFilter();
            model.QQ = model.QQ.TextFilter();
            model.MSN = model.MSN.TextFilter();
            model.Remark = model.Remark.TextFilter();
            model.ReserveField = model.ReserveField.TextFilter();
            model.ReserveField1 = model.ReserveField1.TextFilter();
            model.ReserveField2 = model.ReserveField2.TextFilter();
            model.ReserveField3 = model.ReserveField3.TextFilter();
            model.ReserveField4 = model.ReserveField4.TextFilter();
            model.ReserveField5 = model.ReserveField5.TextFilter();
            if (model.AuthorModel != null&&!string.IsNullOrWhiteSpace(model.AuthorModel.Pwd))
                model.AuthorModel.Pwd = WKT.Common.Security.MD5Handle.Encrypt(model.AuthorModel.Pwd);
            result = AuthorDetailBusProvider.SaveAuthor(model);
            if (result)
            {
                execResult.result = EnumJsonResult.success.ToString();
                execResult.msg = "保存用户详细信息成功！";
            }
            else
            {
                execResult.result = EnumJsonResult.failure.ToString();
                execResult.msg = "保存用户详细信息失败！";
            }
            return execResult;
        }

        /// <summary>
        /// 删除作者信息
        /// </summary>
        /// <param name="AuthorID"></param>
        /// <returns></returns>
        public ExecResult DelAuthorDetail(Int64[] AuthorID)
        {
            ExecResult result = new ExecResult();
            if (AuthorID == null)
            {
                result.result = EnumJsonResult.failure.ToString();
                result.msg = "没有删除任何数据！";
                return result;
            }
            bool flag = AuthorDetailBusProvider.DelAuthorDetail(AuthorID);
            if (flag)
            {
                result.result = EnumJsonResult.success.ToString();
                result.msg = "删除成功！";
            }
            else
            {
                result.result = EnumJsonResult.success.ToString();
                result.msg = "删除失败！";
            }
            return result;
        }

        /// <summary>
        /// 设置作者为专家信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExecResult SetAuthorExpert(AuthorDetailQuery query)
        {
            ExecResult result = new ExecResult();
            if (query.AuthorIDs == null)
            {
                result.result = EnumJsonResult.failure.ToString();
                result.msg = "没有选择设置的作者数据！";
                return result;
            }
            bool flag = AuthorDetailBusProvider.SetAuthorExpert(query);
            if (flag)
            {
                result.result = EnumJsonResult.success.ToString();
                result.msg = "设置成功！";
            }
            else
            {
                result.result = EnumJsonResult.success.ToString();
                result.msg = "设置失败！";
            }
            return result;
        }

        /// <summary>
        /// 取消作者为专家信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ExecResult CancelAuthorExpert(AuthorDetailQuery query)
        {
            ExecResult result = new ExecResult();
            if (query.AuthorIDs == null)
            {
                result.result = EnumJsonResult.failure.ToString();
                result.msg = "没有选择设置的作者数据！";
                return result;
            }
            bool flag = AuthorDetailBusProvider.CancelAuthorExpert(query);
            if (flag)
            {
                result.result = EnumJsonResult.success.ToString();
                result.msg = "取消设置成功！";
            }
            else
            {
                result.result = EnumJsonResult.success.ToString();
                result.msg = "取消设置失败！";
            }
            return result;
        }

        #region 专家分组配置
        /// <summary>
        /// 获取专家分组信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IList<AuthorDetailEntity> GetExpertGroupMapList(ExpertGroupMapEntity query)
        {
            return AuthorDetailBusProvider.GetExpertGroupMapList(query);
        }

        /// <summary>
        /// 保存专家分组信息
        /// </summary>
        /// <param name="list"></param>
        /// <param name="ExpertGroupID"></param>
        /// <param name="JournalID"></param>
        /// <returns></returns>
        public ExecResult SaveExpertGroupMap(IList<ExpertGroupMapEntity> list, Int32 ExpertGroupID, Int64 JournalID)
        {
            ExecResult result = new ExecResult();
            bool flag = AuthorDetailBusProvider.SaveExpertGroupMap(list, ExpertGroupID, JournalID);
            if (flag)
            {
                result.result = EnumJsonResult.success.ToString();
                result.msg = "设置成功！";
            }
            else
            {
                result.result = EnumJsonResult.success.ToString();
                result.msg = "设置失败！";
            }
            return result;
        }
        #endregion
    }
}
