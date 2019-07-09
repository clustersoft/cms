var CMSUserID = GetCMSData().CMSUserID, listlength = 0;
var ViewSpotID = GetQueryString("ViewSpotID");
function addnew() {
    location.href = 'AddViewSpotDetail.html?ViewSpotID=' + ViewSpotID+'&pages=' + GetQueryString("pages") + "&keyword=" + GetQueryString("keyword");
}
function goback() {
    location.href = 'ViewSpotManager.html?pages=' + GetQueryString("pages") + "&keyword=" + GetQueryString("keyword");
}
list();
$("#cx").click(function () {
    list();
});
$("#del").click(function () {
    del();
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
        PostAjax('/api/viewspot/detaildelete', "IDs=" + ids.substring(0, ids.length - 1) + "&UserID=" + CMSUserID, delSuccessCallback, ErrorCallback);
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
    crossDomainAjax("/api/viewspot/detaillist?ViewSpotID=" + ViewSpotID + "&ran=" + Math.random(), SuccessCallback, ErrorCallback);
    crossDomainAjax("/api/permission/navshowlist?NavCode=ViewSpotDetail&UserID=" + CMSUserID + "&ran=" + Math.random(), PerSuccessCallback, ErrorCallback);
}
function PerSuccessCallback(data, index) {
    if (data.Result.IsAdmin != 1) {
        if ((data.Result.ActionCode).indexOf('add') > 0) {
            if (listlength != 4) {
                $("#add").css("display", "inline-block");
            }
        }
        if ((data.Result.ActionCode).indexOf('delete') > 0) {
            $("#del").css("display", "inline-block");
            $("col[name='choose']").show();
            $("td[name='choose']").show();
            $("th[name='choose']").show();
        }
        if ((data.Result.ActionCode).indexOf('edit') > 0) {
            $("col[name='edit']").show();
            $("td[name='edit']").show();
            $("th[name='edit']").show();
        }
    } else {
        if (listlength != 4) {
            $("#add").css("display", "inline-block");
        }
        $("#del").css("display", "inline-block");
        $("col[name='choose']").show();
        $("td[name='choose']").show();
        $("th[name='choose']").show();
        $("col[name='edit']").show();
        $("td[name='edit']").show();
        $("th[name='edit']").show();
    }
    layer.closeAll('loading');
}
function SuccessCallback(data, index) {
    var list = eval(data.Result);
    var msghtml = "";
    listlength = list.length;
    for (var i = 0; i < list.length; i++) {
        msghtml += "<tr>" +
            "<td style='text-align:center;' name='choose'><input type='checkbox'  id='ck' ids='" + list[i].ID + "' name='ck' lay-skin='primary'></td>" +
            "<td style='text-align:center;'>" + list[i].TypeName + "</td>" +
            "<td style='text-align:center;' name='edit' ><a href='EditViewSpotDetail.html?ViewSpotID=" + ViewSpotID + "&DetailID=" + list[i].ID + "&pages=" + GetQueryString("pages")+"&keyword=" + GetQueryString("keyword")+"'>编辑</a></td>" +
            "</tr>";
    }
    if (msghtml == "") {
        msghtml = "<td colspan=\"3\" style=\"text-align: center;\">暂无数据</td>";
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
}
layer.closeAll('loading');