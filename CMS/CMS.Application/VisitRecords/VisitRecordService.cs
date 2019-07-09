using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Extension;
using CMS.Application.VisitRecords.Dto;
using CMS.Model;
using CMS.Utils;

namespace CMS.Application.VisitRecords
{
    public class VisitRecordService : BaseService<VisitRecord>, IVisitRecordService
    {
        public VisitRecordService(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public GetAnalyOutput GetInfo()
        {
            //LogHelper.WriteLog("。。。。。。开始统计。。。。。。");
            DateTime today = DateTime.Now;
            DateTime yestday = DateTime.Now.AddDays(-1);

            TimeSpan d3 = db.VisitRecords.Max(a => a.VisitTime).Subtract(db.VisitRecords.Min(a => a.VisitTime));
            int totalDay = d3.Days;
            //int totalCount = db.VisitRecords.Count(a => true);
            //int totalIP = db.VisitRecords.Select(a => a.VisitIp).Distinct().Count();
            //var list = db.VisitRecords.ToList();
            //var slist =list.GroupBy(a => a.VisitTime.ToString("yyyy-MM-dd")).OrderByDescending(a => a.Count()).Select(s => new {count = s.Count(), k = s.Key}).FirstOrDefault();
            //var iplist =list.GroupBy(a => a.VisitTime.ToString("yyyy-MM-dd")).OrderByDescending(a => a.Count()).Select(s => new {count=s.Select(x => x.VisitIp).Distinct().Count(), k = s.Key}).FirstOrDefault();
            //var analy=new GetAnalyOutput()
            //{
            //    TodayCount = list.Count(a => a.VisitTime.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd")),
            //    TodayIP = list.Where(a => a.VisitTime.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd")).Select(a=>a.VisitIp).Distinct().Count(),
            //    yestodayCount = list.Count(a => a.VisitTime.ToString("yyyy-MM-dd") == yestday.ToString("yyyy-MM-dd")),
            //    yestodayIP = list.Where(a => a.VisitTime.ToString("yyyy-MM-dd") == yestday.ToString("yyyy-MM-dd")).Select(a => a.VisitIp).Distinct().Count(),
            //    TotalCount = totalCount,
            //    TotalIP = totalIP,
            //    averageCount = totalCount/totalDay,
            //    averageIP = totalIP/totalDay,
            //    HighestCountDate = slist==null?"": slist.k,
            //    HHighestIPDate = iplist == null ? "" : slist.k,
            //};

            int totalCount = db.VisitRecords.Count(a => true);
            int totalIP = db.VisitRecords.Select(a => a.VisitIp).Distinct().Count();
            var slist = db.VisitRecords.GroupBy(a => new { a.Year, a.Month, a.Day }).Select(s => new { count = s.Count(), k = s.Key }).OrderByDescending(a => a.count).FirstOrDefault();
            var iplist = db.VisitRecords.GroupBy(a => new { a.Year, a.Month, a.Day }).Select(s => new { count = s.Select(x => x.VisitIp).Distinct().Count(), k = s.Key }).OrderByDescending(a => a.count).FirstOrDefault();

            var analy = new GetAnalyOutput()
            {
                TodayCount = db.VisitRecords.Where(x => x.Year == today.Year && x.Month == today.Month && x.Day == today.Day).GroupBy(a => new { a.Year, a.Month, a.Day })
                             .Select(s => s.Count()).FirstOrDefault(),
                TodayIP = db.VisitRecords.Where(x => x.Year == today.Year && x.Month == today.Month && x.Day == today.Day).GroupBy(a => new { a.Year, a.Month, a.Day })
                           .Select(s => s.Select(x => x.VisitIp).Distinct().Count()).FirstOrDefault(),
                yestodayCount = db.VisitRecords.Where(x => x.Year == yestday.Year && x.Month == yestday.Month && x.Day == yestday.Day).GroupBy(a => new { a.Year, a.Month, a.Day })
                             .Select(s => s.Count()).FirstOrDefault(),
                yestodayIP = db.VisitRecords.Where(x => x.Year == yestday.Year && x.Month == yestday.Month && x.Day == yestday.Day).GroupBy(a => new { a.Year, a.Month, a.Day })
                           .Select(s => s.Select(x => x.VisitIp).Distinct().Count()).FirstOrDefault(),
                TotalCount = totalCount,
                TotalIP = totalIP,
                averageCount = totalCount / totalDay,
                averageIP = totalIP / totalDay,
                MonthCount = db.VisitRecords.Where(x => x.Year == today.Year && x.Month == today.Month).GroupBy(a => new { a.Year, a.Month }).Select(s => s.Count()).FirstOrDefault(),
                MonthIP = db.VisitRecords.Where(x => x.Year == today.Year && x.Month == today.Month).GroupBy(a => new { a.Year, a.Month }).Select(s => s.Select(x => x.VisitIp).Distinct().Count()).FirstOrDefault(),
                hapendayCount = slist == null ? "" : slist.k.Year + "-" + slist.k.Month + "-" + slist.k.Day,
                HapendayIP = iplist == null ? "" : slist.k.Year + "-" + slist.k.Month + "-" + slist.k.Day,
                HighestCount=slist.count,
                HighestIP= iplist.count
            };
            //LogHelper.WriteLog("。。。。。。结束统计。。。。。。");
            return analy;
        }

        public List<GetLMListOutput> GetLMlist(DateTime? StartDate, DateTime? EndDate)
        {
            DateTime beginTime = StartDate.HasValue ? StartDate.Value : DateTime.Now.AddDays(-30);
            DateTime endTime = EndDate.HasValue ? EndDate.Value : DateTime.Now;

            var tempList = (from a in db.VisitRecords
                            join b in db.ArticleCategories on a.CategoryId equals b.ID
                            where a.VisitTime >= beginTime && a.VisitTime <= endTime
                            select new
                            {
                                a,
                                b.Name
                            });

            int totalVisit = tempList.Count();
            var totalVisitList = tempList.GroupBy(a => a.Name).Select(a => a.Select(m => m.a.VisitIp).Distinct().Count());
            int totalVisitIP = totalVisitList.Any() ? totalVisitList.Sum() : 0;

            var list = tempList.GroupBy(a => a.Name)
                        .OrderByDescending(a => a.Count())
                        .Skip(0)
                        .Take(10)
                        .Select(s => new GetLMListOutput()
                        {
                            栏目名称 = s.Key,
                            访问量 = s.Count(),
                            访问量百分比 = (Math.Round((float)s.Count() / totalVisit, 4) * 100) + "%",
                            IP数 = s.Select(m => m.a.VisitIp).Distinct().Count(),
                            IP数百分比 = (Math.Round(((float)s.Select(m => m.a.VisitIp).Distinct().Count()) / totalVisitIP, 4) * 100) + "%"
                        }).AsNoTracking().ToList();
            return list;
        }

        public List<GetLMtbCountlistOutput> GetLMtbCountlist(DateTime? StartDate, DateTime? EndDate)
        {
            DateTime beginTime = StartDate.HasValue ? StartDate.Value : DateTime.Now.AddDays(-30);
            DateTime endTime = EndDate.HasValue ? EndDate.Value : DateTime.Now;

            var tempList = (from a in db.VisitRecords
                            join b in db.ArticleCategories on a.CategoryId equals b.ID
                            where a.VisitTime >= beginTime && a.VisitTime <= endTime
                            select new
                            {
                                a,
                                b.Name
                            });

            var list = tempList.GroupBy(a => a.Name).OrderByDescending(a => a.Count()).Take(10).Skip(0).Select(s => new GetLMtbCountlistOutput() { Count = s.Count(), Name = s.Key }).AsNoTracking().ToList();

            if (list.Count > 10)
            {
                var otherList = tempList.GroupBy(a => a.Name)
                 .OrderByDescending(a => a.Count())
                 .Skip(10)
                 .Select(s => new { cou = s.Count() });

                list.Add(new GetLMtbCountlistOutput()
                {
                    Count = otherList.Sum(a => a.cou),
                    Name = "其他"
                });
            }

            return list;
        }

        public List<GetLMtbIPListOutput> GetLMtbIPList(DateTime? StartDate, DateTime? EndDate)
        {
            DateTime beginTime = StartDate.HasValue ? StartDate.Value : DateTime.Now.AddDays(-30);
            DateTime endTime = EndDate.HasValue ? EndDate.Value : DateTime.Now;

            var tempList = from a in db.VisitRecords
                           join b in db.ArticleCategories on a.CategoryId equals b.ID
                           where a.VisitTime >= beginTime && a.VisitTime <= endTime
                           select new
                           {
                               a,
                               b.Name
                           };

            var list = tempList.GroupBy(a => a.Name)
                    .OrderByDescending(a => a.Select(x => x.a.VisitIp).Distinct().Count())
                    .Skip(0)
                    .Take(10)
                    .Select(s => new GetLMtbIPListOutput()
                    {
                        IP = s.Select(x => x.a.VisitIp).Distinct().Count(),
                        Name = s.Key
                    }).AsNoTracking().ToList();

            if (list.Count > 10)
            {
                var otherList = tempList.GroupBy(a => a.a.ArticleId)
                 .OrderByDescending(a => a.Select(x => x.a.VisitIp).Distinct().Count())
                 .Skip(10)
                 .Select(s => new { IP = s.Select(x => x.a.VisitIp).Distinct().Count() });
                list.Add(new GetLMtbIPListOutput() { IP = otherList.Sum(a => a.IP), Name = "其他" });
            }

            return list;
        }

        public List<GetNRCountListOutput> GetNrCountList(DateTime? StartDate, DateTime? EndDate)
        {
            DateTime beginTime = StartDate.HasValue ? StartDate.Value : DateTime.Now.AddDays(-30);
            DateTime endTime = EndDate.HasValue ? EndDate.Value : DateTime.Now;

            var tempList = (from a in db.VisitRecords
                            join b in db.Articles on a.ArticleId equals b.ID
                            where a.VisitTime >= beginTime && a.VisitTime <= endTime
                            select new
                            {
                                a,
                                b.Title
                            });

            var list = tempList.GroupBy(a => new { a.a.ArticleId, a.Title }).OrderByDescending(a => a.Count()).Take(10).Skip(0).Select(s => new GetNRCountListOutput() { Count = s.Count(), Name = s.Key.Title }).AsNoTracking().ToList();

            if (list.Count > 10)
            {
                var otherList = tempList.GroupBy(a => a.a.ArticleId)
                    .OrderByDescending(a => a.Count())
                    .Skip(10)
                    .Select(s => new {cou = s.Count()});
                list.Add(new GetNRCountListOutput() { Count = otherList.Sum(a => a.cou), Name = "其他" });
            }

            return list;
        }

        public List<GetNRIPListOutput> GetNrIPList(DateTime? StartDate, DateTime? EndDate)
        {
            DateTime beginTime = StartDate.HasValue ? StartDate.Value : DateTime.Now.AddDays(-30);
            DateTime endTime = EndDate.HasValue ? EndDate.Value : DateTime.Now;

            var tempList = (from a in db.VisitRecords
                            join b in db.Articles on a.ArticleId equals b.ID
                            where a.VisitTime >= beginTime && a.VisitTime <= endTime
                            select new
                            {
                                a,
                                b.Title
                            });

            var list = tempList.GroupBy(a => new { a.a.ArticleId, a.Title })
                .OrderByDescending(a => a.Select(x => x.a.VisitIp).Distinct().Count())
                .Skip(0)
                .Take(10)
                .Select(s => new GetNRIPListOutput()
                {
                    IP = s.Select(x => x.a.VisitIp).Distinct().Count(),
                    Name = s.Key.Title
                }).AsNoTracking().ToList();


            if (list.Count > 10)
            {
                var otherList = tempList.GroupBy(a => new { a.a.ArticleId, a.Title })
                .OrderByDescending(a => a.Select(x => x.a.VisitIp).Distinct().Count())
                .Skip(10)
                .Select(s => new
                {
                    cou = s.Select(x => x.a.VisitIp).Distinct().Count()
                });
                list.Add(new GetNRIPListOutput() { IP = otherList.Sum(a => a.cou), Name = "其他" });
            }

            return list;
        }

        public List<GetNRListOutput> GetNrList(DateTime? StartDate, DateTime? EndDate)
        {
            DateTime beginTime = StartDate.HasValue ? StartDate.Value : DateTime.Now.AddDays(-30);
            DateTime endTime = EndDate.HasValue ? EndDate.Value : DateTime.Now;

            var tempList = (from a in db.VisitRecords
                            join b in db.Articles on a.ArticleId equals b.ID
                            where a.VisitTime >= beginTime && a.VisitTime <= endTime
                            select new
                            {
                                a,
                                b.Title
                            });

            int totalVisit = tempList.Count();
            var totalVisitList = tempList.GroupBy(a => new { a.a.ArticleId }).Select(a => a.Select(m => m.a.VisitIp).Distinct().Count());
            int totalVisitIP = totalVisitList.Any() ? totalVisitList.Sum() : 0;

            var list = tempList.GroupBy(a => new { a.a.ArticleId, a.Title })
                        .OrderByDescending(a => a.Count())
                        .Skip(0)
                        .Take(10)
                        .Select(s => new GetNRListOutput()
                        {
                            内容名称 = s.Key.Title,
                            访问量 = s.Count(),
                            访问量百分比 = Math.Round((float)s.Count() / totalVisit, 4) * 100 + "%",
                            IP数 = s.Select(m => m.a.VisitIp).Distinct().Count(),
                            IP数百分比 = Math.Round(((float)s.Select(m => m.a.VisitIp).Distinct().Count()) / totalVisitIP, 4) * 100 + "%"
                        }).AsNoTracking().ToList();
            return list;
        }

        public List<GetAnalyListOutput> GetAnalyList(DateTime? StartDate, DateTime? EndDate)
        {
            DateTime beginTime = StartDate.HasValue ? StartDate.Value : DateTime.Now.AddDays(-30);
            DateTime endTime = EndDate.HasValue ? EndDate.Value : DateTime.Now;

            //var tempList = db.VisitRecords.Where(a => a.VisitTime >= beginTime && a.VisitTime <= endTime).ToList();
            //var list = tempList
            //            .GroupBy(a =>a.VisitTime.ToString("yyyy-MM-dd"))
            //            .Select(s => new GetAnalyListOutput()
            //            {
            //                VisitCount = s.Count(),
            //                VisitTime = s.Key.ToString(),
            //                VisitDay = s.Key.Substring(8, 2),
            //                VisitIP = s.Select(x=>x.VisitIp).Distinct().Count()
            //            });
            var list = db.VisitRecords.Where(a => a.VisitTime >= beginTime && a.VisitTime <= endTime).GroupBy(a => new { a.Year, a.Month, a.Day }).OrderBy(a => new { a.Key.Year, a.Key.Month, a.Key.Day })
            .Select(s => new GetAnalyListOutput()
            {
                VisitCount = s.Count(),
                VisitTime = s.Key.Year + "-" + s.Key.Month + "-" + s.Key.Day,
                VisitDay = s.Key.Day.ToString(),
                VisitIP = s.Select(x => x.VisitIp).Distinct().Count()
            });
            return list.AsNoTracking().ToList();
        }

        public DateTime MinDateTime()
        {
           var datetime=db.VisitRecords.Min(a => a.VisitTime);
            return datetime;
        }
    }
}
