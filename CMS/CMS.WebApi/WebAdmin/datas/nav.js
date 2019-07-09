var navs = [{
    "title": "后台首页",
    "icon": "fa-globe",
    "href": "HomePage.aspx",
    "spread": false
},
{
    "title": "内容管理",
    "icon": "fa-file-text",
    "spread": false,
    "children": [{
        "title": "栏目管理",
        "icon": "fa-file-text",
        "href": "ContentManager/ColumnManager.aspx"
    }, {
        "title": "内容管理",
        "icon": "fa-bullhorn",
        "href": "ContentManager/ContentListManager.aspx"
    }]
},
{
    "title": "网站管理",
    "icon": "fa-file-text",
    "spread": false,
    "children": [{
        "title": "模版管理",
        "icon": "fa-file-text",
        "href": "WebsiteManager/ModuleManager.aspx"
    }, {
        "title": "日志管理",
        "icon": "fa-bullhorn",
        "href": "WebsiteManager/LogsManager.aspx"
    }, {
        "title": "访问量查询",
        "icon": "fa-bullhorn",
        "href": "WebsiteManager/ModuleEdit.aspx"
    }, {
        "title": "访问量分析",
        "icon": "fa-bullhorn",
        "href": "WebsiteManager/VisitSearch.aspx"
    }]
},
{
    "title": "宣传管理",
    "icon": "fa-cogs",
    "spread": false,
    "children": [{
        "title": "图片链接",
        "icon": "fa-table",
        "href": "PublicityManager/PictureManager.aspx"
    }, {
        "title": "文本链接",
        "icon": "fa-navicon",
        "href": "PublicityManager/TextManager.aspx"
    }, {
        "title": "位置管理",
        "icon": "&#xe628;",
        "href": "PublicityManager/PositionManager.aspx"
    }]
},
{
    "title": "系统管理",
    "icon": "fa-cogs",
    "spread": false,
    "children": [{
        "title": "用户管理",
        "icon": "fa-user",
        "href": "SystemManager/UsersListManager.aspx"
    }, {
        "title": "角色管理",
        "icon": "fa-navicon",
        "href": "SystemManager/RolesListManager.aspx"
    }, {
        "title": "导航管理",
        "icon": "fa-table",
        "href": "SystemManager/NavListManager.aspx"
    }, {
        "title": "系统配置",
        "icon": "fa-table",
        "href": "SystemManager/SysConfigManager.aspx"
    }]
},
{
    "title": "投诉建议",
    "icon": "fa-globe",
    "href": "AdviceManager/Advice.aspx",
    "spread": false
}];