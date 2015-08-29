using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WKT.Model;

namespace WKT.Facade.Service.Interface
{
    public interface IAuthorPlatformFacadeAPIService
    {
        #region 作者详细信息
        /// <summary>
        /// 获取作者详细信息实体
        /// </summary>
        /// <param name="loginAuthor"></param>
        /// <returns></returns>
        AuthorDetailEntity GetAuthorDetailModel(AuthorDetailQuery query);

        /// <summary>
        /// 保存作者详细信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ExecResult SaveAuthorDetail(AuthorDetailEntity model);
        #endregion
    }
}
