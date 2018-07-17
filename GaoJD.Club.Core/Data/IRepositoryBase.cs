using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace GaoJD.Club.Core
{

    public interface IRepositoryBase<TEntity> where TEntity : class
    {


        #region  是否存在
        bool IsExists(Expression<Func<TEntity, bool>> predicate);
        #endregion

        #region 查询
        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <returns></returns>
        List<TEntity> GetAll();

        /// <summary>
        /// 获取总数量
        /// </summary>
        /// <returns></returns>
        long GetCount();

        /// <summary>
        /// 根据lambda表达式条件获取总数量
        /// </summary>
        /// <returns></returns>
        long GetCountByQuery(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <returns></returns>
        TEntity GetById(int id, WriteAndRead writeAndRead = WriteAndRead.Read);

        /// <summary>
        /// 根据lambda表达式条件获取单个实体
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        TEntity GetItemByQuery(Expression<Func<TEntity, bool>> predicate, WriteAndRead writeAndRead = WriteAndRead.Read);

        /// <summary>
        /// 根据lambda表达式条件获取实体集合
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        List<TEntity> GetListByQuery(Expression<Func<TEntity, bool>> predicate, bool isDesc = false, Expression<Func<TEntity, object>> orderBy = null);

        /// <summary>
        /// 根据lambda表达式条件获取实体集合
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        List<TEntity> GetListByQueryExtensions(Expression<Func<TEntity, bool>> predicate, bool isDesc = false, string orderBy = null);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="startPage">页码</param>
        /// <param name="pageSize">单页数据数</param>
        /// <param name="rowCount">行数</param>
        /// <param name="where">条件</param>
        /// <param name="order">排序</param>
        /// <returns></returns>
        List<TEntity> GetPagedList(int startPage, int pageSize, out int rowCount, bool isDesc = false, Expression<Func<TEntity, bool>> where = null, Expression<Func<TEntity, object>> order = null);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="startPage">页码</param>
        /// <param name="pageSize">单页数据数</param>
        /// <param name="rowCount">行数</param>
        /// <param name="where">条件</param>
        /// <param name="order">排序</param>
        /// <returns></returns>
        List<TEntity> GetPagedListExtensions(int startPage, int pageSize, out int rowCount, bool isDesc = false, Expression<Func<TEntity, bool>> where = null, string order = null);



        /// <summary>
        /// 执行Sql查询语句,返回受影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        int ExecuteSql(string sql, SqlParameter[] sqlParameters = null, WriteAndRead writeAndRead = WriteAndRead.Read);


        /// <summary>
        /// 执行存储过程，返回受影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        int ExecuteStoredProcedure(string sql, SqlParameter[] sqlParameters = null, WriteAndRead writeAndRead = WriteAndRead.Read);


        /// <summary>
        /// 执行Sql查询语句，返回实体
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        List<TEntity> ExecuteSqlQuery(string sql, SqlParameter[] sqlParameters = null, WriteAndRead writeAndRead = WriteAndRead.Read);


        /// <summary>
        /// 执行存储过程，返回实体
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        List<TEntity> ExecuteStoredProcedureQuery(string sql, SqlParameter[] sqlParameters = null, WriteAndRead writeAndRead = WriteAndRead.Read);



        #endregion

        #region 增删改
        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        TEntity Insert(TEntity entity);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        TEntity Update(TEntity entity);


        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        void Delete(TEntity entity);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id">实体主键</param>
        void Delete(int id);

        /// <summary>
        /// 根据条件删除实体
        /// </summary>
        /// <param name="where">lambda表达式</param>
        void Delete(Expression<Func<TEntity, bool>> where);

        /// <summary>
        /// 批量新增，实体类型默认为数据库表名(*注意：批量新增时实体类中字段的顺序一定要和数据库中列的顺序保持一致)
        /// </summary>
        /// <param name="entities">数据列表</param>
        void AddList(List<TEntity> entities);

        /// <summary>
        /// 批量添加(*注意：批量新增时实体类中字段的顺序一定要和数据库中列的顺序保持一致)
        /// </summary>
        /// <param name="tableName">批量新增的表名</param>
        /// <param name="entities">数据列表</param>
        /// <returns></returns>
        void AddList(string tableName, List<TEntity> entities);


        /// <summary>
        /// 批量添加(*注意：批量新增时DataTable中列的顺序一定要和数据库中列的顺序保持一致)
        /// </summary>
        /// <param name="tableName">批量新增的表名</param>
        /// <param name="dt">数据源</param>
        /// <returns></returns>
        void AddList(string tableName, DataTable dt);

        #endregion

    }
}
