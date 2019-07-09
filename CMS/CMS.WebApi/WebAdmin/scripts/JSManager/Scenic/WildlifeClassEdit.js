var CMSUserID = GetCMSData().CMSUserID;
function backs() {
    location.href = "WildlifeClass.html?pages=" + GetQueryString("pages") + "&keyword=" + escape(GetQueryString("keyword").replace(/(^\s*)|(\s*$)/g, ""));
}
            var id = GetQueryString("ID");//获取Url中的ID值
            
            layui.use(['form'], function () {
                var form = layui.form();

                $("#submit").click(function () {
                    formSubmit('submit');
                });
            });

            $.ajax({
                url: geturl() + "/api/wildlifeclass/getinfo",
                type: "GET",
                headers: { Authorization: GetCMSData().CMSToken },
                data: "id=" + id,
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.Success == "1") {
                        $("#CateName").val(data.Result.CateName);
                        $("#Type").val(data.Result.Type);
                        $("#OrderID").val(data.Result.OrderID);
                    } else {
                        layer.msg("获取数据失败！", { time: 1000 });
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    layer.msg("获取数据失败，消息：" + textStatus + "  " + errorThrown, { time: 1000 });
                }
            }).fail(function (xhr) {
                if (xhr.status == 401) {
                    ReToken();
                } else {
                    ErrorCallback(data, index);
                }
            });
            var angularjs = angular.module('myClass', []);
            angularjs.controller('ClassController', function ($scope) {
                //alert($("#CateName").val())
                $scope.CateName = $("#CateName").val();
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
                    ID:id,
                    CateName: $("#CateName").val(),
                    Type: $("#Type").val(),
                    OrderID: $("#OrderID").val(),
                    EditUser: CMSUserID
                }
                PostAjaxJson(geturl() + '/api/wildlifeclass/edit', JSON.stringify(postdata), AddSuccessCallback, ErrorCallback);
                //$.ajax({
                //    url: geturl() + "/api/wildlifeclass/edit",
                //    type: "POST",
                //    data: JSON.stringify(postdata),
                //    contentType: "application/json; charset=utf-8",   //内容类型
                //    success: function (data) {
                //        //alert(data.Success);
                //        if (data.Success == "1") {
                //            layer.msg("编辑成功！", { time: 1000 }, function () { location.href = "WildlifeClass.html"; });
                //        } else {
                //            layer.msg("编辑失败！", { time: 1000 });
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
                    layer.msg("编辑成功！", { time: 1000 }, function () { backs(); });
                } else {
                    layer.msg("编辑失败！", { time: 1000 });
                    $("#submit").removeClass("clicking");
                }
                layer.closeAll('loading');
            }