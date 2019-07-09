var CMSUserID = GetCMSData().CMSUserID;
layui.use('form', function () {
    var form = layui.form();
    tablelist();
});
function PerSuccessCallback(data) {
    if (data.Result.IsAdmin != 1) {
        //tablelist();
    }
    else {
        tablelist();
    }
}

function tablelist() {
    crossDomainAjax("/api/article/shtotallist?pageIndex=1&LogUser=" + CMSUserID, SuccessCallback, ErrorCallback);
    //$.ajax({
    //    url: geturl() + "/api/article/shtotallist",
    //    async: false,
    //    type: "GET",
    //    data: "pageIndex=1&LogUser=" + CMSUserID,
    //    contentType: "application/json; charset=utf-8",
    //    timeout: 10000,
    //    dataType: "json",
    //    success: function (data) {
    //        layer.close(index);
    //        //alert("Success:" + data.Success + ";PageCount:" + data.Result.pageCount + ";ToTalCount:" + data.Result.totalCount);
    //        $("#pagecount").val(data.Result.pageCount);
    //        $("#totalcount").val(data.Result.totalCount);
    //    },
    //    error: function (XMLHttpRequest, textStatus, errorThrown) {
    //        layer.msg("获取数据失败，消息：" + textStatus + "  " + errorThrown, { time: 1000 });
    //    }
    //});
    var pages = $("#pagecount").html();
    laypage({
        cont: 'laypages',
        pages: pages,
        skip: true, //是否开启跳页
        skin: 'molv',
        jump: function (obj) {
            var index = layer.load(2);
            crossDomainAjax("/api/article/shtotallist?pageIndex=" + obj.curr + "&LogUser=" + CMSUserID, PageSuccessCallback, ErrorCallback);
            //$.ajax({
            //    url: geturl() + "/api/article/shtotallist",
            //    async: true,
            //    type: "GET",
            //    data: "pageIndex=" + obj.curr + "&LogUser=" + CMSUserID,
            //    contentType: "application/json; charset=utf-8",
            //    success: function (data) {
            //        layer.close(index);
            //        var listdata = eval(data.Result.list);
            //        var msghtml = "";
            //        for (var i = 0; i < listdata.length; i++) {
            //            msghtml +=
            //            "<tr>" +
            //                "<td style='text-align:center;'>" + (i + 1) + "</td>" +
            //                "<td style='text-align:center;'>" + listdata[i].Title + "</td>" +
            //                "<td style='text-align:center;'>" + listdata[i].CategoryNames + "</td>" +
            //                "<td style='text-align:center;'>" + listdata[i].Status + "</td>" +
            //                "<td style='text-align:center;'>" + listdata[i].CreateUserName + "</td>" +
            //                "<td style='text-align:center;'>" + listdata[i].PubTime + "</td>";
            //            msghtml += "<td style='text-align:center;'><a href='ContentEdit.aspx?ID=" + listdata[i].ID + "&from=1&source=ArticleWshList'>审核</a>";
            //            msghtml += "</td></tr>";
            //        }
            //        if (msghtml == "") {
            //            msghtml = "<tr>" +
            //                    "<td colspan='7' style='text-align: center;'>暂无数据</td>" +
            //                "</tr>";
            //        }
            //        $("#msg").empty().append(msghtml);
            //        $("input[type='checkbox']").removeAttr("checked");
            //    }
            //});
        }
    });
}

function SuccessCallback(data, index) {
    $("#pagecount").html(data.Result.pageCount);
    $("#totalcount").html(data.Result.totalCount);
    if (data.Result.pageCount <= 1) {
        $("#allpage").attr("style", "display:none;");
    } else {
        $("#allpage").attr("style", "display:block;text-align:center;");
    }
}

function PageSuccessCallback(data, index) {
    layer.close(index);
    var listdata = eval(data.Result.list);
    var msghtml = "";
    for (var i = 0; i < listdata.length; i++) {
        msghtml +=  "<tr>" +
            "<td style='text-align:center;'>" + (i + 1) + "</td>" +
            "<td style='text-align:center;'>" + listdata[i].Title + "</td>" +
            "<td style='text-align:center;'>" + listdata[i].CategoryNames + "</td>" +
            "<td style='text-align:center;'>" + listdata[i].Status + "</td>" +
            "<td style='text-align:center;'>" + listdata[i].CreateUserName + "</td>" +
            "<td style='text-align:center;'>" + listdata[i].PubTime + "</td>";
        msghtml += "<td style='text-align:center;'><a href='ContentEdit.aspx?ID=" + listdata[i].ID + "&from=1&source=ArticleWshList'>审核</a>";
        msghtml += "</td></tr>";
    }
    if (msghtml == "") {
        msghtml = "<tr>" +
                "<td colspan='7' style='text-align: center;'>暂无数据</td>" +
            "</tr>";
    }
    $("#msg").empty().append(msghtml);
    $("input[type='checkbox']").removeAttr("checked");
    layer.closeAll('loading');
}
function ErrorCallback(data, index) {
    layer.msg("获取数据失败！");
    layer.closeAll('loading');
    return false;
}