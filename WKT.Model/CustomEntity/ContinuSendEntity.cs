using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WKT.Model
{
    /// <summary>
    /// 稿件续送
    /// </summary>
    [Serializable, DataContract]
    public class ContinuSendEntity
    {

        [DataMember]
        public long JournalID
        {
            get;
            set;
        }

        [DataMember]
        public long CID
        {
            get;
            set;
        }

        [DataMember]
        public long StatusID
        {
            get;
            set;
        }

        [DataMember]
        public long SendUserID
        {
            get;
            set;
        }

        [DataMember]
        public long[] RecArrayID
        {
            get;
            set;
        }
    }
}
