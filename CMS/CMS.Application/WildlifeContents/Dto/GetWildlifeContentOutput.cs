﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleAttachs.Dto;

namespace CMS.Application.WildlifeContents
{
    public class GetWildlifeContentOutput
    {
        public int Type { get; set; }

        public string TypeName { get; set; }

        public string Content { get; set; }

        public List<GetObjAttachOutput>  Attachs { get; set; }

    }
}
