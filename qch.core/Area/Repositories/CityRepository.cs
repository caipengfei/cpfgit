using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    [TableName("S_City")]
    [PrimaryKey("CityID")]
    [ExplicitColumns]
    public partial class inf_city
    {
        [Column]
        public long CityID { get; set; }
        [Column]
        public string CityName { get; set; }
        [Column]
        public string ZipCode { get; set; }
        [Column]
        public long ProvinceID { get; set; }
        [Column]
        public DateTime DateCreated { get; set; }
        [Column]
        public DateTime DateUpdated { get; set; }
    }
    /// <summary>
    /// 市区信息资源
    /// </summary>
    public class CityRepository:Repository<inf_city>
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public inf_city GetById(int Id)
        {
            try
            {
                return this.Get(" where CityID=@0", new object[] { Id });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                return null;
            }
        }
    }
}
