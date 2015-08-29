using System;
using System.Collections.Generic;

namespace Web.Admin
{
    public class SiteGlobalSettings
    {

        public int ID { get; set; }

        public string SiteID { get; set; }

        public byte IsEnableRegActivate { get; set; }

        public byte IsHideEditorInfoForAuthor { get; set; }

        public byte IsHideEditorInfoForExpert { get; set; }

        public byte ShowMoreFlowInfoForAuthor { get; set; }

        public byte ShowHistoryFlowInfoForExpert { get; set; }

        public byte isAutoHandle { get; set; }

        public byte isStatByGroup { get; set; }

        public string NotAccessSearchUsers { get; set; }

    }
}