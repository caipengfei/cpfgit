using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    public class SelectPlace
    {
        public string Guid { get; set; }
        public string PlaceName { get; set; }
        public string PlacePic { get; set; }
        public string PlaceAddr { get; set; }
        public string t_Place_ProvideService { get; set; }
        public string t_Place_Tips { get; set; }
        public string t_Place_CheckIn { get; set; }
        public string PlaceImage
        {
            get
            {
                return "http://www.cn-qch.com:8002/Attach/Images/" + PlacePic;
            }
        }
    }
}
