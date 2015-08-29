using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;

namespace WKT.Service.Interface
{
    public partial interface ITokenService
    {
        /// <summary>
        /// 获取Token实体对象
        /// </summary>
        /// <param name="tokenQuery"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        TokenEntity GetToken(TokenQuery tokenQuery);

        /// <summary>
        /// 新增Token
        /// </summary>
        /// <param name="token">TokenEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        bool AddToken(TokenEntity token);
    }
}






