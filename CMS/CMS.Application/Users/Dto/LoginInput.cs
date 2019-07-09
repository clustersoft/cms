using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Users.Dto
{
    public class LoginInput
    {
        [StringLength(20, MinimumLength = 2)]
        public string loginname { get; set; }
        /// <summary>
        /// 32位MD5值（大写）
        /// </summary>
        public string password { get; set; }
    }
}
