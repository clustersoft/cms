    layer.load(2);
    var userid = GetCMSData().CMSUserID;
    var angularjs = angular.module('myUser', []);
    angularjs.controller('userController', function ($scope) {
        $scope.isDup = function (repassword) {
            if ($("#password").val().length > 6 && $("#repassword").val().length>6) {
                if ($("#password").val() != $("#repassword").val()) {
                    $scope.myForm.repassword.$setValidity("repassword", false);
                } else {
                    $scope.myForm.repassword.$setValidity("repassword", true);
                }
            }
        }
        $scope.ispwd = function (password) {
            if ($("#password").val().length > 6 && $("#repassword").val().length > 6) {
                if ($("#password").val() != $("#repassword").val()) {
                    $scope.myForm.repassword.$setValidity("repassword", false);
                } else {
                    $scope.myForm.repassword.$setValidity("repassword", true);
                }
            }
        }
    });
    layui.use('form', function () {
        var form = layui.form(); //只有执行了这一步，部分表单元素才会修饰成功
    });
    $('.valid').css('display', 'block');
    layer.closeAll('loading');
    $("#submit").click(function () {
        $("#submit").removeClass("layui-btn layui-btn");
        $("#submit").addClass("layui-btn layui-btn-disabled");
        $("#submit").attr("disabled", "disabled");
        layer.load(2);
        var oldpwd = hex_md5($("#oldPwd").val()).toUpperCase();
        var newpwd = hex_md5($("#password").val()).toUpperCase();
        var postdata = "ID=" + userid + "&oldPwd=" + oldpwd + "&newPwd=" + newpwd;
        PostAjax('/api/user/changePwd', postdata, SubmitSuccessCallback, PostErrorCallback);
    });
    function SubmitSuccessCallback(data, index) {
        if (data.Success == 1) {
            layer.msg("修改密码成功！", { time: 1000 });
            layer.closeAll('loading');
            setTimeout(function () {
                var index = parent.layer.getFrameIndex(window.name); parent.layer.close(index);
            }, 1000);
        } else {
            layer.msg("修改密码失败！");
            $("#submit").removeClass("layui-btn layui-btn-disabled");
            $("#submit").addClass("layui-btn layui-btn");
            $("#submit").removeAttr("disabled");
            layer.closeAll('loading');
        }
    }

