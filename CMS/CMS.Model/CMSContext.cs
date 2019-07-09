using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Model
{
    public class CMSContext:DbContext
    {
        public CMSContext() : base("CMSConnect")
        {
            Database.SetInitializer<CMSContext>(null);
        }

        public DbSet<CMSAction> Actions { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<VisitRecord> VisitRecords { get; set; }

        public DbSet<VisitRecordIpArea> VisitRecordIpAreas { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<ArticleArticleCategory> ArticleArticleCategories { get; set; }

        public DbSet<ArticleContent> ArticleContents { get; set; }

        public DbSet<ArticleCategory> ArticleCategories { get; set; }

        public DbSet<ArticleAuditLog> ArticleAuditLogs { get; set; }

        public DbSet<ArticleAttach> ArticleAttaches { get; set; }

        public DbSet<PublicityContent> PublicityContents { get; set; }

        public DbSet<PublicityCategory> PublicityCategories { get; set; }

        public DbSet<PublicityType> PublicityTypes { get; set; }

        public DbSet<Log> Logs { get; set; }

        public DbSet<SystemConfiguration> SystemConfigurations { get; set; }

        public DbSet<LeaveMessage> LeaveMessages { get; set; }

        public DbSet<RoleNavAction> RoleNavActions { get; set; }

        public DbSet<RoleArticleCategoryAction> RoleArticleCategoryActions { get; set; }

        public DbSet<Template> Templates { get; set; }

        public DbSet<Navgation> Navgations { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<ViewSpot> ViewSpots { get; set; }

        public DbSet<ViewSpotContent> ViewSpotContents { get; set; }

        public DbSet<WildlifeManagement> WildlifeManagements { get; set; }

        public DbSet<WildlifeContent> WildlifeContents { get; set; }

        public DbSet<WildlifeCategory> WildlifeCategories { get; set; }

        public DbSet<Route> Routes { get; set; }

        public DbSet<RouteViewSpot> RouteViewSpots { get; set; }

        public DbSet<IconList> IconLists { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //关闭默认联级删除
            //如果需要再使用.WillCascadeOnDelete(true) 单独打开某个主外键的级联删除
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}
