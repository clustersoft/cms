<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogsManager.aspx.cs" Inherits="CMSSystem.WebsiteManager.LogsManager" %>

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
</head>
<body>
    <div class="main-wrap">
        <blockquote class="layui-elem-quote fhui-admin-main_hd">
            <h2>日志管理</h2>
        </blockquote>
        <div class="layui-form layui-form-pane">
            <div class="head-box">
                <span class="fr">
                    <div class="layui-input-inline selectwidth">
                        <select id="userlist" name="userlist" lay-verify="selectuser" lay-search>
                            <option value="">全部用户</option>
                        </select>
                    </div>
                    <div class="layui-input-inline keywordsWidth">
                        <input type="text" autocomplete="off" id="skey" placeholder="关键字" class="layui-input">
                    </div>
                    <a class="layui-btn layui-btn-normal" id="cx"><i class="fa fa-search"></i>&nbsp;查询</a>
                </span>
            </div>
        </div>
        <form id="form1" runat="server" class="layui-form layui-form-pane">
            <div class="fhui-admin-table-container">
                <table class="layui-table">
                    <colgroup>
                        <col width="200">
                        <col>
                        <col width="200">
                        <col width="200">
                        <col width="200">
                    </colgroup>
                    <thead>
                        <tr>
                            <%-- <th>
                                <input type="checkbox" name="" lay-skin="primary" lay-filter="allChoose"></th>--%>
                            <th style="text-align: center;">编号</th>
                            <th style="text-align: center;">动作记录</th>
                            <th style="text-align: center;">用户</th>
                            <th style="text-align: center;">IP</th>
                            <th style="text-align: center;">时间</th>
                        </tr>
                    </thead>
                    <tbody id="msg">
                        <tr>
                            <td colspan="5" style="text-align: center;">暂无数据</td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <%-- <input id="pagecount" type="hidden" />
            <input id="totalcount" type="hidden" />
            <div id="laypages" style="margin-top: 5px; text-align: center;"></div>--%>
            <div style="display: none;" id="allpage">
                <div style="font-size: 12px; color: #666; display: inline-block; line-height: 34px;">共<label id="totalcount"></label>条记录，共<label id="pagecount"></label>页</div>
                <div id="laypages" style="display: inline-block; line-height: 34px;"></div>
            </div>
        </form>
    </div>

    <link href="../scripts/laypage/skin/laypage.css" rel="stylesheet" />
    <script src="../scripts/laypage/laypage.js"></script>
    <script src="../scripts/model/apiurl.js"></script>
    <script src="../scripts/layui/layui.js"></script>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/bootstrap/icheck.min.js"></script>
    <script src="../scripts/JSManager/WebsiteManager/LogsManager.js"></script>
</body>
</html>
