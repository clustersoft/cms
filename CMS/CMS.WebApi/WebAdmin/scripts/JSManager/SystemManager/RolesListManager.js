var CMSUserID = GetCMSData().CMSUserID;
var nowpages = 1, loadpages = GetQueryString("pages") == null ? 1 : GetQueryString("pages");
$("#skey").val(GetQueryString("keyword") == null ? "" : GetQueryString("keyword"));
function add() {
    location.href = "AddRoleInfo.aspx?pages=" + nowpages + "&keyword=" + escape($("#skey").val().replace(/(^\s*)|(\s*$)/g, ""));
}
        list();
        $("#cx").click(function () {
            loadpages = 1;
            list();
        });
        $("#del").click(function () {
            del();
        });
        $("body").keydown(function () {
            if (event.keyCode == "13") {//keyCode=13是回车键
                $('#cx').click();
            }
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
                PostAjax('/api/role/delete', "ids=" + ids.substring(0, ids.length - 1) + "&userID=" + CMSUserID, delSuccessCallback, ErrorCallback);
            });
        }

        function delSuccessCallback(data, index) {
            if (data.Success == "1") {
                layer.msg("删除成功！", { time: 1000 });
                list();
            } else if (data.Success == "0" && data.Result == "角色下有用户不可删除") {
                layer.msg("该角色信息正在使用不能删除！");
                return false;
            } else {
                layer.msg("删除失败，请联系管理员！");
                return false;
            }
        }
        function list() {
            layer.load(2);
            crossDomainAjax("/api/role/list?pageIndex=1&keywords=" + escape($("#skey").val().replace(/(^\s*)|(\s*$)/g, "")) + "&ran=" + Math.random(), SuccessCallback, ErrorCallback);
            var pages = $("#pagecount").html();
            //调用分页
            laypage({
                cont: 'laypages',
                pages: pages,
                curr: loadpages,
                skip: true, //是否开启跳页
                skin: 'molv',
                jump: function (obj) {
                    nowpages = obj.curr;
                    crossDomainAjax("/api/role/list?pageIndex=" + obj.curr + "&keywords=" + escape($("#skey").val().replace(/(^\s*)|(\s*$)/g, "")) + "&ran=" + Math.random(), PageSuccessCallback, ErrorCallback);
                }
            });
        }

        function PerSuccessCallback(data, index) {
            if (data.Result.IsAdmin != 1) {
                if ((data.Result.ActionCode).indexOf('add') > 0) {
                    $("#add").css("display", "inline-block");
                }
                if ((data.Result.ActionCode).indexOf('delete') > 0) {
                    $("#del").css("display", "inline-block");
                    $("col[name='choose']").show();
                    $("td[name='choose']").show();
                    $("th[name='choose']").show();
                }
                if ((data.Result.ActionCode).indexOf('edit') > 0) {
                    $("col[name='edit']").show();
                    $("td[name='edit']").show();
                    $("th[name='edit']").show();
                }
            } else {
                $("#add").css("display", "inline-block");
                $("#del").css("display", "inline-block");
                $("col[name='choose']").show();
                $("td[name='choose']").show();
                $("th[name='choose']").show();
                $("col[name='edit']").show();
                $("td[name='edit']").show();
                $("th[name='edit']").show();
            }
            layer.closeAll('loading');
        }
        function SuccessCallback(data, index) {
            $("#pagecount").empty().append(data.Result.pageCount);
            $("#totalcount").empty().append(data.Result.totalCount);
        }
        function PageSuccessCallback(data, index) {
            var theadmsg = "";
            theadmsg += "<tr>" +
            "<th style='text-align: center;' name='choose'>" +
            "<input type='checkbox' id='chkChoose' lay-skin='primary' lay-filter='allChoose'></th>" +
            "<th style='text-align: center;'>角色名称</th>" +
            "<th style='text-align: center;'>备注</th>" +
            " <th style='text-align: center;'>排序</th>" +
            " <th style='text-align: center;' name='edit'>操作</th>" +
                    "</tr>";


            var rolelist = eval(data.Result.list);
            var msghtml = "";
            for (var i = 0; i < rolelist.length; i++) {
                msghtml +=
                "<tr>" +
                    "<td style='text-align:center;' name='choose'><input type='checkbox'  id='ck' ids='" + rolelist[i].ID + "' name='ck' lay-skin='primary'></td>" +
                    "<td style='text-align:center;'>" + rolelist[i].RoleName + "</td>" +
                    "<td>" + rolelist[i].Remark + "</td>" +
                    "<td style='text-align:center;'>" + rolelist[i].OrderID + "</td>" +
                    "<td style='text-align:center;' name='edit'><a href='EditRoleInfo.aspx?RoleID=" + rolelist[i].ID + "&pages=" + nowpages + "&keyword=" + escape($("#skey").val().replace(/(^\s*)|(\s*$)/g, "")) + "'>编辑</a></td>" +
                "</tr>";
            }
            if (msghtml == "") {
                msghtml = "<td colspan=\"5\" style=\"text-align: center;\">暂无数据</td>";
            }
            $("#theadmsg").empty().append(theadmsg);
            $("#msg").empty().append(msghtml);
            if ($("#pagecount").html() <= 1) {
                $("#allpage").attr("style", "display:none;");
            } else {
                $("#allpage").attr("style", "display:block;text-align:center;");
            }
            crossDomainAjax("/api/permission/navshowlist?NavCode=RoleInfo&UserID=" + CMSUserID + "&ran=" + Math.random(), PerSuccessCallback, ErrorCallback);
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
        layer.closeAll('loading');
