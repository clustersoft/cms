<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContentListManager.aspx.cs" Inherits="CMSSystem.ContentManager.ContentListManager" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>内容管理</title>
    <link href="../scripts/zTree/css/zTreeStyle/zTreeStyle.css" rel="stylesheet" />
    <link href="../scripts/layui/css/layui.css" rel="stylesheet" />
    <link href="../css/cmsui.css" rel="stylesheet" />
    <link href="../css/font-awesome.min.css" rel="stylesheet" />
    <link href="../scripts/layui/css/modules/laydate/laydate.css" rel="stylesheet" />
    <script src="../scripts/laydate/laydate.js"></script>
</head>
<body>
    <form id="form1" class="layui-form layui-form-pane">
        <blockquote class="layui-elem-quote">
            内容管理
        </blockquote>
        <div class="tree-table-box">
            <div class="tree-table-tree-box">
                <div class="treeset">
                    <ul id="emailtreelist" class="ztree"></ul>
                    <ul id="wztreelist" class="ztree"></ul>
                    <div style="height: 100px;"></div>
                </div>
            </div>

            <div class="tree-table-table-box">
                <div id="divlb">
                    <div class="head-box">
                        <span class="fl">
                            <a class="layui-btn btn-default btn-add" id="add" href="ContentAdd.aspx"><i class="fa fa-plus"></i>&nbsp;新增</a>
                            <a class="layui-btn btn-default layui-btn-danger" id="del"><i class="fa fa-trash-o"></i>&nbsp;删除</a>
                        </span>
                        <span class="fr">
                            <div class="layui-input-inline" style="width: 120px;">
                                <select id="status" name="status" lay-filter="status">
                                    <option value=" " selected="">全部状态</option>
                                    <option value="-1">暂存</option>
                                    <option value="0">未审核</option>
                                    <option value="1">审核通过</option>
                                    <%--<option value="2">审核中</option>--%>
                                    <option value="3">被退回</option>
                                </select>
                            </div>

                            <div class="layui-input-inline">
                                <label class="layui-form-label">发布日期</label>
                                <div class="layui-input-inline dateWidth">
                                    <input class="layui-input" placeholder="开始日" id="starttime" lay-filter="date">
                                </div>
                                <div class="layui-input-inline dateWidth">
                                    <input class="layui-input" placeholder="截止日" id="endtime" lay-filter="date">
                                </div>
                            </div>
                            <div class="layui-input-inline keywordsWidth">
                                <input type="text" autocomplete="off" id="skey" placeholder="关键字" class="layui-input">
                            </div>
                            <a class="layui-btn layui-btn-normal" id="cx"><i class="fa fa-search"></i>&nbsp;查询</a>
                        </span>
                    </div>
                    <table id="dateTable" class="layui-table margintop">
                        <colgroup>
                            <col width="50" name="choose" >
                            <col width="28%" />
                            <col width="20%" />
                            <col width="11%" />
                            <col width="9%" />
                            <col width="13%" />
                            <col width="8%" />
                            <col width="11%" name="edit" />
                        </colgroup>
                        <thead>
                            <tr>
                                <th style="text-align: center;" name="choose">
                                    <input type="checkbox" name="" lay-skin="primary" lay-filter="allChoose" /></th>
                                <th style="text-align: center;">标题</th>
                                <th style="text-align: center;">栏目</th>
                                <th style="text-align: center;">状态</th>
                                <th style="text-align: center;">撰稿人</th>
                                <th style="text-align: center;">发布日期</th>
                                <th style="text-align: center;">属性</th>
                                <th style="text-align: center;" name="edit">操作</th>
                            </tr>
                        </thead>
                        <tbody id="msg">
                            <%--<tr>
                                <td colspan="8" style="text-align: center;">暂无数据</td>
                            </tr>--%>
                        </tbody>
                    </table>
                    
                    <div style="display: none;" id="allpage">
                        <div style="font-size: 12px; color: #666; display: inline-block; line-height: 34px;">共<label id="totalcount"></label>条记录，共<label id="pagecount"></label>页</div>
                        <div id="laypages" style="display: inline-block; line-height: 34px;"></div>
                    </div>
                </div>
            </div>
        </div>
        <%--<input id="pagecount" type="hidden" />
        <input id="totalcount" type="hidden" />--%>
        <input id="type" type="hidden" value="1" />
        <link href="../scripts/laypage/skin/laypage.css" rel="stylesheet" />
        <script src="../scripts/jquery-1.10.2.min.js"></script>
        <script src="../scripts/layer/layer.js"></script>
        <script src="../scripts/layui/layui.js"></script>
        <script src="../scripts/laypage/laypage.js"></script>

        <script src="../scripts/model/apiurl.js"></script>
        <script src="../datas/emailset.js"></script>
        <script src="../scripts/datepicker.js"></script>
        <script src="../scripts/zTree/js/jquery.ztree.core.js"></script>
        <script src="../scripts/zTree/js/jquery.ztree.exedit.js"></script>
        <script src="../scripts/JSManager/ContentManager/ContentListManager.js"></script>
    </form>
</body>
</html>
