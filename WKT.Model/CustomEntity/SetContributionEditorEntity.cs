using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WKT.Model
{
    [DataContract, Serializable]
    public class SetContributionEditorEntity
    {
        /// <summary>
        /// 编辑部ID
        /// </summary>
        [DataMember]
        public long JournalID
        {
            get;
            set;
        }

        /// <summary>
        /// 责任编辑ID
        /// </summary>
        [DataMember]
        public long AuthorID
        {
            get;
            set;
        }

        /// <summary>
        /// 稿件ID
        /// </summary>
        [DataMember]
        public long[] CIDArray
        {
            get;
            set;
        }
    }
}
