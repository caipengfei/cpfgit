using qch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qch.Models
{
    /// <summary>
    /// 课程实体类
    /// </summary>
    public class CourseModel : T_Course
    {
        
        /// <summary>
        /// 导师名称
        /// </summary>
        public string LecturerName { get; set; }
        /// <summary>
        /// 导师头像
        /// </summary>
        public string LecturerAvator { get; set; }
    }
}
