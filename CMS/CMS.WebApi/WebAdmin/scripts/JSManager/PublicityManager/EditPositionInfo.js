var id = GetQueryString("ID");
var CMSUserID = GetCMSData().CMSUserID;
function back() {
    location.href = "PositionManager.aspx?pages=" + GetQueryString("pages") + "&keyword=" + escape(GetQueryString("keyword")) + "&PublicityTypesID=" + GetQueryString("PublicityTypesID");
}
        //获取位置类型列表
        $.ajax({
            headers: { Authorization: GetCMSData().CMSToken },
            url: "/api/publicity/positiontypelist?ran=" + Math.random(),
            async: false,
            type: "GET",
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            success: function (data) {
                if (data.Success == 1) {
                    var list = eval(data.Result);
                    for (var i = 0; i < list.length; i++) {
                        $("#PublicityTypesID").append("<option value='" + list[i].ID + "'>" + list[i].TypeName + "</option>");
                    }
                } else {
                    layer.msg("请求数据失败");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layer.msg("获取数据失败！");
                return false;
            }
        });
        //获取详细信息
        $.ajax({
            headers: { Authorization: GetCMSData().CMSToken },
            url: "/api/publicity/getpositionInfo?ran=" + Math.random(),
            async: false,
            type: "GET",
            data:"ID="+id,
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            success: function (data) {
                if (data.Success==1) {
                    $("#PublicityCategoryName").val(data.Result.PublicityCategoryName);
                    $("#PublicityTypesID").val(data.Result.PublicityTypesID);
                    $("#orderid").val(data.Result.OrderID);
                    $("#remark").val(data.Result.Remark);
                } else {
                    layer.msg("请求数据失败");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layer.msg("获取数据失败！");
                return false;
            }
        });
        var angularjs = angular.module('myPosition', []);
        angularjs.controller('positionController', function ($scope) {
            $scope.PublicityCategoryName = $("#PublicityCategoryName").val();
            $scope.orderid = $("#orderid").val();
        });
        layui.use('form', function () {
            var form = layui.form(); //只有执行了这一步，部分表单元素才会修饰成功
        });
        $('.valid').css('display', 'block');
        $("#refresh").click(function () {
            location.reload();
        });
        $("#submit").click(function () {
            $("#submit").removeClass("layui-btn layui-btn");
            $("#submit").addClass("layui-btn layui-btn-disabled");
            $("#submit").attr("disabled", "disabled");
            var postdata = "ID=" + id + "&PublicityTypesID=" + $("#PublicityTypesID").val() + "&PublicityCategoryName=" + $("#PublicityCategoryName").val() + "&OrderID=" + $("#orderid").val() + "&Remark=" + $("#remark").val() + "&ModifyUser=" + CMSUserID;
            PostAjax('/api/publicity/editpositionInfo', postdata, SubmitSuccessCallback, PostErrorCallback);
        });
        function SubmitSuccessCallback(data, index) {
            if (data.Success == 1) {
                layer.msg("位置编辑成功！", { time: 1000 }, function () { location.href = "PositionManager.aspx?pages=" + GetQueryString("pages") + "&keyword=" + escape(GetQueryString("keyword")) + "&PublicityTypesID=" + GetQueryString("PublicityTypesID"); });
            } else {
                layer.msg("位置编辑失败！");
                $("#submit").removeClass("layui-btn layui-btn-disabled");
                $("#submit").addClass("layui-btn layui-btn");
                $("#submit").removeAttr("disabled");
                layer.closeAll('loading');
            }
        }