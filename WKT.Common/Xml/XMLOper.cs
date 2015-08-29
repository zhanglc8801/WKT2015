using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Collections;

namespace WKT.Common.Xml
{
    public class XmlHandler
    {
        protected string strXmlFile;
        protected XmlDocument objXmlDoc = new XmlDocument();

        #region XmlHandler构造函数
        /// <summary>
        /// XmlHandler构造函数
        /// </summary>
        public XmlHandler()
        { }

        /// <summary>
        /// XmlHandler构造函数
        /// </summary>
        /// <param name="XmlFile">Xml文件的物理地址</param>
        public XmlHandler(string XmlFile)
        {
            try
            {
                objXmlDoc.Load(XmlFile);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            strXmlFile = XmlFile;
        }
        #endregion

        #region 通过DataSet访问XML
        /// <summary>
        /// 读取通过DataSet访问XML 
        /// </summary>
        /// <param name="path">设置一个参数 string path 获得要读取的XML地址</param>
        /// <returns>返回一个DataSet数据集</returns>
        public DataSet ReadXMLByPath(string path)
        {
            using (DataSet ds = new DataSet())
            {
                ds.ReadXml(path);
                return ds;
            }
        }
        #endregion

        /// <summary>
        /// 获取指定属性的值列表
        /// </summary>
        /// <param name="attName"></param>
        /// <returns></returns>
        public ArrayList GetAttributeList(string noteName, string attName)
        {
            ArrayList attList = new ArrayList();
            XmlNodeList xnlEntity = objXmlDoc.GetElementsByTagName(noteName);
            foreach (XmlNode node in xnlEntity)
            {
                attList.Add(node.Attributes[attName].Value);
            }
            return attList;
        }

        /// <summary>
        /// 读取指定节点的值
        /// </summary>
        /// <param name="XmlPathNode">节点的XPATH</param>
        /// <returns>返回一个DataView</returns>
        public DataSet GetData(string XmlPathNode)
        {
            DataSet ds = new DataSet();
            StringReader read = new StringReader(objXmlDoc.SelectSingleNode(XmlPathNode).OuterXml);
            ds.ReadXml(read);
            return ds;
        }

        /// <summary>
        /// 读取指定节点的值
        /// </summary>
        /// <param name="XmlPathNode">节点的XPATH</param>
        /// <returns></returns>
        public string GetNodeData(string XmlPathNode)
        {
            return objXmlDoc.SelectSingleNode(XmlPathNode).InnerText;
        }

        /// <summary>
        /// 获取指定的节点
        /// </summary>
        /// <param name="XmlPathNode">节点的XPATH</param>
        /// <returns></returns>
        public XmlNode GetPointNode(string XmlPathNode)
        {
            return objXmlDoc.SelectSingleNode(XmlPathNode);
        }

        /// <summary>
        /// 更新指定的节点
        /// </summary>
        /// <param name="XmlPathNode">节点的XPATH</param>
        /// <param name="Content">新内容</param>
        public void Replace(string XmlPathNode, string Content)
        {
            objXmlDoc.SelectSingleNode(XmlPathNode).InnerText = Content;
        }

        /// <summary>
        /// 删除指定的节点
        /// </summary>
        /// <param name="XmlPathNode">节点的XPATH</param>
        public void Delete(string XmlPathNode)
        {
            string mainNode = XmlPathNode.Substring(0, XmlPathNode.LastIndexOf("/"));
            objXmlDoc.SelectSingleNode(mainNode).RemoveChild(objXmlDoc.SelectSingleNode(XmlPathNode));
        }

        /// <summary>
        /// 插入一节点和此节点的子节点
        /// </summary>
        /// <param name="MainNode"></param>
        /// <param name="ChildNode"></param>
        /// <param name="Element"></param>
        /// <param name="Content"></param>
        public void InsertNode(string MainNode, string ChildNode, string Element, string Content)
        {
            XmlNode objRootNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement objChildNode = objXmlDoc.CreateElement(ChildNode);
            objRootNode.AppendChild(objChildNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.InnerText = Content;
            objChildNode.AppendChild(objElement);
        }
        /// <summary>
        /// 插入一个节点，带一属性
        /// </summary>
        /// <param name="MainNode">要添加节点的节点XPATH</param>
        /// <param name="Element">要添加的节点名称</param>
        /// <param name="Attrib">添加的节点属性名称</param>
        /// <param name="AttribContent">添加的节点属性值</param>
        /// <param name="Content">添加的节点值</param>
        public void InsertElement(string MainNode, string Element, string Attrib, string AttribContent, string Content)
        {
            XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.SetAttribute(Attrib, AttribContent);
            objElement.InnerText = Content;
            objNode.AppendChild(objElement);
        }

        /// <summary>
        /// 插入一个节点，不带属性
        /// </summary>
        /// <param name="MainNode">要添加节点的节点XPATH</param>
        /// <param name="Element">要添加的节点名称</param>
        /// <param name="Content">要添加节点内容</param>
        public void InsertElement(string MainNode, string Element, string Content)
        {

            XmlNode objNode = objXmlDoc.SelectSingleNode(MainNode);
            XmlElement objElement = objXmlDoc.CreateElement(Element);
            objElement.InnerText = Content;
            objNode.AppendChild(objElement);
        }

        # region 使用XPathExpression、XPathNavigator排序

        /// <summary>
        /// XML排序
        /// </summary>
        /// <param name="sortXpath">按照排序的节点XPATH</param>
        /// <param name="xsOrder">排序方式 升序还是降序</param>
        /// <param name="xdtType">排序的数据类型 数字型还是字符型</param>
        /// <returns></returns>
        //public string SortXml(string sortXpath, XmlSortOrder xsOrder,XmlDataType xdtType)
        //{
        //    // 保存结果集
        //    StringBuilder sbSortResult = new StringBuilder();
        //    // 载入xml文件
        //    XPathDocument doc = new XPathDocument(strXmlFile);
        //    // 创建XML游标模型对象
        //    XPathNavigator nav = doc.CreateNavigator();
        //    // 在指定的XPATH上创建表达式对象
        //    XPathExpression exp = nav.Compile(sortXpath);
        //    // 添加排序属性
        //    exp.AddSort("text()", xsOrder, XmlCaseOrder.None, "zh-cn", xdtType);
        //    // 执行查询结果
        //    XPathNodeIterator nodeIter = nav.Select(exp);

        //    while (nodeIter.MoveNext())
        //    {
        //        sbSortResult.Append(nodeIter.Current.Value + "<br />");
        //    }
        //    return sbSortResult.ToString();
        //}

        # endregion

        /// <summary>
        /// 取XML中的第几条记录
        /// </summary>
        /// <param name="pageSize">每页显示的记录条数</param>
        /// <param name="pageIndex">页索引</param>
        /// <returns></returns>
        public DataSet GetRecord(int pageSize, int pageIndex)
        {
            // 开始记录位置
            int sartIndex = pageSize * (pageIndex - 1);
            // 终止记录位置
            int endIndex = pageSize * pageIndex;
            // 返回结果集
            DataSet dsResult = new DataSet();
            // 临时DataSet，用来读取XML内容
            DataSet dsTemp = new DataSet();
            // 读取XML内容
            dsTemp.ReadXml(strXmlFile);

            if (endIndex > dsTemp.Tables[0].Rows.Count)
            {
                endIndex = dsTemp.Tables[0].Rows.Count;
            }

            if (dsTemp.Tables.Count > 0)
            {
                if (dsTemp.Tables[0].Rows.Count > 0)
                {
                    // 克隆XML数据结构到结果集中
                    dsResult = dsTemp.Clone();
                    // 循环把要读取的记录添加到结果集中
                    for (int i = sartIndex; i < endIndex; i++)
                    {
                        dsResult.Tables[0].Rows.Add(dsTemp.Tables[0].Rows[i].ItemArray);
                    }
                }
            }

            return dsResult;
        }

        /// <summary>
        /// 取XML中的第几条记录,指定排序字段
        /// </summary>
        /// <param name="pageSize">每页显示的记录条数</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="orderbyFiled">排序字段</param>
        /// <param name="sortOrder">排序顺序 true：倒叙 false：正序</param>
        /// <returns></returns>
        public DataView GetRecord(int pageSize, int pageIndex, string orderbyFiled, bool sortOrder)
        {
            // 开始记录位置
            int sartIndex = pageSize * (pageIndex - 1);
            // 终止记录位置
            int endIndex = pageSize * pageIndex;
            // 返回结果集
            DataSet dsResult = new DataSet();
            // 临时DataSet，用来读取XML内容
            DataSet dsTemp = new DataSet();
            // 读取XML内容
            dsTemp.ReadXml(strXmlFile);

            if (endIndex > dsTemp.Tables[0].Rows.Count)
            {
                endIndex = dsTemp.Tables[0].Rows.Count;
            }

            // 克隆XML数据结构到结果集中
            dsResult = dsTemp.Clone();
            // 循环把要读取的记录添加到结果集中
            for (int i = sartIndex; i < endIndex; i++)
            {
                dsResult.Tables[0].Rows.Add(dsTemp.Tables[0].Rows[i].ItemArray);
            }

            DataView dvResult = dsResult.Tables[0].DefaultView;
            if (sortOrder)
            {
                dvResult.Sort = orderbyFiled + " desc";
            }
            else
            {
                dvResult.Sort = orderbyFiled + " asc";
            }

            return dvResult;
        }

        /// <summary>
        /// 保存修改
        /// </summary>
        public void Save()
        {
            try
            {
                objXmlDoc.Save(strXmlFile);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            objXmlDoc = null;
        }
    }
}
