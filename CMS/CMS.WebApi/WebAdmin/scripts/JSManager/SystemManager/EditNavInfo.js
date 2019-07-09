layer.load(2);
var navid = GetQueryString("ID");//
var CMSUserID = GetCMSData().CMSUserID;
var actions,iconChoose;
crossDomainAjax("/api/navgation/getInfo?ID=" + navid + "&ran=" + Math.random(), SuccessCallback, ErrorCallback);

angular.module('myNav', []).controller('navController', ['$scope', function ($scope) {
    $scope.navtitle = $("#navtitle").val();
    $scope.orderid = $("#orderid").val();
}]);



//获取图标列表
crossDomainAjax("/api/icon/list?ran=" + Math.random(), IconListSuccessCallback, ErrorCallback);
function IconListSuccessCallback(data, index) {
    if (data.Success == 1) {
        var iconhtml = "";
        var actionlist = eval(data.Result);
        for (var i = 0; i < actionlist.length; i++) {
            if (actionlist[i].IconName == iconChoose) {
                iconhtml += "<div style='display:inline-block'><input type='radio' name='icon' value='" + actionlist[i].IconName + "' title=' ' checked='true'/><i class='fa " + actionlist[i].IconName + "' aria-hidden='true' data-icon='" + actionlist[i].IconName + "' style='margin-left:-18px;'></i>&nbsp;&nbsp;&nbsp;</div>";
            } else {
                iconhtml += "<div style='display:inline-block'><input type='radio' name='icon' value='" + actionlist[i].IconName + "' title=' '/><i class='fa " + actionlist[i].IconName + "' aria-hidden='true' data-icon='" + actionlist[i].IconName + "' style='margin-left:-18px;'></i>&nbsp;&nbsp;&nbsp;</div>";
            }
        }
        $("#iconhtml").empty().append(iconhtml);

        layer.closeAll('loading');
    }
}

//获取操作权限列表
crossDomainAjax("/api/navgation/actionslist?ran=" + Math.random(), ListSuccessCallback, ErrorCallback);


function SuccessCallback(navgation, index) {
    if (navgation.Success == "1") {
        $("#parentname").val(navgation.Result.ParentName);
        $("#navname").val(navgation.Result.NavName);
        $("#navtitle").val(navgation.Result.NavTitle);
        $("#iconurl").val(navgation.Result.IconUrl);
        iconChoose = navgation.Result.IconUrl;
        $("#linkurl").val(navgation.Result.LinkUrl);
        $("#orderid").val(navgation.Result.OrderID);
        $("#parentid").val(navgation.Result.ParentID);
        actions = navgation.Result.ActionTypes.split(',');
    } else {
        layer.msg("导航详细信息请求失败！");
    }
}
function ListSuccessCallback(data, index) {

    if (data.Success == 1) {
        var msghtml = "";
        var actionlist = eval(data.Result);
        for (var i = 0; i < actionlist.length; i++) {
            //是否选中
            var ischeck = "";
            for (var j = 0; j < actions.length; j++) {
                if (actionlist[i].ActionCode == actions[j]) {
                    ischeck = "checked";
                }
            }
            //插入操作按钮
            msghtml += "<input type='checkbox' name='roles' value='on' ids='" + actionlist[i].ActionCode + "' title='" + actionlist[i].ActionName + "' " + ischeck + " />";
        }
        $("#actionmsg").empty().append(msghtml);
       
        layer.closeAll('loading');
    } else {
        layer.msg("获取操作权限列表失败！");
    }
}
layui.use(['form', 'element'], function () {
    var form = layui.form(); //只有执行了这一步，部分表单元素才会修饰成功
    var element = layui.element();
});
$('.valid').css('display', 'block');
$("#refresh").click(function () {
    location.reload();
});
$("#submit").click(function () {
    var IconUrl = "";
    var radio = document.getElementsByName("icon");
    for (i = 0; i < radio.length; i++) {
        if (radio[i].checked) {
            IconUrl = radio[i].value;
        }
    }
    $("#submit").removeClass("layui-btn layui-btn");
    $("#submit").addClass("layui-btn layui-btn-disabled");
    $("#submit").attr("disabled", "disabled");
    layer.load(2);
    //查出选择的记录
    if ($(".layui-input-block input[name='roles']:checked").size() < 1) {
        layer.msg('请选择操作权限！', { time: 3000 });
        layer.closeAll('loading');
        return false;
    }
    var ids = "";
    var checkObj = $(".layui-input-block input[name='roles']:checked");
    for (var i = 0; i < checkObj.length; i++) {
        if (checkObj[i].checked && $(checkObj[i]).attr("disabled") != "disabled")
            ids += $(checkObj[i]).attr("ids") + ','; //如果选中，将value添加到变量idlist中    
    }
    var postdata = "ID=" + navid + "&ParentID=" + $("#parentid").val() + "&NavName=" + $("#navname").val() + "&NavTitle=" + $("#navtitle").val() + "&IconUrl=" + IconUrl + "&LinkUrl=" + $("#linkurl").val() + "&ActionTypes=" + ids.substring(0, ids.length - 1) + "&OrderID=" + $("#orderid").val() + "&ModifyUser=" + CMSUserID + "";
    PostAjax('/api/navgation/editInfo', postdata, SubmitSuccessCallback, PostErrorCallback);
});
function SubmitSuccessCallback(data, index) {
    if (data.Success == 1) {
        layer.msg("导航编辑成功！", { time: 1000 }, function () { location.href = "NavListManager.aspx"; });
        layer.closeAll('loading');
    } else {
        layer.msg("导航编辑失败！");
        $("#submit").removeClass("layui-btn layui-btn-disabled");
        $("#submit").addClass("layui-btn layui-btn");
        $("#submit").removeAttr("disabled");
        layer.closeAll('loading');
    }
}

