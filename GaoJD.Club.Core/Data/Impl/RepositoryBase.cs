using GaoJD.Club.Utility;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace GaoJD.Club.Core
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        //定义数据访问上下文对象
        public readonly IDbProvider _readDbContext;
        public readonly IDbProvider _WriteDbContext;

        public RepositoryBase()
        {
            _readDbContext = DbContextFactory.CallContext<SqlserverProvider>(WriteAndRead.Read);
            _WriteDbContext = DbContextFactory.CallContext<SqlserverProvider>(WriteAndRead.Write);

        }


        #region  是否存在
        /// <summary>
        /// 根据条件查询是否存在
        /// <param name="condition">查询条件</param>
        /// </summary>
        /// <returns></returns>
        public bool IsExists(Expression<Func<TEntity, bool>> predicate)
        {
            return _readDbContext.IsExists<TEntity>(predicate);
        }
        #endregion

        #region  查询
        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <returns></returns>
        public List<TEntity> GetAll()
        {
            return _readDbContext.GetAll<TEntity>();
        }

        /// <summary>
        /// 获取总数量
        /// </summary>
        /// <returns></returns>
        public long GetCount()
        {
            return _readDbContext.GetCount<TEntity>();
        }


        /// <summary>
        /// 根据lambda表达式条件获取总数量
        /// </summary>
        /// <returns></returns>
        public long GetCountByQuery(Expression<Func<TEntity, bool>> predicate)
        {
            return _readDbContext.GetCountByQuery<TEntity>(predicate);
        }


        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <returns></returns>
        public TEntity GetById(int id, WriteAndRead writeAndRead = WriteAndRead.Read)
        {
            IDbProvider _provider;
            if (writeAndRead == WriteAndRead.Read)
            {
                _provider = _readDbContext;
            }
            else
            {
                _provider = _WriteDbContext;
            }
            return _provider.GetById<TEntity>(id);
        }

        /// <summary>
        /// 根据lambda表达式条件获取单个实体
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        public TEntity GetItemByQuery(Expression<Func<TEntity, bool>> predicate, WriteAndRead writeAndRead = WriteAndRead.Read)
        {
            IDbProvider _provider;
            if (writeAndRead == WriteAndRead.Read)
            {
                _provider = _readDbContext;
            }
            else
            {
                _provider = _WriteDbContext;
            }
            return _provider.GetItemByQuery<TEntity>(predicate);
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
            return _readDbContext.GetListByQuery<TEntity>(predicate, isDesc, orderBy);

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
            return _readDbContext.GetListByQueryExtensions<TEntity>(predicate, isDesc, orderBy);
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
            return _readDbContext.GetPagedList<TEntity>(startPage, pageSize, out rowCount, isDesc, where, order);
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
            return _readDbContext.GetPagedListExtensions<TEntity>(startPage, pageSize, out rowCount, isDesc, where, order);
        }

        /// <summary>
        /// 执行Sql查询语句,返回受影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public int ExecuteSql(string sql, SqlParameter[] sqlParameters = null, WriteAndRead writeAndRead = WriteAndRead.Read)
        {
            IDbProvider _provider;
            if (writeAndRead == WriteAndRead.Read)
            {
                _provider = _readDbContext;
            }
            else
            {
                _provider = _WriteDbContext;
            }

            return _provider.ExecuteSql(sql, sqlParameters);

        }

        /// <summary>
        /// 执行存储过程，返回受影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public int ExecuteStoredProcedure(string sql, SqlParameter[] sqlParameters = null, WriteAndRead writeAndRead = WriteAndRead.Read)
        {
            IDbProvider _provider;
            if (writeAndRead == WriteAndRead.Read)
            {
                _provider = _readDbContext;
            }
            else
            {
                _provider = _WriteDbContext;
            }
            return _provider.ExecuteStoredProcedure(sql, sqlParameters);
        }

        /// <summary>
        /// 执行Sql查询语句，返回实体
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public List<TEntity> ExecuteSqlQuery(string sql, SqlParameter[] sqlParameters = null, WriteAndRead writeAndRead = WriteAndRead.Read)

        {
            IDbProvider _provider;
            if (writeAndRead == WriteAndRead.Read)
            {
                _provider = _readDbContext;
            }
            else
            {
                _provider = _WriteDbContext;
            }
            return _provider.ExecuteSqlQuery<TEntity>(sql, sqlParameters);
        }

        /// <summary>
        /// 执行存储过程，返回实体
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public List<TEntity> ExecuteStoredProcedureQuery(string sql, SqlParameter[] sqlParameters = null, WriteAndRead writeAndRead = WriteAndRead.Read)
        {
            IDbProvider _provider;
            if (writeAndRead == WriteAndRead.Read)
            {
                _provider = _readDbContext;
            }
            else
            {
                _provider = _WriteDbContext;
            }
            return _provider.ExecuteStoredProcedureQuery<TEntity>(sql, sqlParameters);
        }


        #endregion

        #region 增删改

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <returns></returns>
        public TEntity Insert(TEntity entity)
        {
            return _WriteDbContext.Insert<TEntity>(entity);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        public TEntity Update(TEntity entity)
        {
            return _WriteDbContext.Update<TEntity>(entity);
        }


        //private void EntityToEntity<T>(T pTargetObjSrc, T pTargetObjDest)
        //{
        //    foreach (var mItem in typeof(T).GetProperties())
        //    {
        //        mItem.SetValue(pTargetObjDest, mItem.GetValue(pTargetObjSrc, new object[] { }), null);
        //    }
        //}


        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        public void Delete(TEntity entity)
        {
            _WriteDbContext.Delete<TEntity>(entity);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id">实体主键</param>
        public void Delete(int id)
        {
            _WriteDbContext.Delete<TEntity>(id);
        }

        /// <summary>
        /// 根据条件删除实体
        /// </summary>
        /// <param name="where">lambda表达式</param>
        public void Delete(Expression<Func<TEntity, bool>> where)
        {
            _WriteDbContext.Delete<TEntity>(where);
        }

        /// <summary>
        /// 批量新增，实体类型默认为数据库表名(*注意：批量新增时实体类中字段的顺序一定要和数据库中列的顺序保持一致)
        /// </summary>
        /// <param name="entities">数据列表</param>
        public void AddList(List<TEntity> entities)
        {
            _WriteDbContext.AddList<TEntity>(entities);
        }

        /// <summary>
        /// 批量添加(*注意：批量新增时实体类中字段的顺序一定要和数据库中列的顺序保持一致)
        /// </summary>
        /// <param name="tableName">批量新增的表名</param>
        /// <param name="entities">数据列表</param>
        /// <returns></returns>
        public void AddList(string tableName, List<TEntity> entities)
        {
            _WriteDbContext.AddList<TEntity>(tableName, entities);
        }

        /// <summary>
        /// 批量添加(*注意：批量新增时DataTable中列的顺序一定要和数据库中列的顺序保持一致)
        /// </summary>
        /// <param name="tableName">批量新增的表名</param>
        /// <param name="dt">数据源</param>
        /// <returns></returns>
        public void AddList(string tableName, DataTable dt)
        {
            _WriteDbContext.AddList(tableName, dt);
        }


        #endregion

        /// <summary>
        /// 根据主键构建判断表达式
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        protected static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(int id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));
            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof(int))
                );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }

    }
}
