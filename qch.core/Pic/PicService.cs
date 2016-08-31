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
        public IEnumerable<PicModel> GetByGuid1(string Guid)
        {
            try
            {
                var model = rp.GetByGuid(Guid);
                if (model != null && model.Count() == 1)
                {
                    foreach (var item in model)
                    {
                        //string path = Server.MapPath("img/1.gif");
                        //System.Drawing.Image image = System.Drawing.Image.FromFile(path);
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
    }
}
