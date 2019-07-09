<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysConfigManager.aspx.cs" Inherits="CMSSystem.SystemManager.SysConfigManager" %>

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
<body>
    <blockquote class="layui-elem-quote">系统配置</blockquote>
    <form name="myForm" class="layui-form layui-form-pane" ng-app="mySys" ng-controller="sysController">

        
<div class="layui-tab layui-tab-brief" lay-filter="docDemoTabBrief">
  <ul class="layui-tab-title">
    <li class="layui-this">基础设置</li>
    <li>附件配置</li>
    <li>图片裁剪</li>
  </ul>
  <div class="layui-tab-content">
    <div class="layui-tab-item layui-show">
        <div class="layui-form-item">
            <label class="layui-form-label">系统名称</label>
            <div class="layui-input-inline">
                <input id="Title" name="Title" class="layui-input" type="text" placeholder="请输入" autocomplete="off" ng-model='Title' required>
                <span class="valid" ng-show="myForm.Title.$error.required">系统名称不能为空</span>
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">分页数量</label>
            <div class="layui-input-inline">
                <input id="PageSizes" name="PageSizes" class="layui-input" type="text" placeholder="请输入" autocomplete="off" ng-model='PageSizes' required>
                <span class="valid" ng-show="myForm.PageSizes.$error.required">分页数量不能为空</span>
            </div>
        </div>

    </div>
    <div class="layui-tab-item">
        <div class="layui-form-item">
            <label class="layui-form-label">上传路径</label>
            <div class="layui-input-inline">
                <input id="UploadPath" name="UploadPath" class="layui-input" type="text" placeholder="请输入" autocomplete="off" ng-model='UploadPath' required>
                <span class="valid" ng-show="myForm.UploadPath.$error.required">上传路径不能为空</span>
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">图片格式</label>
            <div class="layui-input-inline">
                <input id="ImgFormat" name="ImgFormat" class="layui-input" type="text" placeholder="请输入" autocomplete="off" ng-model='ImgFormat' required>
                <span class="valid" ng-show="myForm.ImgFormat.$error.required">图片格式不能为空</span>
            </div>
                <span class="layui-form-mid layui-word-aux marginleft">只需填写后缀名并以逗号“,”隔开</span>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">视频格式</label>
            <div class="layui-input-inline">
                <input id="VideoFormat" name="VideoFormat" class="layui-input" type="text" placeholder="请输入" autocomplete="off" ng-model='VideoFormat' required>
                <span class="valid" ng-show="myForm.VideoFormat.$error.required">视频格式不能为空</span>
            </div>
                <span class="layui-form-mid layui-word-aux marginleft">只需填写后缀名并以逗号“,”隔开</span>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">语音格式</label>
            <div class="layui-input-inline">
                <input id="SpeakFormat" name="SpeakFormat" class="layui-input" type="text" placeholder="请输入" autocomplete="off" ng-model='SpeakFormat' required>
                <span class="valid" ng-show="myForm.SpeakFormat.$error.required">语音格式不能为空</span>
            </div>
                <span class="layui-form-mid layui-word-aux marginleft">只需填写后缀名并以逗号“,”隔开</span>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">附件格式</label>
            <div class="layui-input-inline">
                <input id="AttachFormat" name="AttachFormat" class="layui-input" type="text" placeholder="请输入" autocomplete="off" ng-model='AttachFormat' required>
                <span class="valid" ng-show="myForm.AttachFormat.$error.required">附件格式不能为空</span>
            </div>
                <span class="layui-form-mid layui-word-aux marginleft">只需填写后缀名并以逗号“,”隔开</span>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">图片大小</label>
            <div class="layui-input-inline">
                <input id="MaxImgKB" name="MaxImgKB" class="layui-input" type="text" placeholder="请输入" autocomplete="off" ng-model='MaxImgKB' required>
                <span class="valid" ng-show="myForm.MaxImgKB.$error.required">图片大小不能为空</span>
            </div>
                <span class="layui-form-mid layui-word-aux marginleft">单位：KB</span>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">附件大小</label>
            <div class="layui-input-inline">
                <input id="MaxAttachKB" name="MaxAttachKB" class="layui-input" type="text" placeholder="请输入" autocomplete="off" ng-model='MaxAttachKB' required>
                <span class="valid" ng-show="myForm.MaxAttachKB.$error.required">附件大小不能为空</span>
            </div>
                <span class="layui-form-mid layui-word-aux marginleft">单位：KB</span>
        </div>
    </div>
    <div class="layui-tab-item"><div class="layui-form-item">
            <label class="layui-form-label">启用裁剪</label>
            <div class="layui-input-block">
                <input id="IsCutImg" type="checkbox" name="IsCutImg" lay-skin="switch" lay-text="是|否" >
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">图片高度</label>
            <div class="layui-input-inline">
                <input id="MaxResolutionHeight" name="MaxResolutionHeight" class="layui-input" type="text" placeholder="请输入" autocomplete="off">
            </div>
        </div>
        <div class="layui-form-item">
            <label class="layui-form-label">图片宽度</label>
            <div class="layui-input-inline">
                <input id="MaxResolutionWidth" name="MaxResolutionWidth" class="layui-input" type="text" placeholder="请输入" autocomplete="off" >
            </div>
        </div></div>
  </div>
</div> 
        
        
        <div class="layui-form-item">
            <div class="layui-input-block">
                <input class="layui-btn layui-btn" data-ng-class="{true:'layui-btn-disabled',false:''}[myForm.$invalid]" data-ng-disabled="myForm.$invalid" type="submit" id="submit" value="提交" />
            </div>
        </div>        
    </form>
    <script src="../scripts/angular.min.js"></script>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/layui/lay/modules/layer.js"></script>
    <script src="../scripts/layui/layui.js"></script>
    <script src="../scripts/model/apiurl.js"></script>
    <script src="../scripts/JSManager/SystemManager/SysConfigManager.js"></script>
</body>
</html>
