using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Repositories
{
    [TableName("S_District")]
    [PrimaryKey("DistrictID")]
    [ExplicitColumns]
    public partial class inf_district
    {
        [Column]
        public long DistrictID { get; set; }
        [Column]
        public string DistrictName { get; set; }
        [Column]
        public long CityID { get; set; }
        [Column]
        public DateTime DateCreated { get; set; }
        [Column]
        public DateTime DateUpdated { get; set; }
    }
    public class DistrictRepository:Repository<inf_district>
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public inf_district GetById(int Id)
        {
            try
            {
                return this.Get(" where DistrictID=@0", new object[] { Id });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message.ToString());
                return null;
            }
        }
    }
}
