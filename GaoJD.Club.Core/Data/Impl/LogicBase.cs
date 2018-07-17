using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GaoJD.Club.Core
{

    public class LogicBase<TEntity> : ILogicBase<TEntity> where TEntity : class
    {

        public IRepositoryBase<TEntity> _dalBase;

        public LogicBase(IRepositoryBase<TEntity> dalBase)
        {
            this._dalBase = dalBase;
        }


        #region  是否存在
        /// <summary>
        /// 根据条件查询是否存在
        /// <param name="condition">查询条件</param>
        /// </summary>
        /// <returns></returns>
        public bool IsExists(Expression<Func<TEntity, bool>> predicate)
        {
            return _dalBase.IsExists(predicate);
        }
        #endregion

        #region  查询
        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <returns></returns>
        public List<TEntity> GetAll()
        {
            return _dalBase.GetAll();
        }

        /// <summary>
        /// 获取总数量
        /// </summary>
        /// <returns></returns>
        public long GetCount()
        {
            return _dalBase.GetCount();
        }


        /// <summary>
        /// 根据lambda表达式条件获取总数量
        /// </summary>
        /// <returns></returns>
        public long GetCountByQuery(Expression<Func<TEntity, bool>> predicate)
        {
            return _dalBase.GetCountByQuery(predicate);
        }


        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <returns></returns>
        public TEntity GetById(int id, WriteAndRead writeAndRead = WriteAndRead.Read)
        {
            return _dalBase.GetById(id);
        }

        /// <summary>
        /// 根据lambda表达式条件获取单个实体
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        public TEntity GetItemByQuery(Expression<Func<TEntity, bool>> predicate, WriteAndRead writeAndRead = WriteAndRead.Read)
        {
            return _dalBase.GetItemByQuery(predicate);
        }

        /// <summary>
        /// 根据lambda表达式条件获取实体集合
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        public List<TEntity> GetListByQuery(Expression<Func<TEntity, bool>> predicate, bool isDesc = false, Expression<Func<TEntity, object>> orderBy = null)
        {
            return _dalBase.GetListByQuery(predicate, isDesc, orderBy);
        }


        /// <summary>
        /// 根据lambda表达式条件获取实体集合
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="isDesc"></param>
        /// <returns></returns>
        public List<TEntity> GetListByQueryExtensions(Expression<Func<TEntity, bool>> predicate, bool isDesc = false, string orderBy = null)
        {
            return _dalBase.GetListByQueryExtensions(predicate, isDesc, orderBy);
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
        public List<TEntity> GetPagedList(int startPage, int pageSize, out int rowCount, bool isDesc = false, Expression<Func<TEntity, bool>> where = null, Expression<Func<TEntity, object>> order = null)
        {
            return _dalBase.GetPagedList(startPage, pageSize, out rowCount, isDesc, where, order);
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
        public List<TEntity> GetPagedListExtensions(int startPage, int pageSize, out int rowCount, bool isDesc = false, Expression<Func<TEntity, bool>> where = null, string order = null)
        {
            return _dalBase.GetPagedListExtensions(startPage, pageSize, out rowCount, isDesc, where, order);
        }

        /// <summary>
        /// 执行Sql查询语句,返回受影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public int ExecuteSql(string sql, SqlParameter[] sqlParameters = null, WriteAndRead writeAndRead = WriteAndRead.Read)
        {
            return _dalBase.ExecuteSql(sql, sqlParameters);
        }

        /// <summary>
        /// 执行存储过程，返回受影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public int ExecuteStoredProcedure(string sql, SqlParameter[] sqlParameters = null, WriteAndRead writeAndRead = WriteAndRead.Read)
        {
            return _dalBase.ExecuteStoredProcedure(sql, sqlParameters);
        }

        /// <summary>
        /// 执行Sql查询语句，返回实体
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public List<TEntity> ExecuteSqlQuery(string sql, SqlParameter[] sqlParameters = null, WriteAndRead writeAndRead = WriteAndRead.Read)
        {
            return _dalBase.ExecuteSqlQuery(sql, sqlParameters);
        }

        /// <summary>
        /// 执行存储过程，返回实体
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public List<TEntity> ExecuteStoredProcedureQuery(string sql, SqlParameter[] sqlParameters = null, WriteAndRead writeAndRead = WriteAndRead.Read)
        {
            return _dalBase.ExecuteStoredProcedureQuery(sql, sqlParameters);
        }




        #endregion

        #region 增删改

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public TEntity Insert(TEntity entity)
        {
            return _dalBase.Insert(entity);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        public TEntity Update(TEntity entity)
        {
            return _dalBase.Update(entity);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        public void Delete(TEntity entity)
        {
            _dalBase.Delete(entity);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id">实体主键</param>
        public void Delete(int id)
        {
            _dalBase.Delete(id);
        }

        /// <summary>
        /// 根据条件删除实体
        /// </summary>
        /// <param name="where">lambda表达式</param>
        public void Delete(Expression<Func<TEntity, bool>> where)
        {
            _dalBase.Delete(where);
        }

        /// <summary>
        /// 批量新增，实体类型默认为数据库表名(*注意：批量新增时实体类中字段的顺序一定要和数据库中列的顺序保持一致)
        /// </summary>
        /// <param name="entities">数据列表</param>
        public void AddList(List<TEntity> entities)
        {
            _dalBase.AddList(entities);
        }

        /// <summary>
        /// 批量添加(*注意：批量新增时实体类中字段的顺序一定要和数据库中列的顺序保持一致)
        /// </summary>
        /// <param name="tableName">批量新增的表名</param>
        /// <param name="entities">数据列表</param>
        /// <returns></returns>
        public void AddList(string tableName, List<TEntity> entities)
        {
            _dalBase.AddList(tableName, entities);
        }

        /// <summary>
        /// 批量添加(*注意：批量新增时DataTable中列的顺序一定要和数据库中列的顺序保持一致)
        /// </summary>
        /// <param name="tableName">批量新增的表名</param>
        /// <param name="dt">数据源</param>
        /// <returns></returns>
        public void AddList(string tableName, DataTable dt)
        {
            _dalBase.AddList(tableName, dt);
        }



        #endregion
    }
}
