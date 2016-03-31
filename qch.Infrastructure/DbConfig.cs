using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public static class DbConfig
    {
        public static string qch { get { return "qch"; } }
        /// <summary>
        /// DB_QCH
        /// </summary>
        public static string MarketDataBase { get { return "cpf"; } }
    }
}
