using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using WKT.Model;
using WKT.Data.SQL;

namespace WKT.DataAccess
{
    /// <summary>
    ///  数据持久化抽象实现类
    ///  本类为生成代码，如果要修改增加方法，建议使用新建部分类（partial）文件，避免代码生成后覆盖
    /// </summary>
    public partial class MenuDataAccess
    {
        private string dbPrividerName = string.Empty;
        private IDbProvider db=null;
        /// <summary>
        /// 数据初始化
        /// </summary>
        public MenuDataAccess()
        {
            db = WKT.Data.SQL.DbProviderFactory.CreateInstance("WKTDB");
            if(db == null)
               throw new Exception("数据库对象初始化失败，请检查数据库连接配置文件");
        }
        
        private static MenuDataAccess _instance = new MenuDataAccess();

        /// <summary>
        ///   数据持久化抽象访问类单例  
        /// </summary>
        public static MenuDataAccess Instance
        {
            get
            {
                return _instance;
            }
        }
        
        #region 组装SQL条件
        
        /// <summary>
        /// 将查询实体转换为Where语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Where语句，不包含Where</returns>
        /// </summary>
        public string MenuQueryToSQLWhere(MenuQuery query)
        {
            StringBuilder sbWhere = new StringBuilder(" JournalID = " + query.JournalID);
            if (query.GroupID > 0)
            {
                sbWhere.Append(" AND GroupID = ").Append(query.GroupID);
            }
            if (query.Status != null)
            {
                sbWhere.Append(" AND [Status] = ").Append(query.Status.Value);
            }
            return sbWhere.ToString();
        }
        /// <summary>
        /// 将查询实体转换为Order语句
        /// <param name="query">查询实体</param>
        /// <returns>获取Order语句，不包含Order</returns>
        /// </summary>
        public string MenuQueryToSQLOrder(MenuQuery query)
        {
            return " SortID ASC, MenuID ASC";
        }
        
        #endregion 组装SQL条件
        
        #region 获取一个实体对象
        
        public MenuEntity GetMenu(Int64 menuID)
        {
            MenuEntity menuEntity=null;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT TOP 1  MenuID,JournalID,PMenuID,MenuName,MenuUrl,IconUrl,SortID,MenuType,GroupID,Status,IsContentMenu,AddDate FROM dbo.Menu WITH(NOLOCK)");
            sqlCommandText.Append(" WHERE  MenuID=@MenuID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@MenuID",DbType.Int64,menuID);
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                menuEntity = MakeMenu(dr);
            }
            return menuEntity;
        }
        
        #endregion
        
        #region 根据条件获取所有实体对象
        
