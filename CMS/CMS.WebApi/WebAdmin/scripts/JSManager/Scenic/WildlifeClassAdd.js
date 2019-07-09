var CMSUserID = GetCMSData().CMSUserID;
//alert(GetQueryString("pages"))
function backs() {
    location.href = "WildlifeClass.html?pages=" + GetQueryString("pages") + "&keyword=" + GetQueryString("keyword");
}
            var angularjs = angular.module('myClass', []);
            angularjs.controller('ClassController', function ($scope) {
            });

            layui.use(['form'], function () {
                var form = layui.form();

                form.render();

                $("#submit").click(function () {
                    formSubmit('submit');
                });
            });

            function formSubmit() {
                if ($("#submit").hasClass('clicking')) {
                    return;
                }
                $("#submit").toggleClass("clicking");

                if ($("#Type").val() == "") {
                    layer.msg("请选择类型！");
                    $("#submit").removeClass("clicking");
                    return;
                }

                var postdata = {
                    CateName: $("#CateName").val(),
                    Type: $("#Type").val(),
                    OrderID: $("#OrderID").val(),
                    CreatUser: CMSUserID
                }

                PostAjaxJson(geturl() + '/api/wildlifeclass/add', JSON.stringify(postdata), AddSuccessCallback, ErrorCallback);
                //$.ajax({
                //    url: geturl() + "/api/wildlifeclass/add",
                //    type: "POST",
                //    data: JSON.stringify(postdata),
                //    contentType: "application/json; charset=utf-8",   //内容类型
                //    success: function (data) {
                //        //alert(data.Success);
                //        if (data.Success == "1") {
                //            layer.msg("新增成功！", { time: 1000 }, function () { location.href = "WildlifeClass.html"; });
                //        } else {
                //            layer.msg("新增失败！", { time: 1000 });
                //            $("#submit").removeClass("clicking");
                //        }
                //    },
                //    error: function (XMLHttpRequest, textStatus, errorThrown) {
                //        layer.msg("请求失败，消息：" + textStatus + "  " + errorThrown, { time: 1000 });
                //    }
                //});
            }
            function AddSuccessCallback(data, index) {
                if (data.Success == "1") {
                    layer.msg("新增成功！", { time: 1000 }, function () { backs(); });
                } else {
                    layer.msg("新增失败！", { time: 1000 });
                    $("#submit").removeClass("clicking");
                }
                layer.closeAll('loading');
            }