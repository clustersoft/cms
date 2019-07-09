using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.Users.Dto
{
    public class GetUserListInput
    {
        [Range(1,9999)]
        public int pageIndex { get; set; }

        public string keyword { get; set; }
    }
}
