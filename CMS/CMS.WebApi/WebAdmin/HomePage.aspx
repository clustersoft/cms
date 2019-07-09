<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="CMSSystem.HomePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>首页</title>
    <link href="scripts/layui/css/layui.css" rel="stylesheet" />
    <link href="css/cmsui.css" rel="stylesheet" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server" class="layui-form layui-form-pane">
        <blockquote class="layui-elem-quote">首页</blockquote>

        <div class="home-box fl">
            <div class="symbol bgcolor fl">
                <i class="fa fa-file-word-o"></i>
            </div>
            <div class="fl value">                
                    <b class="home-number" id="articleSum">0</b>                
                <p>
                   <span class="home-words">文章总数</span>
                </p>
            </div>
        </div>
        <div class="home-box fl">
            <div class="symbol bgcolor-wsh fl">
                <i class="fa fa-check-circle"></i>
            </div>
            <div class="fl value">
                <div id="div">
                    </div>
            </div>
        </div>

        <table id="dateTable" class="layui-table" style="width: 99%;position:relative;top:20px;">
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
    </form>
    <script src="scripts/jquery-1.10.2.min.js"></script>
    <script src="scripts/layer/layer.js"></script>
    <script src="scripts/layui/layui.js"></script>
    
    <script src="scripts/model/apiurl.js"></script>
    <script type="text/javascript">
        var CMSUserID = GetCMSData().CMSUserID;
        layui.use('form', function () {
            var form = layui.form();
            tablelist('audit');
        });

        function tablelist(type) {
            var index = layer.load(2);
            crossDomainAjax("/api/article/shlist?LogUser=" + CMSUserID, SuccessCallback, ErrorCallback);

            //$.ajax({
            //    url:  "/api/article/shlist",
            //    async: true,
            //    data: "LogUser=" + CMSUserID,
            //    type: "GET",
            //    contentType: "application/json; charset=utf-8",
            //    success: function (data) {
            //        layer.close(index);
            //        if (data.Success == "1") {
            //            var msg = "";
            //            var listdata = eval(data.Result.List);
            //            var msghtml = "";
            //            for (var i = 0; i < listdata.length; i++) {
            //                msghtml +=
            //                "<tr>" +
            //                    "<td style='text-align:center;'>" + (i + 1) + "</td>" +
            //                    "<td style='text-align:center;'>" + listdata[i].Title + "</td>" +
            //                    "<td style='text-align:center;'>" + listdata[i].CategoryNames + "</td>" +
            //                    "<td style='text-align:center;'>" + listdata[i].Status + "</td>" +
            //                    "<td style='text-align:center;'>" + listdata[i].CreateUserName + "</td>" +
            //                    "<td style='text-align:center;'>" + listdata[i].PubTime + "</td>" +
            //                    "<td style='text-align:center;'><a href='ContentManager/ContentEdit.aspx?ID=" + listdata[i].ID + "&from=1&source=HomePage'>审核</a>";
            //                "</td></tr>";
            //            }
            //            if (msghtml == "") {
            //                msghtml = "<tr>" +
            //                        "<td colspan='7' style='text-align: center;'>暂无数据</td>" +
            //                    "</tr>";
            //            }
            //            $("#msg").empty().append(msghtml);
            //            if (data.Result.Totalwsh == 0) {
            //                msg = '<b class="home-number">0</b>' +
            //                   '<p>' +
            //                   '<span class="home-words">待审文章总数</span>' +
            //                   '</p>';
            //                $("#dateTable").attr("style", "display:none;");
            //            } else {
            //                msg = "<a href='javascript:locationhref();' style='color:black;text-decoration:none;' >" +
            //                    '<b class="home-number">' + data.Result.Totalwsh + '</b>' +
            //                    '<p>' +
            //                    '<span class="home-words">待审文章总数</span>' +
            //                    '</p></a>';
            //            }
            //            $("#div").html(msg)
            //            $("#articleSum").html(data.Result.TotalArticle);
            //        }                    
            //    }
            //});
        }
        function SuccessCallback(data, index) {
            layer.close(index);
            if (data.Success == "1") {
                var msg = "";
                var listdata = eval(data.Result.List);
                var msghtml = "";
                for (var i = 0; i < listdata.length; i++) {
                    msghtml +=
                    "<tr>" +
                        "<td style='text-align:center;'>" + (i + 1) + "</td>" +
                        "<td style='text-align:center;'>" + listdata[i].Title + "</td>" +
                        "<td style='text-align:center;'>" + listdata[i].CategoryNames + "</td>" +
                        "<td style='text-align:center;'>" + listdata[i].Status + "</td>" +
                        "<td style='text-align:center;'>" + listdata[i].CreateUserName + "</td>" +
                        "<td style='text-align:center;'>" + listdata[i].PubTime + "</td>" +
                        "<td style='text-align:center;'><a href='ContentManager/ContentEdit.aspx?ID=" + listdata[i].ID + "&from=1&source=HomePage'>审核</a>";
                    "</td></tr>";
                }

                if (msghtml == "") {
                    msghtml = "<tr>" +
                            "<td colspan='7' style='text-align: center;'>暂无数据</td>" +
                        "</tr>";
                }

                $("#msg").empty().append(msghtml);

                if (data.Result.Totalwsh == 0) {
                    msg = '<b class="home-number">0</b>' +
                       '<p>' +
                       '<span class="home-words">待审文章总数</span>' +
                       '</p>';
                    $("#dateTable").attr("style", "display:none;");
                } else {
                    msg = "<a href='javascript:locationhref();' style='color:black;text-decoration:none;' >" +
                        '<b class="home-number">' + data.Result.Totalwsh + '</b>' +
                        '<p>' +
                        '<span class="home-words">待审文章总数</span>' +
                        '</p></a>';
                }

                $("#div").html(msg)
                $("#articleSum").html(data.Result.TotalArticle);
            }
            layer.closeAll('loading');
        }

        function locationhref() {
            window.location.href='ContentManager/ArticleWshList.aspx';
        }

        function ErrorCallback(data, index) {
            layer.msg("获取数据失败！");
            layer.closeAll('loading');
            return false;
        }
    </script>
</body>
</html>
