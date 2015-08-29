using System;
using System.Collections.Generic;
using System.Text;

using WKT.Model;

namespace WKT.BLL.Interface
{
    public partial interface IEditorAutoAllotBusiness
    {       
        /// <summary>
        /// 得到投稿自动分配配置
        /// </summary>
        /// <param name="autoAllotQuery"></param>
        /// <returns></returns>
        EditorAutoAllotEntity GetEditorAutoAllot(EditorAutoAllotQuery autoAllotQuery);
        
        /// <summary>
        /// 更新投稿自动配置
        /// </summary>
        /// <param name="cSetEntity"></param>
        /// <returns></returns>
        bool SetAutoAllot(EditorAutoAllotEntity cSetEntity);

        /// <summary>
        /// 得到投稿自动分配的编辑
        /// </summary>
        /// <param name="autoAllotQuery"></param>
        /// <returns></returns>
        AuthorInfoEntity GetAutoAllotEditor(EditorAutoAllotQuery autoAllotQuery);
       
    }
}






