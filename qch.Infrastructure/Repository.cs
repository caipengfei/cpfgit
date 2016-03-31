using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// qch
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> where T : class
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string ConnStr { get; set; }
        public Repository()
        {
            ConnStr = "qch";
        }

        public IEnumerable<T> GetAll(string sql, object[] obj)
        {
            try
            {
                IEnumerable<T> target;
                using (PetaPoco.Database db = new PetaPoco.Database(ConnStr))
                {
                    target = db.Fetch<T>(sql, obj);
                }
                return target;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Repository<{0}>,方法名:{1},错误信息:{2}", typeof(T).Name, "GetAll()", ex.Message));
                log.Error(string.Format("sql语句:{0}", sql));
                return null;
            }
        }

        public IEnumerable<T> GetAll(string sql)
        {
            try
            {
                IEnumerable<T> target;
                using (PetaPoco.Database db = new PetaPoco.Database(ConnStr))
                {
                    target = db.Fetch<T>(sql);
                }
                return target;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Repository<{0}>,方法名:{1},错误信息:{2}", typeof(T).Name, "GetAll()", ex.Message));
                log.Error(string.Format("sql语句:{0}", sql));
                return null;
            }
        }

        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="sql"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public PetaPoco.Page<T> GetPageData(int pageIndex, int PageSize, string sql)
        {
            try
            {
                PetaPoco.Page<T> target;
                using (PetaPoco.Database db = new PetaPoco.Database(ConnStr))
                {
                    target = db.Page<T>(pageIndex, PageSize, sql);
                }
                return target;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Repository<{0}>,方法名:{1},错误信息:{2}", typeof(T).Name, "GetPageData()", ex.Message));
                log.Error(string.Format("sql语句:{0}", sql));
                return null;
            }
        }

        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="sql"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public PetaPoco.Page<T> GetPageData(int pageIndex, int PageSize, string sql, object[] obj)
        {
            try
            {
                PetaPoco.Page<T> target;
                using (PetaPoco.Database db = new PetaPoco.Database(ConnStr))
                {
                    target = db.Page<T>(pageIndex, PageSize, sql, obj);
                }
                return target;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Repository<{0}>,方法名:{1},错误信息:{2}", typeof(T).Name, "GetPageData()", ex.Message));
                log.Error(string.Format("sql语句:{0}", sql));
                return null;
            }
        }

        public object ExecuteScalar(string sql)
        {
            try
            {
                long i = 0;
                using (PetaPoco.Database db = new PetaPoco.Database(ConnStr))
                {
                    //return db.ExecuteScalar<long>(sql);
                    return db.ExecuteScalar<object>(sql);
                }

            }
            catch (Exception ex)
            {
                log.Error(string.Format("Repository<{0}>,方法名:{1},错误信息:{2}", typeof(T).Name, "ExecuteScalar()", ex.Message));
                log.Error(string.Format("sql语句:{0}", sql));
                return 0;
            }

        }

        public object ExecuteScalar(string sql, object[] obj)
        {
            try
            {
                long i = 0;
                using (PetaPoco.Database db = new PetaPoco.Database(ConnStr))
                {
                    return db.ExecuteScalar<object>(sql, obj);
                }

            }
            catch (Exception ex)
            {
                log.Error(string.Format("Repository<{0}>,方法名:{1},错误信息:{2}", typeof(T).Name, "ExecuteScalar()", ex.Message));
                log.Error(string.Format("sql语句:{0}", sql));
                return null;
            }

        }

        public T Get(string sql, object[] obj)
        {
            try
            {
                T target;
                using (PetaPoco.Database db = new PetaPoco.Database(ConnStr))
                {
                    target = db.SingleOrDefault<T>(sql, obj);
                }
                return target;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Repository<{0}>,方法名:{1},错误信息:{2}", typeof(T).Name, "Get()", ex.Message));
                log.Error(string.Format("sql语句:{0}", sql));
                return null;
            }
        }
        /// <summary>
        /// 批量修改收益明细的时候用到
        /// cpf 08-20增加
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public object Execute(string sql, object[] obj)
        {
            try
            {
                using (PetaPoco.Database db = new PetaPoco.Database(ConnStr))
                {
                    return db.Execute(sql, obj);
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Repository<{0}>,方法名:{1},错误信息:{2}", typeof(T).Name, "", ex.Message));
                log.Error(string.Format("sql语句:{0}", sql));
                return null;
            }
        }

        public object Insert(T t)
        {
            try
            {
                using (PetaPoco.Database db = new PetaPoco.Database(ConnStr))
                {
                    return db.Insert(t);
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Repository<{0}>,方法名:{1},错误信息:{2}", typeof(T).Name, "Execute()", ex.Message));
                return null;
            }
        }

        public object Update(T t)
        {
            try
            {
                using (PetaPoco.Database db = new PetaPoco.Database(ConnStr))
                {
                    return db.Update(t);
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Repository<{0}>,方法名:{1},错误信息:{2}", typeof(T).Name, "Update()", ex.Message));

                return null;
            }
        }

        public object Delete(T t)
        {
            try
            {
                using (PetaPoco.Database db = new PetaPoco.Database(ConnStr))
                {
                    return db.Delete(t);

                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Repository<{0}>,方法名:{1},错误信息:{2}", typeof(T).Name, "Delete()", ex.Message));

                return null;
            }
        }


    }

    /// <summary>
    /// 测试资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository2<T> where T : class
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IEnumerable<T> GetAll(string sql, object[] obj)
        {
            try
            {
                IEnumerable<T> target;
                using (PetaPoco.Database db = new PetaPoco.Database(DbConfig.MarketDataBase))
                {
                    target = db.Fetch<T>(sql, obj);
                }
                return target;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }

        public IEnumerable<T> GetAll(string sql)
        {
            try
            {
                IEnumerable<T> target;
                using (PetaPoco.Database db = new PetaPoco.Database(DbConfig.MarketDataBase))
                {
                    target = db.Fetch<T>(sql);
                }
                return target;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="sql"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public PetaPoco.Page<T> GetPageData(int pageIndex, int PageSize, string sql)
        {
            try
            {
                PetaPoco.Page<T> target;
                using (PetaPoco.Database db = new PetaPoco.Database(DbConfig.MarketDataBase))
                {
                    target = db.Page<T>(pageIndex, PageSize, sql);
                }
                return target;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="sql"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public PetaPoco.Page<T> GetPageData(int pageIndex, int PageSize, string sql, object[] obj)
        {
            try
            {
                PetaPoco.Page<T> target;
                using (PetaPoco.Database db = new PetaPoco.Database(DbConfig.MarketDataBase))
                {
                    target = db.Page<T>(pageIndex, PageSize, sql, obj);
                }
                return target;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }

        public object ExecuteScalar(string sql)
        {
            try
            {
                //long i = 0;
                //using (PetaPoco.Database db = new PetaPoco.Database(DbConfig.MarketDataBase))
                //{
                //    i = (long)db.ExecuteScalar<long>(sql);
                //}
                //return i;

                using (PetaPoco.Database db = new PetaPoco.Database(DbConfig.MarketDataBase))
                {
                    return db.ExecuteScalar<object>(sql);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return 0;
            }

        }

        public object ExecuteScalar(string sql, object[] obj)
        {
            try
            {
                //long i = 0;
                //using (PetaPoco.Database db = new PetaPoco.Database(DbConfig.MarketDataBase))
                //{
                //    i = (long)db.ExecuteScalar<long>(sql);
                //}
                //return i;

                using (PetaPoco.Database db = new PetaPoco.Database(DbConfig.MarketDataBase))
                {
                    return db.ExecuteScalar<object>(sql, obj);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return 0;
            }

        }

        public T Get(string sql, object[] obj)
        {
            try
            {
                T target;
                using (PetaPoco.Database db = new PetaPoco.Database(DbConfig.MarketDataBase))
                {
                    target = db.SingleOrDefault<T>(sql, obj);
                }
                return target;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }


        public object Update(T t)
        {
            try
            {
                using (PetaPoco.Database db = new PetaPoco.Database(DbConfig.MarketDataBase))
                {
                    return db.Update(t);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }

        public object Delete(T t)
        {
            try
            {
                using (PetaPoco.Database db = new PetaPoco.Database(DbConfig.MarketDataBase))
                {
                    return db.Delete(t);

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }

        public int Delete<T>(string sql, object[] obj)
        {
            try
            {
                using (PetaPoco.Database db = new PetaPoco.Database(DbConfig.MarketDataBase))
                {
                    return db.Delete<T>(sql, obj);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return 0;
            }
        }

        public object Insert(T t)
        {
            try
            {
                using (PetaPoco.Database db = new PetaPoco.Database(DbConfig.MarketDataBase))
                {
                    return db.Insert(t);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
    }

    /// <summary>
    /// 业务资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository3<T> : Repository<T> where T : class
    {
        public Repository3()
        {
            this.ConnStr = "qch";
        }
    }
}
