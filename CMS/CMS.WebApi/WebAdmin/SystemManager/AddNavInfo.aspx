<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddNavInfo.aspx.cs" Inherits="CMSSystem.SystemManager.AddNavInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../scripts/layui/css/layui.css" rel="stylesheet" />
    <link href="../css/global.css" rel="stylesheet" />
    <link href="../css/font-awesome.min.css" rel="stylesheet" />
    <link href="../css/cmsui.css" rel="stylesheet" />
    <link href="../scripts/layui/css/modules/layer/default/layer.css" rel="stylesheet" />
    <style>
        .valid {
            top: 0px; width: 100%; text-align: right; right: 0px; color: rgb(169, 68, 66); line-height: 38px; padding-right: 20px; padding-bottom: 0px; font-size: 12px; margin-top: 0px; display: block; position: absolute; z-index: 2; pointer-events: none; animation-duration: 0.2s; display:none;
        }
    </style>
</head>
<body ng-app="myNav" ng-controller="navController">
    <blockquote class="layui-elem-quote">
        添加导航
        <div class="fr" style="position:relative;top:-8px;">
            <input type='button' value='返回' class='layui-btn layui-btn' onclick="javascript:window.history.back();" />
        </div>
    </blockquote>
    <form name="myForm" class="layui-form layui-form-pane">
        <div class="layui-form-item">
            <label class="layui-form-label">上级导航</label>
            <div class="layui-input-inline">
                <input id="parentname" name="parentname" class="layui-input layui-disabled" disabled="disabled" type="text">
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">导航标识码</label>
            <div class="layui-input-inline">
                <input id="navname" name="navname" class="layui-input" type="text" placeholder="请输入" autocomplete="off" ng-model='user.navname' ng-pattern="/^[A-Za-z0-9]+$/" ng-keyup="isDup(user.navname);" required>
                <span class="valid" ng-show="myForm.navname.$error.required">导航标识码不能为空</span>
                <span class="valid" ng-show="myForm.navname.$error.navname">导航标识码已存在</span>
                <span class="valid" ng-show="myForm.navname.$dirty && myForm.navname.$error.pattern">请输入数字或字母</span>
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">导航名称</label>
            <div class="layui-input-inline">
                <input id="navtitle" name="navtitle" class="layui-input" type="text" placeholder="请输入" autocomplete="off" ng-model='user.navtitle' required>
                <span class="valid" ng-show="myForm.navtitle.$error.required">导航名称不能为空</span>
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">图标</label>
            <div class="layui-input-block">
                    <div class="layui-collapse" lay-filter="test">
                        <div class="layui-colla-item">
                            <h2 class="layui-colla-title" style="height:36px; line-height:36px;">图标选择</h2>
                            <div class="layui-colla-content" style="padding: 5px;" id="iconhtml">
                                    
                            </div>
                        </div>
                    </div>
                <%--<input id="iconurl" name="iconurl" class="layui-input" type="text" placeholder="请输入，非必填" autocomplete="off">--%>
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">链接地址</label>
            <div class="layui-input-inline">
                <input id="linkurl" name="linkurl" class="layui-input" type="text" placeholder="请输入，非必填" autocomplete="off">
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">操作权限</label>
            <div id="actionmsg" class="layui-input-block">
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">排序</label>
            <div class="layui-input-inline">
                <input id="orderid" name="orderid" class="layui-input" placeholder="请输入" autocomplete="off" ng-model='orderid' ng-init="orderid='99'" type="text" ng-pattern="/^[0-9]*$/"  required>
                <span class="valid" ng-show="myForm.orderid.$error.required">排序不能为空</span>
                <span class="valid" ng-show="myForm.orderid.$dirty && myForm.orderid.$error.pattern">请输入数字</span>
            </div>
        </div>
        <div class="layui-form-item">
            <div class="layui-input-block">
                <input class="layui-btn layui-btn" data-ng-class="{true:'layui-btn-disabled',false:''}[myForm.$invalid]" data-ng-disabled="myForm.$invalid" type="submit" id="submit" value="提交" />
                <button type="reset" class="layui-btn layui-btn-primary">重置</button>
            </div>
        </div>
    </form>
    <script src="../scripts/angular.min.js"></script>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/layui/lay/modules/layer.js"></script>
    <script src="../scripts/layui/layui.js"></script>
    <script src="../scripts/model/apiurl.js"></script>
    <script src="../scripts/JSManager/SystemManager/AddNavInfo.js"></script>
</body>
</html>
