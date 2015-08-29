using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WKT.Common.Security
{
    public class SecurityUtils
    {
        /// <summary>
        /// 对字符串进行SQL安全转化
        /// </summary>
        /// <param name="text">要安全转化的文本</param>
        /// <returns></returns>
        public static string SafeSqlString(string text)
        {
            if (text == null)
                return null;
            string restr = System.Text.RegularExpressions.Regex.Replace(text, "exec", "ｅｘｅｃ", System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            restr = System.Text.RegularExpressions.Regex.Replace(restr, "declare", "ｄｅｃｌａｒｅ", System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            restr = System.Text.RegularExpressions.Regex.Replace(restr, "update", "ｕｐｄａｔｅ", System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            restr = System.Text.RegularExpressions.Regex.Replace(restr, "delete", "ｄｅｌｅｔｅ", System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            restr = System.Text.RegularExpressions.Regex.Replace(restr, "select", "ｓｅｌｅｃｔ", System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            restr = restr.Replace("'", "''");
            return restr;
        }
    }
}
