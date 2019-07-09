<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModuleManager.aspx.cs" Inherits="CMSSystem.WebsiteManager.ModuleManager" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>模版管理</title>
    <link href="../scripts/zTree/css/zTreeStyle/zTreeStyle.css" rel="stylesheet" />
    <link href="../scripts/layui/css/layui.css" rel="stylesheet" />
    <link href="../css/cmsui.css" rel="stylesheet" />
    <link href="../css/font-awesome.min.css" rel="stylesheet" />
</head>
<body>
    <blockquote class="layui-elem-quote">
        模版管理
    </blockquote>
    <div class="tree-table-box">
        <div>
            <div id="divlb">
                <div class="head-box">
                    <span class="fl">
                        <a class="layui-btn btn-default btn-add" id="add" onclick="add()"><i class="fa fa-plus"></i>&nbsp;新增</a>
                        <a class="layui-btn btn-default layui-btn-danger" id="del"><i class="fa fa-trash-o"></i>&nbsp;删除</a>
                    </span>
                    <span class="fr">
                        <div class="layui-input-inline">
                            <input type="text" autocomplete="off" id="skey" placeholder="关键字" class="layui-input">
                        </div>
                        <a class="layui-btn layui-btn-normal" id="cx"><i class="fa fa-search"></i>&nbsp;查询</a>
                    </span>
                </div>
                <form id="form1" runat="server" class="layui-form layui-form-pane">
                    <table id="dateTable" class="layui-table">
                        <colgroup>
                            <col width="50" name="choose" />
                            <col width="10%" />
                            <col width="50%" />
                            <col width="20%" />
                            <col width="20%" name="edit" />
                        </colgroup>
                        <thead>
                            <tr>
                                <th style="text-align: center;" name="choose">
                                    <input type="checkbox" name="" lay-skin="primary" lay-filter="allChoose"></th>
                                <th style="text-align: center;">编号</th>
                                <th style="text-align: center;">名称</th>
                                <th style="text-align: center;">是否停用</th>
                                <th style="text-align: center;" name="edit">操作</th>
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
            </div>
            <div id="divEdit" style="display: none;">
                <iframe id="editcolumn" src="" width="100%" height="1000" style="border: 0px"></iframe>
            </div>
        </div>
    </div>

    <link href="../scripts/laypage/skin/laypage.css" rel="stylesheet" />
    <script src="../scripts/laypage/laypage.js"></script>

    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/layer/layer.js"></script>
    <script src="../scripts/layui/layui.js"></script>

    <script src="../scripts/model/base.js"></script>
    <script src="../scripts/model/apiurl.js"></script>
    <script src="../scripts/layer/layer.js"></script>
    <script src="../scripts/bootstrap/icheck.min.js"></script>
    <script src="../scripts/JSManager/WebsiteManager/ModuleManager.js"></script>
</body>
</html>
