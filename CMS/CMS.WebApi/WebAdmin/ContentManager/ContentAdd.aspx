<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContentAdd.aspx.cs" Inherits="CMSSystem.ContentManager.ContentAdd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" ng-app="myContent">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>添加内容</title>
    <!--附件引用-->
    <link href="../scripts/webuploader/dist/webuploader.css" rel="stylesheet" />
    <link href="../css/styles1.css" rel="stylesheet" />
    <link href="../scripts/layui/css/layui.css" rel="stylesheet" />
    <link href="../css/font-awesome.min.css" rel="stylesheet" />
    <link href="../css/cmsui.css" rel="stylesheet" />
</head>
<body ng-controller="contentController">
    <form id="myForm" name="myForm" class="layui-form layui-form-pane">
        <blockquote class="layui-elem-quote">
            添加新内容
            <div class="fr" style="position: relative; top: -8px;">
                <input type="button" id="backtop" class="layui-btn" value="返回" onclick="javascript: window.location.href = 'ContentListManager.aspx?backsource=1'" />
            </div>
        </blockquote>
        <div style="width: 1200px;">
            <div class="layui-form-item">
                <label class="layui-form-label">标题</label>
                <div class="layui-input-inline">
                    <input id="Title" name="Title" class="layui-input" placeholder="标题" autocomplete="off" ng-model="Title" required />
                    <span class="valid" ng-show="myForm.Title.$error.required">标题不能为空</span>
                </div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">栏目</label>
                <div class="layui-input-inline">
                    <input id="Column" name="Column" class="layui-input" placeholder="" autocomplete="off" lay-verify="required" required onclick="layeropen()" readonly="readonly" ng-model="Column" ng-focus="LMcheck(Column);" />
                    <input id="hidColumn" type="hidden" />
                    <span class="valid" ng-show="myForm.Column.$error.required">栏目不能为空</span>
                </div>
                <input type="button" id="btnChoose" value="选择" class="layui-btn" />
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">是否置顶</label>
                <div class="layui-input-inline">
                    <input type="checkbox" title="置顶" lay-filter="issticktop" name="issticktop" id="issticktop" value="0">
                    <%--<input type="checkbox" name="like[intro]" title="推荐">--%>
                </div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">内容</label>
                <div class="layui-input-block">
                    <input type="radio" name="content" value="0" title="输入内容" lay-filter="content" checked>
                    <input type="radio" name="content" value="1" title="使用直接链接" lay-filter="content">
                    <input type="hidden" id="contentvalue" value="0" />
                </div>
            </div>
            <div class="layui-form-item" id="divpath" style="display: none;">
                <label class="layui-form-label">直链地址</label>
                <div class="layui-input-inline">
                    <input id="LinkPath" name="LinkPath" class="layui-input" placeholder="直链地址" autocomplete="off" lay-verify="required" ng-model="LinkPath"  />
                    <span class="valid" ng-show="myForm.LinkPath.$error.required">直链地址不能为空</span>
                </div>
            </div>
            <div class="layui-form-item" id="diveditor">
                <label class="layui-form-label">输入内容</label>
                <div class="layui-input-block">
                    <div id="container" name="txtcontent" style="height: 300px;">
                    </div>
                </div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">内容来源</label>
                <div class="layui-input-inline">
                    <input id="ContentSource" class="layui-input" placeholder="内容来源" autocomplete="off" lay-verify="required" />
                </div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">选择导航图</label>
                <div class="layui-input-block">
                    <div id="uploader-demo" class="layui-inline marginleft">
                        <div id="fileList" class="uploader-list"></div>
                        <div id="filePicker" class="layui-inline fl">选择图片</div>
                        <%--<input type="button" id="ctlBtn" class="layui-btn layui-btn-primary" value="开始上传" style="position: relative; top: -5px;" />--%>
                        <input type="button" id="deletePic" class="layui-btn layui-btn-primary" style="display: none;" value="删除图片" />
                    </div>
                </div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">添加附件</label>
                <div class="layui-input-block">
                    <div id="uploader-demo2" class="marginleft">
                        <div id="filePicker2" class="layui-inline fl">选择文件</div>
                        <input type="button" id="ctlBtn2" class="layui-btn layui-btn-primary" value="开始上传" />
                        <div id="fileList2" class="uploader-list">
                            <table class="layui-table" lay-skin="line" id="tbl" style="margin: 0 0 10px 0; display: none;">
                                <colgroup>
                                    <col />
                                    <col width="200" />
                                    <col width="200" />
                                </colgroup>
                                <thead>
                                    <tr style="height: 37px;">
                                        <th style="text-align: center; padding: 0;">名称</th>
                                        <th style="text-align: center; padding: 0;">进度</th>
                                        <th style="text-align: center; padding: 0;">操作</th>
                                    </tr>
                                </thead>
                                <tbody id="msg">
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="layui-form-item  layui-form-text">
                <label class="layui-form-label">摘要</label>
                <div class="layui-input-block">
                    <textarea id="Summary" name="Summary" placeholder="摘要内容" class="layui-textarea" maxlength="500"></textarea>
                </div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">发布时间</label>
                <div class="fl">
                    <input type="radio" name="Pub" value="0" title="立即发布" lay-filter="pub" checked />
                    <input type="radio" name="Pub" value="1" title="设置发布时间" lay-filter="pub" />
                    <input type="hidden" id="pub" value="0" />
                </div>
                <div class="dateTimeWidth fl">
                    <input class="layui-input" placeholder="设置发布时间" id="PubTime" onclick="laydate({ istime: true, istoday: false, format: 'YYYY-MM-DD hh:mm:ss', choose: function (dates) { $('#PubTime').val(getNowFormatTime(dates)); } })" disabled />
                </div>
                <span class="layui-form-mid layui-word-aux marginleft">格式：2018-08-08 08:08:08 </span>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">过期时间</label>
                <div class="fl">
                    <input type="radio" name="guoqi" value="0" title="永不过期" lay-filter="gq" checked />
                    <input type="radio" name="guoqi" value="1" title="设置过期时间" lay-filter="gq" />
                    <input type="hidden" id="gq" value="0" />
                </div>
                <div class="dateTimeWidth fl">
                    <input class="layui-input" placeholder="设置过期时间" id="GQTime" onclick="laydate({ istime: true, istoday: false, format: 'YYYY-MM-DD hh:mm:ss', choose: function (dates) { $('#GQTime').val(getNowFormatTime(dates)); } })" disabled />
                </div>
                <div class="layui-form-mid layui-word-aux marginleft">格式：2018-08-08 08:08:08 </div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">排序优先级</label>
                <div class="layui-input-inline">
                    <input id="OrderID" class="layui-input" placeholder="排序优先级" autocomplete="off" lay-verify="required" value="99" />
                </div>
                <div class="layui-form-mid layui-word-aux">优先级可调范围：1~99999，越小越靠前。</div>
            </div>
            <%--<div class="layui-form-item">
                <label class="layui-form-label">IP段限制</label>
                <div class="layui-input-inline">
                    <input id="IPStart" class="layui-input fl" placeholder="IP段起始" autocomplete="off" style="width:150px;" />
                    <span class="layui-form-mid fl">&nbsp;&nbsp;---></span>
                    <input id="IPEnd" class="layui-input fl" placeholder="IP段结束" autocomplete="off" style="width:150px;" />
                    <input class="layui-btn layui-btn-primary fl" value="加入" style="width:100px;"  />
                </div>
                <div class="layui-form-mid layui-word-aux">不填则默认所有IP段可见</div>
            </div>--%>
            <div class="layui-form-item">
                <label class="layui-form-label">文章点击率</label>
                <div class="layui-input-inline">
                    <label class="layui-form-label">0</label>
                </div>
            </div>
            <div class="layui-form-item">
                <div class="layui-input-block">
                    <input type="button" id="pauseSave" class="layui-btn" value="暂存" data-ng-class="{true:'layui-btn-disabled',false:''}[myForm.$invalid]" data-ng-disabled="myForm.$invalid" />
                    <input type="button" id="submit" class="layui-btn" value="提交" data-ng-class="{true:'layui-btn-disabled',false:''}[myForm.$invalid]" data-ng-disabled="myForm.$invalid" />
                    <button type="reset" class="layui-btn layui-btn-primary">重置</button>
                    <input type="button" id="back" class="layui-btn" value="返回" onclick="javascript: window.location.href = 'ContentListManager.aspx?backsource=1'" />
                </div>
            </div>
        </div>
        <script src="../scripts/angular.min.js"></script>
        
        <script src="../scripts/jquery-1.10.2.min.js"></script>
        <script src="../scripts/layer/layer.js"></script>
        <script src="../scripts/layui/layui.js"></script>
        <script src="../scripts/model/apiurl.js"></script>
        <script src="../scripts/laydate/laydate.js"></script>
        <script src="../scripts/datepicker.js"></script>
        <script src="../scripts/webuploader/dist/webuploader.js"></script>

        <!-- 配置文件 -->
        <script src="../scripts/UEditor/ueditor.config.js"></script>
        <!-- 编辑器源码文件 -->
        <script src="../scripts/UEditor/ueditor.all.js"></script>
        <!-- 实例化编辑器 -->
        <script src="../scripts/JSManager/ContentManager/ContentAdd.js"></script>
    </form>
</body>
</html>