        public List<MenuEntity> GetMenuList()
        {
            List<MenuEntity> menuEntity=new List<MenuEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT  MenuID,JournalID,PMenuID,MenuName,MenuUrl,IconUrl,SortID,MenuType,GroupID,Status,IsContentMenu,AddDate FROM dbo.Menu WITH(NOLOCK)");
            
            DbCommand cmd=db.GetSqlStringCommand(sqlCommandText.ToString());
            
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                menuEntity = MakeMenuList(dr);
            }
            return menuEntity;
        }
        
        public List<MenuEntity> GetMenuList(MenuQuery query)
        {
            List<MenuEntity> list = new List<MenuEntity>();
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("SELECT MenuID,JournalID,PMenuID,MenuName,MenuUrl,IconUrl,SortID,MenuType,GroupID,Status,IsContentMenu,AddDate FROM dbo.Menu WITH(NOLOCK)");
            string whereSQL = MenuQueryToSQLWhere(query);
            string orderBy=MenuQueryToSQLOrder(query);
            if(!string.IsNullOrEmpty(whereSQL)) sqlCommandText.Append(" WHERE " + whereSQL);
            if(!string.IsNullOrEmpty(orderBy)) sqlCommandText.Append(" ORDER BY " + orderBy);
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            using(IDataReader dr = db.ExecuteReader(cmd))
            {
                list = MakeMenuList(dr);
            }
            return list;
        }
       
        #endregion
        
        #region 根据查询条件分页获取对象
        
        /// <summary>
        /// 分页获取符合查询条件的数据
        /// </summary>
        /// <param name="CommonQuery">通用查询对象</param>
        /// <returns>Pager<MenuEntity></returns>
        public Pager<MenuEntity> GetMenuPageList(CommonQuery query)
        {
            int recordCount = 0;
            DataSet ds = db.GetPagingData("Menu", "MenuID,JournalID,PMenuID,MenuName,MenuUrl,IconUrl,SortID,MenuType,GroupID,Status,IsContentMenu,AddDate", query.Order, query.Where, query.CurrentPage, query.PageSize, out recordCount);
            Pager<MenuEntity>  pager = new Pager<MenuEntity>();
            if(ds != null && ds.Tables.Count > 0)
            {
                 pager.ItemList= MakeMenuList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<MenuEntity> GetMenuPageList(QueryBase query)
        {
            int recordCount=0;
            DataSet ds = db.GetPagingData("Menu", "MenuID,JournalID,PMenuID,MenuName,MenuUrl,IconUrl,SortID,MenuType,GroupID,Status,IsContentMenu,AddDate", " MenuID DESC", "", query.CurrentPage, query.PageSize, out recordCount);
            Pager<MenuEntity>  pager=new Pager<MenuEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeMenuList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        public Pager<MenuEntity> GetMenuPageList(MenuQuery query)
        {
            int recordCount=0;
            string whereSQL=MenuQueryToSQLWhere(query);
            string orderBy=MenuQueryToSQLOrder(query);
            DataSet ds = db.GetPagingData("Menu", "MenuID,JournalID,PMenuID,MenuName,MenuUrl,IconUrl,SortID,MenuType,GroupID,Status,IsContentMenu,AddDate", orderBy, whereSQL, query.CurrentPage, query.PageSize, out recordCount);
            Pager<MenuEntity>  pager=new Pager<MenuEntity>();
            if(ds!=null && ds.Tables.Count>0)
            {
                 pager.ItemList= MakeMenuList(ds.Tables[0]);
            }
            pager.CurrentPage=query.CurrentPage;
            pager.PageSize=query.PageSize;
            pager.TotalRecords=recordCount;
            return pager;
        }
        
        #endregion
        
        #region 持久化一个新对象（保存新对象到存储媒介中）
        
        public bool AddMenu(MenuEntity menuEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();

            sqlCommandText.Append("@JournalID");
            sqlCommandText.Append(", @PMenuID");
            sqlCommandText.Append(", @MenuName");
            sqlCommandText.Append(", @MenuUrl");
            sqlCommandText.Append(", @IconUrl");
            sqlCommandText.Append(", @SortID");
            sqlCommandText.Append(", @MenuType");
            sqlCommandText.Append(", @GroupID");

              
            DbCommand cmd = db.GetSqlStringCommand(String.Format("INSERT INTO dbo.Menu ({0}) VALUES ({1})",sqlCommandText.ToString().Replace("@", ""),sqlCommandText.ToString()));
            

            db.AddInParameter(cmd,"@JournalID",DbType.Int64,menuEntity.JournalID);
            db.AddInParameter(cmd,"@PMenuID",DbType.Int32,menuEntity.PMenuID);
            db.AddInParameter(cmd,"@MenuName",DbType.AnsiString,menuEntity.MenuName);
            db.AddInParameter(cmd,"@MenuUrl",DbType.AnsiString,menuEntity.MenuUrl);
            db.AddInParameter(cmd,"@IconUrl",DbType.AnsiString,menuEntity.IconUrl);
            db.AddInParameter(cmd,"@SortID",DbType.Int32,menuEntity.SortID);
            db.AddInParameter(cmd,"@MenuType",DbType.Byte,menuEntity.MenuType);
            db.AddInParameter(cmd,"@GroupID",DbType.Byte,menuEntity.GroupID);

            try
            {
                db.ExecuteNonQuery(cmd);
                flag = true;
            }
            catch(SqlException sqlEx)
            {
                throw sqlEx;
            }
            return flag;
        }
        
        #endregion
        
        #region 更新数据

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="menuEntity"></param>
        /// <returns></returns>
        public bool UpdateStaus(MenuQuery menuQuery)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            if (menuQuery.MenuIDList.Count == 0)
            {
                whereCommandText.Append(" MenuID=@MenuID AND JournalID=@JournalID ");
            }
            else
            {
                whereCommandText.AppendFormat(" MenuID IN ({0}) AND JournalID=@JournalID ",string.Join(",",menuQuery.MenuIDList));
            }
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("Status=@Status");

            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.Menu SET {0} WHERE  {1}", sqlCommandText.ToString(), whereCommandText.ToString()));

            if (menuQuery.MenuIDList.Count == 0)
            {
                db.AddInParameter(cmd, "@MenuID", DbType.Int64, menuQuery.MenuID);
            }
            db.AddInParameter(cmd, "@JournalID", DbType.Int64, menuQuery.JournalID);
            db.AddInParameter(cmd, "@Status", DbType.Byte, menuQuery.Status);

            try
            {
                db.ExecuteNonQuery(cmd);
                flag = true;
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            return flag;
        }
     
        public bool UpdateMenu(MenuEntity menuEntity)
        {
            bool flag = false;
            StringBuilder whereCommandText = new StringBuilder();
            whereCommandText.Append("  MenuID=@MenuID");
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append(" JournalID=@JournalID");
            sqlCommandText.Append(", PMenuID=@PMenuID");
            sqlCommandText.Append(", MenuName=@MenuName");
            sqlCommandText.Append(", MenuUrl=@MenuUrl");
            sqlCommandText.Append(", IconUrl=@IconUrl");
            sqlCommandText.Append(", SortID=@SortID");
            sqlCommandText.Append(", MenuType=@MenuType");
            sqlCommandText.Append(", GroupID=@GroupID");
            sqlCommandText.Append(", IsContentMenu=@IsContentMenu");
            sqlCommandText.Append(", Status=@Status");
            sqlCommandText.Append(", AddDate=@AddDate");
            
            DbCommand cmd = db.GetSqlStringCommand(String.Format("UPDATE dbo.Menu SET {0} WHERE  {1}",sqlCommandText.ToString(),whereCommandText.ToString()));
            
            db.AddInParameter(cmd,"@MenuID",DbType.Int64,menuEntity.MenuID);
            db.AddInParameter(cmd,"@JournalID",DbType.Int64,menuEntity.JournalID);
            db.AddInParameter(cmd,"@PMenuID",DbType.Int32,menuEntity.PMenuID);
            db.AddInParameter(cmd,"@MenuName",DbType.AnsiString,menuEntity.MenuName);
            db.AddInParameter(cmd,"@MenuUrl",DbType.AnsiString,menuEntity.MenuUrl);
            db.AddInParameter(cmd,"@IconUrl",DbType.AnsiString,menuEntity.IconUrl);
            db.AddInParameter(cmd,"@SortID",DbType.Int32,menuEntity.SortID);
            db.AddInParameter(cmd,"@MenuType",DbType.Byte,menuEntity.MenuType);
            db.AddInParameter(cmd,"@GroupID",DbType.Byte,menuEntity.GroupID);
            db.AddInParameter(cmd,"@Status",DbType.Byte,menuEntity.Status);
            db.AddInParameter(cmd, "@IsContentMenu", DbType.Boolean, menuEntity.IsContentMenu);
            db.AddInParameter(cmd,"@AddDate",DbType.DateTime,menuEntity.AddDate);

            try
            {
                db.ExecuteNonQuery(cmd);
                flag = true;
            }
            catch(SqlException sqlEx)
            {
                throw sqlEx;
            }
            return flag;
        }
        
        #endregion
       
        #region 删除对象
        
        #region 删除一个对象
        
        public bool DeleteMenu(MenuEntity menuEntity)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.Menu");
            sqlCommandText.Append(" WHERE  MenuID=@MenuID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            
            db.AddInParameter(cmd,"@MenuID",DbType.Int64,menuEntity.MenuID);
            
            try
            {
                db.ExecuteNonQuery(cmd);
                flag = true;
            }
            catch(SqlException sqlEx)
            {
                throw sqlEx;
            }
            return flag;
        }
        
        public bool DeleteMenu(Int64 menuID)
        {
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("DELETE FROM dbo.Menu");
            sqlCommandText.Append(" WHERE  MenuID=@MenuID");
              
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            db.AddInParameter(cmd,"@MenuID",DbType.Int64,menuID);
            try
            {
                db.ExecuteNonQuery(cmd);
                flag = true;
            }
            catch(SqlException sqlEx)
            {
                throw sqlEx;
            }
            return flag;
        }
        
        #endregion
        
        #region 批量删除
        
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="menuID"></param>
        /// <returns>true:删除成功 false：删除失败</returns>
        public bool BatchDeleteMenu(Int64[] menuID)
        {   
            bool flag = false;
            StringBuilder sqlCommandText = new StringBuilder();
            sqlCommandText.Append("delete from Menu where ");
           
            for(int i=0;i<menuID.Length;i++)
            {
                if(i>0)sqlCommandText.Append(" or ");
                   sqlCommandText.Append("( MenuID=@MenuID"+i+" )");
            }
            
            DbCommand cmd = db.GetSqlStringCommand(sqlCommandText.ToString());
            for(int i=0;i<menuID.Length;i++)
            {
            db.AddInParameter(cmd,"@MenuID"+i,DbType.Int64,menuID[i]);
            }
            try
            {
                db.ExecuteNonQuery(cmd);
                flag = true;
            }
            catch(SqlException sqlEx)
            {
                throw sqlEx;
            }
            
            return flag;
        }
        
        #endregion
        
        #endregion
        
        #region 根据数据组装一个对象
        
        public MenuEntity MakeMenu(IDataReader dr)
        {
            MenuEntity menuEntity = null;
            if(dr.Read())
            {
             menuEntity=new MenuEntity();
             menuEntity.MenuID = (Int64)dr["MenuID"];
             menuEntity.JournalID = (Int64)dr["JournalID"];
             menuEntity.PMenuID = (Int32)dr["PMenuID"];
             menuEntity.MenuName = (String)dr["MenuName"];
             menuEntity.MenuUrl = (String)dr["MenuUrl"];
             menuEntity.IconUrl = (String)dr["IconUrl"];
             menuEntity.SortID = (Int32)dr["SortID"];
             menuEntity.MenuType = (Byte)dr["MenuType"];
             menuEntity.GroupID = (Byte)dr["GroupID"];
             menuEntity.Status = (Byte)dr["Status"];
             menuEntity.IsContentMenu = (Boolean)dr["IsContentMenu"];
             menuEntity.AddDate = (DateTime)dr["AddDate"];
            }
            dr.Close();
            return menuEntity;
        }
        
        public MenuEntity MakeMenu(DataRow dr)
        {
            MenuEntity menuEntity=null;
            if(dr!=null)
            {
                 menuEntity=new MenuEntity();
                 menuEntity.MenuID = (Int64)dr["MenuID"];
                 menuEntity.JournalID = (Int64)dr["JournalID"];
                 menuEntity.PMenuID = (Int32)dr["PMenuID"];
                 menuEntity.MenuName = (String)dr["MenuName"];
                 menuEntity.MenuUrl = (String)dr["MenuUrl"];
                 menuEntity.IconUrl = (String)dr["IconUrl"];
                 menuEntity.SortID = (Int32)dr["SortID"];
                 menuEntity.MenuType = (Byte)dr["MenuType"];
                 menuEntity.GroupID = (Byte)dr["GroupID"];
                 menuEntity.Status = (Byte)dr["Status"];
                 menuEntity.IsContentMenu = (Boolean)dr["IsContentMenu"];
                 menuEntity.AddDate = (DateTime)dr["AddDate"];
            }
            return menuEntity;
        }
        #endregion
        
        #region 根据数据组装一组对象数据
        
        public List<MenuEntity> MakeMenuList(IDataReader dr)
        {
            List<MenuEntity> list=new List<MenuEntity>();
            while (dr.Read())
            {
                MenuEntity menuEntity = new MenuEntity();
                menuEntity.MenuID = (Int64)dr["MenuID"];
                menuEntity.JournalID = (Int64)dr["JournalID"];
                menuEntity.PMenuID = (Int32)dr["PMenuID"];
                menuEntity.MenuName = (String)dr["MenuName"];
                menuEntity.MenuUrl = (String)dr["MenuUrl"];
                menuEntity.IconUrl = (String)dr["IconUrl"];
                menuEntity.SortID = (Int32)dr["SortID"];
                menuEntity.MenuType = (Byte)dr["MenuType"];
                menuEntity.GroupID = (Byte)dr["GroupID"];
                menuEntity.Status = (Byte)dr["Status"];
                menuEntity.IsContentMenu = (Boolean)dr["IsContentMenu"];
                menuEntity.AddDate = (DateTime)dr["AddDate"];
                list.Add(menuEntity);
            }
            dr.Close();
            return list;
        }
        
        
        public List<MenuEntity> MakeMenuList(DataTable dt)
        {
            List<MenuEntity> list=new List<MenuEntity>();
            if(dt!=null)
            {
                for(int i=0;i<dt.Rows.Count;i++)
                {
                   MenuEntity menuEntity=MakeMenu(dt.Rows[i]);
                   list.Add(menuEntity);
                }
            }
            return list;
        }
        
        #endregion
    }
}

