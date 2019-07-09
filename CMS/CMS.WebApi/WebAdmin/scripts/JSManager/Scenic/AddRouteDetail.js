var CMSUserID = GetCMSData().CMSUserID;
var RouteID = GetQueryString("RouteID");
var angularjs = angular.module('mySpot', []);
angularjs.controller('spotController', function ($scope) {

});
crossDomainAjax("/api/route/getinfo?ID=" + RouteID + "&ran=" + Math.random(), RouteCallback, ErrorCallback);
function RouteCallback(data, index) {
    if (data.Success == "1") {
        $("#RouteName").val(data.Result.RouteName);
    } else {
        layer.msg("路线信息请求失败！");
    }
}
crossDomainAjax("/api/viewspot/routelist?RouteID=" + RouteID + "&ran=" + Math.random(), SuccessCallback, ErrorCallback);
function SuccessCallback(data, index) {
    if (data.Success == "1") {
        var list = eval(data.Result);
        var roleshtml = "";
        if (list.length > 0) {
            for (var i = 0; i < list.length; i++) {
                if (i == 0) {
                    roleshtml += "<input type='radio' name='viewspotid' id='" + list[i].ID + "' value='" + list[i].Name + "' title='" + list[i].Name + "' checked>";
                } else {
                    roleshtml += "<input type='radio' name='viewspotid' id='" + list[i].ID + "' value='" + list[i].Name + "' title='" + list[i].Name + "' >";
                }
            }
            $("#viewspots").append(roleshtml);
        } else {
            layer.msg("景点已全部添加到推荐路线！", { time: 3000 }, function () { location.href = "RouteDetailManager.html?RouteID=" + RouteID + "&pages=" + GetQueryString("pages") + "&keyword=" + escape(GetQueryString("keyword")); });
            return false;
        }
    } else {
        layer.msg("路线信息请求失败！");
    }
    layui.use('form', function () {
        var form = layui.form(); //只有执行了这一步，部分表单元素才会修饰成功
    });
    layer.closeAll('loading');
}
$('.valid').css('display', 'block');
$("#submit").click(function () {
    $("#submit").removeClass("layui-btn layui-btn");
    $("#submit").addClass("layui-btn layui-btn-disabled");
    $("#submit").attr("disabled", "disabled");
    var chkRadio = $('input:radio[name="viewspotid"]:checked').attr('id');
    var postdata = {
        RouteID: RouteID,
        ViewSpotID: chkRadio,
        OrderID: $("#orderid").val(),
        CreatUser: CMSUserID
    };
    PostAjax('/api/route/addroutespot', postdata, SubmitSuccessCallback, PostErrorCallback);
});
function SubmitSuccessCallback(data, index) {
    if (data.Success == 1) {
        layer.msg("新增路线景点成功！", { time: 1000 }, function () { location.href = "RouteDetailManager.html?RouteID=" + RouteID + "&pages=" + GetQueryString("pages") + "&keyword=" + escape(GetQueryString("keyword")); });
        layer.closeAll('loading');
    } else {
        layer.msg("新增路线景点失败！");
        $("#submit").removeClass("layui-btn layui-btn-disabled");
        $("#submit").addClass("layui-btn layui-btn");
        $("#submit").removeAttr("disabled");
        layer.closeAll('loading');
    }
}