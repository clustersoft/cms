var CMSUserID = GetCMSData().CMSUserID;
var nowpages = 1, loadpages = GetQueryString("pages") == null ? 1 : GetQueryString("pages");
$("#skey").val(GetQueryString("keyword") == null ? "" : GetQueryString("keyword"));
function add() {
    location.href = "AddTextLinkInfo.aspx?pages=" + nowpages + "&keyword=" + escape($("#skey").val().replace(/(^\s*)|(\s*$)/g, "")) + "&PublicityCateID=" + $("#PublicityTypesID").val();
}
crossDomainAjax("/api/publicity/selpositionlist?PublicityTypesID=2&ran=" + Math.random(), CateSuccessCallback, ErrorCallback);
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
        PostAjax('/api/publicity/deletecontent', "IDs=" + ids.substring(0, ids.length - 1) + "&UserID=" + CMSUserID, delSuccessCallback, ErrorCallback);
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
    crossDomainAjax("/api/publicity/contentlist?pageIndex=1&keyword=" + escape($("#skey").val().replace(/(^\s*)|(\s*$)/g, "")) + "&PublicityTypesID=2&PublicityCateID=" + $("#PublicityTypesID").val() + "&ran=" + Math.random(), SuccessCallback, ErrorCallback);
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
            crossDomainAjax("/api/publicity/contentlist?pageIndex=" + obj.curr + "&keyword=" + escape($("#skey").val().replace(/(^\s*)|(\s*$)/g, "")) + "&PublicityTypesID=2&PublicityCateID=" + $("#PublicityTypesID").val() + "&ran=" + Math.random(), PageSuccessCallback, ErrorCallback);
        }
    });
}

function CateSuccessCallback(data, index) {
    var list = eval(data.Result);
    for (var i = 0; i < list.length; i++) {
        if (list[i].ID==GetQueryString("PublicityCateID")) {
            $("#PublicityTypesID").append("<option value='" + list[i].ID + "' selected>" + list[i].PublicityCategoryName + "</option>");
        } else {
            $("#PublicityTypesID").append("<option value='" + list[i].ID + "'>" + list[i].PublicityCategoryName + "</option>");
        }
    }
    layer.closeAll('loading');
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
            $("col[name='edit']").show();
            $("td[name='edit']").show();
            $("th[name='edit']").show();
        }
    } else {
        $("#add").css("display", "inline-block");
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
    $("#pagecount").empty().append(data.Result.pageCount);
    $("#totalcount").empty().append(data.Result.totalCount);
}
function PageSuccessCallback(data, index) {
    var theadmsg = "";
    theadmsg += "<tr>" +
    "<th style='text-align: center;' name='choose'>" +
    "<input type='checkbox' id='chkChoose' lay-skin='primary' lay-filter='allChoose'></th>" +
    "<th style='text-align: center;'>是否显示</th>" +
    "<th style='text-align: center;'>链接名称</th>" +
    " <th style='text-align: center;'>位置名称</th>" +
    " <th style='text-align: center;'>发布时间</th>" +
    " <th style='text-align: center;'>过期时间</th>" +
    " <th style='text-align: center;'>排序</th>" +
    " <th style='text-align: center;' name='edit'>操作</th>" +
            "</tr>";
    var list = eval(data.Result.list);
    var msghtml = "";
    for (var i = 0; i < list.length; i++) {
        msghtml +=
        "<tr>" +
            "<td style='text-align:center;' name='choose'><input type='checkbox'  id='ck' ids='" + list[i].ID + "' name='ck' lay-skin='primary'></td>" +
            "<td style='text-align:center;'>" + (list[i].ShowType == "1" ? "显示" : "不显示") + "</td>" +
            "<td style='text-align:center;'>" + list[i].PublicityName + "</td>" +
            "<td style='text-align:center;'>" + list[i].PublicityCategoryName + "</td>" +
            "<td style='text-align:center;'>" + (list[i].PublishType == "0" ? "不限" : list[i].PublishTime) + "</td>" +
            "<td style='text-align:center;'>" + (list[i].ExpiredType == "0" ? "不限" : list[i].ExpiredTime) + "</td>" +
            "<td style='text-align:center;'>" + list[i].OrderID + "</td>" +
            "<td style='text-align:center;' name='edit'><a href='EditTextLinkInfo.aspx?ID=" + list[i].ID + "&pages=" + nowpages + "&keyword=" + escape($("#skey").val().replace(/(^\s*)|(\s*$)/g, "")) + "&PublicityCateID=" + $("#PublicityTypesID").val() + "'>编辑</a></td>" +
        "</tr>";
    }
    if (msghtml == "") {
        msghtml = "<td colspan=\"8\" style=\"text-align: center;\">暂无数据</td>";
    }
    $("#theadmsg").empty().append(theadmsg);
    $("#msg").empty().append(msghtml);
    if ($("#pagecount").html() <= 1) {
        $("#allpage").attr("style", "display:none;");
    } else {
        $("#allpage").attr("style", "display:block;text-align:center;");
    }
    crossDomainAjax("/api/permission/navshowlist?NavCode=TextLink&UserID=" + CMSUserID + "&ran=" + Math.random(), PerSuccessCallback, ErrorCallback);
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
function ErrorCallback(data, index) {
    layer.msg("获取数据失败！");
    return false;
}
