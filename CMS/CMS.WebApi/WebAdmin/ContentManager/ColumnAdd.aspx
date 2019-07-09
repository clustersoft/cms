<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ColumnAdd.aspx.cs" Inherits="CMSSystem.ContentManager.ColumnAdd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" ng-app="columnyz">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>添加栏目</title>
    <!--附件引用-->
    <link href="../scripts/webuploader/dist/webuploader.css" rel="stylesheet" />
    <link href="../css/styles1.css" rel="stylesheet" />

    <link href="../scripts/layui/css/layui.css" rel="stylesheet" />
    <link href="../css/font-awesome.min.css" rel="stylesheet" />
    <link href="../css/cmsui.css" rel="stylesheet" />
    <link href="../scripts/layui/css/modules/layer/default/layer.css" rel="stylesheet" />
</head>
<body ng-controller="columnbodyyz">
    <blockquote class="layui-elem-quote">添加栏目</blockquote>
    <form name="myForm" class="layui-form layui-form-pane">
        <div style="width: 1200px;">
            <div class="layui-form-item">
                <label class="layui-form-label">栏目名称</label>
                <div class="layui-input-inline">
                    <input id="Name" name="Namelm" class="layui-input" placeholder="栏目名称" autocomplete="off" lay-verify="required" ng-model="Namelm" ng-keyup="isDup(Namelm);" required />
                    <span class="valid" ng-show="myForm.Namelm.$error.required">栏目名称不能为空</span>
                    <span class="valid" ng-show="myForm.Namelm.$error.Namelm">栏目名已存在</span>
                </div>
                <div class="layui-form-mid layui-word-aux">相同父栏目的同一级栏目，名称不能重复</div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">栏目标识码</label>
                <div class="layui-input-inline">
                    <input id="RefNo" name="RefNo" class="layui-input" placeholder="栏目标识码" autocomplete="off" lay-verify="required" ng-model='RefNo' ng-keyup="isDupNo(RefNo);" />
                    <span class="valid" ng-show="myForm.RefNo.$error.RefNo">栏目标识码已存在</span>
                </div>
                <div class="layui-form-mid layui-word-aux">网站前台可以通过引用这个号码获取对应的栏目及内容</div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">父栏目</label>
                <div class="layui-input-inline selectwidth">
                    <input id="ParentID" name="ParentID" class="layui-input" autocomplete="off" disabled />
                    <input type="hidden" id="pid" />
                </div>
                <div class="layui-form-mid layui-word-aux">从左侧栏目选择</div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">是否直链</label>
                <div class="layui-input-block">
                    <input type="checkbox" name="linktype" lay-skin="switch" lay-filter="isLink" lay-text="是|否" value="0" />
                </div>
            </div>
            <div class="layui-form-item" id="linkopen" style="display: none;">
                <label class="layui-form-label">直链地址</label>
                <div class="layui-input-inline">
                    <input id="LinkPath" class="layui-input" placeholder="直链地址" autocomplete="off" lay-verify="required" />
                </div>
            </div>
            <div id="linkclose" style="display: block;">
                <div class="layui-form-item">
                    <label class="layui-form-label">页面模版</label>
                    <div class="layui-input-inline selectwidth">
                        <select id="TemplateID" name="interest" lay-filter="TemplateID" lay-verify="required">
                            <option value="" selected="">请选择模版</option>
                        </select>
                    </div>
                    <div class="layui-form-mid layui-word-aux">如果不选择将继承上一级模版或使用默认模版</div>
                </div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">栏目预览图</label>
                <div class="layui-input-inline">
                    <div id="uploader-demo" class="layui-inline marginleft">
                        <div id="fileList" class="uploader-list"></div>
                        <div id="filePicker" class="layui-inline fl">选择图片</div>
                        <input type="button" id="deletePic" class="layui-btn layui-btn-primary" style="display:none; "  value="删除图片" />
                    </div>
                </div>
            </div>
            <%--<div class="layui-form-item" pane>
                <label class="layui-form-label">是否菜单栏</label>
                <div class="layui-input-block">
                    <input type="checkbox" name="BeCategory" lay-skin="switch" lay-filter="switchTest" lay-text="是|否" value="1" checked />
                </div>
            </div>--%>
            <div class="layui-form-item">
                <label class="layui-form-label">添加内容</label>
                <div class="fl">
                    <input type="checkbox" name="AddArticlePermissions" lay-skin="switch" lay-filter="AddArticle" lay-text="是|否" value="0" />
                </div>
                <div class="layui-form-mid layui-word-aux marginleft">内容管理是否允许添加内容</div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">是否隐藏</label>
                <div class="layui-input-block">
                    <input type="checkbox" name="state" lay-skin="switch" lay-filter="state" lay-text="是|否" value="0" />
                </div>
            </div>
            <div class="layui-form-item layui-form-text">
                <label class="layui-form-label">备注</label>
                <div class="layui-input-block">
                    <textarea id="remark" name="remark" placeholder="请输入备注,非必填" class="layui-textarea"></textarea>
                </div>
            </div>
            <div class="layui-form-item">
                <div class="layui-input-block">
                    <input type="submit" id="submit" class="layui-btn" value="提交" data-ng-class="{true:'layui-btn-disabled',false:''}[myForm.$invalid]" data-ng-disabled="myForm.$invalid" />
                    <button type="reset" class="layui-btn layui-btn-primary">重置</button>
                </div>
            </div>
        </div>
    </form>

    <script src="../scripts/layui/layui.js"></script>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/angular.min.js"></script>
    <script src="../scripts/model/apiurl.js"></script>
    
    <script src="../scripts/webuploader/dist/webuploader.js"></script>
    <script src="../scripts/JSManager/ContentManager/ColumnAdd.js"></script>
</body>
</html>
