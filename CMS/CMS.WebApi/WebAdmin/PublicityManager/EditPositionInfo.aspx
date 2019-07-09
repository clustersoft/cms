<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditPositionInfo.aspx.cs" Inherits="CMSSystem.PublicityManager.EditPositionInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
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
<body >
    <blockquote class="layui-elem-quote">
        编辑位置
        <div class="fr" style="position:relative;top:-8px;">
            <input type='button' value='返回' class='layui-btn layui-btn' onclick="back()" />
        </div>
    </blockquote>
    <form name="myForm" class="layui-form layui-form-pane" ng-app="myPosition" ng-controller="positionController">
        <div class="layui-form-item">
            <label class="layui-form-label">位置名称</label>
            <div class="layui-input-inline">
                <input id="PublicityCategoryName" name="PublicityCategoryName" class="layui-input" type="text" placeholder="请输入" autocomplete="off" ng-model='PublicityCategoryName' required>
                <span class="valid" ng-show="myForm.PublicityCategoryName.$error.required">位置名称不能为空</span>
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">位置类型</label>
            <div class="layui-input-inline">
                <select id="PublicityTypesID" name="PublicityTypesID" style="width: 220px;"   > 
                </select> 
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">排序</label>
            <div class="layui-input-inline">
                <input id="orderid" name="orderid" class="layui-input" placeholder="请输入" autocomplete="off" ng-model='orderid' type="text" ng-pattern="/^[0-9]*$/"  required>
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
                <button  type="submit" id="refresh" class="layui-btn layui-btn-primary">重置</button>
            </div>
        </div>           
    </form>
    <script src="../scripts/angular.min.js"></script>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/layui/lay/modules/layer.js"></script>
    <script src="../scripts/layui/layui.js"></script>
    <script src="../scripts/model/apiurl.js"></script>
    <script src="../scripts/JSManager/PublicityManager/EditPositionInfo.js"></script>
</body>
</html>
