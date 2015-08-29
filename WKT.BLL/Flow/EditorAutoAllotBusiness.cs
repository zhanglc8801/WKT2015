using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;
using WKT.BLL.Interface;
using WKT.DataAccess;

namespace WKT.BLL
{
    public partial class EditorAutoAllotBusiness : IEditorAutoAllotBusiness
    {
        /// <summary>
        /// 得到投稿自动分配配置
        /// </summary>
        /// <param name="autoAllotQuery"></param>
        /// <returns></returns>
        public EditorAutoAllotEntity GetEditorAutoAllot(EditorAutoAllotQuery autoAllotQuery)
        {
            return EditorAutoAllotDataAccess.Instance.GetEditorAutoAllot(autoAllotQuery);
        }

        /// <summary>
        /// 更新投稿自动配置
        /// </summary>
        /// <param name="cSetEntity"></param>
        /// <returns></returns>
        public bool SetAutoAllot(EditorAutoAllotEntity cSetEntity)
        {
            return EditorAutoAllotDataAccess.Instance.SetAutoAllot(cSetEntity);
        }

        /// <summary>
        /// 得到投稿自动分配的编辑
        /// </summary>
        /// <param name="autoAllotQuery"></param>
        /// <returns></returns>
        public AuthorInfoEntity GetAutoAllotEditor(EditorAutoAllotQuery autoAllotQuery)
        {
            return EditorAutoAllotDataAccess.Instance.GetAutoAllotEditor(autoAllotQuery);
        }
    }
}
