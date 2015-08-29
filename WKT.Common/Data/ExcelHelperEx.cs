using System;
using System.Collections.Generic;
using System.Text;
using CarlosAg.ExcelXmlWriter;
using System.Data;
using System.Reflection;
using System.Web;

namespace WKT.Common.Data
{
    public class ExcelHelperEx
    {
        /// <summary>
        /// 创建Excel  2010-06-23
        /// </summary>
        /// <param name="sheetName">表格名称</param>
        /// <param name="sheetFields">表头说明</param>
        /// <param name="titleWidth">定义每列宽度</param>
        /// <param name="dataFiles">字段名称--要与表头文字对应</param>
        /// <param name="fomateFiles">字段格式化方式</param>
        /// <param name="tableDatas">数据源</param>
        /// <param name="strSavePath">保存路径(不需要Server.MapPath)</param>
        public static void CreateExcel(string sheetName, string[] sheetFields, int[] titleWidth, string[] dataFiles, string[] fomateFiles, DataTable tableDatas, string strSavePath)
        {
            Workbook newBook = new Workbook();
            Worksheet newSheet = newBook.Worksheets.Add(sheetName);

            //设置表格样式
            WorksheetStyle titleStyle = newBook.Styles.Add("titleStyle");
            titleStyle.Font.Bold = true;

            WorksheetStyle dataStyle1 = newBook.Styles.Add("dataStyle1");
            dataStyle1.Font.Color = "Red";
            WorksheetStyle dataStyle2 = newBook.Styles.Add("dataStyle2");
            dataStyle2.Font.Color = "Green";


            for (int i = 0; i < sheetFields.Length; i++)
            {
                newSheet.Table.Columns.Add(titleWidth[i]);
            }

            //设置表头
            WorksheetRow titleRow = newSheet.Table.Rows.Add();
            foreach (string s in sheetFields)
            {
                titleRow.Cells.Add(s, DataType.String, "titleStyle");
            }

            //设置数据列
            WorksheetRow dataRow = null;

            if (tableDatas.Rows.Count > 0)
            {
                foreach (DataRow row in tableDatas.Rows)
                {
                    dataRow = newSheet.Table.Rows.Add();
                    for (int i = 0; i < dataFiles.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(fomateFiles[i]))
                        {
                            if (fomateFiles[i].ToUpper().IndexOf("BIT") != -1)
                            {
                                dataRow.Cells.Add(Convert.ToBoolean(row[dataFiles[i]]) ? fomateFiles[i].Split(':')[1] : fomateFiles[i].Split(':')[2]);
                                if (row[dataFiles[i]].ToString() == "待处理")
                                    dataRow.Cells[i].StyleID = "dataStyle1";
                                if (row[dataFiles[i]].ToString() == "已处理")
                                    dataRow.Cells[i].StyleID = "dataStyle2";
                            }
                            else
                            {
                                dataRow.Cells.Add(String.Format(fomateFiles[i], row[dataFiles[i]].ToString()));
                                if (row[dataFiles[i]].ToString() == "待处理")
                                    dataRow.Cells[i].StyleID = "dataStyle1";
                                if (row[dataFiles[i]].ToString() == "已处理")
                                    dataRow.Cells[i].StyleID = "dataStyle2";
                            }
                        }
                        else
                        {
                            dataRow.Cells.Add(row[dataFiles[i]].ToString());
                            if (row[dataFiles[i]].ToString() == "待处理")
                                dataRow.Cells[i].StyleID = "dataStyle1";
                            if (row[dataFiles[i]].ToString() == "已处理")
                                dataRow.Cells[i].StyleID = "dataStyle2";
                        }
                    }
                }
            }

            //数据添加完毕，保存Excel
            newBook.Save(System.Web.HttpContext.Current.Server.MapPath(strSavePath));
        }

