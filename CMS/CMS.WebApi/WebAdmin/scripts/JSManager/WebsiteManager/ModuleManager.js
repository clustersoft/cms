var CMSUserID = GetCMSData().CMSUserID;
        var index;
        var nowpages = 1, loadpages = GetQueryString("pages") == null ? 1 : GetQueryString("pages");
        $("#skey").val(GetQueryString("keyword") == null ? "" : GetQueryString("keyword"));
        function add() {
            location.href = "ModuleAdd.aspx?pages=" + nowpages + "&keyword=" + escape($("#skey").val().replace(/(^\s*)|(\s*$)/g, ""));
        }

        layui.use(['form'], function () {
            var form = layui.form();
        });

        $(function () {                  
            $("#cx").click(function () {
                loadpages = 1;
                tablelist(geturl());
            });

            $("#del").click(function () {
                del(geturl());
            });

            //加载table列表
            tablelist(geturl());
            //alert(geturl())
        });
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

        function del(WebAPIPath) {
            //查出选择的记录
            if ($(".layui-table tbody input:checked").size() < 1) {
                layer.msg('对不起，请选中您要操作的记录！', { time: 1000 });
                return false;
            }
            var ids = "";
            var checkObj = $(".layui-table tbody input:checked");
            for (var i = 0; i < checkObj.length; i++) {
                if (checkObj[i].checked && $(checkObj[i]).attr("disabled") != "disabled")
                    ids += $(checkObj[i]).attr("ids") + ','; //如果选中，将value添加到变量idlist中    
            }

            layer.confirm("你确定要删除选中项的模版？", { icon: 3, title: '提示' }, function (index) {
                $.ajax({
                    url: WebAPIPath + "/api/template/delete",
                    async: false,
                    type: "POST",
                    headers: { Authorization: GetCMSData().CMSToken },
                    data: "ids=" + ids.substring(0, ids.length - 1) + "&userID=" + CMSUserID,
                    success: function (data) {
                        if (data.Success == "1") {
                            layer.msg("删除成功！", { time: 1000 });
                        } else {
                            layer.msg("删除失败！", { time: 1000 });
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
                //layer.close(index);
                tablelist(WebAPIPath);
            });
        }

        function ErrorCallback(data, index) {
            layer.msg("获取数据失败！");
            layer.closeAll('loading');
            return false;
        }

        function tablelist(WebAPIPath) {            
            $.ajax({
                url: WebAPIPath + "/api/template/list",
                async: false,
                type: "GET",
                headers: { Authorization: GetCMSData().CMSToken },
                data: "pageIndex=1&keyword=" + $("#skey").val(),
                contentType: "application/json; charset=utf-8",
                timeout: 10000,
                dataType: "json",
                success: function (data) {
                    //alert($("#pagecount").html())
                    $("#pagecount").html(data.Result.pageCount);
                    $("#totalcount").html(data.Result.totalCount);
                    if (data.Result.pageCount <= 1) {
                        $("#allpage").attr("style", "display:none;");
                    } else {
                        $("#allpage").attr("style", "display:block;text-align:center;");
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
                    var index = layer.load(2);
                    $.ajax({
                        url: WebAPIPath + "/api/template/list",
                        async: false,
                        type: "GET",
                        headers: { Authorization: GetCMSData().CMSToken },
                        data: "pageIndex=" + obj.curr + "&keyword=" + $("#skey").val(),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            layer.close(index);
                            var list = eval(data.Result.list);
                            var msghtml = "";
                            for (var i = 0; i < list.length; i++) {                               
                                msghtml +=
                                "<tr>" +
                                    "<td style='text-align:center;' name='choose'><input type='checkbox'  id='ck' ids='" + list[i].ID + "' name='ck' lay-skin='primary'></td>" +
                                    "<td style='text-align:center;'>" + list[i].ID + "</td>" +
                                    "<td style='text-align:center;'>" + list[i].Name + "</td>" +
                                    "<td style='text-align:center;'>" + GetTemplateUseable(list[i].Useable) + "</td>" +
                                    "<td style='text-align:center;' name='edit'><a href='ModuleEdit.aspx?ID=" + list[i].ID + "&pages=" + nowpages + "&keyword=" + escape($("#skey").val().replace(/(^\s*)|(\s*$)/g, "")) + "'>编辑</a></td>" +
                                "</tr>";
                            }
                            if (msghtml == "") {
                                msghtml = "<tr>" +
                                        "<td colspan='5' style='text-align: center;'>暂无数据</td>" +
                                    "</tr>";
                            }
                            $("#msg").empty().append(msghtml);
                        }
                    }).fail(function (xhr) {
                        if (xhr.status == 401) {
                            ReToken();
                        } else {
                            ErrorCallback(data, index);
                        }
                    });
                    $("input[type='checkbox']").removeAttr("checked");
                    crossDomainAjax(geturl() + "/api/permission/navshowlist?NavCode=Module&UserID=" + CMSUserID, PerSuccessCallback, ErrorCallback);
                    layui.use('form', function () {
                        var $ = layui.jquery, form = layui.form();
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
                }
            });             
        }

