var CMSUserID = GetCMSData().CMSUserID;
        var id = GetQueryString("ID");//获取Url中的ID值
        var selecttype = "";
        //alert(GetQueryString("pages"))
        function backs() {
            location.href = "WildlifeManager.html?pages=" + GetQueryString("pages") + "&keyword=" + GetQueryString("keyword");
        }

        function addnew() {
            location.href = "WildlifeDetailAdd.html?WildID=" + id + "&select=" + selecttype;
        }

        layui.use(['form'], function () {
            var form = layui.form();

            $("#topaction").html('<a class="layui-btn btn-default btn-add" id="add" href="WildlifeDetailAdd.html?ID="'+id+'" style="display:inline-block;"><i class="fa fa-plus"></i>&nbsp;新增</a>'+
                    '<a class="layui-btn btn-default layui-btn-danger" id="del" style="display:inline-block;"><i class="fa fa-trash-o"></i>&nbsp;删除</a>');

            tablelist(geturl());

            $("#del").click(function () {
                del(geturl());
            });
        });

        function tablelist(WebAPIPath) {            
            $.ajax({
                url: WebAPIPath + "/api/wildlifemanager/detaillist",
                async: false,
                type: "GET",
                headers: { Authorization: GetCMSData().CMSToken },
                data: "WildlifemanagerID=" + id,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var list = eval(data.Result);
                    var msghtml = "";
                    selecttype = "";
                    for (var i = 0; i < list.length; i++) {
                        selecttype += list[i].Type+',';
                        msghtml +=
                        "<tr>" +
                            "<td style='text-align:center;'><input type='checkbox'  id='ck' ids='" + list[i].ID + "' name='ck' lay-skin='primary'></td>" +
                            "<td style='text-align:center;'>" + list[i].TypeName + "</td>" +
                            //"<td style='text-align:center;'>" + list[i].Type + "</td>" +
                            "<td style='text-align:center;'><a href='WildlifeDetailEdit.html?ID=" + list[i].ID + "&WildID="+id+"'>编辑</a></td>" +
                        "</tr>";
                    }
                    if (msghtml == "") {
                        msghtml = "<tr>" +
                                "<td colspan='4' style='text-align: center;'>暂无数据</td>" +
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

        function del(WebAPIPath) {
            //alert(WebAPIPath)
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

            layer.confirm("你确定要删除吗？", { icon: 3, title: '提示' }, function (index) {
                $.ajax({
                    url: WebAPIPath + "/api/wildlifemanager/deletedetail",
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
                tablelist(WebAPIPath);
            });
        }