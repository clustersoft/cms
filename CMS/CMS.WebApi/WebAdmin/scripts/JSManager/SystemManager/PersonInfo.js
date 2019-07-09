    layer.load(2);
    var userid = GetCMSData().CMSUserID; 
    crossDomainAjax("/api/user/getinfo?ID=" + userid + "&ran=" + Math.random(), SuccessCallback, ErrorCallback);
    angular.module('myUser', []).controller('userController', ['$scope', function ($scope) {
        $scope.username = $("#username").val();
        $scope.email = $("#email").val();
    }]);
    function SuccessCallback(user, index) {
        if (user.Success == "1") {
            $("#username").val(user.Result.UserName);
            $("#email").val(user.Result.Email);
            $("#phone").val(user.Result.Phone);
            $("#usersourcefrom").val(user.Result.UserSourceFrom);
        } else {
            layer.msg("用户信息请求失败！");
        }
    }
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
        var postdata = "ID=" + userid + "&UserName=" + $("#username").val() + "&Email=" + $("#email").val() + "&Phone=" + $("#phone").val() + "&UserSourceFrom=" + $("#usersourcefrom").val() + "&ModifyUser=" + userid;
        PostAjax('/api/user/changePersonalInfo', postdata, SubmitSuccessCallback, PostErrorCallback);
    });
    function SubmitSuccessCallback(data, index) {
        if (data.Success == 1) {
            layer.msg("修改个人信息成功！", { time: 1000 });
            layer.closeAll('loading');
            setTimeout(function () {
                var index = parent.layer.getFrameIndex(window.name); parent.layer.close(index);
            }, 1000);
        } else {
            layer.msg("修改个人信息失败！");
            $("#submit").removeClass("layui-btn layui-btn-disabled");
            $("#submit").addClass("layui-btn layui-btn");
            $("#submit").removeAttr("disabled");
            layer.closeAll('loading');
        }
    }
