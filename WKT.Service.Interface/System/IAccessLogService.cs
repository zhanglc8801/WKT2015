using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WKT.Model;

namespace WKT.Service.Interface
{
    public interface IAccessLogService
    {
        /// <summary>
        /// 记录访问日志
        /// </summary>
        /// <param name="logEntity"></param>
        void AddAccessLog(AccessLog logEntity);
    }
}
