layui.use(['form', 'layer'], function () {
    var form = layui.form(),
        layer = layui.layer;

    crossDomainAjax(geturl() + "/api/user/selectlist", ModuleSuccessCallback, ErrorCallback);
    function ModuleSuccessCallback(data, index) {
        layer.closeAll('loading');
        var ddl = $("#userlist");
        var result = eval(data.Result);
        removeAll();
        ddl.append("<option value='0' selected=''>全部用户</option>");
        $(result).each(function (key) {
            var opt = $("<option></option>").text(result[key].UserName).val(result[key].ID);
            ddl.append(opt);
        });
        form.render('select'); //刷新select选择框渲染
    }
    //$.ajax({
    //    url: geturl() + "/api/user/selectlist",
    //    async: false,
    //    type: "GET",
    //    contentType: "application/json; charset=utf-8",
    //    timeout: 10000,
    //    dataType: "json",
    //    success: function (data) {
    //        var ddl = $("#userlist");
    //        var result = eval(data.Result);
    //        removeAll();
    //        ddl.append("<option value='0' selected=''>全部用户</option>");
    //        $(result).each(function (key) {
    //            var opt = $("<option></option>").text(result[key].UserName).val(result[key].ID);
    //            ddl.append(opt);
    //        });
    //        form.render('select'); //刷新select选择框渲染
    //    },
    //    error: function (XMLHttpRequest, textStatus, errorThrown) {
    //        layer.msg("获取数据失败，消息：" + textStatus + "  " + errorThrown, { time: 1000 });
    //    }
    //});

    list(geturl());

    $("#cx").click(function () {
        list(geturl());
    });
    layer.closeAll('loading');
});

function removeAll() {
    var obj = document.getElementById('userlist');
    obj.options.length = 0;
}

//不选择用户、默认时，载入所有用户列表
//function listAll(WebAPIPath) {
//    $.ajax({
//        url: WebAPIPath + "/api/logs/list",
//        async: false,
//        type: "GET",
//        data: "pageIndex=1&keyword=" + $("#skey").val(),
//        contentType: "application/json; charset=utf-8",
//        timeout: 10000,
//        dataType: "json",
//        success: function (data) {
//            //alert("Success:" + data.Success + ";PageCount:" + data.Result.pageCount + ";ToTalCount:" + data.Result.totalCount);
//            $("#pagecount").val(data.Result.pageCount);
//            $("#totalcount").val(data.Result.totalCount);
//        },
//        error: function (XMLHttpRequest, textStatus, errorThrown) {
//            layer.msg("获取数据失败，消息：" + textStatus + "  " + errorThrown, { time: 1000 });
//        }
//    });
//    var pages = $("#pagecount").val();
//    //调用分页
//    laypage({
//        cont: 'laypages',
//        pages: pages,
//        skip: true, //是否开启跳页
//        skin: 'molv',
//        jump: function (obj) {
//            $.ajax({
//                url: WebAPIPath + "/api/logs/list",
//                async: false,
//                type: "GET",
//                data: "pageIndex=" + obj.curr + "&keyword=" + $("#skey").val(),
//                contentType: "application/json; charset=utf-8",
//                dataType: "json",
//                success: function (data) {
//                    var list = eval(data.Result.list);
//                    var msghtml = "";
//                    for (var i = 0; i < list.length; i++) {
//                        msghtml +=
//                        "<tr>" +
//                            //"<td style='text-align:center;'><input type='checkbox'  id='ck' ids='" + list[i].ID + "' name='ck' lay-skin='primary'></td>" +
//                            "<td style='text-align:center;'>" + list[i].ID + "</td>" +
//                            //"<td style='text-align:center;'></td>"+
//                            "<td style='text-align:center;'>" + list[i].ActionContent + "</td>" +
//                            "<td style='text-align:center;'>" + list[i].LogUser + "</td>" +
//                            "<td style='text-align:center;'>" + list[i].LogIPAddress + "</td>" +
//                            //"<td style='text-align:center;'></td>"+
//                            "<td style='text-align:center;'>" + list[i].LogTime + "</td>"
//                        //"<td style='text-align:center;'><a class='layui-btn layui-btn-small do-action' href='EditPositionInfo.aspx?ID=" + list[i].ID + "'><i class='fa fa-edit'></i>编辑</a></td>" +
//                        "</tr>";
//                    }
//                    document.getElementById('msg').innerHTML = msghtml;
//                }
//            });
//        }
//    });
//}

