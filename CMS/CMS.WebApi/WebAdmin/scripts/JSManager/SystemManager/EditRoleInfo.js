        layer.load(2);
        var CMSUserID = GetCMSData().CMSUserID;
        var RoleID = GetQueryString("RoleID");//
        function back() {
            location.href = "RolesListManager.aspx?pages=" + GetQueryString("pages") + "&keyword=" + escape(GetQueryString("keyword"));
        }
        var angularjs = angular.module('myRole', []);
        //初始化权限列表
        crossDomainAjax("/api/role/getInfo?ID=" + RoleID + "&ran=" + Math.random(), ListSuccessCallback, ErrorCallback);
        angularjs.controller('roleController', function ($scope) {
            $scope.rolename = $("#rolename").val();
            $scope.orderid = $("#orderid").val();
            $scope.isDup = function (rolename) {
                //验证登录名是否重复
                $.ajax({
                    headers: { Authorization: GetCMSData().CMSToken },
                    url: "/api/role/valid?rolename=" + escape(rolename) + "&ran=" + Math.random(),
                    cache: false,
                    dataType: 'json',
                    type: 'GET',
                    async: false,
                    success: function (data, success) {
                        if (data.Success == "0") {
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
        function ListSuccessCallback(data, index) {
            if (data.Success == 1) {
                $("#rolename").val(data.Result.RoleName);
                $("#remark").val(data.Result.Remark);
                var roleshtml = "";
                var categoryshtml = "";
                //导航权限
                var roleslist = eval(data.Result.Navlist);
                var allshow = 1, alledit2 = 1, alladd = 1, alldelete = 1, allsave = 1;
                for (var rl = 0; rl < roleslist.length; rl++) {
                    var actions = roleslist[rl].Actions.split(',');
                    var actionsname = roleslist[rl].ActionsName.split(',');
                    var selactions = roleslist[rl].SelActions.split(',');
                    roleshtml +=
                        "<tr>" +
                        "<td>" + lays(roleslist[rl].Layer) + roleslist[rl].NavTitle + "</td>" +
                        "<td name='rolecheck'>";
                    for (var a = 0; a < actions.length; a++) {
                        var issel = 0;
                        for (var s = 0; s < selactions.length; s++) {
                            if (actions[a] == selactions[s]) {
                                issel = 1;
                            }
                        }
                        if (issel == 1) {
                            roleshtml += "<input type='checkbox' name='navchk' ids='" + (roleslist[rl].NavName + "_" + actions[a]) + "' title='" + actionsname[a] + "' checked>";
                        } else {
                            roleshtml += "<input type='checkbox' name='navchk' ids='" + (roleslist[rl].NavName + "_" + actions[a]) + "' title='" + actionsname[a] + "'>";
                            if (actionsname[a]=="显示") {
                                allshow = 0;
                            } else if (actionsname[a] == "添加") {
                                alladd = 0;
                            } else if (actionsname[a] == "编辑") {
                                alledit2 = 0;
                            } else if (actionsname[a] == "删除") {
                                alldelete = 0;
                            } else if (actionsname[a] == "保存") {
                                allsave = 0;
                            }
                        }
                    }
                    roleshtml += "</td>" +
                    "</tr>";
                }
                var navallcheck = "";
                if (allshow == 1) {
                    navallcheck+="<input type='checkbox' name='navchk' value='on' lay-filter='all_show' title='显示全选' checked />";
                }else {
                    navallcheck+="<input type='checkbox' name='navchk' value='on' lay-filter='all_show' title='显示全选' />";
                }
                if (alladd == 1) {
                    navallcheck+="<input type='checkbox' name='navchk' value='on' lay-filter='all_add' title='添加全选' checked />";
                }else {
                    navallcheck+="<input type='checkbox' name='navchk' value='on' lay-filter='all_add' title='添加全选' />";
                }
                if (alledit2 == 1) {
                    navallcheck+="<input type='checkbox' name='navchk' value='on' lay-filter='all_edit2' title='编辑全选' checked/>";
                }else {
                    navallcheck+="<input type='checkbox' name='navchk' value='on' lay-filter='all_edit2' title='编辑全选' />";
                }
                if (alldelete == 1) {
                    navallcheck+="<input type='checkbox' name='navchk' value='on' lay-filter='all_delete' title='删除全选' checked/>";
                }else {
                    navallcheck+="<input type='checkbox' name='navchk' value='on' lay-filter='all_delete' title='删除全选' />";
                }
                if (allsave == 1) {
                    navallcheck += "<input type='checkbox' name='navchk' value='on' lay-filter='all_save' title='保存全选' checked/>";
                }else {
                    navallcheck += "<input type='checkbox' name='navchk' value='on' lay-filter='all_save' title='保存全选' />";
                }
                $("#navallcheck").empty().append(navallcheck);
                $("#navroles").append(roleshtml);
                //栏目权限
                var categoryslist = eval(data.Result.Categorylist);


                var allaudit = 1, alledit = 1;
                for (var rl = 0; rl < categoryslist.length; rl++) {
                    var selactions = categoryslist[rl].SelActions.split(',');
                    categoryshtml +=
                        "<tr>" +
                        "<td>" + lays(categoryslist[rl].Layer) + categoryslist[rl].CateName + "</td>" +
                        "<td name='rolecheck'>";

                    var isedit = 0;
                    for (var s = 0; s < selactions.length; s++) {
                        if (selactions[s] == "edit") {
                            isedit = 1;
                        }
                    } if (isedit == 0) {
                        categoryshtml +=
                            "<input type='checkbox' name='catechk' ids='" + categoryslist[rl].ID + "_edit' title='编辑'>";
                        alledit = 0;
                    } else {

                        categoryshtml +=
                            "<input type='checkbox' name='catechk' ids='" + categoryslist[rl].ID + "_edit' title='编辑' checked>";
                    }
                    var isaudit = 0;
                    for (var s = 0; s < selactions.length; s++) {
                        if (selactions[s] == "audit") {
                            isaudit = 1;
                        }
                    } if (isaudit == 0) {
                        categoryshtml +=
                            "<input type='checkbox' name='catechk' ids='" + categoryslist[rl].ID + "_audit' title='审核'>";
                        allaudit = 0;
                    } else {

                        categoryshtml +=
                            "<input type='checkbox' name='catechk' ids='" + categoryslist[rl].ID + "_audit' title='审核' checked>";
                    }
                    categoryshtml +=
                        "</td>" +
                        "</tr>";
                }
                var cateallcheck = "";
                if (alledit == 1) {
                    cateallcheck += "<input type='checkbox' name='navchk' value='on' lay-filter='all_edit' title='编辑全选' checked />";
                } else {
                    cateallcheck += "<input type='checkbox' name='navchk' value='on' lay-filter='all_edit' title='编辑全选' />";
                }
                if (allaudit == 1) {
                    cateallcheck += "<input type='checkbox' name='navchk' value='on' lay-filter='all_audit' title='审核全选' checked />";
                } else {
                    cateallcheck += "<input type='checkbox' name='navchk' value='on' lay-filter='all_audit' title='审核全选' />";
                }
                $("#cateallcheck").empty().append(cateallcheck);
                $("#cateroles").append(categoryshtml);
                $("#orderid").val(data.Result.OrderID);
                layui.use(['element', 'layer', 'form'], function () {
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
                });
                layer.closeAll('loading');
            } else {
                layer.msg("获取角色列表失败！");
            }
        }
        $('.valid').css('display', 'block');
        $("#refresh").click(function () {
            location.reload();
        });
        $("#submit").click(function () {
            $("#submit").removeClass("layui-btn layui-btn");
            $("#submit").addClass("layui-btn layui-btn-disabled");
            $("#submit").attr("disabled", "disabled");
            layer.load(2);
            var navsel = "";
            var checkObj = $("#navroles input:checked");
            for (var i = 0; i < checkObj.length; i++) {
                if (checkObj[i].checked && $(checkObj[i]).attr("disabled") != "disabled")
                    navsel += $(checkObj[i]).attr("ids") + ','; //如果选中，将value添加到变量idlist中    
            }
            var catesel = "";
            var catecheckObj = $("#cateroles input:checked");
            for (var i = 0; i < catecheckObj.length; i++) {
                if (catecheckObj[i].checked && $(catecheckObj[i]).attr("disabled") != "disabled")
                    catesel += $(catecheckObj[i]).attr("ids") + ','; //如果选中，将value添加到变量idlist中    
            }
            var postdata = "ID=" + RoleID + "&RoleName=" + $("#rolename").val() + "&Nav_SelActions=" + navsel.substring(0, navsel.length - 1) + "&Cate_SelActions="  +catesel.substring(0, catesel.length - 1) + "&Remark=" + $("#remark").val() + "&ModifyUser=" + CMSUserID + "&OrderID=" + $("#orderid").val();
            PostAjax('/api/role/editInfo', postdata, SubmitSuccessCallback, PostErrorCallback);
        });
        function SubmitSuccessCallback(data, index) {
            if (data.Success == 1) {
                layer.msg("编辑角色成功！",{time:1000}, function () { location.href = "RolesListManager.aspx?pages=" + GetQueryString("pages") + "&keyword=" +escape(GetQueryString("keyword")); });
                layer.closeAll('loading');
            } else {
                layer.msg("编辑角色失败！");
                $("#submit").removeClass("layui-btn layui-btn-disabled");
                $("#submit").addClass("layui-btn layui-btn");
                $("#submit").removeAttr("disabled");
                layer.closeAll('loading');
            }
        }