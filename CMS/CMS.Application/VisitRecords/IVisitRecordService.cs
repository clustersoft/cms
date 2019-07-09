using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.VisitRecords.Dto;
using CMS.Model;

namespace CMS.Application.VisitRecords
{
    public interface IVisitRecordService:IBaseService<VisitRecord>
    {
        List<GetAnalyListOutput> GetAnalyList(DateTime? StartDate, DateTime? EndDate);

        /// <summary>
        /// 综合报告网站流量
        /// </summary>
        /// <returns></returns>
        GetAnalyOutput GetInfo();

        List<GetNRCountListOutput> GetNrCountList(DateTime? StartDate,DateTime? EndDate);

        List<GetNRListOutput> GetNrList(DateTime? StartDate,DateTime? EndDate);

        List<GetNRIPListOutput> GetNrIPList(DateTime? StartDate, DateTime? EndDate);

        List<GetLMtbCountlistOutput> GetLMtbCountlist(DateTime? StartDate, DateTime? EndDate);

        List<GetLMtbIPListOutput> GetLMtbIPList(DateTime? StartDate, DateTime? EndDate);

        List<GetLMListOutput> GetLMlist(DateTime? StartDate, DateTime? EndDate);

        /// <summary>
        /// 获取访问记录最早时间
        /// </summary>
        /// <returns></returns>
        DateTime MinDateTime();
    }
}
