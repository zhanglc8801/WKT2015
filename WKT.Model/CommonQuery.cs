using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WKT.Model
{
    /// <summary>
    /// 通用查询类，可用于拼装查询条件、返回字段、排序等实现分页查询的实体
    /// </summary>
    public class CommonQuery : QueryBase
    {
        /// <summary>
        /// 查询返回列名 以逗号为分割
        /// </summary>
        public string FieldName
        {
            get;
            set;
        }
        /// <summary>
        /// 查询结果排序 (如：id desc) 可增加多个以逗号为分割 (如：id1 desc,id2 desc)
        /// </summary>
        public string Order
        {
            get;
            set;
        }
        /// <summary>
        /// 查询条件 里面不能包含where  参考SQL where 条件 （如： id>0 and id <10))
        /// </summary>
        public string Where
        {
            get;
            set;
        }
        /// <summary>
        /// 数据表名
        /// </summary>
        public string DataTableName
        {
            get;
            set;
        }
    }
}
