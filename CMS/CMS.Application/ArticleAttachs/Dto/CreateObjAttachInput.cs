using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.ArticleAttachs.Dto
{
    public class CreateObjAttachInput
    {
        public string HashValue { get; set; }

        public string AttachName { get; set; }

        public string AttachUrl { get; set; }

        public string AttachNewName { get; set; }

        public string AttachFormat { get; set; }

        public int AttachBytes { get; set; }

        public int AttachType { get; set; }
    }
}
