using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.ViewSpotContents.Dto;
using CMS.Model;

namespace CMS.Application.ViewSpotContents
{
    public interface IViewSpotContentService:IBaseService<ViewSpotContent>
    {
        ViewSpotContent Add(CreateViewSpotContentInput input);

        bool Edit(UpdateViewSpotContentInput input);

        List<GetViewSpotContentInfoOutput> GetInfo(int id);
    }
}
