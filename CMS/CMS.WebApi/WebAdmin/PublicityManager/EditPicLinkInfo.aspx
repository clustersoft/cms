<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditPicLinkInfo.aspx.cs" Inherits="CMSSystem.PublicityManager.EditPicLinkInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../scripts/webuploader/dist/webuploader.css" rel="stylesheet" />
    <link href="../css/styles1.css" rel="stylesheet" />
    <link href="../scripts/layui/css/layui.css" rel="stylesheet" />
    <link href="../css/font-awesome.min.css" rel="stylesheet" />
    <link href="../css/cmsui.css" rel="stylesheet" />
    <link href="../scripts/layui/css/modules/layer/default/layer.css" rel="stylesheet" />
    <style>
        .valid {
            top: 0px; width: 100%; text-align: right; right: 0px; color: rgb(169, 68, 66); line-height: 38px; padding-right: 20px; padding-bottom: 0px; font-size: 12px; margin-top: 0px; display: block; position: absolute; z-index: 2; pointer-events: none; animation-duration: 0.2s; display:none;
        }
    </style>
</head>
<body ng-app="myPosition" ng-controller="positionController">
    <blockquote class="layui-elem-quote">
        编辑图片链接
        <div class="fr" style="position:relative;top:-8px;">
            <input type='button' value='返回' class='layui-btn layui-btn' onclick="back()" />
        </div>
    </blockquote>
    <form name="myForm" class="layui-form layui-form-pane">
        <div class="layui-form-item">
            <label class="layui-form-label">链接名称</label>
            <div class="layui-input-inline">
                <input id="PublicityName" name="PublicityName" class="layui-input" type="text" placeholder="请输入" autocomplete="off" ng-model='PublicityName' required>
                <span class="valid" ng-show="myForm.PublicityName.$error.required">链接名称不能为空</span>
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">链接位置</label>
            <div class="layui-input-inline">
                <select id="PublicityCategoryID" name="PublicityCategoryID" style="width: 220px;"   > 
                </select>
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">上传图片</label>
            <div class="layui-input-inline">
                <div id="uploader-demo" style=" margin-left:15px;">
                    <!--用来存放item-->
                    <div id="fileList" class="uploader-list"></div>
                    <div>
                    <div id="filePicker" style="float:left;" >选择图片</div>
                    <input type="button" id="deletePic" class="layui-btn layui-btn-primary" style="float:left; margin-left:15px;"  value="删除图片" />
                    </div>
                </div>
            </div>
        </div>

        <div class="layui-form-item">
            <label class="layui-form-label">超链接类型</label>
            <div class="layui-input-inline">
                <input type="radio" id="NavType0" name="NavType" value="0" title="直接链接">
                <input type="radio" id="NavType1" name="NavType" value="1" title="间接链接">
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">链接地址</label>
            <div class="layui-input-inline">
                <input id="NavUrl" name="NavUrl" class="layui-input" type="text" placeholder="请输入" autocomplete="off" ng-model='NavUrl' required>
                <span class="valid" ng-show="myForm.NavUrl.$error.required">链接地址不能为空</span>
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">发布时间</label>
            <div class="fl">
                <input type="radio" id="PublishType0" name="PublishType" value="0" title="不限" lay-filter="pub" checked="">
                <input type="radio" id="PublishType1" name="PublishType" value="1" title="自定义" lay-filter="pub">
            </div>
            <div id="pubdiv" style="display:none">
                <div class="dateTimeWidth fl">
                    <input class="layui-input" placeholder="设置发布时间" name="PublishTime" id="PublishTime" onclick="laydate({ istime: true, format: 'YYYY-MM-DD hh:mm:ss', min: '1900-01-01 00:00:00', max: '9999-12-31 23:59:59', choose: function (dates) { $('#PublishTime').val(getNowFormatTime(dates)); } })" />
                </div>
                <span class="layui-form-mid layui-word-aux marginleft">若不填则默认为当前时间 格式：2018-08-08 08:08:08</span>
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">过期时间</label>
            <div class="fl">
                <input type="radio" id="ExpiredType0" name="ExpiredType" value="0" title="不限" lay-filter="exp" checked="">
                <input type="radio" id="ExpiredType1" name="ExpiredType" value="1" title="自定义" lay-filter="exp">
            </div>
            <div id="expdiv" style="display:none">
                <div class="dateTimeWidth fl">
                    <input class="layui-input" placeholder="设置过期时间" name="ExpiredTime" id="ExpiredTime"  onclick="laydate({ istime: true, format: 'YYYY-MM-DD hh:mm:ss', min: '1900-01-01 00:00:00', max: '9999-12-31 23:59:59', choose: function (dates) { $('#ExpiredTime').val(getNowFormatTime(dates)); } })" />
                </div>
                <span class="layui-form-mid layui-word-aux marginleft">若不填则默认为永不过期 格式：2018-08-08 08:08:08 </span>
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">是否显示</label>
            <div class="layui-input-block">
                <input id="ShowType" type="checkbox" name="ShowType" lay-skin="switch" lay-text="是|否">
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">排序</label>
            <div class="layui-input-inline">
                <input id="orderid" name="orderid" class="layui-input" placeholder="请输入" autocomplete="off" ng-model='orderid' type="text" ng-pattern="/^[0-9]*$/" required>
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
    <script src="../scripts/webuploader/dist/webuploader.js"></script>
    <script src="../scripts/layui/lay/modules/layer.js"></script>
    <script src="../scripts/layui/layui.js"></script>
    <script src="../scripts/model/apiurl.js"></script>
    <script src="../scripts/laydate/laydate.js"></script>
    <script src="../scripts/datepicker.js"></script>
    <script src="../scripts/JSManager/PublicityManager/EditPicLinkInfo.js"></script>
</body>
</html>
