using Dapper;
using GaoJD.Club.Core.Utility;
using GaoJD.Club.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace GaoJD.Club.Core
{
    public class SqlserverProvider : IDbProvider, IDisposable
    {

        private string m_connectionString;
        public SqlserverProvider(string connection)
        {
            m_connectionString = connection;
        }

        #region 是否存在

        public bool IsExists<T>(Expression<Func<T, bool>> condition) where T : class
        {
            DynamicParameters paramsValue = null;
            int paramsLength;
            string where = SqlServerUtilities.GetWhereParameterized<T>(condition, ref paramsValue, out paramsLength);
            string sql = this.ToParameterizedSqlString<T>(DbOperationType.EXISTS, ref paramsValue);
            if (!string.IsNullOrEmpty(where))
            {
                sql += " WHERE " + where;
            }
            using (SqlConnection connection = new SqlConnection(m_connectionString))
            {
                object ret = connection.ExecuteScalar(sql, paramsValue);
                return ret != null;
            }
        }

        #endregion


        #region 查询
        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetAll<T>() where T : class
        {
            DynamicParameters paramsValue = null;
            string sql = this.ToParameterizedSqlString<T>(DbOperationType.SELECT, ref paramsValue);
            using (SqlConnection connection = new SqlConnection(m_connectionString))
            {
                return connection.Query<T>(sql).ToList();
            }
        }
        /// <summary>
        /// 获取总数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public long GetCount<T>() where T : class
        {
            DynamicParameters paramsValue = null;
            string sql = this.ToParameterizedSqlString<T>(DbOperationType.COUNT, ref paramsValue);

            using (SqlConnection connection = new SqlConnection(m_connectionString))
            {
                return connection.Query<int>(sql).First();
            }
        }

        /// <summary>
        /// 根据lambda表达式条件获取总数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <returns></returns>
        public long GetCountByQuery<T>(Expression<Func<T, bool>> condition) where T : class
        {
            DynamicParameters paramsValue = null;
            int paramsLength;
            string where = SqlServerUtilities.GetWhereParameterized<T>(condition, ref paramsValue, out paramsLength);
            string sql = this.ToParameterizedSqlString<T>(DbOperationType.COUNT, ref paramsValue);
            if (!string.IsNullOrEmpty(where))
            {
                sql += " WHERE " + where;
            }
            using (SqlConnection connection = new SqlConnection(m_connectionString))
            {
                return connection.Query<int>(sql, paramsValue).First();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityIds"></param>
        /// <returns></returns>
        public T GetById<T>(params object[] entityIds) where T : class
        {
            List<MapperPropertyInfo> keys = SqlServerUtilities.GetPrimaryKeys<T>();
            DynamicParameters paramsValue;
            string where = this.GetPrimaryKeysParameterizedSqlString(keys, out paramsValue);
            for (int i = 0; i < entityIds.Length; i++)
            {
                paramsValue.Add("@p" + i.ToString(), entityIds[i]);
            }
            string sql = this.ToParameterizedSqlString<T>(DbOperationType.SELECT, ref paramsValue);
            if (!string.IsNullOrEmpty(where))
            {
                sql += " WHERE " + where;
            }
            using (SqlConnection connection = new SqlConnection(m_connectionString))
            {
                return connection.Query<T>(sql, paramsValue).SingleOrDefault();
            }
        }
        /// <summary>
        /// 根据lambda表达式条件获取单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <returns></returns>
        public T GetItemByQuery<T>(Expression<Func<T, bool>> condition) where T : class
        {
            DynamicParameters paramsValue = null;
            int paramsLength;
            string where = SqlServerUtilities.GetWhereParameterized<T>(condition, ref paramsValue, out paramsLength);
            string sql = this.ToParameterizedSqlString<T>(DbOperationType.SELECT, ref paramsValue);
            if (!string.IsNullOrEmpty(where))
            {
                sql += " WHERE " + where;
            }
            using (SqlConnection connection = new SqlConnection(m_connectionString))
            {
                return connection.Query<T>(sql, paramsValue).FirstOrDefault();
            }
        }

        /// <summary>
        /// 根据lambda表达式条件获取实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <param name="orderBy"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        public List<T> GetListByQueryExtensions<T>(Expression<Func<T, bool>> predicate, bool isDesc = false, string orderBy = null) where T : class
        {
            DynamicParameters paramsValue = null;
            int paramsLength;
            string where = SqlServerUtilities.GetWhereParameterized<T>(predicate, ref paramsValue, out paramsLength);
            string sql = this.ToParameterizedSqlString<T>(DbOperationType.SELECT, ref paramsValue);
            if (!string.IsNullOrEmpty(where))
            {
                sql += " WHERE " + where;
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                sql += " ORDER BY " + orderBy;
                if (isDesc)
                {
                    sql += " DESC";
                }
            }
            using (SqlConnection connection = new SqlConnection(m_connectionString))
            {
                return connection.Query<T>(sql, paramsValue).ToList();
            }
        }


        /// <summary>
        /// 根据lambda表达式条件获取实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition"></param>
        /// <param name="orderBy"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        public List<T> GetListByQuery<T>(Expression<Func<T, bool>> predicate, bool isDesc = false, Expression<Func<T, object>> orderBy = null) where T : class
        {
            string order = string.Empty;
            if (orderBy != null)
            {
                order = orderBy.ToMSSqlString();
            }
            return this.GetListByQueryExtensions<T>(predicate, isDesc, order);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="startPage">页码</param>
        /// <param name="pageSize">单页数据数</param>
        /// <param name="rowCount">行数</param>
        /// <param name="where">条件</param>
        /// <param name="order">排序</param>
        /// <returns></returns>
        public List<T> GetPagedList<T>(int offset, int limit, out int total, bool isDesc = false, Expression<Func<T, bool>> condition = null, Expression<Func<T, object>> orderBy = null) where T : class
        {
            string order = string.Empty;
            if (orderBy != null)
            {
                order = orderBy.ToMSSqlString();
            }
            return this.GetPagedListExtensions<T>(offset, limit, out total, isDesc, condition, order);
        }


        public List<T> GetPagedListExtensions<T>(int offset, int limit, out int total, bool isDesc = false, Expression<Func<T, bool>> condition = null, string orderBy = null)
          where T : class
        {
            MapperPropertyInfo[] mappers = SqlServerUtilities.GetMapperPropertyInfo<T>(null);
            //fields
            string[] fields = SqlServerUtilities.GetFields(mappers);
            //tableName
            string tableName = "dbo.[" + typeof(T).Name + "]";
            //orderBy
            string order = string.IsNullOrEmpty(orderBy)
                ? string.Format("[{0}] ASC", mappers.First().Name)
                : orderBy.IndexOf("[") > -1 ? string.Format("{0} {1}", orderBy, (isDesc ? "DESC" : "ASC")) : string.Format("[{0}] {1}", orderBy, (isDesc ? "DESC" : "ASC"));
            //分页sql语句
            string sql = "SELECT * FROM ( SELECT ROW_NUMBER() OVER ( ORDER BY " + order + " ) AS rownumber ,";
            sql += fields.ToString(p => "[" + p + "]");
            sql += " FROM " + tableName;
            //where条件
            string where = condition.ToMSSqlString();
            if (!string.IsNullOrEmpty(where))
            {
                sql += " WHERE " + where;
            }
            sql += " ) tmp";
            sql += " WHERE tmp.rownumber BETWEEN ( " + (offset + 1).ToString() + " ) AND ( " + (offset + limit).ToString() + " );";
            //查询total
            string sqlTotal = "SELECT COUNT(*) FROM " + tableName;
            if (!string.IsNullOrEmpty(where))
            {
                sqlTotal += " WHERE " + where;
            }
            sqlTotal += ";";
            using (SqlConnection connection = new SqlConnection(m_connectionString))
            {
                List<T> list = connection.Query<T>(sql).ToList();
                total = connection.ExecuteScalar<int>(sqlTotal);
                return list;
            }
        }



        public int ExecuteSql(string sql, SqlParameter[] parms)
        {
            using (SqlConnection connection = new SqlConnection(m_connectionString))
            {
                DynamicParameters paramsValue = null;
                if (parms != null)
                {
                    paramsValue = CommonExtensions.ConvertToDynamicParameters(parms);
                }
                int result = connection.Execute(sql, paramsValue, null, null, null);
                if (parms != null)
                {
                    CommonExtensions.ConvertToSqlParameters(paramsValue, ref parms);
                }
                return result;
            }
        }

        public List<T> ExecuteSqlQuery<T>(string sql, SqlParameter[] parms) where T : class
        {
            using (SqlConnection connection = new SqlConnection(m_connectionString))
            {
                DynamicParameters paramsValue = null;
                if (parms != null)
                {
                    paramsValue = CommonExtensions.ConvertToDynamicParameters(parms);
                }
                IEnumerable<T> result = connection.Query<T>(sql, paramsValue, null, true, null, null);
                if (parms != null)
                {
                    CommonExtensions.ConvertToSqlParameters(paramsValue, ref parms);
                }
                return result.ToList();
            }
        }

        public int ExecuteStoredProcedure(string procedureName, SqlParameter[] parms)
        {
            using (SqlConnection connection = new SqlConnection(m_connectionString))
            {
                DynamicParameters paramsValue = null;
                if (parms != null)
                {
                    paramsValue = CommonExtensions.ConvertToDynamicParameters(parms);
                }

                int result = connection.Execute(procedureName, paramsValue, null, null, CommandType.StoredProcedure);
                if (parms != null)
                {
                    CommonExtensions.ConvertToSqlParameters(paramsValue, ref parms);
                }
                return result;
            }
        }

        public List<T> ExecuteStoredProcedureQuery<T>(string procedureName, SqlParameter[] parms) where T : class
        {
            using (SqlConnection connection = new SqlConnection(m_connectionString))
            {
                DynamicParameters paramsValue = null;
                if (parms != null)
                {
                    paramsValue = CommonExtensions.ConvertToDynamicParameters(parms);
                }
                IEnumerable<T> result = connection.Query<T>(procedureName, paramsValue, null, true, null, CommandType.StoredProcedure);
                if (parms != null)
                {
                    CommonExtensions.ConvertToSqlParameters(paramsValue, ref parms);
                }
                return result.ToList();
            }
        }
        #endregion

        #region 增删改
        public T Insert<T>(T entity) where T : class
        {
            DynamicParameters paramsValue = null;
            string sql = this.ToParameterizedSqlString<T>(DbOperationType.INSERT, entity, ref paramsValue) + ";SELECT @@IDENTITY;";
            using (SqlConnection connection = new SqlConnection(m_connectionString))
            {
                object ret = connection.ExecuteScalar(sql, paramsValue);
                if (ret != null)
                {//如果返回值不是null,设置自增键的值
                    SqlServerUtilities.SetIdentityValue<T>(entity, (IConvertible)ret);
                }
            }
            return entity;
        }


        public void Delete<T>(T entity) where T : class
        {
            DynamicParameters paramsValue = null;
            List<MapperPropertyInfo> keys = SqlServerUtilities.GetPrimaryKeys<T>(entity);
            string where = this.GetPrimaryKeysParameterizedSqlString(keys, out paramsValue);
            string sql = this.ToParameterizedSqlString<T>(DbOperationType.DELETE, ref paramsValue);
            if (!string.IsNullOrEmpty(where))
            {
                sql += " WHERE " + where;
            }
            using (SqlConnection connection = new SqlConnection(m_connectionString))
            {
                connection.Execute(sql, paramsValue);
            }
        }


        public void Delete<T>(Expression<Func<T, bool>> condition) where T : class
        {
            DynamicParameters paramsValue = null;
            int paramsLength;
            string where = SqlServerUtilities.GetWhereParameterized<T>(condition, ref paramsValue, out paramsLength);
            string sql = this.ToParameterizedSqlString<T>(DbOperationType.DELETE, ref paramsValue);
            if (!string.IsNullOrEmpty(where))
            {
                sql += " WHERE " + where;
            }
            using (SqlConnection connection = new SqlConnection(m_connectionString))
            {
                connection.Execute(sql, paramsValue);
            }
        }


        public void Delete<T>(params object[] entityIds) where T : class
        {
            List<MapperPropertyInfo> keys = SqlServerUtilities.GetPrimaryKeys<T>();
            DynamicParameters paramsValue;
            string where = this.GetPrimaryKeysParameterizedSqlString(keys, out paramsValue);
            for (int i = 0; i < entityIds.Length; i++)
            {
                paramsValue.Add("@p" + i.ToString(), entityIds[i]);
            }
            string sql = this.ToParameterizedSqlString<T>(DbOperationType.DELETE, ref paramsValue);
            if (!string.IsNullOrEmpty(where))
            {
                sql += " WHERE " + where;
            }
            using (SqlConnection connection = new SqlConnection(m_connectionString))
            {
                connection.Execute(sql, paramsValue);
            }
        }

        public T Update<T>(T entity) where T : class
        {
            List<MapperPropertyInfo> keys = SqlServerUtilities.GetPrimaryKeys<T>(entity);
            DynamicParameters paramsValue;
            string where = this.GetPrimaryKeysParameterizedSqlString(keys, out paramsValue);
            string sql = this.ToParameterizedSqlString<T>(DbOperationType.UPDATE, entity, keys.Count, ref paramsValue);
            if (!string.IsNullOrEmpty(where))
            {
                sql += " WHERE " + where;
            }
            using (SqlConnection connection = new SqlConnection(m_connectionString))
            {
                connection.Execute(sql, paramsValue);
            }
            return entity;
        }

        /// <summary>
        /// 批量新增，实体类型默认为数据库表名(*注意：批量新增时实体类中字段的顺序一定要和数据库中列的顺序保持一致)
        /// </summary>
        /// <param name="entities">数据列表</param>
        public void AddList<T>(List<T> entities) where T : class
        {
            Utils.CheckNotEmpty<List<T>>(entities, "entities");
            Type t = typeof(T);
            string tableName = "dbo.[" + t.Name + "]";
            this.AddList<T>(tableName, entities);
        }

        /// <summary>
        /// 批量添加(*注意：批量新增时实体类中字段的顺序一定要和数据库中列的顺序保持一致)
        /// </summary>
        /// <param name="tableName">批量新增的表名</param>
        /// <param name="entities">数据列表</param>
        /// <returns></returns>
        public void AddList<T>(string tableName, List<T> entities) where T : class
        {
            Utils.CheckNotEmpty<string>(tableName, "tableName");
            Utils.CheckNotEmpty<List<T>>(entities, "entities");
            DataTable dt = ConvertHelper.ConvertListToDT<T>(entities);
            this.AddList(tableName, dt);
        }

        /// <summary>
        /// 批量添加(*注意：批量新增时DataTable中列的顺序一定要和数据库中列的顺序保持一致)
        /// </summary>
        /// <param name="tableName">批量新增的表名</param>
        /// <param name="dt">数据源</param>
        /// <returns></returns>
        public void AddList(string tableName, DataTable dt)
        {
            SqlServerUtilities.AddList(m_connectionString, tableName, dt);
        }

        #endregion




        #region Private Method
        /// <summary>
        /// 获取主键的参数化Sql语句
        /// </summary>
        /// <param name="keys">主键列表</param>
        /// <param name="parms">如果主键有值，返回参数列表</param>
        /// <returns></returns>
        private string GetPrimaryKeysParameterizedSqlString(List<MapperPropertyInfo> keys, out DynamicParameters parms)
        {
            SqlServerUtilities.CheckNotEmpty(keys, "keys");
            parms = new DynamicParameters();
            string sql = string.Empty;
            for (int i = 0; i < keys.Count; i++)
            {
                if (i == (keys.Count - 1))
                {
                    sql += string.Format("{0}=@p{1} ", "[" + keys[i].Name + "]", i);
                }
                else
                {
                    sql += string.Format("{0}=@p{1} AND ", "[" + keys[i].Name + "]", i);
                }
                if (keys[i].Value != null)
                {
                    parms.Add("@p" + i.ToString(), keys[i].Value);
                }
            }
            return sql;
        }
        /// <summary>
        /// 通过operationType将实体对象转换成参数化的Sql语句
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="operationType">数据库操作类型</param>
        /// <param name="dynamicParameters">参数列表(INSERT,UPDATE,DELETE,SELECT时使用)</param>
        /// <returns></returns>
        private string ToParameterizedSqlString<T>(DbOperationType operationType, ref DynamicParameters dynamicParameters)
        where T : class
        {
            return ToParameterizedSqlString<T>(operationType, null, 0, ref dynamicParameters);
        }
        /// <summary>
        /// 通过operationType将实体对象转换成参数化的Sql语句
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="operationType">数据库操作类型</param>
        /// <param name="entity">实体对象(INSERT,UPDATE时使用)</param>
        /// <param name="dynamicParameters">参数列表(INSERT,UPDATE,DELETE,SELECT时使用)</param>
        /// <returns></returns>
        private string ToParameterizedSqlString<T>(DbOperationType operationType, T entity, ref DynamicParameters dynamicParameters)
            where T : class
        {
            return ToParameterizedSqlString<T>(operationType, entity, 0, ref dynamicParameters);
        }
        /// <summary>
        /// 通过operationType将实体对象转换成参数化的Sql语句
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="operationType">数据库操作类型</param>
        /// <param name="entity">实体对象(INSERT,UPDATE时使用)</param>
        /// <param name="paramsStartIndex">参数化Sql开始的索引</param>
        /// <param name="dynamicParameters">参数列表(INSERT,UPDATE,DELETE,SELECT时使用)</param>
        /// <returns></returns>
        private string ToParameterizedSqlString<T>(DbOperationType operationType, T entity, int paramsStartIndex, ref DynamicParameters dynamicParameters)
            where T : class
        {
            Type t = typeof(T);
            MapperPropertyInfo[] mappers = SqlServerUtilities.GetMapperPropertyInfo<T>(entity);
            string[] fields = new string[0];

            string[] parameterized = new string[0];
            string sql = string.Empty;
            switch (operationType)
            {
                case DbOperationType.INSERT:
                    parameterized = SqlServerUtilities.GetParameterized(mappers, paramsStartIndex, ref dynamicParameters, out fields);
                    sql = string.Format("INSERT INTO dbo.[{0}] ({1}) VALUES ({2})",
                               t.Name,
                               fields.ToString(p => "[" + p + "]"),
                               parameterized.ToString(null)
                               );
                    break;
                case DbOperationType.UPDATE:
                    parameterized = SqlServerUtilities.GetParameterized(mappers, paramsStartIndex, ref dynamicParameters, out fields);
                    sql = string.Format("UPDATE dbo.[{0}] SET {1}",
                        t.Name,
                        fields.Combine(parameterized, (p, q) => "[" + p + "]" + "=" + q).ToString(null)
                        );
                    break;
                case DbOperationType.DELETE:
                    sql = string.Format("DELETE FROM dbo.[{0}]", t.Name);
                    break;
                case DbOperationType.SELECT:
                    fields = SqlServerUtilities.GetFields(mappers);
                    sql = string.Format("SELECT {0} FROM dbo.[{1}] (NOLOCK)",
                        fields.ToString(p => "[" + p + "]"),
                        t.Name
                        );
                    break;
                case DbOperationType.COUNT:
                    sql = string.Format("SELECT COUNT(*) FROM dbo.[{0}] (NOLOCK)", t.Name);
                    break;
                case DbOperationType.EXISTS:
                    sql = string.Format("SELECT 1 FROM dbo.[{0}] (NOLOCK)", t.Name);
                    break;
            }
            return sql;
        }
        #endregion

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
