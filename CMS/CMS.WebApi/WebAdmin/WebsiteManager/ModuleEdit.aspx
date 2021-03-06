﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModuleEdit.aspx.cs" Inherits="CMSSystem.WebsiteManager.ModuleEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>编辑模版</title>
    <!--附件引用-->
    <link href="../scripts/webuploader/dist/webuploader.css" rel="stylesheet" />
    <link href="../css/styles1.css" rel="stylesheet" />

    <link href="../scripts/layui/css/layui.css" rel="stylesheet" />
    <link href="../css/font-awesome.min.css" rel="stylesheet" />
    <link href="../css/cmsui.css" rel="stylesheet" />
</head>
<body>
    <blockquote class="layui-elem-quote">编辑模版</blockquote>
    <form id="myform" name="myForm" class="layui-form layui-form-pane" ng-app="myModule" ng-controller="moduleController">
        <div style="width: 1200px;">
            <div class="layui-form-item">
                <label class="layui-form-label">模版名称</label>
                <div class="layui-input-inline">
                    <input id="Name" name="modName" class="layui-input" placeholder="模版名称" autocomplete="off" ng-model='modName' required />
                    <span class="valid" ng-show="myForm.modName.$error.required">标题不能为空</span>
                </div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">保存路径</label>
                <div class="layui-input-inline">
                    <%--<input id="Path" name="Path" class="layui-input" placeholder="保存路径" autocomplete="off" lay-verify="required" required />--%>
                    <input id="Path" name="Path" class="layui-input" placeholder="保存路径" autocomplete="off" lay-verify="required" ng-model='Path' required />
                    <span class="valid" ng-show="myForm.Path.$error.required">路径不能为空</span>
                </div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">模版文件</label>
                <div class="layui-input-block">
                    <div id="uploader-demo" class="marginleft">
                        <%--<div id="fileList" class="uploader-list"></div>--%>
                         <div id="filePicker" class="layui-inline fl">选择文件</div>
                        <input type="button" id="ctlBtn" class="layui-btn layui-btn-primary" value="开始上传" />
                        <%--<input type="button" id="deletePic" class="layui-btn layui-btn-primary" style="display:none; "  value="删除图片" />--%>
                         <div id="fileList" class="uploader-list">
                            <table class="layui-table" lay-skin="line" id="tbl" style="margin: 0 0 10px 0; display: none;">
                                <colgroup>
                                    <col />
                                    <col width="200" />
                                    <col width="200" />
                                </colgroup>
                                <thead>
                                    <tr style="height: 37px;">
                                        <th style="text-align: center;padding: 0; ">名称</th>
                                        <th style="text-align: center;padding: 0; ">进度</th>
                                        <th style="text-align: center;padding: 0; ">操作</th>
                                    </tr>
                                </thead>
                                <tbody id="msg">
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">排序</label>
                <div class="layui-input-inline">
                    <input id="OrderID" name="Title" class="layui-input" placeholder="排序" autocomplete="off" lay-verify="required" required value="99" />
                </div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">是否停用</label>
                <div class="fl">
                    <input type="radio" name="UseAble" value="0" title="否" lay-filter="UseAble" checked />
                    <input type="radio" name="UseAble" value="1" title="是" lay-filter="UseAble" />
                    <input type="hidden" id="UseAble" value="0" />
                </div>
                <div class="layui-form-mid layui-word-aux marginleft">停用模版仅在模板管理中可见 </div>
            </div>
            <div class="layui-form-item">
                <div class="layui-input-block">
                    <input type="submit" id="submit" class="layui-btn" value="保存" data-ng-class="{true:'layui-btn-disabled',false:''}[myForm.$invalid]" data-ng-disabled="myForm.$invalid" />
                    <input type="button" id="back" class="layui-btn" value="返回" onclick="backs()" />
                </div>
            </div>
        </div>
    </form>
    <script src="../scripts/angular.min.js"></script>    
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/model/apiurl.js"></script>
    <script src="../scripts/layer/layer.js"></script>
    <script src="../scripts/layui/layui.js"></script>
    <script src="../scripts/webuploader/dist/webuploader.js"></script>
    <script src="../scripts/JSManager/WebsiteManager/ModuleEdit.js"></script>
</body>
</html>
