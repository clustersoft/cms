﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.WildlifeCategories.Dto
{
    public class GetWildLifeCategoryListOutput
    {
        public int ID { get; set; }

        public string CateName { get; set; }

        public string Type { get; set; }

        public int OrderID { get; set; }
    }
}
