using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Infrastructure
{
    /// <summary>
    /// 图片模型
    /// </summary>
    public class ImgModel
    {
        /// <summary>
        /// 缩略图
        /// </summary>
        public string Thumbnail { get; set; }
        /// <summary>
        /// 原图
        /// </summary>
        public string OriginalImg { get; set; }
    }
}
