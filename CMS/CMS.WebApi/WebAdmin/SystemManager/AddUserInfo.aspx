<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddUserInfo.aspx.cs" Inherits="CMSSystem.SystemManager.AddUserInfo" %>

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
<body ng-app="myUser" ng-controller="userController">
    <blockquote class="layui-elem-quote">
        添加用户
        <div class="fr" style="position:relative;top:-8px;">
            <input type='button' value='返回' class='layui-btn layui-btn' onclick="back()" />
        </div>
    </blockquote>
    <form name="myForm" class="layui-form layui-form-pane">
        <div class="layui-form-item">
            <label class="layui-form-label">登录名</label>
            <div class="layui-input-inline">
                <input id="loginname" name="loginname" class="layui-input" type="text" placeholder="请输入" autocomplete="off" ng-model='user.loginname' ng-keyup="isDup(user.loginname);" required>
                <span class="valid" ng-show="myForm.loginname.$error.required">登录名不能为空</span>
                <span class="valid" ng-show="myForm.loginname.$error.loginname">登录名已存在</span>
            </div>
        </div>

        <div class="layui-form-item">
            <label class="layui-form-label">密码</label>
            <div class="layui-input-inline">
                <input id="password" name="password" class="layui-input" type="password" placeholder="请输入" autocomplete="off" ng-minlength="6" ng-model='user.password' ng-keyup="ispwd(user.password);" required>
                <span class="valid" ng-show="myForm.password.$error.required">密码不能为空</span>
                <span class="valid" ng-show="myForm.password.$dirty && myForm.password.$error.minlength">密码至少6位</span>
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">确认密码</label>
            <div class="layui-input-inline">
                <input id="repassword" name="repassword" class="layui-input" type="password" placeholder="请输入" autocomplete="off" ng-minlength="6" ng-model='user.repassword' ng-keyup="isrepwd(user.repassword);" required>
                <%--<span class="valid" ng-show="myForm.repassword.$error.required">确认密码不能为空</span>
                <span class="valid" ng-show="myForm.repassword.$error.minlength">确认密码至少6位</span>--%>
                <span class="valid" ng-show="myForm.repassword.$invalid && user.password!=user.repassword">2次输入的密码不同</span>
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">用户姓名</label>
            <div class="layui-input-inline">
                <input id="username" name="username" class="layui-input" type="text" placeholder="请输入" autocomplete="off" ng-minlength="2" ng-model='user.username' required>
                <span class="valid" ng-show="myForm.username.$error.required">用户姓名不能为空</span>
                <span class="valid" ng-show="myForm.username.$dirty && myForm.username.$error.minlength">用户姓名至少2位</span>
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">Email</label>
            <div class="layui-input-inline">
                <input id="email" name="email" class="layui-input" type="text" placeholder="请输入,非必填" autocomplete="off" ng-model='email' ng-pattern="/^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$/">
                <span class="valid" ng-show="myForm.email.$dirty && myForm.email.$error.pattern">Email格式不正确</span>
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">联系电话</label>
            <div class="layui-input-inline">
                <input id="phone" name="phone" class="layui-input" type="text" placeholder="请输入,非必填" autocomplete="off">
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">用户来源</label>
            <div class="layui-input-inline">
                <input id="usersourcefrom" name="usersourcefrom" class="layui-input" type="text" placeholder="请输入,非必填" autocomplete="off">
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">是否启用</label>
            <div class="layui-input-block">
                <input id="status" type="checkbox" name="status" lay-skin="switch" value="0" lay-text="是|否" checked>
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">是否管理员</label>
            <div class="layui-input-block">
                <input id="Type" type="checkbox" name="Type" lay-skin="switch" value="1" lay-text="是|否">
            </div>
        </div>
        <%--<div class="layui-form-item">
            <label class="layui-form-label">IP 段：</label>
            <div class="layui-input-inline">
                <input name="username" class="layui-input" type="text" placeholder="请输入" autocomplete="off" lay-verify="required">---><input name="username" class="layui-input" type="text" placeholder="请输入" lay-verify="required">
            </div>
        </div>--%>
        <div class="layui-form-item" id="roleslist">
            <label class="layui-form-label">角色</label>
            <div id="rolesmsg" class="layui-input-block">
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
        <div class="layui-form-item layui-form-text">
            <label class="layui-form-label">备注</label>
            <div class="layui-input-block">
                <textarea id="remark" name="remark" placeholder="请输入,非必填" class="layui-textarea"></textarea>
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
    <script src="../scripts/md5.js"></script>
    <script src="../scripts/JSManager/SystemManager/AddUserInfo.js"></script>
</body>
</html>
