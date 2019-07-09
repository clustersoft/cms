$("#refresh").click(function () {
        location.reload();
});
function back() {
    location.href = "RouteManager.html?pages=" + GetQueryString("pages") + "&keyword=" + escape(GetQueryString("keyword"));
}
    var CMSUserID = GetCMSData().CMSUserID;
    var ID = GetQueryString("ID");

    var ue = UE.getEditor('RouteContent', {
        autoHeightEnabled: true,
        autoFloatEnabled: true,
        //自定义工具
        toolbars: [['fullscreen', 'undo', 'redo', '|',
    'bold', 'italic', 'underline',  'strikethrough', 'removeformat',  '|', 'forecolor', 'backcolor', 'insertorderedlist', 'insertunorderedlist', 'selectall', 'cleardoc', '|',
    'rowspacingtop', 'rowspacingbottom', 'lineheight', '|',
    'customstyle', 'paragraph', 'fontfamily', 'fontsize', '|',
    'justifyleft', 'justifycenter', 'justifyright', 'justifyjustify']]

    });
    crossDomainAjax("/api/route/getinfo?ID=" + ID+ "&ran=" + Math.random(), SuccessCallback, ErrorCallback);
    function SuccessCallback(data, index) {
        if (data.Success == "1") {
            $("#RouteName").val(data.Result.RouteName);
            ue.ready(function () { ue.setContent(data.Result.RouteContent); });
            $("#orderid").val(data.Result.OrderID);
        } else {
            layer.msg("路线信息请求失败！");
        }
    }
    var angularjs = angular.module('mySpot', []);
    angularjs.controller('spotController', function ($scope) {
        $scope.RouteName = $("#RouteName").val();
        $scope.orderid = $("#orderid").val();
    });
    $('.valid').css('display', 'block');
    $("#submit").click(function () {
        $("#submit").removeClass("layui-btn layui-btn");
        $("#submit").addClass("layui-btn layui-btn-disabled");
        $("#submit").attr("disabled", "disabled");
        var postdata = {
            ID: ID,
            RouteName: $("#RouteName").val(),
            RouteContent: ue.getContent(),
            OrderID: $("#orderid").val(),
            EditUser: CMSUserID
        };
        PostAjax('/api/route/edit', postdata, SubmitSuccessCallback, PostErrorCallback);
    });
    function SubmitSuccessCallback(data, index) {
        if (data.Success == 1) {
            layer.msg("编辑路线成功！", { time: 1000 }, function () { location.href = "RouteManager.html?pages=" + GetQueryString("pages") + "&keyword=" + escape(GetQueryString("keyword")); });
            layer.closeAll('loading');
        } else {
            layer.msg("编辑路线失败！");
            $("#submit").removeClass("layui-btn layui-btn-disabled");
            $("#submit").addClass("layui-btn layui-btn");
            $("#submit").removeAttr("disabled");
            layer.closeAll('loading');
        }
    }