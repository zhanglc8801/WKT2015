using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WKT.Model.FangZ
{
    public class docs
    {
        public docmodel doc
        {
            get;
            set;
        }
    }
    public class docmodel
    {
        public string manuid
        {
            get;
            set;
        }

        public string name
        {
            get;
            set;
        }

        public string title
        {
            get;
            set;
        }

        public string state
        {
            get;
            set;
        }

        public authorentity author
        {
            get;
            set;
        }

        public string editor
        {
            get;
            set;
        }

        public string date
        {
            get;
            set;
        }

        public string url
        {
            get;
            set;
        }
    }

    public class authorentity
    {
        public string chinesename
        {
            get;set;
        }
        public string englishname
        {
            get;set;
        }
    }

}
