using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.core.Invest.Repositories
{
    /// <summary>
    /// 投资机构接收项目
    /// </summary>
    [TableName("T_InvestPlace_Case")]
    [PrimaryKey("Guid", autoIncrement = false)]
    [ExplicitColumns]
    public class T_InvestPlace_Case
    {
        #region Model
        private string _guid;
        private string _t_investplace_guid;
        private string _t_project_guid;
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Column]
        public string Guid
        {
            set { _guid = value; }
            get { return _guid; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Column]
        public string t_InvestPlace_Guid
        {
            set { _t_investplace_guid = value; }
            get { return _t_investplace_guid; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Column]
        public string t_Project_Guid
        {
            set { _t_project_guid = value; }
            get { return _t_project_guid; }
        }
        #endregion Model
    }
}
