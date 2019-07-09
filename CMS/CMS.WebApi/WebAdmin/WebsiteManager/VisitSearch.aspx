<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VisitSearch.aspx.cs" Inherits="CMSSystem.WebsiteManager.VisitSearch" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>网站发布统计</title>
    <link href="../scripts/layui/css/layui.css" rel="stylesheet" />
    <link href="../css/cmsui.css" rel="stylesheet" />
    <link href="../scripts/layui/css/modules/layer/default/layer.css" rel="stylesheet" />
    <link href="../scripts/layui/css/modules/laydate/laydate.css" rel="stylesheet" />
    <script src="../scripts/laydate/laydate.js"></script>
    <link href="../css/font-awesome.min.css" rel="stylesheet" />
</head>
<body>
    <blockquote class="layui-elem-quote">
        网站发布统计
    </blockquote>
   
        <div>
            <div class="layui-input-inline">
                <label class="layui-form-label">统计区间</label>
                <div class="layui-input-inline dateWidth">
                    <input class="layui-input" placeholder="开始日" id="starttime" lay-filter="date">
                </div>
                <div class="layui-input-inline dateWidth">
                    <input class="layui-input" placeholder="截止日" id="endtime" lay-filter="date">
                </div>
            </div>
            <a class="layui-btn" id="cx1"><i class="fa fa-search"></i>&nbsp;按发布者统计</a>
            <a class="layui-btn" id="cx2"><i class="fa fa-search"></i>&nbsp;按消息来源统计</a>
        </div>
     <form id="form1" class="layui-form layui-form-pane">
        <table class="layui-table">
            <colgroup>
                <col width="100" />
                <col width="500" />
                <col width="500" />
                <col width="20%" />
                <col width="20%" />
            </colgroup>
            <thead class="head">
            </thead>
            <tbody class="msg">
            </tbody>
        </table>
    </form>
    <script src="../scripts/layui/layui.js"></script>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/model/apiurl.js"></script>
    <script src="../scripts/datepicker.js"></script>
    <script src="../scripts/layer/layer.js"></script>
    <script src="../scripts/JSManager/WebsiteManager/VisitSearch.js"></script>
</body>
</html>
