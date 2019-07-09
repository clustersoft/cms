        var CMSUserID = GetCMSData().CMSUserID;
        var premission;
        list();
        $("#del").click(function () {
            del();
        });
        function del() {
            //查出选择的记录
            if ($(".layui-table tbody input:checked").size() < 1) {
                layer.msg('对不起，请选中您要操作的记录！');
                return false;
            }
            var ids = "";
            var checkObj = $(".layui-table tbody input:checked");
            for (var i = 0; i < checkObj.length; i++) {
                if (checkObj[i].checked && $(checkObj[i]).attr("disabled") != "disabled")
                    ids += $(checkObj[i]).attr("ids") + ','; //如果选中，将value添加到变量idlist中    
            }
            layer.confirm("你确定要删除选中项？", { icon: 3, title: '提示' }, function (index) {
                PostAjax('/api/navgation/delete', "IDs=" + ids.substring(0, ids.length - 1) + "&UserID=" + CMSUserID, delSuccessCallback, ErrorCallback);
            });
        }
        function delSuccessCallback(data, index) {
            if (data.Success == "1") {
                layer.msg("删除成功！", {time:1000});
                list();
            } else {
                layer.msg("删除失败，请联系管理员！");
                return false;
            }
        }

        function list() {
            crossDomainAjax("/api/navgation/list?ran=" + Math.random(), SuccessCallback, ErrorCallback);
            crossDomainAjax("/api/permission/navshowlist?NavCode=NavInfo&UserID=" + CMSUserID + "&ran=" + Math.random(), PerSuccessCallback, ErrorCallback);
        }
        function PerSuccessCallback(data, index) {
            if (data.Result.IsAdmin != 1) {
                if ((data.Result.ActionCode).indexOf('add') > 0) {
                    $("#add").css("display", "inline-block");
                    $("a[name='add']").css("display", "inline-block");
                }
                if ((data.Result.ActionCode).indexOf('delete')>0) {
                    $("#del").css("display", "inline-block");
                    $("col[name='choose']").show();
                    $("td[name='choose']").show();
                    $("th[name='choose']").show();
                }
                if ((data.Result.ActionCode).indexOf('edit') > 0) {
                    $("a[name='edit']").css("display", "inline-block");
                }
            } else {
                $("#add").css("display", "inline-block");
                $("a[name='add']").css("display", "inline-block");
                $("#del").css("display", "inline-block");
                $("col[name='choose']").show();
                $("td[name='choose']").show();
                $("th[name='choose']").show();
                $("a[name='edit']").css("display", "inline-block");
            }
            layer.closeAll('loading');
        }
        function SuccessCallback(data, index) {
            var userlist = eval(data.Result);
            var msghtml = "";
            for (var i = 0; i < userlist.length; i++) {
                msghtml +=
                "<tr>" +
                    "<td style='text-align:center;' name='choose'><input type='checkbox'  id='ck' ids='" + userlist[i].ID + "' name='ck' lay-skin='primary'></td>" +
                    "<td>" + lays(userlist[i].Layer) + userlist[i].NavTitle + "</td>" +
                    "<td style='text-align:center;'>" + userlist[i].NavName + "</td>" +
                    "<td style='text-align:center;'><i class='fa " + userlist[i].IconUrl + "' aria-hidden='true' data-icon='" + userlist[i].IconUrl + "'></i></td>" +
                    //"<td>" + userlist[i].LinkUrl + "</td>" +
                    "<td style='text-align:center;'>" + userlist[i].ActionTypesName + "</td>" +
                    "<td style='text-align:center;'>" + userlist[i].OrderID + "</td>" +
                    "<td style='text-align:center;'><a name=\"add\" href='AddNavInfo.aspx?NavID=" + userlist[i].ID + "'>添加子级</a>&nbsp;&nbsp;<a name=\"edit\" href='EditNavInfo.aspx?ID=" + userlist[i].ID + "'>编辑</a></td>" +
                "</tr>";
            }
            if (msghtml == "") {
                msghtml = "<td colspan=\"8\" style=\"text-align: center;\">暂无数据</td>";
            }
            $("#msg").empty().append(msghtml);
            layui.use(['form', 'layer'], function () {
                var form = layui.form();//,//只有执行了这一步，部分表单元素才会修饰成功
                form.render('checkbox');
                //全选
                form.on('checkbox(allChoose)', function (data) {
                    var child = $(data.elem).parents('table').find('tbody input[type="checkbox"]');
                    child.each(function (index, item) {
                        item.checked = data.elem.checked;
                    });
                    form.render('checkbox');
                });
            });
            layer.closeAll('loading');
        }
        function ErrorCallback(data, index) {
            layer.msg("获取数据失败！");
            return false;
        }
