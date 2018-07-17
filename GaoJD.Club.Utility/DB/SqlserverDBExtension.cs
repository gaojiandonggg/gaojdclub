using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace GaoJD.Club.Utility
{
    public static class SqlserverDBExtension
    {
        /// <summary>
        /// DbSet拓展，批量添加，表名默认为实体名
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="entities"></param>
        /// <param name="dbConnection"></param>
        public static void AddList<TEntity>(this DbSet<TEntity> dbSet, List<TEntity> entities, DbConnection dbConnection) where TEntity : class
        {
            var t = typeof(TEntity);
            string tableName = "dbo." + t.Name;
            dbSet.AddList<TEntity>(tableName, entities, dbConnection);
        }
        /// <summary>
        ///  DbSet拓展，批量添加，表名默认为实体名
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="tableName"></param>
        /// <param name="entities"></param>
        /// <param name="dbConnection"></param>
        public static void AddList<TEntity>(this DbSet<TEntity> dbSet, string tableName, List<TEntity> entities, DbConnection dbConnection) where TEntity : class
        {
            DataTable dt = ConvertHelper.ConvertListToDT<TEntity>(entities);
            dbSet.AddList<TEntity>(tableName, dt, dbConnection);
        }
        /// <summary>
        ///  DbSet拓展，批量添加，表名默认为实体名
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="tableName"></param>
        /// <param name="dt"></param>
        /// <param name="dbConnection"></param>
        public static void AddList<TEntity>(this DbSet<TEntity> dbSet, string tableName, DataTable dt, DbConnection dbConnection) where TEntity : class
        {
            if (dt.Rows.Count == 0) return;
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(dbConnection as SqlConnection))
            {
                bulkCopy.DestinationTableName = tableName;
                dbConnection.Open();
                bulkCopy.WriteToServer(dt);
                dbConnection.Close();
            }
        }

    }
}
