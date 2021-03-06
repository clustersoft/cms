﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainIndex.aspx.cs" Inherits="CMSSystem.MainIndex" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
     <link href="scripts/layui/css/layui.css" rel="stylesheet" />
    <link href="css/global.css" rel="stylesheet" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <link href="scripts/layui/css/modules/layer/default/layer.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">       
        <div class="layui-layout layui-layout-admin">
             <!--顶部导航栏start-->
            <div class="layui-header header header-admin">
                <div class="layui-main">
                    <div class="admin-login-box">
                        <a class="logo" style="left: 0;" href="#">
                            <span style="font-size: 22px; color: white;" id="Title"></span>
                        </a>
                    </div>

                    <ul class="layui-nav">
                        <li class="layui-nav-item">
                            <a href="javascript:;" class="admin-header-user">
                                <span>账号管理</span>
                            </a>
                            <dl class="layui-nav-child">
                                <dd>
                                    <a href="javascript:;" onclick="PersonInfo();"><i class="fa fa-user-circle"></i> 个人信息</a>
                                </dd>
                                <dd>
                                    <a href="javascript:;" onclick="ChangePWD();"><i class="fa fa-gear" aria-hidden="true"></i> 修改密码</a>
                                </dd>
                            </dl>
                        </li>
                        <li class="layui-nav-item">
                            <a  href="javascript:;" onclick="loginout();"><i class="fa fa-sign-out"></i> 退出</a>
                        </li>
                    </ul>
                </div>
            </div>            
            <!--顶部导航栏end-->
            <!--侧翼导航栏start-->
            <div class="layui-side layui-bg-black" id="sidebar-side">
                <div class="layui-side-scroll" id="admin-navbar-side" lay-filter="side">
                </div>
            </div>
            <!--侧翼导航栏end-->
            <%--admin-body:显隐侧翼导航栏--%>
            <div class="layui-body" style="bottom: 0;border-left: solid 2px #1AA094; overflow:hidden;" id="admin-body">
				<div class="layui-tab admin-nav-card layui-tab-brief" lay-filter="admin-tab">
					<ul class="layui-tab-title">
                        <!--默认标签--后台首页显示-->
						<%--<li class="layui-this">
							<i class="fa fa-globe" aria-hidden="true"></i>
							<cite>后台首页</cite>
						</li>--%>
					</ul>
					<div class="layui-tab-content" style="min-height: 150px; padding: 5px 8px 5px 0 ;">
						<%--<div class="layui-tab-item layui-show">
							<iframe src="HomePage.aspx" data-id="-99"></iframe>
						</div>--%>
					</div>
				</div>
			</div>
            <!--c end-->
        </div>
    </form>
    <script src="scripts/jquery-1.10.2.min.js"></script>   
    <script src="scripts/layui/lay/modules/layer.js"></script>
    <script src="scripts/layui/layui.js"></script>
    <script src="scripts/model/apiurl.js"></script>
    <script>
        var navs;
        var userid = GetCMSData().CMSUserID;

        if (userid!=null) {
            crossDomainAjax("/api/permission/list?UserID=" + userid, successCallback, errorCallback);
        }
        crossDomainAjax("/api/sysconfig/getInfo", SysCallback, ErrorCallback);
        function SysCallback(data, index) {
            if (data.Success == "1") {
                $("#Title").append(data.Result.Title);
                document.title = data.Result.Title;
            } else {
                layer.msg("数据请求失败！");
                return false;
            }
            layer.closeAll('loading');
        }
        function successCallback(data, index) {
            layer.close(index);
            var list = eval(data.Result);
            navs = GetNavs(list);
            dynamicLoading.js("scripts/index.js");
            dynamicLoading.js("scripts/navbar.js");
            dynamicLoading.js("scripts/tab.js");
        }
        function errorCallback(data, index) {
            layer.close(index);
        }
        function PersonInfo() {
            layer.open({
                type: 2,
                area: ['600px', '350px'],
                fixed: false, //不固定
                maxmin: true,
                title: "修改个人信息",
                content: 'SystemManager/PersonInfo.html'
            });
        }
        function ChangePWD() {
            layer.open({
                type: 2,
                area: ['600px', '300px'],
                fixed: false, //不固定
                maxmin: true,
                title: "修改密码",
                content: 'SystemManager/ChangePWD.html'
            });
        }

        //载入后台首页
        layui.use(['element'], function () {
            var element = layui.element();
            element.tabAdd('admin-tab', {
                title: '<i class="fa fa-home" aria-hidden="true"></i> 后台首页 ',
                content: '<iframe src="HomePage.aspx" data-id="-99"></iframe>',
                id: -99
            });
            element.tabChange('admin-tab', -99);    
        });
    </script>
</body>
</html>
