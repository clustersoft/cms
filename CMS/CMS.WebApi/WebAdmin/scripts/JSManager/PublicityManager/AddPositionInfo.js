layer.load(2);
var CMSUserID = GetCMSData().CMSUserID;
function back() {
    location.href = "PositionManager.aspx?pages=" + GetQueryString("pages") + "&keyword=" + escape(GetQueryString("keyword")) + "&PublicityTypesID=" + GetQueryString("PublicityTypesID");
}
var angularjs = angular.module('myPosition', []);
angularjs.controller('positionController', function ($scope) {

});
$.ajax({
    headers: { Authorization: GetCMSData().CMSToken },
    url: "/api/publicity/positiontypelist?ran=" + Math.random(),
    async: false,
    type: "GET",
    contentType: "application/json; charset=utf-8",
    timeout: 10000,
    dataType: "json",
    success: function (data) {
        var list = eval(data.Result);
        for (var i = 0; i < list.length; i++) {
            $("#PublicityTypesID").append("<option value='" + list[i].ID + "'>" + list[i].TypeName + "</option>");
        }
        layer.closeAll('loading');
    },
    error: function (XMLHttpRequest, textStatus, errorThrown) {
        layer.msg("获取数据失败！");
        return false;
    }
});
layui.use('form', function () {
    var form = layui.form(); //只有执行了这一步，部分表单元素才会修饰成功
});
$('.valid').css('display', 'block');
$("#submit").click(function () {
    $("#submit").removeClass("layui-btn layui-btn");
    $("#submit").addClass("layui-btn layui-btn-disabled");
    $("#submit").attr("disabled", "disabled");
    layer.load(2);
    var postdata = {
        PublicityTypesID: $("#PublicityTypesID").val(),
        PublicityCategoryName: $("#PublicityCategoryName").val(),
        OrderID: $("#orderid").val(),
        Remark: $("#remark").val(),
        CreateUser: CMSUserID
    };
    PostAjax('/api/publicity/addpositionInfo', postdata, SubmitSuccessCallback, PostErrorCallback);
});
function SubmitSuccessCallback(data, index) {
    if (data.Success == 1) {
        layer.msg("位置新增成功！", { time: 1000 }, function () { location.href = "PositionManager.aspx?pages=" + GetQueryString("pages") + "&keyword=" +escape(GetQueryString("keyword")) + "&PublicityTypesID=" + GetQueryString("PublicityTypesID"); });
        layer.closeAll('loading');
    } else {
        layer.msg("位置新增失败！");
        $("#submit").removeClass("layui-btn layui-btn-disabled");
        $("#submit").addClass("layui-btn layui-btn");
        $("#submit").removeAttr("disabled");
        layer.closeAll('loading');
    }
}
