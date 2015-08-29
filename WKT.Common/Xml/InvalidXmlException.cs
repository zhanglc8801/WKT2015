using System;
using System.Text;

namespace WKT.Common.Xml
{
    public class InvalidXmlException : Exception
    {
        public InvalidXmlException(string message)
            : base(message)
        {
        }
    }
}
