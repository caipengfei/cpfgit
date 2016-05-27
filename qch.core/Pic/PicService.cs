using qch.Models;
using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.core
{
    public class PicService
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        PicRepository rp = new PicRepository();

        /// <summary>
        /// 获取某个对象所有关联的图片
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public IEnumerable<PicModel> GetByGuid(string Guid)
        {
            try
            {
                return rp.GetByGuid(Guid);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
    }
}
