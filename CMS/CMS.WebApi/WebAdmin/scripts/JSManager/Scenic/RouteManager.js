var CMSUserID = GetCMSData().CMSUserID;
var nowpages = 1, loadpages = GetQueryString("pages") == null ? 1 : GetQueryString("pages");
$("#skey").val(GetQueryString("keyword") == null ? "" : GetQueryString("keyword"));
function add() {
    location.href = "AddRoute.html?pages=" + nowpages + "&keyword=" + escape($("#skey").val().replace(/(^\s*)|(\s*$)/g, ""));
}
list();
$("#cx").click(function () {
    loadpages = 1;
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
        PostAjax('/api/route/delete', "IDs=" + ids.substring(0, ids.length - 1) + "&UserID=" + CMSUserID, delSuccessCallback, ErrorCallback);
    });
}
function delSuccessCallback(data, index) {
    if (data.Success == "1") {
        layer.msg("删除成功！", { time: 1000 });
        list();
    } else {
        if (data.Result == "有路线景点顺序不能删除") {
            layer.msg("无法删除，存在景点列表！");
            return false;
        } else {
            layer.msg("删除失败，请联系管理员！");
            return false;
        }
    }
}
function list() {
    layer.load(2);
    crossDomainAjax("/api/route/list?Pageindex=1&Keywords=" + escape($("#skey").val().replace(/(^\s*)|(\s*$)/g, "")) + "&ran=" + Math.random(), SuccessCallback, ErrorCallback);
    setTimeout(function () {
        var pages = $("#pagecount").html();
        //调用分页
        laypage({
            cont: 'laypages',
            pages: pages,
            curr: loadpages,
            skip: true, //是否开启跳页
            skin: 'molv',
            jump: function (obj) {
                nowpages = obj.curr;
                crossDomainAjax("/api/route/list?Pageindex=" + obj.curr + "&Keywords=" + escape($("#skey").val().replace(/(^\s*)|(\s*$)/g, ""))+ "&ran=" + Math.random(), PageSuccessCallback, ErrorCallback);
            }
        });
    }, 500);
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
        if ((data.Result.ActionCode).indexOf('edit') > 0) {
            $("a[name='edit']").css("display", "inline-block");
        }
    } else {
        $("#add").css("display", "inline-block");
        $("#del").css("display", "inline-block");
        $("col[name='choose']").show();
        $("td[name='choose']").show();
        $("th[name='choose']").show();
        $("a[name='edit']").css("display", "inline-block");
    }
    layer.closeAll('loading');
}
function SuccessCallback(data, index) {
    $("#pagecount").empty().append(data.Result.pageCount);
    $("#totalcount").empty().append(data.Result.totalCount);
}
function PageSuccessCallback(data, index) {
    var theadmsg = "";
    theadmsg += "<tr>" +
    "<th style='text-align: center;' name='choose'>" +
    "<input type='checkbox' id='chkChoose' lay-skin='primary' lay-filter='allChoose'></th>" +
    "<th style='text-align: center;'>路线名称</th>" +
    "<th style='text-align: center;'>排序</th>" +
    "<th style='text-align: center;'>操作</th>" +
            "</tr>";
    var list = eval(data.Result.list);
    var msghtml = "";
    for (var i = 0; i < list.length; i++) {
        msghtml += "<tr>" +
        "<td style='text-align:center;' name='choose'><input type='checkbox'  id='ck' ids='" + list[i].ID + "' name='ck' lay-skin='primary'></td>" +
        "<td style='text-align:center;'>" + list[i].RouteName + "</td>" +
        "<td style='text-align:center;'>" + list[i].OrderID + "</td>" +
        "<td style='text-align:center;' ><a href='EditRoute.html?ID=" + list[i].ID + "&pages=" + nowpages + "&keyword=" + escape($("#skey").val().replace(/(^\s*)|(\s*$)/g, "")) + "' name='edit'>编辑</a>&nbsp;&nbsp;<a href='RouteDetailManager.html?RouteID=" + list[i].ID + "&pages=" + nowpages + "&keyword=" + escape($("#skey").val().replace(/(^\s*)|(\s*$)/g, "")) + "'>景点列表</a></td>" +
        "</tr>";
    }
    if (msghtml == "") {
        msghtml = "<td colspan=\"9\" style=\"text-align: center;\">暂无数据</td>";
    }
    $("#theadmsg").empty().append(theadmsg);
    $("#msg").empty().append(msghtml);
    if ($("#pagecount").html() <= 1) {
        $("#allpage").attr("style", "display:none;");
    } else {
        $("#allpage").attr("style", "display:block;text-align:center;");
    }
    crossDomainAjax("/api/permission/navshowlist?NavCode=Route&UserID=" + CMSUserID + "&ran=" + Math.random(), PerSuccessCallback, ErrorCallback);
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
layer.closeAll('loading');