//根据用户载入列表
function list(WebAPIPath) {
    var loguser = $("#userlist").val();

    $.ajax({
        url: WebAPIPath + "/api/logs/list",
        async: false,
        type: "GET",
        headers: { Authorization: GetCMSData().CMSToken },
        data: "pageIndex=1&keyword=" + $("#skey").val() + "&logUserID=" + loguser,
        contentType: "application/json; charset=utf-8",
        timeout: 10000,
        dataType: "json",
        success: function (data) {
            //alert("Success:" + data.Success + ";PageCount:" + data.Result.pageCount + ";ToTalCount:" + data.Result.totalCount);
            $("#pagecount").html(data.Result.pageCount);
            $("#totalcount").html(data.Result.totalCount);
            if (data.Result.pageCount <= 1) {
                $("#allpage").attr("style", "display:none;");
            } else {
                $("#allpage").attr("style", "display:block;text-align:center;");
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            layer.msg("获取数据失败，消息：" + textStatus + "  " + errorThrown, { time: 1000 });
        }
    }).fail(function (xhr) {
        if (xhr.status == 401) {
            ReToken();
        } else {
            ErrorCallback(data, index);
        }
    });
    var pages = $("#pagecount").html();
    //调用分页
    laypage({
        cont: 'laypages',
        pages: pages,
        skip: true, //是否开启跳页
        skin: 'molv',
        jump: function (obj) {
            var index = layer.load(2);
            $.ajax({
                url: WebAPIPath + "/api/logs/list",
                async: true,
                type: "GET",
                headers: { Authorization: GetCMSData().CMSToken },
                data: "pageIndex=" + obj.curr + "&keyword=" + $("#skey").val() + "&logUserID=" + loguser,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    layer.close(index);
                    var msghtml = "";
                    var list = eval(data.Result.list);
                    for (var i = 0; i < list.length; i++) {
                        msghtml +=
                        "<tr>" +
                             "<td style='text-align:center;'>" + list[i].ID + "</td>" +
                        "<td style='text-align:center;'>" + list[i].ActionContent + "</td>" +
                        "<td style='text-align:center;'>" + list[i].LogUser + "</td>" +
                        "<td style='text-align:center;'>" + list[i].LogIPAddress + "</td>" +
                        "<td style='text-align:center;'>" + list[i].LogTime + "</td>"
                        //"<td style='text-align:center;'><a class='layui-btn layui-btn-small do-action' href='EditPositionInfo.aspx?ID=" + list[i].ID + "'><i class='fa fa-edit'></i>编辑</a></td>" +
                        "</tr>";
                    }
                    if (msghtml == "") {
                        msghtml = "<tr>" +
                                "<td colspan='5' style='text-align: center;'>暂无数据</td>" +
                            "</tr>";
                    }
                    $("#msg").empty().append(msghtml);
                }
            }).fail(function (xhr) {
                if (xhr.status == 401) {
                    ReToken();
                } else {
                    ErrorCallback(data, index);
                }
            });
            //layui.use('form', function () {
            //    var $ = layui.jquery, form = layui.form();
            //    form.render('checkbox');
            //    //全选
            //    form.on('checkbox(allChoose)', function (data) {
            //        var child = $(data.elem).parents('table').find('tbody input[type="checkbox"]');
            //        child.each(function (index, item) {
            //            item.checked = data.elem.checked;
            //        });
            //        form.render('checkbox');
            //    });
            //});
        }
    });
}