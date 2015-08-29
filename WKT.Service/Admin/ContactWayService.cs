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
    public partial class ContactWayService:IContactWayService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private IContactWayBusiness contactWayBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IContactWayBusiness ContactWayBusProvider
        {
            get
            {
                 if(contactWayBusProvider == null)
                 {
                     contactWayBusProvider = new ContactWayBusiness();//ServiceBusContainer.Instance.Container.Resolve<IContactWayBusiness>();
                 }
                 return contactWayBusProvider;
            }
            set
            {
              contactWayBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ContactWayService()
        {
        }
        
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="contactID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public ContactWayEntity GetContactWay(Int64 contactID)
        {
           return ContactWayBusProvider.GetContactWay( contactID);
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<ContactWayEntity></returns>
        public List<ContactWayEntity> GetContactWayList()
        {
            return ContactWayBusProvider.GetContactWayList();
        }
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="contactWayQuery">ContactWayQuery查询实体对象</param>
        /// <returns>List<ContactWayEntity></returns>
        public List<ContactWayEntity> GetContactWayList(ContactWayQuery contactWayQuery)
        {
            return ContactWayBusProvider.GetContactWayList(contactWayQuery);
        }
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询实体对象</param>
        /// <returns>Pager<ContactWayEntity></returns>
        public Pager<ContactWayEntity> GetContactWayPageList(CommonQuery query)
        {
            return ContactWayBusProvider.GetContactWayPageList(query);
        }
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<ContactWayEntity></returns>
        public Pager<ContactWayEntity> GetContactWayPageList(QueryBase query)
        {
            return ContactWayBusProvider.GetContactWayPageList(query);
        }
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="contactWayQuery">ContactWayQuery查询实体对象</param>
        /// <returns>Pager<ContactWayEntity></returns>
        public Pager<ContactWayEntity> GetContactWayPageList(ContactWayQuery contactWayQuery)
        {
            return ContactWayBusProvider.GetContactWayPageList(contactWayQuery);
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="contactWay">ContactWayEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddContactWay(ContactWayEntity contactWay)
        {
            return ContactWayBusProvider.AddContactWay(contactWay);
        }
        
        #endregion
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="contactWay">ContactWayEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        public bool UpdateContactWay(ContactWayEntity contactWay)
        {
            return ContactWayBusProvider.UpdateContactWay(contactWay);
        }

        #endregion
        
        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="contactID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteContactWay(Int64 contactID)
        {
            return ContactWayBusProvider.DeleteContactWay( contactID);
        }
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="contactWay">ContactWayEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool DeleteContactWay(ContactWayEntity contactWay)
        {
            return ContactWayBusProvider.DeleteContactWay(contactWay);
        }
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="contactID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteContactWay(Int64[] contactID)
        {
            return ContactWayBusProvider.BatchDeleteContactWay( contactID);
        }
        
        #endregion
        
        #endregion

        /// <summary>
        /// 保存联系人
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ExecResult Save(ContactWayEntity model)
        {
            ExecResult execResult = new ExecResult();            
            bool result = false;
            model.Company = model.Company.TextFilter();
            model.LinkMan = model.LinkMan.TextFilter();
            model.Tel = model.Tel.TextFilter();
            model.Fax = model.Fax.TextFilter();
            model.ZipCode = model.ZipCode.TextFilter();
            model.Address = model.Address.TextFilter();
            if (model.ContactID == 0)
            {
                result = AddContactWay(model);
                if (result)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "新增联系人信息成功！";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "新增联系人信息失败！";
                }
            }
            else
            {
                result = UpdateContactWay(model);
                if (result)
                {
                    execResult.result = EnumJsonResult.success.ToString();
                    execResult.msg = "修改联系人信息成功！";
                }
                else
                {
                    execResult.result = EnumJsonResult.failure.ToString();
                    execResult.msg = "修改联系人信息失败！";
                }
            }
            return execResult;
        }
    }
}
