using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WKT.Model
{
    [DataContract]
    public class AuthorStatEntity : ObjectBase
    {
        [DataMember]
        public string StatItem
        {
            get;
            set;
        }

        [DataMember]
        public int StatID
        {
            get;
            set;
        }

        [DataMember]
        public int Count
        {
            get;
            set;
        }
    }
}
