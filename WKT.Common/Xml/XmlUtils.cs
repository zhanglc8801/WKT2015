using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace WKT.Common.Xml
{
    public class XmlUtils
    {
        public static string Serialize(object objectToSerialize)
        {
            MemoryStream mem = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(objectToSerialize.GetType());
            ser.Serialize(mem, objectToSerialize);
            ASCIIEncoding ascii = new ASCIIEncoding();
            return Encoding.UTF8.GetString(mem.ToArray());
        }

        public static object Deserialize(Type typeToDeserialize, string xmlString)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(xmlString);
            MemoryStream mem = new MemoryStream(bytes);
            XmlSerializer ser = new XmlSerializer(typeToDeserialize);
            return ser.Deserialize(mem);
        }

        public static T ConvertFromXml<T>(string XML)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringReader reader = new StringReader(XML);
            T t = (T)serializer.Deserialize(reader);
            return t;
        }
    }
}
