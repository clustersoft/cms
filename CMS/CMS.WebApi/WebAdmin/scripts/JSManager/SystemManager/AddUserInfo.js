        layer.load(2);
        var CMSUserID = GetCMSData().CMSUserID;
        function back() {
            location.href = "UsersListManager.aspx?pages=" + GetQueryString("pages") + "&keyword=" + escape(GetQueryString("keyword"));
        }
        var angularjs = angular.module('myUser', []);
        angularjs.controller('userController', function ($scope) {

            $scope.isrepwd = function (repassword) {
                if ($("#password").val() != $("#repassword").val()) {
                    $scope.myForm.repassword.$setValidity("repassword", false);
                } else {
                    $scope.myForm.repassword.$setValidity("repassword", true);
                }
            }
            $scope.ispwd = function (password) {
                if ($("#password").val() != $("#repassword").val()) {
                    $scope.myForm.repassword.$setValidity("repassword", false);
                } else {
                    $scope.myForm.repassword.$setValidity("repassword", true);
                }
            }

            $scope.isDup = function (loginname) {
                //验证登录名是否重复
                $.ajax({
                    headers: { Authorization: GetCMSData().CMSToken },
                    url: "/api/user/valid?loginname=" + loginname + "&ran=" + Math.random(),
                    cache: false,
                    dataType: 'json',
                    type: 'GET',
                    async: false,
                    success: function (data, success) {
                        if (data.Success == "0") {
                            $scope.myForm.loginname.$setValidity("loginname", false)
                        } else {
                            $scope.myForm.loginname.$setValidity("loginname", true);
                        }
                    },
                    error: function (data) {
                        errorCallback(data, index);
                    }
                });
            }
        });

        //获取角色列表
        crossDomainAjax("/api/role/roleslist?ran=" + Math.random(), ListSuccessCallback, ErrorCallback);
       
        function ListSuccessCallback(data, index) {
            if (data.Success == 1) {
                var msghtml = "";
                var roleslist = eval(data.Result);
                for (var i = 0; i < roleslist.length; i++) {
                    msghtml += "<input type='checkbox' name='roles' ids='" + roleslist[i].ID + "' title='" + roleslist[i].RoleName + "'/>";
                }
                $("#rolesmsg").append(msghtml);
                layui.use('form', function () {
                    var form = layui.form(); //只有执行了这一步，部分表单元素才会修饰成功
                    form.on('switch', function (data) {
                        if (data.value == "1" && data.elem.checked) {
                            $("#roleslist").attr("style", "display:none");
                        } else if (data.value == "1" && data.elem.checked == false) {
                            $("#roleslist").removeAttr("style");
                        }
                    });
                });
                layer.closeAll('loading');
            } else {
                layer.msg("获取角色列表失败！");
            }
        }
        $('.valid').css('display', 'block');
        $("#submit").click(function () {
            $("#submit").removeClass("layui-btn layui-btn");
            $("#submit").addClass("layui-btn layui-btn-disabled");
            $("#submit").attr("disabled", "disabled");
            layer.load(2);
            var roleids = "";
            var checkObj = $("#rolesmsg input:checked");
            for (var i = 0; i < checkObj.length; i++) {
                if (checkObj[i].checked && $(checkObj[i]).attr("disabled") != "disabled")
                    roleids += $(checkObj[i]).attr("ids") + ','; //如果选中，将value添加到变量idlist中    
            }
            var Type = "0";
            var grade = "1";
            if (document.getElementById("Type").checked) {
                Type = "1";
                grade = "254";
            }
            var status = "0";
            if (document.getElementById("status").checked) {
                status = "1";
            }
            var tmp = hex_md5($("#password").val()).toUpperCase();
            var postdata = "LoginName=" + $("#loginname").val() + "&UserName=" + $("#username").val() + "&PassWord=" + tmp + "&Email=" + $("#email").val() + "&Phone=" + $("#phone").val() + "&Status=" + status + "&OrderID=" + $("#orderid").val() + "&Remark=" + $("#remark").val() + "&AgreeIP=&UserSourceFrom=" + $("#usersourcefrom").val() + "&Grade=" + grade + "&CreateUser=" + CMSUserID + "&Type=" + Type + "&RoleIDS=" + roleids.substring(0, roleids.length - 1);
            PostAjax('/api/user/addInfo',postdata, SubmitSuccessCallback, PostErrorCallback);
        });
        function SubmitSuccessCallback(data, index) {
            if (data.Success == 1) {
                layer.msg("新增用户成功！", { time: 1000 }, function () { location.href = "UsersListManager.aspx?pages=" + GetQueryString("pages") + "&keyword=" + escape(GetQueryString("keyword")); });
                layer.closeAll('loading');
            } else {
                layer.msg("新增用户失败！");
                $("#submit").removeClass("layui-btn layui-btn-disabled");
                $("#submit").addClass("layui-btn layui-btn");
                $("#submit").removeAttr("disabled");
                layer.closeAll('loading');
            }
        }
