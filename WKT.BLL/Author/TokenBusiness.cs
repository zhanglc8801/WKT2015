using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;


namespace WKT.BLL
{
    public partial class TokenBusiness : ITokenBusiness
    {
        /// <summary>
        /// 获取令牌实体对象
        /// </summary>
        /// <param name="tokenQuery"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public TokenEntity GetToken(TokenQuery tokenQuery)
        {
            return TokenDataAccess.Instance.GetToken(tokenQuery);
        }
        
        /// <summary>
        /// 新增令牌
        /// </summary>
        /// <param name="token">TokenEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddToken(TokenEntity token)
        {
            return TokenDataAccess.Instance.AddToken(token);
        }
    }
}
