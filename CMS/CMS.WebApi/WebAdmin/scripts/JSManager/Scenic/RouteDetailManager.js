var CMSUserID = GetCMSData().CMSUserID;
var RouteID = GetQueryString("RouteID");
list();
$("#cx").click(function () {
    list();
});
$("#del").click(function () {
    del();
});
$("body").keydown(function () {
    if (event.keyCode == "13") {//keyCode=13是回车键
        $('#cx').click();
    }
});
$("[name='orderid']").keyup(function () {
    $(this).val($(this).val().replace(/[^0-9]/g, ''));
}).bind("paste", function () { //CTR+V事件处理  $(this).val($(this).val().replace(/[^0-9.]/g,''));   
}).css("ime-mode", "disabled");
function adddetail() {
    location.href = "AddRouteDetail.html?RouteID=" + RouteID + "&pages=" + GetQueryString("pages") + "&keyword=" + GetQueryString("keyword");
}
function goback() {
    location.href = "RouteManager.html?pages=" + GetQueryString("pages") + "&keyword=" + GetQueryString("keyword");
}
function del() {
    //查出选择的记录
    if ($(".layui-table tbody input:checked").size() < 1) {
        layer.msg('对不起，请选中您要操作的记录！');
        return false;
    }
    var ids = "";
    var checkObj = $(".layui-table tbody input:checked");
    for (var i = 0; i < checkObj.length; i++) {
        if (checkObj[i].checked && $(checkObj[i]).attr("disabled") != "disabled")
            ids += $(checkObj[i]).attr("ids") + ','; //如果选中，将value添加到变量idlist中
    }
    layer.confirm("你确定要删除选中项？", { icon: 3, title: '提示' }, function (index) {
        PostAjax('/api/route/deleteroutespot', "IDs=" + ids.substring(0, ids.length - 1) + "&UserID=" + CMSUserID, delSuccessCallback, ErrorCallback);
    });
}
function delSuccessCallback(data, index) {
    if (data.Success == "1") {
        layer.msg("删除成功！", { time: 1000 });
        list();
    } else {
        layer.msg("删除失败，请联系管理员！");
        return false;
    }
}
function list() {
    layer.load(2);
    crossDomainAjax("/api/route/routespotlist?RouteID=" + RouteID+ "&ran=" + Math.random(), SuccessCallback, ErrorCallback);
    crossDomainAjax("/api/permission/navshowlist?NavCode=RouteViewSpot&UserID=" + CMSUserID+ "&ran=" + Math.random(), PerSuccessCallback, ErrorCallback);
}
function PerSuccessCallback(data, index) {
    if (data.Result.IsAdmin != 1) {
        if ((data.Result.ActionCode).indexOf('add') > 0) {
            $("#add").css("display", "inline-block");
        }
        if ((data.Result.ActionCode).indexOf('delete') > 0) {
            $("#del").css("display", "inline-block");
            $("col[name='choose']").show();
            $("td[name='choose']").show();
            $("th[name='choose']").show();
        }
        if ((data.Result.ActionCode).indexOf('save') > 0) {
            $("#save").css("display", "inline-block");
        }
    } else {
        $("#add").css("display", "inline-block");
        $("#save").css("display", "inline-block");
        $("#del").css("display", "inline-block");
        $("col[name='choose']").show();
        $("td[name='choose']").show();
        $("th[name='choose']").show();
    }
    layer.closeAll('loading');
}
function SuccessCallback(data, index) {
    var list = eval(data.Result);
    var msghtml = "";
    for (var i = 0; i < list.length; i++) {
        msghtml += "<tr>" +
        "<td style='text-align:center;' name='choose'><input type='checkbox'  id='ck' ids='" + list[i].ID + "' name='ck' lay-skin='primary'></td>" +
        "<td style='text-align:center;'>" + list[i].ViewSpotName + "</td>" +
        "<td style='text-align:center;'>" + list[i].RouteName + "</td>" +
        "<td style='text-align:center;' name='padd'><input id='" + list[i].ID + "' vid='" + list[i].ViewSpotID + "' rid='" + list[i].RouteID + "' name='orderid' class='layui-input' style='height:28px; line-height:28px;' type='text' value='" + list[i].OrderID + "'></td>" +
        "</tr>";
    }
    if (msghtml == "") {
        msghtml = "<td colspan=\"9\" style=\"text-align: center;\">暂无数据</td>";
    }
    $("#msg").empty().append(msghtml);
    layui.use('form', function () {
        var $ = layui.jquery, form = layui.form();
        form.render('checkbox');
        //全选
        form.on('checkbox(allChoose)', function (data) {
            var child = $(data.elem).parents('table').find('tbody input[type="checkbox"]');
            child.each(function (index, item) {
                item.checked = data.elem.checked;
            });
            form.render('checkbox');
        });
    });
    layer.closeAll('loading');
}
$("#save").click(function () {
    $("input[name=orderid]").each(function (a, b) {
        var postdata = {
            ID: $(b).attr("id"),
            RouteID: $(b).attr("rid"),
            ViewSpotID: $(b).attr("vid"),
            OrderID: $(b).val(),
            EditUser: CMSUserID
        };
        PostAjax('/api/route/editroutespot', postdata, SubmitSuccessCallback, ErrorCallback);
    });
});

function SubmitSuccessCallback(data, index) {
    if (data.Success != 1) {
        layer.msg("保存排序失败！");
        layer.closeAll('loading');
    } else {
        setTimeout(function () {
            layer.msg("排序更新成功！", { time: 1000 }, function () {
                list();
            });
        }, 500)
    }
}

layer.closeAll('loading');
