var CMSUserID = GetCMSData().CMSUserID;
function back() {
    location.href = "RouteManager.html?pages=" + GetQueryString("pages") + "&keyword=" + escape(GetQueryString("keyword"));
}
var angularjs = angular.module('mySpot', []);
angularjs.controller('spotController', function ($scope) {

});
var ue = UE.getEditor('RouteContent', {
    autoHeightEnabled: true,
    autoFloatEnabled: true,
    //自定义工具
    toolbars: [['fullscreen', 'undo', 'redo', '|',
'bold', 'italic', 'underline', 'strikethrough', 'removeformat', '|', 'forecolor', 'backcolor', 'insertorderedlist', 'insertunorderedlist', 'selectall', 'cleardoc', '|',
'rowspacingtop', 'rowspacingbottom', 'lineheight', '|',
'customstyle', 'paragraph', 'fontfamily', 'fontsize', '|',
'justifyleft', 'justifycenter', 'justifyright', 'justifyjustify']]

});
$('.valid').css('display', 'block');
$("#submit").click(function () {
    $("#submit").removeClass("layui-btn layui-btn");
    $("#submit").addClass("layui-btn layui-btn-disabled");
    $("#submit").attr("disabled", "disabled");
    var postdata = {
        RouteName: $("#RouteName").val(),
        RouteContent: ue.getContent(),
        OrderID: $("#orderid").val(),
        CreatUser: CMSUserID
    };
    PostAjax('/api/route/add', postdata, SubmitSuccessCallback, PostErrorCallback);
});
function SubmitSuccessCallback(data, index) {
    if (data.Success == 1) {
        layer.msg("新增路线成功！", { time: 1000 }, function () { location.href = "RouteManager.html?pages=" + GetQueryString("pages") + "&keyword=" + escape(GetQueryString("keyword")); });
        layer.closeAll('loading');
    } else {
        layer.msg("新增路线失败！");
        $("#submit").removeClass("layui-btn layui-btn-disabled");
        $("#submit").addClass("layui-btn layui-btn");
        $("#submit").removeAttr("disabled");
        layer.closeAll('loading');
    }
}