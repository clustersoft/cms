using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.ArticleAttachs.Dto
{
    public class GetArticleAttachOutput
    {
        public int AttachID { get; set; }

        public string AttachUrl { get; set; }

        public string AttachName { get; set; }

        public string AttachNewName { get; set; }

        public int AttachIndex { get; set; }
    }
}
