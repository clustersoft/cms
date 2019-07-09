<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Advice.aspx.cs" Inherits="CMSSystem.ComplainManager.Advice" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../scripts/layui/css/layui.css" rel="stylesheet" />
    <link href="../css/global.css" rel="stylesheet" />
    <link href="../css/font-awesome.min.css" rel="stylesheet" />
    <link href="../css/cmsui.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server" class="layui-form layui-form-pane">
        <blockquote class="layui-elem-quote fhui-admin-main_hd">
            <h2>投诉建议</h2>
        </blockquote>
        <div class="head-box">
            <span class="fr">
                <%--<div class="layui-input-inline selectwidth">
                        <select id="PublicityTypesID" name="PublicityTypesID" lay-filter="PublicityTypesID">
                            <option value="" selected="">全部类型</option>
                        </select>
                    </div>--%>
                <div class="layui-input-inline keywordsWidth">
                    <input type="text" autocomplete="off" id="skey" placeholder="关键字" class="layui-input" />
                </div>
                <a class="layui-btn layui-btn-normal" id="cx"><i class="fa fa-search"></i>&nbsp;查询</a>
            </span>
        </div>
        <table class="layui-table">
            <colgroup>
                <%--<col width="50">--%>
                <col width="100">
                <col>
                <col width="180">
                <col width="180">
                <col width="180">
                <%--<col width="100">--%>
            </colgroup>
            <thead>
                <tr>
                    <%--<th>
                                <input type="checkbox" name="" lay-skin="primary" lay-filter="allChoose"></th>--%>
                    <th style="text-align: center;">留言种类</th>
                    <th style="text-align: center;">留言内容</th>
                    <th style="text-align: center;">留言人姓名</th>
                    <th style="text-align: center;">联系电话</th>
                    <th style="text-align: center;">留言时间</th>
                    <%--<th style="text-align: center;">操作</th>--%>
                </tr>
            </thead>
            <tbody id="msg">
                <tr>
                    <td colspan="5" style="text-align: center;">暂无数据</td>
                </tr>
            </tbody>
        </table>
        <%--<input id="pagecount" type="hidden" />
        <input id="totalcount" type="hidden" />
        <div id="laypages" style="margin-top: 5px; text-align: center;"></div>--%>
        <div style="display: none;" id="allpage">
            <div style="font-size: 12px; color: #666; display: inline-block; line-height: 34px;">共<label id="totalcount"></label>条记录，共<label id="pagecount"></label>页</div>
            <div id="laypages" style="display: inline-block; line-height: 34px;"></div>
        </div>
    </form>
    <link href="../scripts/laypage/skin/laypage.css" rel="stylesheet" />
    <script src="../scripts/laypage/laypage.js"></script>
    <script src="../scripts/model/apiurl.js"></script>    
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/layui/lay/modules/layer.js"></script>
    <script src="../scripts/layui/layui.js"></script>
    <script src="../scripts/JSManager/AdviceManager/Advice.js"></script>
</body>
</html>
