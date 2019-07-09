using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using CMS.Application.ArticleAttachs.Dto;
using CMS.Application.ArticleAuditLogs.Dto;
using CMS.Application.ArticleCategories.Dto;
using CMS.Application.Articles.Dto;
using CMS.Application.LeaveMessages.Dto;
using CMS.Application.Logs.Dto;
using CMS.Application.Navgations.Dto;
using CMS.Application.PublicityCategories.Dto;
using CMS.Application.PublicityContents.Dto;
using CMS.Application.PublicityTypes.Dto;
using CMS.Application.Roles.Dto;
using CMS.Application.Routes.Dto;
using CMS.Application.RouteViewSpots.Dto;
using CMS.Application.Templates.Dto;
using CMS.Application.Users.Dto;
using CMS.Application.ViewSpotContents.Dto;
using CMS.Application.ViewSpots.Dto;
using CMS.Application.WildlifeCategories.Dto;
using CMS.Application.WildlifeContents;
using CMS.Application.WildlifeContents.Dto;
using CMS.Application.WildlifeManagers.Dto;
using CMS.Model;
using CMS.Model.Enum;

namespace CMS.WebApi.App_Start
{
    public class Bootstrapper
    {
        public static void RegisterAutoMapper()
        {
            Mapper.Initialize(cfg =>
            {
                //string为null时通用设置
                cfg.CreateMap<string, string>().ConvertUsing(s =>string.IsNullOrEmpty(s)? string.Empty:s.Trim());

                //User
                cfg.CreateMap<User, LoginOutput>();
                cfg.CreateMap<CreateUserInput, User>();
                cfg.CreateMap<ChangePersonalInfoInput, User>();
                //password为空则不匹配
                cfg.CreateMap<UpdateUserInput, User>().ForMember(a=>a.PassWord,opt => opt.Condition(src=>!string.IsNullOrEmpty(src.PassWord)));
                cfg.CreateMap<User, GetUserOutput>().ForMember(a => a.RoleNames, opt => opt.MapFrom(src => string.Join(",",src.UserRoles.Select(a=>a.Role.RoleName))));
                cfg.CreateMap<User, GetUserListOutPut>().ForMember(a => a.RoleNames, opt => opt.MapFrom(src => string.Join(",",src.UserRoles.Select(a=>a.Role.RoleName))));

                //Role
                cfg.CreateMap<Role, GetRoleListOutput>();
                cfg.CreateMap<Role, GetRoleListListOutput>();
                cfg.CreateMap<Role, GetRoleOutput>();
                cfg.CreateMap<UpdateRoleInput, Role>();

                //Article
                cfg.CreateMap<Article, GetArticleOutput>().ForMember(a => a.CategoryNames, opt => opt.MapFrom(src => string.Join(",", src.ArticleArticleCategories.Select(a => a.ArticleCategory.Name))))
                .ForMember(a => a.CategoryIDs, opt => opt.MapFrom(src => string.Join(",", src.ArticleArticleCategories.Select(a => a.ArticleCategory.ID))));
                cfg.CreateMap<Article, GetArticleListOutput>().ForMember(a => a.CategoryNames, opt => opt.MapFrom(src => string.Join(",", src.ArticleArticleCategories.Select(a => a.ArticleCategory.Name))))
                .ForMember(a => a.CategoryIDs, opt => opt.MapFrom(src => string.Join(",", src.ArticleArticleCategories.Select(a => a.ArticleCategory.ID))));
                cfg.CreateMap<Article, GetShListData>().ForMember(a => a.CategoryNames, opt => opt.MapFrom(src => string.Join(",", src.ArticleArticleCategories.Select(a => a.ArticleCategory.Name))))
      .ForMember(a => a.CategoryIDs, opt => opt.MapFrom(src => string.Join(",", src.ArticleArticleCategories.Select(a => a.ArticleCategory.ID))));

                //ArticleCategory
                cfg.CreateMap<ArticleCategory, GetCategoryTreeListOutput>();
                cfg.CreateMap<ArticleCategory, GetArticeCategoryTreeListOutput>();
                cfg.CreateMap<ArticleCategory, GetCategoryListOutput>();
                cfg.CreateMap<ArticleCategory, GetCategoryOutput>();
                cfg.CreateMap<ArticleCategory, GetRoleCategoryListOutput>().ForMember(a => a.CateName, opt => opt.MapFrom(src => src.Name)); 
                cfg.CreateMap<ArticleCategory, GetLoadCategoryListOutput>().ForMember(a => a.CateName, opt => opt.MapFrom(src => src.Name));
                cfg.CreateMap<ArticleAttach, GetArticleAttachOutput>().ForMember(a=>a.AttachID,opt=>opt.MapFrom(src=>src.ID));
                cfg.CreateMap<CreateCategoryInput, ArticleCategory>();
                cfg.CreateMap<UpdateCategoryInput, ArticleCategory>();

                //ArticleAuditLog
                cfg.CreateMap<ArticleAuditLog, GetArticleAuditLogOutput>();


                //ViewSpot
                cfg.CreateMap<CreateViewSpotInput, ViewSpot>();
                cfg.CreateMap<UpdateViewSpotInput, ViewSpot>();
                cfg.CreateMap<ViewSpot, GetViewSpotOutput>();
                cfg.CreateMap<ViewSpot, GetViewSpotInfoOutput>();

                //ViewSpotContent
                cfg.CreateMap<CreateViewSpotContentInput, ViewSpotContent>();
                cfg.CreateMap<UpdateViewSpotContentInput, ViewSpotContent>();
                cfg.CreateMap<ViewSpotContent, GetViewSpotContentOutput>();
                cfg.CreateMap<ViewSpotContent, GetViewSpotContentListOutput>().ForMember(a => a.TypeName, opt => opt.MapFrom(src =>((ViewSpotContentTypesEnum)src.Type).ToString()));

                //WildlifeManagement
                cfg.CreateMap<CreateWildlifeManagementInput, WildlifeManagement>().ForMember(a => a.WildlifeCategoryID, opt => opt.MapFrom(src => src.WildlifemanagerID));
                cfg.CreateMap<UpdateWildlifeManagementInput, WildlifeManagement>().ForMember(a => a.WildlifeCategoryID, opt => opt.MapFrom(src => src.WildlifemanagerID));
                cfg.CreateMap<WildlifeManagement, GetWildlifeManagementOutput>();

                //WildlifeContent
                cfg.CreateMap<CreateWildlifeContentInput, WildlifeContent>().ForMember(a => a.WildlifeID, opt => opt.MapFrom(src => src.WildlifemanagerID));
                cfg.CreateMap<UpdateWildlifeContentInput, WildlifeContent>().ForMember(a => a.WildlifeID, opt => opt.MapFrom(src => src.WildlifemanagerID));
                cfg.CreateMap<WildlifeContent, GetWildlifeContentOutput>().ForMember(a => a.TypeName, opt => opt.MapFrom(src => ((WildlifeContentTypesEnum)src.Type).ToString()));
                cfg.CreateMap<WildlifeContent, GetWildlifeContentInfoOutput>().ForMember(a => a.TypeName, opt => opt.MapFrom(src => ((WildlifeContentTypesEnum)src.Type).ToString()));

                //routeViewSpot
                cfg.CreateMap<CreateWildLifeCategoryInput, WildlifeCategory>();
                cfg.CreateMap<UpdateWildLifeCategoryInput, WildlifeCategory>();
                cfg.CreateMap<WildlifeCategory, GetWildLifeCategoryOutput>();
                cfg.CreateMap<WildlifeCategory, GetWildLifeCategoryListOutput>().ForMember(a => a.Type, opt => opt.MapFrom(src => src.Type.HasValue ? ((WildlifeManagerTypesEnum)src.Type).ToString() : ""));

                //route
                cfg.CreateMap<CreateRouteInput, Route>().ForMember(a => a.Content, opt => opt.MapFrom(src => src.RouteContent));
                cfg.CreateMap<UpdateRouteInput, Route>().ForMember(a => a.Content, opt => opt.MapFrom(src => src.RouteContent));
                cfg.CreateMap<Route, GetRouteOutput>().ForMember(a => a.RouteContent, opt => opt.MapFrom(src => src.Content));
                cfg.CreateMap<Route, GetRouteListOutput>();

                //routeViewSpot
                cfg.CreateMap<CreateRouteViewSpotInput, RouteViewSpot>();
                cfg.CreateMap<UpdateRouteViewSpotInput, RouteViewSpot>();

                //Navgation
                cfg.CreateMap<Navgation, GetNavgationListOutput>().ForMember(a => a.ActionTypesName, opt => opt.MapFrom(src => src.ActionTypes));
                cfg.CreateMap<Navgation, GetRoleNavgationListOutput>().ForMember(a => a.Actions, opt => opt.MapFrom(src => src.ActionTypes));
                cfg.CreateMap<Navgation, GetLoadNavgationListOutput>().ForMember(a => a.Actions, opt => opt.MapFrom(src => src.ActionTypes));
                cfg.CreateMap<Navgation, GetNavgationOutput>();
                cfg.CreateMap<Navgation, GetPermissionNavListOutput>();
                cfg.CreateMap<CreateNavgationInput, Navgation>();
                cfg.CreateMap<UpdateNavgationInput, Navgation>();

                ///PublicityType
                cfg.CreateMap<PublicityType, GetPublicityTypeListOutput>();

                //PublicityCategory
                cfg.CreateMap<PublicityCategory, GetPublicityCategoryListOutput>().ForMember(a => a.PublicityTypesName, opt => opt.MapFrom(src => src.PublicityType.TypeName));
                cfg.CreateMap<CreatePublicityCategoryInput, PublicityCategory>();
                cfg.CreateMap<UpdatePublicityCategoyrInput, PublicityCategory>();
                cfg.CreateMap<PublicityCategory, GetPublicityCategoryOutput>();
                cfg.CreateMap<PublicityCategory, GetSelCategoryOutput>();

                //PublicityContent
                cfg.CreateMap<PublicityContent, GetPublicityContentListOutput>().ForMember(a=>a.PublicityCategoryName,opt=>opt.MapFrom(src=>src.PublicityCategory.PublicityCategoryName));
                cfg.CreateMap<PublicityContent, GetPublicityContentOutput>();

                //log
                cfg.CreateMap<Log, GetLogOutput>().ForMember(a=>a.LogUser,opt=>opt.MapFrom(src=>src.User.UserName));
                cfg.CreateMap<Log, GetLogListOutput>().ForMember(a=>a.LogUser,opt=>opt.MapFrom(src=>src.User.UserName));

                //leaveMeaasge
                cfg.CreateMap<LeaveMessage, GetLeaveMessageListOuput>();

                //Template
                cfg.CreateMap<CreateTemplateInput, Template>();
                cfg.CreateMap<UpdateTemplateInput, Template>();
                cfg.CreateMap<Template, GetTemplateOutput>();
                cfg.CreateMap<Template, GetTemplateListOutput>();
                cfg.CreateMap<Template, GetTemplateSelectListOutput>();

                //attach
                cfg.CreateMap<CreateObjAttachInput, ArticleAttach>();
                cfg.CreateMap<UpdateObjAttachInput, ArticleAttach>();
                cfg.CreateMap<ArticleAttach, GetTemplateAttachOutput>();
                cfg.CreateMap<ArticleAttach, GetObjAttachOutput>();

            });//一次性加载所有映射配置
        }

        public static void RegisterLog()
        {
            log4net.Config.XmlConfigurator.Configure(
                new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + "\\log4net.config"));
        }
    }
}