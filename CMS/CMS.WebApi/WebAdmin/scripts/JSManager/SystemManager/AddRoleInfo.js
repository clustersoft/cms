        layer.load(2);
        var CMSUserID = GetCMSData().CMSUserID;
        function back() {
            location.href = "RolesListManager.aspx?pages=" + GetQueryString("pages") + "&keyword=" + escape(GetQueryString("keyword"));
        }
        var angularjs = angular.module('myRole', []);
        angularjs.controller('roleController', function ($scope) {
            $scope.isDup = function (rolename) {
                //验证登录名是否重复
                var result;
                $.ajax({
                    headers: { Authorization: GetCMSData().CMSToken },
                    url: "/api/role/valid?rolename=" + escape(rolename) + "&ran=" + Math.random(),
                    cache: false,
                    dataType: 'json',
                    type: 'GET',
                    async: false,
                    success: function (data, success) {
                        result = data.Success;
                        if (result == "0") {
                            $scope.myForm.rolename.$setValidity("rolename", false)
                        } else {
                            $scope.myForm.rolename.$setValidity("rolename", true);
                        }
                    },
                    error: function (data) {
                        errorCallback(data, index);
                    }
                });
            }
        });

        //初始化权限列表
        crossDomainAjax("/api/role/firstloadInfo?ran=" + Math.random(), ListSuccessCallback, ErrorCallback);
        $('.valid').css('display', 'block');
        function ListSuccessCallback(data, index) {
            if (data.Success == 1) {
                var roleshtml = "";
                var categoryshtml = "";
                //导航权限
                var roleslist = eval(data.Result.navlist);
                for (var rl = 0; rl < roleslist.length; rl++) {
                    var actions = roleslist[rl].Actions.split(',');
                    var actionsname = roleslist[rl].ActionsName.split(',');
                    roleshtml +=
                        "<tr>" +
                        "<td>" + lays(roleslist[rl].Layer) + roleslist[rl].NavTitle + "</td>" +
                        "<td name='rolecheck'>";
                    for (var a = 0; a < actions.length; a++) {
                        roleshtml += "<input type='checkbox' name='navchk' value='on' ids='" + (roleslist[rl].NavName + "_" + actions[a]) + "' title='" + actionsname[a] + "' />";
                    }
                    roleshtml += "</td>" +
                     "</tr>";
                }
                $("#navroles").append(roleshtml);
                //栏目权限
                var categoryslist = eval(data.Result.categorylist);
                for (var rl = 0; rl < categoryslist.length; rl++) {
                    categoryshtml +=
                        "<tr>" +
                        "<td>" + lays(categoryslist[rl].Layer) + categoryslist[rl].CateName + "</td>" +
                        "<td name='rolecheck'>" +
                        "<input type='checkbox' name='catechk' value='on' ids='" + categoryslist[rl].ID + "_edit' title='编辑' />" +
                        "<input type='checkbox' name='catechk' value='on' ids='" + categoryslist[rl].ID + "_audit' title='审核' />" +
                        "</td>" +
                        "</tr>";
                }
                $("#cateroles").append(categoryshtml);
                layui.use(['element', 'layer','form'], function () {
                    var form = layui.form(); //只有执行了这一步，部分表单元素才会修饰成功

                    form.on('checkbox(all_show)', function (data) {
                        var child = $(data.elem).parents('table').find('tbody input[title="显示"]');
                        child.each(function (index, item) {
                            item.checked = data.elem.checked;
                        });
                        form.render('checkbox');
                    });
                    form.on('checkbox(all_add)', function (data) {
                        var child = $(data.elem).parents('table').find('tbody input[title="添加"]');
                        child.each(function (index, item) {
                            item.checked = data.elem.checked;
                        });
                        form.render('checkbox');
                    });
                    form.on('checkbox(all_edit2)', function (data) {
                        var child = $(data.elem).parents('table').find('tbody input[title="编辑"]');
                        child.each(function (index, item) {
                            item.checked = data.elem.checked;
                        });
                        form.render('checkbox');
                    });
                    form.on('checkbox(all_delete)', function (data) {
                        var child = $(data.elem).parents('table').find('tbody input[title="删除"]');
                        child.each(function (index, item) {
                            item.checked = data.elem.checked;
                        });
                        form.render('checkbox');
                    });
                    form.on('checkbox(all_save)', function (data) {
                        var child = $(data.elem).parents('table').find('tbody input[title="保存"]');
                        child.each(function (index, item) {
                            item.checked = data.elem.checked;
                        });
                        form.render('checkbox');
                    });


                    form.on('checkbox(all_edit)', function (data) {
                        var child = $(data.elem).parents('table').find('tbody input[title="编辑"]');
                        child.each(function (index, item) {
                            item.checked = data.elem.checked;
                        });
                        form.render('checkbox');
                    });
                    form.on('checkbox(all_audit)', function (data) {
                        var child = $(data.elem).parents('table').find('tbody input[title="审核"]');
                        child.each(function (index, item) {
                            item.checked = data.elem.checked;
                        });
                        form.render('checkbox');
                    });

                    var element = layui.element();
                    var layer = layui.layer;
  
                    //监听折叠
                    //element.on('collapse(test)', function(data){
                        //layer.msg('展开状态：'+ data.show);
                    //});

                });
                layer.closeAll('loading');
            } else {
                layer.msg("获取角色列表失败！");
            }
        }
        //$("#navroles").hide();
        //$("#cateroles").hide();
        $("#submit").click(function () {
            $("#submit").removeClass("layui-btn layui-btn");
            $("#submit").addClass("layui-btn layui-btn-disabled");
            $("#submit").attr("disabled", "disabled");
            layer.load(2);
            var navsel = "";
            var checkObj = $("#navroles input:checked");
            for (var i = 0; i < checkObj.length; i++) {
                if (checkObj[i].checked && $(checkObj[i]).attr("disabled") != "disabled")
                    navsel += $(checkObj[i]).attr("ids") + ','; 
            }
            var catesel = "";
            var catecheckObj = $("#cateroles input:checked");
            for (var i = 0; i < catecheckObj.length; i++) {
                if (catecheckObj[i].checked && $(catecheckObj[i]).attr("disabled") != "disabled")
                    catesel += $(catecheckObj[i]).attr("ids") + ','; 
            }
            var postdata = "RoleName=" + $("#rolename").val() + "&Nav_SelActions=" + navsel.substring(0, navsel.length - 1) + "&Cate_SelActions=" + catesel.substring(0, catesel.length - 1) + "&Remark=" + $("#remark").val() + "&OrderID=" + $("#orderid").val() + "&CreateUser=" + CMSUserID;
            PostAjax('/api/role/addInfo', postdata, SubmitSuccessCallback, PostErrorCallback);
        });
        function SubmitSuccessCallback(data, index) {
            if (data.Success == 1) {
                layer.msg("新增角色成功！", { time: 1000 }, function () { location.href = "RolesListManager.aspx?pages=" + GetQueryString("pages") + "&keyword=" + escape(GetQueryString("keyword")); });
                layer.closeAll('loading');
            } else {
                layer.msg("新增角色失败！");
                $("#submit").removeClass("layui-btn layui-btn-disabled");
                $("#submit").addClass("layui-btn layui-btn");
                $("#submit").removeAttr("disabled");
                layer.closeAll('loading');
            }
        }
