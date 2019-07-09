var PTime = {
                elem: '#starttime',
                format: 'YYYY-MM-DD',
                min: '1900-01-01 00:00:00',
                max: '9999-12-31 23:59:59',
                istime: true,
                choose: function (datas) {
                    ETime.min = datas; //开始日选好后，重置结束日的最小日期
                    ETime.start = datas //将结束日的初始值设定为开始日
                }
            }; laydate(PTime);
            var ETime = {
                elem: '#endtime',
                format: 'YYYY-MM-DD',
                min: '1900-01-01 00:00:00',
                max: '9999-12-31 23:59:59',
                istime: true,
                choose: function (datas) {
                    PTime.max = datas; //结束日选好后，重置开始日的最大日期
                }
            }; laydate(ETime);

            var CMSUserID = GetCMSData().CMSUserID;
            var CMSUserName = GetCMSData().CMSUserName;
            var ur = geturl();
            var urlss = ur + "/api/article/list";
            var pageindex = 1;
            var treenode;
            var dat = "articleCategorysID=0&userID=" + CMSUserID + "&status=" + $("#status").val() + "&pageIndex=" + pageindex + "&keywords=" + $("#skey").val() + "&addTimeStart=" + $("#starttime").val() + "&addTimeEnd=" + $("#endtime").val();
            var backsource = GetQueryString("backsource");

            layui.use(['form'], function () {
                var form = layui.form();
                list();//树
                var index = layer.load(2);
                //内容添加、编辑返回时才需要读取cookie数据
                if (backsource == "1") {
                    var ContentSearchCookie = getCookie("ContentSearch");
                    if (ContentSearchCookie == null || ContentSearchCookie == "" || ContentSearchCookie == "undifined") {
                        //alert('null')
                    } else {
                        var ContentSearch = $.parseJSON(ContentSearchCookie);
                        if (ContentSearch.status != null) {
                            $("#status").val(ContentSearch.status);
                        }
                        if (ContentSearch.keywords != null) {
                            $("#skey").val(ContentSearch.keywords);
                        }
                        if (ContentSearch.addTimeStart != null) {
                            $("#starttime").val(ContentSearch.addTimeStart);
                        }
                        if (ContentSearch.addTimeEnd != null) {
                            $("#endtime").val(ContentSearch.addTimeEnd);
                        }
                        pageindex = ContentSearch.pageIndex;
                        if (ContentSearch.url == "1") {
                            urlss = ur + "/api/article/list";
                            dat = "articleCategorysID=" + ContentSearch.articleCategorysID + "&userID=" + CMSUserID + "&status=" + $("#status").val() + "&pageIndex=" + pageindex + "&keywords=" + $("#skey").val() + "&addTimeStart=" + $("#starttime").val() + "&addTimeEnd=" + $("#endtime").val();
                            tablelist(urlss, dat, ContentSearch.articleCategorysID);
                        }
                        else {
                            urlss = ur + "/api/article/userlist";
                            dat = "userID=" + CMSUserID + "&status=" + ContentSearch.status + "&pageIndex=" + pageindex + "&keywords=" + $("#skey").val() + "&addTimeStart=" + $("#starttime").val() + "&addTimeEnd=" + $("#endtime").val();
                            emaillist(urlss, dat, ContentSearch.status);
                        }
                    }
                } else {
                    setCookie("ContentSearch", "", 1);
                    tablelist(urlss, dat);
                }

                form.render();
                layer.close(index);

                $("#del").click(function () {
                    del();
                });
            });

            $("#cx").click(function () {
                var id = 0;
                if (treenode != null) {
                    id = treenode.id;
                }

                if ($("#type").val() == "0") {
                    beforeClickemail(0, treenode);
                }
                else {
                    beforeClick(0, treenode);
                }
            });

            function beforeClick(treeId, treeNode) {
                $("#type").val(1);//判断是点击邮件还是栏目，0为邮件，1为栏目
                backsource = 0;
                var cid = 0;                
                if (treeNode != null) {
                    treenode = treeNode;
                    cid = treenode.id;
                }
                urlss = ur + "/api/article/list";
                dat = "articleCategorysID=" + cid + "&userID=" + CMSUserID + "&status=" + $("#status").val() + "&pageIndex=1&keywords=" + $("#skey").val() + "&addTimeStart=" + $("#starttime").val() + "&addTimeEnd=" + $("#endtime").val();
                //alert(dat)
                tablelist(urlss, dat);
            }
            function beforeClickemail(treeId, treeNode) {
                $("#type").val(0);//判断是点击邮件还是栏目，0为邮件，1为栏目
                backsource = 0;
                var status = 1;
                if (treeNode != null) {
                    treenode = treeNode;
                }
                switch (treeNode.name) {
                    case '收件箱': status = 3; break;
                    case '发件箱': status = 1; break;
                    case '草稿箱': status = -1; break;
                    case '垃圾箱': status = 4; break;
                    default: status = 0; break;
                }
                if ($("#status").val() != " " && $("#status").val() != null) {
                    status = $("#status").val();
                }
                urlss = ur + "/api/article/userlist";
                dat = "userID=" + CMSUserID + "&pageIndex=1&keywords=" + $("#skey").val() + "&status=" + status + "&addTimeStart=" + $("#starttime").val() + "&addTimeEnd=" + $("#endtime").val();
                emaillist(urlss, dat,status);
            }

            var lis = [];
            function list() {
                $("#emailtreelist").html("");
                $("#wztreelist").html("");
                var emailsetting = {
                    view: {
                        //dblClickExpand: false,  
                        expandSpeed: 300 //设置树展开的动画速度，IE6下面没效果，  
                    },
                    data: {
                        simpleData: {   //简单的数据源，一般开发中都是从数据库里读                      
                            enable: true,
                            idKey: "id",  //id和pid，这里不用多说了吧，树的目录级别  
                            pIdKey: "pId",
                            rootPId: 0   //根节点  
                        }
                    },
                    callback: {     /**回调函数的设置**/
                        beforeClick: beforeClickemail,
                        //alert('ok');
                    }
                };
                $(document).ready(function () {  
                    var zTree = $.fn.zTree.init($("#emailtreelist"), emailsetting, emaillis);
                });
                var index = layer.load(2);
                $.ajax({
                    url: ur + "/api/article/treelist",
                    type: "GET",
                    async: true,
                    headers: { Authorization: GetCMSData().CMSToken },
                    data: "userID="+CMSUserID,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        layer.close(index);
                        var ls = eval(data.Result);
                        var list, tes = [];
                        tes = {
                            id: 0,
                            name: '根节点',
                            pId: 0,
                            OrderID: 0,
                            open: true
                        }
                        lis.push(tes);
                        for (var i = 0; i < ls.length; i++) {
                            if (ls[i].ParentID == 0) {
                                tes = {
                                    id: ls[i].ID,
                                    name: ls[i].Name,
                                    pId: ls[i].ParentID,
                                    OrderID: ls[i].OrderID,
                                    open: true
                                }
                            }
                            else {
                                tes = {
                                    id: ls[i].ID,
                                    name: ls[i].Name,
                                    pId: ls[i].ParentID,
                                    OrderID: ls[i].OrderID,
                                    open: false
                                }
                            }
                            lis.push(tes);
                        }
                        var setting = {
                            view: {
                                //dblClickExpand: false,  
                                expandSpeed: 300 //设置树展开的动画速度，IE6下面没效果，  
                            },
                            data: {
                                simpleData: {   //简单的数据源，一般开发中都是从数据库里读                      
                                    enable: true,
                                    idKey: "id",  //id和pid，这里不用多说了吧，树的目录级别  
                                    pIdKey: "pId",
                                    rootPId: 0   //根节点  
                                }
                            },
                            callback: {     /**回调函数的设置**/
                                beforeClick: beforeClick,
                            }
                        };
                        $(document).ready(function () {//初始化ztree对象     
                            var zTree = $.fn.zTree.init($("#wztreelist"), setting, lis);
                        });
                        lis.splice(0, lis.length);
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
            }
      
            //列表
            function tablelist(urlall, datas,articleCategorysID) {                
                var id = 0;
                if (articleCategorysID != null && articleCategorysID != "") {
                    id = articleCategorysID;
                }
                if (treenode != null) {
                    id = treenode.id;
                }
                var index = layer.load(2);
                $.ajax({
                    url: urlall,
                    async: false,
                    type: "GET",
                    headers: { Authorization: GetCMSData().CMSToken },
                    data: datas,
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    timeout: 10000,
                    dataType: "json",
                    success: function (data) {
                        layer.close(index);
                        //alert("Success:" + data.Success + ";PageCount:" + data.Result.pageCount + ";ToTalCount:" + data.Result.totalCount);
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
                var currpage = 1;
                if (backsource == "1") {
                    currpage = pageindex;
                    backsource = 0;
                }
                             
                laypage({
                    cont: 'laypages',
                    pages: pages,
                    skip: true, //是否开启跳页
                    curr: currpage  ,
                    skin: 'molv',
                    jump: function (obj) {
                        datas = datas.replace("pageIndex=" + pageindex, "pageIndex=" + obj.curr);
                        pageindex = obj.curr;

                        setCookie("ContentSearch", "", 1);
                        var cookie = {
                            url: $("#type").val(),
                            articleCategorysID: id,
                            status: $("#status").val() == "" ? "" : $("#status").val(),
                            pageIndex: pageindex,
                            keywords: $("#skey").val() == "" ? "" : $("#skey").val(),
                            addTimeStart: $("#starttime").val() == "" ? "" : $("#starttime").val(),
                            addTimeEnd: $("#endtime").val() == "" ? "" : $("#endtime").val()
                        };
                        setCookie("ContentSearch", JSON.stringify(cookie), 1);
                        
                        var index = layer.load(2);
                        $.ajax({
                            url: urlall,
                            async: true,
                            type: "GET",
                            headers: { Authorization: GetCMSData().CMSToken },
                            data: datas,
                            cache: false,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                var listdata = eval(data.Result.list);
                                var msghtml = "";
                                for (var i = 0; i < listdata.length; i++) {
                                    msghtml +=
                                    "<tr>" +
                                        "<td style='text-align:center;' name='choose'><input type='checkbox'  id='ck' ids='" + listdata[i].ID + "' name='ck' lay-skin='primary'></td>" +
                                        "<td style='text-align:center;'>" + listdata[i].Title + "</td>" +
                                        "<td style='text-align:center;'>" + listdata[i].CategoryNames + "</td>" +
                                        "<td style='text-align:center;'>" + listdata[i].Status + "</td>" +
                                        "<td style='text-align:center;'>" + listdata[i].CreateUserName + "</td>" +
                                        "<td style='text-align:center;'>" + listdata[i].PubTime + "</td>";
                                    if (listdata[i].IsStickTop == 0) {
                                        msghtml += "<td style='text-align:center;'><a title='置顶' href='javascript:return false;' onclick='changeTop(" + listdata[i].ID + "," + listdata[i].IsStickTop + ")'><div class='border pointer sticktop'><i class='fa fa-arrow-up' style='color:lightgray;'></i></div></a></td>";
                                    } else {
                                        msghtml += "<td style='text-align:center;'><a title='取消置顶' onclick='changeTop(" + listdata[i].ID + "," + listdata[i].IsStickTop + ")'><div class='border pointer sticktop'><i class='fa fa-arrow-up'></i></div></a></td>";
                                    }
                                    msghtml += "<td style='text-align:center;' name='edit'><a href='ContentEdit.aspx?ID=" + listdata[i].ID + "'>编辑</a>";
                                    //<i class='fa fa-edit'></i> <i class='fa fa-eercast'></i>
                                    if (listdata[i].Status != "审核通过" && listdata[i].Status != "暂存" && listdata[i].Status.indexOf('删除') == -1 && listdata[i].CreateUserName != CMSUserName) {
                                        msghtml += " <a href='ContentEdit.aspx?ID=" + listdata[i].ID + "&from=1&source=ContentListManager'>审核</a>";
                                    }
                                    msghtml += "</td></tr>";
                                }

                                if (msghtml == "") {
                                    msghtml = "<tr>" +
                                            "<td colspan='8' style='text-align: center;'>暂无数据</td>" +
                                        "</tr>";
                                }
                                $("#msg").empty().append(msghtml);
                                $("input[type='checkbox']").removeAttr("checked");
                                crossDomainAjax(geturl() + "/api/permission/navshowlist?NavCode=Article&UserID=" + CMSUserID, PerSuccessCallback, ErrorCallback);

                                layui.use('form', function () {
                                    var $ = layui.jquery, form = layui.form();
                                    form.render('checkbox');
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
                        }).fail(function (xhr) {
                            if (xhr.status == 401) {
                                ReToken();
                            } else {
                                ErrorCallback(data, index);
                            }
                        });                        
                    }
                });
            }
            //邮箱列表
            function emaillist(urlall, datas,status) {
                var id = 0;
                if (treenode != null) {
                    id = treenode.id;
                }
                $.ajax({
                    url: urlall,
                    async: false,
                    type: "GET",
                    headers: { Authorization: GetCMSData().CMSToken },
                    cache:false,
                    data: datas,
                    contentType: "application/json; charset=utf-8",
                    timeout: 10000,
                    dataType: "json",
                    success: function (data) {
                        //alert("Success:" + data.Success + ";PageCount:" + data.Result.pageCount + ";ToTalCount:" + data.Result.totalCount);
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
                //var total = $("#totalcount").html();
                
                //调用分页
                var currpage = 1;
                if (backsource == "1") {
                    currpage = pageindex;
                    backsource = 0;
                }
                
                laypage({
                    cont: 'laypages',
                    pages: pages,
                    skip: true, //是否开启跳页
                    curr: currpage,
                    skin: 'molv',
                    jump: function (obj) {
                        datas = datas.replace("pageIndex=" + pageindex, "pageIndex=" + obj.curr);
                        pageindex = obj.curr;

                        setCookie("ContentSearch", "", 1);
                        var cookie = {
                            url: $("#type").val(),
                            userID:CMSUserID,
                            status: status,
                            pageIndex: pageindex,
                            keywords: $("#skey").val() == "" ? "" : $("#skey").val(),
                            addTimeStart: $("#starttime").val() == "" ? "" : $("#starttime").val(),
                            addTimeEnd: $("#endtime").val() == "" ? "" : $("#endtime").val()
                        };
                        setCookie("ContentSearch", JSON.stringify(cookie), 1);
                        var index = layer.load(2);
                        $.ajax({
                            url: urlall,
                            async: false,
                            type: "GET",
                            headers: { Authorization: GetCMSData().CMSToken },
                            data: datas,
                            cache: false,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                layer.close(index);
                                var listdata = eval(data.Result.list);
                                var msghtml = "";
                                for (var i = 0; i < listdata.length; i++) {
                                    msghtml +=
                                    "<tr>" +
                                        "<td style='text-align:center;' name='choose'><input type='checkbox'  id='ck' ids='" + listdata[i].ID + "' name='ck' lay-skin='primary'></td>" +
                                        "<td style='text-align:center;'>" + listdata[i].Title + "</td>" +
                                        "<td style='text-align:center;'>" + listdata[i].CategoryNames + "</td>" +
                                        "<td style='text-align:center;'>" + listdata[i].Status + "</td>" +
                                        "<td style='text-align:center;'>" + listdata[i].CreateUserName + "</td>" +
                                        "<td style='text-align:center;'>" + listdata[i].PubTime + "</td>";
                                    if (listdata[i].IsStickTop == 0) {
                                        msghtml += "<td style='text-align:center;'><a title='置顶' onclick='changeTop(" + listdata[i].ID + "," + listdata[i].IsStickTop + ")'><div class='border pointer sticktop'><i class='fa fa-arrow-up' style='color:lightgray;'></i></div></a></td>";
                                    } else {
                                        msghtml += "<td style='text-align:center;'><a title='取消置顶' onclick='changeTop(" + listdata[i].ID + "," + listdata[i].IsStickTop + ")'><div class='border pointer sticktop'><i class='fa fa-arrow-up'></i></div></a></td>";
                                    }
                                    msghtml += "<td style='text-align:center;' name='edit'><a class='layui-btn layui-btn-small do-action' href='ContentEdit.aspx?ID=" + listdata[i].ID + "'>编辑</a>";

                                    if (listdata[i].Status != "审核通过" && listdata[i].Status != "暂存" && listdata[i].Status.indexOf('删除') == -1 && listdata[i].CreateUserName != CMSUserName) {
                                        msghtml += " <a class='layui-btn layui-btn-small do-action' href='ContentEdit.aspx?ID=" + listdata[i].ID + "&from=1&source=ContentListManager'>审核</a>";
                                    }
                                    msghtml += "</td></tr>";
                                }

                                if (msghtml == "") {
                                    msghtml = "<tr>" +
                                            "<td colspan='8' style='text-align: center;'>暂无数据</td>" +
                                        "</tr>";
                                }
                                $("#msg").empty().append(msghtml);
                                $("input[type='checkbox']").removeAttr("checked");
                            }
                        }).fail(function (xhr) {
                            if (xhr.status == 401) {
                                ReToken();
                            } else {
                                ErrorCallback(data, index);
                            }
                        });
                        
                        crossDomainAjax(geturl() + "/api/permission/navshowlist?NavCode=Article&UserID=" + CMSUserID, PerSuccessCallback, ErrorCallback);

                        layui.use('form', function () {
                            var $ = layui.jquery, form = layui.form();
                            form.render('checkbox');
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
            function changeTop(id, top) {
                var newtop = 0;
                if (top == 0) { newtop = 1; }
                //alert(newtop);
                var change = {
                    ID: id,
                    IsStickTop: newtop,
                    ModifyUser: CMSUserID
                }
                $.ajax({
                    url: ur + "/api/article/ChangeStickTop",
                    async: true,
                    type: "POST",
                    headers: { Authorization: GetCMSData().CMSToken },
                    data: JSON.stringify(change),
                    cache: false,
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {                        
                        if (data.Success == "1") {
                            //alert(urlss + "," + dat);
                            tablelist(urlss, dat);
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
            function del() {
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
                layer.confirm("你确定要删除选中的文章？", { icon: 3, title: '提示' }, function (index) {
                    $.ajax({
                        url: ur + "/api/article/ChangeStatesList",
                        async: false,
                        type: "POST",
                        headers: { Authorization: GetCMSData().CMSToken },
                        data: "Ids=" + ids.substring(0, ids.length - 1) + "&Status=4&userID=" + CMSUserID,
                        success: function (data) {
                            if (data.Success == "1") {
                                layer.msg("删除成功！", { time: 1000 });
                                list();
                                //加载列表
                                tablelist(urlss, dat);
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

                });
            }

            function ErrorCallback(data, index) {
                layer.msg("获取数据失败！");
                layer.closeAll('loading');
                return false;
            }
