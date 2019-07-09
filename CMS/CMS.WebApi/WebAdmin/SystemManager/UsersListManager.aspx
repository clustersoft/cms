<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UsersListManager.aspx.cs" Inherits="CMSSystem.SystemManager.UsersListManager" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../scripts/layui/css/modules/layer/default/layer.css" rel="stylesheet" />
    <link href="../scripts/layui/css/layui.css" rel="stylesheet" />
    <link href="../css/cmsui.css" rel="stylesheet" />
    <link href="../css/font-awesome.min.css" rel="stylesheet" />
</head>
<body>
    <blockquote class="layui-elem-quote fhui-admin-main_hd">
        <h2>用户管理</h2>
    </blockquote>
    <div class="layui-form layui-form-pane">
        <div class="head-box">
            <span class="fl">
                <a class="layui-btn btn-default btn-add" id="add" onclick="add()"><i class="fa fa-plus"></i>&nbsp;新增</a>
                <a class="layui-btn btn-default layui-btn-danger" id="del"><i class="fa fa-trash-o"></i>&nbsp;删除</a>
            </span>
            <span class="fr">
                <div class="layui-input-inline keywordsWidth">
                    <input id="skey" name="skey" value="" autocomplete="off" placeholder="请输入关键字" class="layui-input" type="text">
                </div>
                <a class="layui-btn layui-btn-normal" id="cx"><i class="fa fa-search"></i>&nbsp;查询</a>
            </span>
        </div>
    </div>

    <%--<form id="form1" runat="server" class="layui-form layui-form-pane">--%>
        <div class="layui-form">
            <table class="layui-table">
                <colgroup>
                    <col min-width="50" width="50" name="choose">
                    <col>
                    <col>
                    <col>
                    <col>
                    <col min-width="110">
                    <col min-width="180">
                    <col min-width="80">
                    <col min-width="100" name='edit'>
                </colgroup>
                <thead id="theadmsg">
                    
                </thead>
                <tbody id="msg">
                </tbody>
            </table>
        </div>
       <div style="display:none;" id="allpage">
            <div style="font-size:12px; color:#666;display:inline-block; line-height:34px;">共<label id="totalcount"></label>条记录，共<label id="pagecount"></label>页</div>
            <div id="laypages"  style="display:inline-block; line-height:34px;"></div>
        </div>
    <%--</form>--%>

    <link href="../scripts/laypage/skin/laypage.css" rel="stylesheet" />
    <script src="../scripts/laypage/laypage.js"></script>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/layer/layer.js"></script>
    <script src="../scripts/layui/layui.js"></script>
    <script src="../scripts/model/base.js"></script>
    <script src="../scripts/model/apiurl.js"></script>
    <script src="../scripts/bootstrap/icheck.min.js"></script>
    <script src="../scripts/JSManager/SystemManager/UsersListManager.js"></script>
</body>
</html>
