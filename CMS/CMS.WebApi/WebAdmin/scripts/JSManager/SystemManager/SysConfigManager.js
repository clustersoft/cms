        var CMSUserID = GetCMSData().CMSUserID;
        var id = GetQueryString("ID");
        crossDomainAjax("/api/sysconfig/getInfo?ran=" + Math.random(), SuccessCallback, ErrorCallback);
        var angularjs = angular.module('mySys', []);
        angularjs.controller('sysController', function ($scope) {
            $scope.PageSizes = $("#PageSizes").val();
            $scope.UploadPath = $("#UploadPath").val();
            $scope.ImgFormat = $("#ImgFormat").val();
            $scope.Title = $("#Title").val();
            $scope.VideoFormat = $("#VideoFormat").val();
            $scope.SpeakFormat = $("#SpeakFormat").val();
            $scope.AttachFormat = $("#AttachFormat").val();
            $scope.MaxImgKB = $("#MaxImgKB").val();
            $scope.MaxAttachKB = $("#MaxAttachKB").val();
            $scope.MaxResolutionHeight = $("#MaxResolutionHeight").val();
            $scope.MaxResolutionWidth = $("#MaxResolutionWidth").val();
        });
        function SuccessCallback(data, index) {
            if (data.Success == "1") {
                $("#PageSizes").val(data.Result.PageSizes);
                $("#UploadPath").val(data.Result.Uploadpath);
                $("#Title").val(data.Result.Title);
                $("#ImgFormat").val(data.Result.ImgFormat);
                $("#VideoFormat").val(data.Result.VideoFormat);
                $("#SpeakFormat").val(data.Result.SpeakFormat);
                $("#AttachFormat").val(data.Result.AttachFormat);
                $("#MaxImgKB").val(data.Result.MaxImgKB);
                $("#MaxAttachKB").val(data.Result.MaxAttachKB);
                $("#MaxResolutionHeight").val(data.Result.MaxResolutionHeight);
                $("#MaxResolutionWidth").val(data.Result.MaxResolutionWidth);
                if (data.Result.IsCutImg == 1) {
                    document.getElementById("IsCutImg").checked = true;
                }
            } else {
                layer.msg("数据请求失败！");
                return false;
            }
            layer.closeAll('loading');
        }
        layui.use(['element','form'], function () {
            var form = layui.form(); 
            var $ = layui.jquery
            , element = layui.element(); //Tab的切换功能，切换事件监听等，需要依赖element模块

        });

        $('.valid').css('display', 'block');
        $("#submit").click(function () {
            $("#submit").removeClass("layui-btn layui-btn");
            $("#submit").addClass("layui-btn layui-btn-disabled");
            $("#submit").attr("disabled", "disabled");
            var IsCutImg = 0;
            if (document.getElementById("IsCutImg").checked) {
                IsCutImg = 1;
            }
            var MaxResolutionHeight = 0;
            if ($("#MaxResolutionHeight").val() != "") {
                MaxResolutionHeight = $("#MaxResolutionHeight").val();
            }
            var MaxResolutionWidth = 0;
            if ($("#MaxResolutionWidth").val() != "") {
                MaxResolutionWidth = $("#MaxResolutionWidth").val();
            }
            var postdata = {
                ID: 1,
                Title: $("#Title").val(),
                PageSizes: $("#PageSizes").val(),
                UploadPath: $("#UploadPath").val(),
                ImgFormat: $("#ImgFormat").val(),
                VideoFormat: $("#VideoFormat").val(),
                SpeakFormat: $("#SpeakFormat").val(),
                AttachFormat: $("#AttachFormat").val(),
                MaxImgKB: $("#MaxImgKB").val(),
                MaxAttachKB: $("#MaxAttachKB").val(),
                MaxResolutionHeight: MaxResolutionHeight,
                MaxResolutionWidth: MaxResolutionWidth,
                IsCutImg: IsCutImg
            };
            PostAjax('/api/sysconfig/editInfo', postdata, SubmitSuccessCallback, PostErrorCallback);
        });
        function SubmitSuccessCallback(data, index) {
            if (data.Success == 1) {
                layer.msg("系统配置编辑成功！", { time: 1000 }, function () { location.reload(); });
            } else {
                layer.msg("系统配置编辑失败！");
            }
        }

