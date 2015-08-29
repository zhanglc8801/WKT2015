using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;

namespace WKT.Service.Interface
{
    public interface ISysAccountInfoService
    {       
        #region 获取一个实体对象
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="adminID"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        SysAccountInfoEntity GetSysAccountInfo(Int32 adminID);
        
        #endregion

        #region 根据条件获取所有实体对象
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>List<SysAccountInfoEntity></returns>
        List<SysAccountInfoEntity> GetSysAccountInfoList();
        
        /// <summary>
        /// 获取所有符合查询条件的数据
        /// </summary>
        /// <param name="sysAccountInfoQuery">SysAccountInfoQuery查询实体对象</param>
        /// <returns>List<SysAccountInfoEntity></returns>
        List<SysAccountInfoEntity> GetSysAccountInfoList(SysAccountInfoQuery sysAccountInfoQuery);
        
        #endregion 
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<SysAccountInfoEntity></returns>
        Pager<SysAccountInfoEntity> GetSysAccountInfoPageList(CommonQuery query);
        
        
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns>Pager<SysAccountInfoEntity></returns>
        Pager<SysAccountInfoEntity> GetSysAccountInfoPageList(QueryBase query);
        
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="sysAccountInfoQuery">SysAccountInfoQuery查询实体对象</param>
        /// <returns>Pager<SysAccountInfoEntity></returns>
        Pager<SysAccountInfoEntity> GetSysAccountInfoPageList(SysAccountInfoQuery sysAccountInfoQuery);
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        /// <summary>
        /// 将实体数据存入存储媒介（持久化一个对象）
        /// </summary>
        /// <param name="sysAccountInfo">SysAccountInfoEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        bool AddSysAccountInfo(SysAccountInfoEntity sysAccountInfo);
        
        #endregion 
        
        #region 更新一个持久化对象
        
        /// <summary>
        /// 更新存储媒介中的实体数据
        /// </summary>
        /// <param name="sysAccountInfo">SysAccountInfoEntity实体对象</param>
        /// <returns>true:更新成功 false：更新失败</returns>
        bool UpdateSysAccountInfo(SysAccountInfoEntity sysAccountInfo);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="AdminID"></param>
        /// <param name="NewPwd"></param>
        /// <returns></returns>
        bool UpdatePwd(int AdminID, string NewPwd);

        /// <summary>
        /// 修改登录信息
        /// </summary>
        /// <param name="AdminID"></param>
        /// <param name="NewPwd"></param>
        /// <returns></returns>
        bool UpdateAccountLoginInfo(int AdminID, string IP, string LoginDate);
        
        #endregion 
       
        #region 从存储媒介中删除对象
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="adminID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeleteSysAccountInfo(Int32 adminID);
        
        /// <summary>
        /// 从存储媒介删除实体数据
        /// </summary>
        /// <param name="sysAccountInfo">SysAccountInfoEntity实体对象</param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool DeleteSysAccountInfo(SysAccountInfoEntity sysAccountInfo);
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="adminID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        bool BatchDeleteSysAccountInfo(Int32[] adminID);
        
        #endregion
        
        #endregion 
    }
}






