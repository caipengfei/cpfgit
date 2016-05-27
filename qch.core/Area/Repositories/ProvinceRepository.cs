using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    [TableName("S_Province")]
    [PrimaryKey("ProvinceID")]
    [ExplicitColumns]
    public partial class inf_province
    {
        [Column]
        public long ProvinceID { get; set; }
        [Column]
        public string ProvinceName { get; set; }
        [Column]
        public DateTime DateCreated { get; set; }
        [Column]
        public DateTime DateUpdated { get; set; }
    }

    public class ProvinceRepository:Repository<inf_province>
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public inf_province GetById(int Id)
        {
            try
            {
                return this.Get(" where ProvinceID=@0", new object[] { Id });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                return null;
            }
        }
    }
}
