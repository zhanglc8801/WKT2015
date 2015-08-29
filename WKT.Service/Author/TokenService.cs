using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.Unity;

using WKT.Model;
using WKT.BLL;
using WKT.BLL.Interface;
using WKT.Service.Interface;

namespace WKT.Service
{
    public partial class TokenService : ITokenService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private ITokenBusiness tokenBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public ITokenBusiness TokenBusProvider
        {
            get
            {
                 if(tokenBusProvider == null)
                 {
                      tokenBusProvider = new TokenBusiness();//ServiceBusContainer.Instance.Container.Resolve<ITokenBusiness>();
                 }
                 return tokenBusProvider;
            }
            set
            {
              tokenBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public TokenService()
        {
        }
        
        /// <summary>
        /// 获取一个实体对象
        /// </summary>
        /// <param name="tokenQuery"></param>
        /// <returns>获取一个实体，如果数据不存在返回Null</returns>
        public TokenEntity GetToken(TokenQuery tokenQuery)
        {
            return TokenBusProvider.GetToken(tokenQuery);
        }
        

        /// <summary>
        /// 新增令牌
        /// </summary>
        /// <param name="token">TokenEntity实体对象</param>
        /// <returns>true:存储成功 false：存储失败</returns>
        public bool AddToken(TokenEntity token)
        {
            return TokenBusProvider.AddToken(token);
        }
     
    }
}
