<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditRoleInfo.aspx.cs" Inherits="CMSSystem.SystemManager.EditRoleInfo" %>

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
            top: 0px;
            width: 100%;
            text-align: right;
            right: 0px;
            color: rgb(169, 68, 66);
            line-height: 38px;
            padding-right: 20px;
            padding-bottom: 0px;
            font-size: 12px;
            margin-top: 0px;
            display: block;
            position: absolute;
            z-index: 2;
            pointer-events: none;
            animation-duration: 0.2s;
            display: none;
        }
    </style>
</head>
<body ng-app="myRole" ng-controller="roleController">
    <blockquote class="layui-elem-quote">
        编辑角色
        <div class="fr" style="position: relative; top: -8px;">
            <input type='button' value='返回' class='layui-btn layui-btn' onclick="back()" />
        </div>
    </blockquote>
    <form name="myForm" class="layui-form layui-form-pane">
        <div class="layui-form-item">
            <label class="layui-form-label">角色名称</label>
            <div class="layui-input-inline">
                <input id="rolename" name="rolename" class="layui-input" type="text" placeholder="请输入" autocomplete="off" ng-model='rolename' ng-keyup="isDup(rolename);" required>
                <span class="valid" ng-show="myForm.rolename.$error.required">角色名称不能为空</span>
                <span class="valid" ng-show="myForm.rolename.$error.rolename">角色名称已存在</span>
            </div>
        </div>


        <div class="layui-form-item">
            <div class="layui-collapse" lay-filter="test">
                <div class="layui-colla-item">
                    <h2 class="layui-colla-title">导航权限设置</h2>
                    <div class="layui-colla-content" style="padding: 5px;">

                        <!--导航权限-->
                        <table class="layui-table" style="margin: 0;">
                            <colgroup>
                                <col width="30%">
                                <col>
                            </colgroup>
                            <thead>
                                <tr>
                                    <th style="text-align: center">导航名称</th>
                                    <th name='rolecheck' id="navallcheck">
                                        <%--<input type='checkbox' name='navchk' value='on' lay-filter="all_show" title='显示全选' /><input type='checkbox' name='navchk' value='on' lay-filter='all_add' title='添加全选' /><input type='checkbox' name='navchk' value='on' lay-filter="all_edit2" title='编辑全选' /><input type='checkbox' name='navchk' value='on' lay-filter='all_delete' title='删除全选' /><input type='checkbox' name='navchk' value='on' lay-filter='all_save' title='保存全选' />--%></th>
                                </tr>
                            </thead>
                            <tbody id="navroles">
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <div class="layui-form-item">
            <div class="layui-collapse" lay-filter="test2">
                <div class="layui-colla-item">
                    <h2 class="layui-colla-title">栏目权限设置</h2>
                    <div class="layui-colla-content" style="padding: 5px;">

                        <!--栏目权限-->
                        <table class="layui-table" style="margin: 0;">
                            <colgroup>
                                <col width="30%">
                                <col>
                            </colgroup>
                            <thead>
                                <tr>
                                    <th style="text-align: center">栏目名称</th>
                                    <th name='rolecheck' id="cateallcheck">
                                        <%--<input type='checkbox' name='catechk' value='on' lay-filter="all_edit" title='编辑全选' /><input type='checkbox' name='catechk' value='on' lay-filter='all_audit' title='审核全选' />--%></th>
                                </tr>
                            </thead>
                            <tbody id="cateroles">
                            </tbody>
                        </table>
                    </div>
                </div>

            </div>
        </div>


        <div class="layui-form-item">
            <label class="layui-form-label">排序</label>
            <div class="layui-input-inline">
                <input id="orderid" name="orderid" class="layui-input" type="text" placeholder="请输入" autocomplete="off" ng-model='orderid' ng-pattern="/^[0-9]*$/" required>
                <span class="valid" ng-show="myForm.orderid.$dirty && myForm.orderid.$invalid &&myForm.orderid.$error.required">排序不能为空</span>
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
                <button type="submit" id="refresh" class="layui-btn layui-btn-primary">重置</button>
            </div>
        </div>
    </form>
    <script src="../scripts/angular.min.js"></script>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/layui/lay/modules/layer.js"></script>
    <script src="../scripts/layui/layui.js"></script>
    <script src="../scripts/model/apiurl.js"></script>
    <script src="../scripts/JSManager/SystemManager/EditRoleInfo.js"></script>
</body>
</html>
