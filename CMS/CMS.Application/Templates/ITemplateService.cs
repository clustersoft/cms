using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ArticleCategories.Dto;
using CMS.Application.Templates.Dto;
using CMS.Model;

namespace CMS.Application.Templates
{
    public interface ITemplateService:IBaseService<Template>
    {
        Template Addinfo(CreateTemplateInput input);

        bool Editinfo(UpdateTemplateInput input);

        
    }
}
