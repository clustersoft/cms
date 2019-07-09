$(function () {
    list();
})
function list() {
    $.ajax({
        url: geturl() + "/api/advice/list",
        async: false,
        type: "GET",
        headers: { Authorization: GetCMSData().CMSToken },
        data: "pageIndex=1&keyword=" + $("#skey").val(),
        contentType: "application/json; charset=utf-8",
        timeout: 10000,
        dataType: "json",
        success: function (data) {
            $("#pagecount").html(data.Result.pageCount);
            $("#totalcount").html(data.Result.totalCount);
            if (data.Result.pageCount <= 1) {
                $("#allpage").attr("style", "display:none;");
            } else {
                $("#allpage").attr("style", "display:block;text-align:center;");
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            layer.msg("获取数据失败，消息：" + textStatus + "  " + errorThrown);
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
            $.ajax({
                url: geturl() + "/api/advice/list",
                async: true,
                type: "GET",
                headers: { Authorization: GetCMSData().CMSToken },
                data: "pageIndex=" + obj.curr + "&keyword=" + $("#skey").val(),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var list = eval(data.Result.list);
                    var msghtml = "";
                    for (var i = 0; i < list.length; i++) {
                        //alert(JSON.stringify(list[i]))
                        msghtml +=
                        "<tr>" +
                            //"<td style='text-align:center;'><input type='checkbox'  id='ck' ids='" + list[i].ID + "' name='ck' lay-skin='primary'></td>" +
                            "<td style='text-align:center;'>" + list[i].LeaveType + "</td>" +
                            "<td style='text-align:center;'>" + list[i].Contents + "</td>" +
                            "<td style='text-align:center;'>" + list[i].Name + "</td>" +
                            "<td style='text-align:center;'>" + list[i].Phone + "</td>" +
                            "<td style='text-align:center;'>" + list[i].LeaveTime + "</td>" +
                            //"<td style='text-align:center;'></td>"
                        //"<td style='text-align:center;'><a class='layui-btn layui-btn-small do-action' href='EditPositionInfo.aspx?ID=" + list[i].ID + "'><i class='fa fa-edit'></i>编辑</a></td>" +
                        "</tr>";
                    }
                    if (msghtml == "") {
                        msghtml = "<tr>" +
                                "<td colspan='7' style='text-align: center;'>暂无数据</td>" +
                            "</tr>";
                    }
                    //alert(msghtml)
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

$("#cx").click(function () {
    list();
})