        public static void CreateExcel<T>(string sheetName, string[] sheetFields, int[] titleWidth, string[] dataFiles, string[] fomateFiles, IList<T> list, string strSavePath)
        {
            Workbook newBook = new Workbook();
            Worksheet newSheet = newBook.Worksheets.Add(sheetName);

            
            //设置表格样式
            WorksheetStyle titleStyle = newBook.Styles.Add("titleStyle");
            titleStyle.Font.Bold = true;
            titleStyle.Interior.Color = "#999999";
            titleStyle.Interior.Pattern = StyleInteriorPattern.Solid;
            titleStyle.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            titleStyle.Alignment.Vertical = StyleVerticalAlignment.Center;

            WorksheetStyle dataStyle1 = newBook.Styles.Add("dataStyle1");
            dataStyle1.Font.Color = "Red";
            WorksheetStyle dataStyle2 = newBook.Styles.Add("dataStyle2");
            dataStyle2.Font.Color = "Green";


            for (int i = 0; i < sheetFields.Length; i++)
            {
                newSheet.Table.Columns.Add(titleWidth[i]);
            }
            

            //设置表头
            WorksheetRow titleRow = newSheet.Table.Rows.Add();
            foreach (string s in sheetFields)
            {
                titleRow.Cells.Add(s, DataType.String, "titleStyle");
            }
            
            //设置数据列
            WorksheetRow dataRow = null;

            DataTable tableDatas = new DataTable();
            tableDatas = ListToDataTable(list);

            if (tableDatas.Rows.Count > 0)
            {
                foreach (DataRow row in tableDatas.Rows)
                {
                    dataRow = newSheet.Table.Rows.Add();
                    for (int i = 0; i < dataFiles.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(fomateFiles[i]))
                            dataRow.Cells.Add(String.Format(fomateFiles[i], row[dataFiles[i]].ToString()));
                        else
                            dataRow.Cells.Add(row[dataFiles[i]].ToString());
                    }
                }
            }

            //newSheet.Options.Selected = true;
            //newSheet.Options.ProtectObjects = false;
            //newSheet.Options.ProtectScenarios = false;
            //newSheet.Options.SplitHorizontal = 1;
            //newSheet.Options.SplitVertical = 1;
            //newSheet.Options.ActivePane = 1;
            //数据添加完毕，保存Excel
            newBook.Save(System.Web.HttpContext.Current.Server.MapPath(strSavePath));

        }


        #region 将泛类型集合List类转换成DataTable
        /// <summary>
        /// 将泛类型集合List类转换成DataTable
        /// </summary>
        /// <param name="list">泛类型集合</param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(IList<T> entitys)
        {
            //检查实体集合不能为空
            if (entitys == null || entitys.Count < 1)
            {
                throw new Exception("需转换的集合为空");
            }
            //取出第一个实体的所有Propertie
            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();

            //生成DataTable的structure
            //生产代码中，应将生成的DataTable结构Cache起来，此处略
            DataTable dt = new DataTable();
            for (int i = 0; i < entityProperties.Length; i++)
            {
                //dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);
                dt.Columns.Add(entityProperties[i].Name);
            }
            //将所有entity添加到DataTable中
            foreach (object entity in entitys)
            {
                //检查所有的的实体都为同一类型
                if (entity.GetType() != entityType)
                {
                    throw new Exception("要转换的集合元素类型不一致");
                }
                object[] entityValues = new object[entityProperties.Length];
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);
                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        } 
        #endregion

        #region 生成随机文件名
        /// <summary>
        /// 生成随机文件名
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="ext"></param>
        /// <returns></returns>
        public static string GetRandomFileName(string fileName, string ext)
        {
            string strFileName;
            Random random = new Random(DateTime.Now.Millisecond);
            string str = random.Next(0x7fffffff).ToString();

            if (!string.IsNullOrEmpty(fileName))
            {
                strFileName = fileName + "_" + str + "." + ext;
            }
            else
            {
                strFileName = str + "." + ext;
            }
            return strFileName;
        } 
        #endregion

    }
}
