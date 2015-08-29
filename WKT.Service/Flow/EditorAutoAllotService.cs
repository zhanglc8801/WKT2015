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
    public partial class EditorAutoAllotService:IEditorAutoAllotService
    {
        /// <summary>
        /// 总线接口
        /// </summary>
        private IEditorAutoAllotBusiness editorAutoAllotBusProvider = null;
        /// <summary>
        /// 总线接口
        /// </summary>
        public IEditorAutoAllotBusiness EditorAutoAllotBusProvider
        {
            get
            {
                 if(editorAutoAllotBusProvider == null)
                 {
                      editorAutoAllotBusProvider = new EditorAutoAllotBusiness();//ServiceBusContainer.Instance.Container.Resolve<IEditorAutoAllotBusiness>();
                 }
                 return editorAutoAllotBusProvider;
            }
            set
            {
              editorAutoAllotBusProvider = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public EditorAutoAllotService()
        {
        }
        
        /// <summary>
        /// 得到投稿自动分配配置
        /// </summary>
        /// <param name="autoAllotQuery"></param>
        /// <returns></returns>
        public EditorAutoAllotEntity GetEditorAutoAllot(EditorAutoAllotQuery autoAllotQuery)
        {
            return EditorAutoAllotBusProvider.GetEditorAutoAllot(autoAllotQuery);
        }
        
        
        /// <summary>
        /// 更新投稿自动配置
        /// </summary>
        /// <param name="cSetEntity"></param>
        /// <returns></returns>
        public bool SetAutoAllot(EditorAutoAllotEntity cSetEntity)
        {
            return EditorAutoAllotBusProvider.SetAutoAllot(cSetEntity);
        }

        /// <summary>
        /// 得到投稿自动分配的编辑
        /// </summary>
        /// <param name="autoAllotQuery">指定稿件编号，编辑部ID，学科ID</param>
        /// <returns></returns>
        public AuthorInfoEntity GetAutoAllotEditor(EditorAutoAllotQuery autoAllotQuery)
        {
            return EditorAutoAllotBusProvider.GetAutoAllotEditor(autoAllotQuery);
        }
    }
}
