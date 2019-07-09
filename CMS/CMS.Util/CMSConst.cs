using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Util
{
    public class CMSConst
    {
        /// <summary>
        /// Token过期时间
        /// </summary>
        public const int AccessTokenExpireTimeSpanMinute = 480;

        /// <summary>
        /// RefreshToken过期时间
        /// </summary>
        public const int AccessRefreshTokenExpireTimeSpanMinute = 480;

        /// <summary>
        /// 系统默认分页
        /// </summary>
        public const int PageSiez = 20;

        /// <summary>
        /// 上传文件夹
        /// </summary>
        public const string UploadFolder = "Upload";

    }
}
