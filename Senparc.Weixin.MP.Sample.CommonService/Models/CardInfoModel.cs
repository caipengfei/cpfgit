using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Senparc.Weixin.MP.Sample.CommonService.Models
{
    public class CardInfoModel
    {
        public string Province { get; set; }

        public string City { get; set; }


        public string Dis { get; set; }
        //直推人数
        public string T1 { get; set; }
        //间推人数
        public string T2 { get; set; }

        public string TrueName { get; set; }

        public string CardNo { get; set; }

        public string Balance { get; set; }//总余额        
        /// <summary>
        /// 可用余额
        /// </summary>
        /// <value>The available balance.</value>
        public string AvailableBalance { get; set; }//可用余额

        public string Deposit { get; set; }//押金

        public string TixianBalance { get; set; }//提现余额

        public string Awards { get; set; }//所有奖励总和

         
    }
}
