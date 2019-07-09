<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ArticleWshList.aspx.cs" Inherits="CMSSystem.ContentManager.ArticleWshList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>待审核列表</title>
    <link href="../scripts/layui/css/layui.css" rel="stylesheet" />
    <link href="../css/cmsui.css" rel="stylesheet" />
    <link href="../css/font-awesome.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server" class="layui-form layui-form-pane">
        <blockquote class="layui-elem-quote">
            待审核列表
            <div class="fr" style="position: relative; top: -8px;">
                <input type="button" id="backtop" class="layui-btn" value="返回" onclick="window.location.href = '../HomePage.aspx'" /><%--javascript: window.history.back(-1);--%>
            </div>
        </blockquote>
        <table id="dateTable" class="layui-table" style="width: 99%;">
            <colgroup>
                <col width="8%" />
                <col width="25%" />
                <col width="15%" />
                <col width="10%" />
                <col width="10%" />
                <col width="20%" />
                <col width="10%" />
            </colgroup>
            <thead>
                <tr>
                    <th style="text-align: center;">序号</th>
                    <th style="text-align: center;">标题</th>
                    <th style="text-align: center;">栏目</th>
                    <th style="text-align: center;">状态</th>
                    <th style="text-align: center;">撰稿人</th>
                    <th style="text-align: center;">发布日期</th>
                    <th style="text-align: center;">操作</th>
                </tr>
            </thead>
            <tbody id="msg">
            </tbody>
        </table>
         <div style="display: none;" id="allpage">
                        <div style="font-size: 12px; color: #666; display: inline-block; line-height: 34px;">共<label id="totalcount"></label>条记录，共<label id="pagecount"></label>页</div>
                        <div id="laypages" style="display: inline-block; line-height: 34px;"></div>
                    </div>
    </form>
    <link href="../scripts/laypage/skin/laypage.css" rel="stylesheet" />
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/layer/layer.js"></script>
    <script src="../scripts/layui/layui.js"></script>    
    <script src="../scripts/model/apiurl.js"></script>
    <script src="../scripts/laypage/laypage.js"></script>
    <script src="../scripts/JSManager/ContentManager/ArticleWshList.js"></script>
</body>
</html>
