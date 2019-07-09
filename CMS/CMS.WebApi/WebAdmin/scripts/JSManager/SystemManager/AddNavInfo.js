        layer.load(2);
        var CMSUserID = GetCMSData().CMSUserID;
        var navid = GetQueryString("NavID");//获取Url中的UserID值
        if (navid != "0") {
            crossDomainAjax("/api/navgation/getInfo?ID=" + navid + "&ran=" + Math.random(), SuccessCallback, ErrorCallback);
        } else {
            $("#parentname").val("无上级导航");
        }
        angular.module('myNav', []).controller('navController', function ($scope) {
            $scope.isDup = function (navname) {
                //验证登录名是否重复
                var url = "/api/navgation/valid?NavName=" + escape(navname) + "&ran=" + Math.random();
                $.ajax({
                    headers: { Authorization: GetCMSData().CMSToken },
                    url: url,
                    cache: false,
                    dataType: 'json',
                    type: 'GET',
                    async: false,
                    success: function (data, success) {
                        if (data.Success == "0") {
                            $scope.myForm.navname.$setValidity("navname", false)
                        } else {
                            $scope.myForm.navname.$setValidity("navname", true);
                        }
                    },
                    error: function (data) {
                        errorCallback(data, index);
                    }
                });
            }
        });
        //获取图标列表
        crossDomainAjax("/api/icon/list?ran=" + Math.random(), IconListSuccessCallback, ErrorCallback);
        function IconListSuccessCallback(data, index) {
            if (data.Success == 1) {
                var iconhtml = "";
                var actionlist = eval(data.Result);
                for (var i = 0; i < actionlist.length; i++) {
                    if (i==0) {
                        iconhtml += "<div style='display:inline-block'><input type='radio' name='icon' value='" + actionlist[i].IconName + "' title=' ' checked='true'/><i class='fa " + actionlist[i].IconName + "' aria-hidden='true' data-icon='" + actionlist[i].IconName + "' style='margin-left:-18px;'></i>&nbsp;&nbsp;&nbsp;</div>";
                    } else {
                        iconhtml += "<div style='display:inline-block'><input type='radio' name='icon' value='" + actionlist[i].IconName + "' title=' '/><i class='fa " + actionlist[i].IconName + "' aria-hidden='true' data-icon='" + actionlist[i].IconName + "' style='margin-left:-18px;'></i>&nbsp;&nbsp;&nbsp;</div>";
                    }
                }
                $("#iconhtml").empty().append(iconhtml);
                
                layer.closeAll('loading');
            }
        }
        //获取操作权限列表
        crossDomainAjax("/api/navgation/actionslist?ran=" + Math.random(), ListSuccessCallback, ErrorCallback);
        $('.valid').css('display', 'block');
        function SuccessCallback(data, index) {
            if (data.Success == "1") {
                $("#parentname").val(data.Result.NavTitle);
            }
        }
        function ListSuccessCallback(data, index) {
            if (data.Success == 1) {
                var msghtml = "";
                var actionlist = eval(data.Result);
                for (var i = 0; i < actionlist.length; i++) {
                    msghtml += "<input type='checkbox' name='roles' value='on' ids='" + actionlist[i].ActionCode + "' title='" + actionlist[i].ActionName + "' />";
                }
                $("#actionmsg").empty().append(msghtml);
                layer.closeAll('loading');
            }
        }
        layui.use(['form', 'element'], function () {
            var form = layui.form(); //只有执行了这一步，部分表单元素才会修饰成功
            var element = layui.element();
        });
        $("#submit").click(function () {
            var IconUrl = "";
            var radio = document.getElementsByName("icon");
            for (i = 0; i < radio.length; i++) {
                if (radio[i].checked) {
                    IconUrl=radio[i].value;
                }
            }
            $("#submit").removeClass("layui-btn layui-btn");
            $("#submit").addClass("layui-btn layui-btn-disabled");
            $("#submit").attr("disabled", "disabled");
            layer.load(2);
            //查出选择的记录
            if ($(".layui-input-block input[name='roles']:checked").size() < 1) {
                layer.msg('请选择操作权限！', { time: 3000 });
                layer.closeAll('loading');
                return false;
            }
            var ids = "";
            var checkObj = $(".layui-input-block input[name='roles']:checked");
            for (var i = 0; i < checkObj.length; i++) {
                if (checkObj[i].checked && $(checkObj[i]).attr("disabled") != "disabled")
                    ids += $(checkObj[i]).attr("ids") + ','; //如果选中，将value添加到变量idlist中    
            }
            var postdata = "ParentID=" + navid + "&NavName=" + $("#navname").val() + "&NavTitle=" + $("#navtitle").val() + "&IconUrl=" + IconUrl + "&LinkUrl=" + $("#linkurl").val() + "&ActionTypes=" + ids.substring(0, ids.length - 1) + "&OrderID=" + $("#orderid").val() + "&CreateUser=" + CMSUserID + "";
            PostAjax('/api/navgation/addInfo', postdata, SubmitSuccessCallback, PostErrorCallback);
        });
        function SubmitSuccessCallback(data, index) {
            if (data.Success == 1) {
                layer.msg("新增导航成功！", { time: 1000 }, function () { location.href = "NavListManager.aspx"; });
                layer.closeAll('loading');
            } else {
                layer.msg("新增导航失败！");
                $("#submit").removeClass("layui-btn layui-btn-disabled");
                $("#submit").addClass("layui-btn layui-btn");
                $("#submit").removeAttr("disabled");
                layer.closeAll('loading');
            }
        }
