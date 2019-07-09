var ur = geturl();
var CMSGrade = GetCMSData().CMSGrade;
var CMSUserID = GetCMSData().CMSUserID;
var lis = [];
var nowpages = 1, loadpages = GetQueryString("pages") == null ? 1 : GetQueryString("pages");
var parent = GetQueryString("parent") == null ? 0 : GetQueryString("parent");
$("#skey").val(GetQueryString("keyword") == null ? "" : GetQueryString("keyword"));
//function add() {
//    location.href = "ModuleAdd.aspx?pages=" + nowpages + "&parent="+parent+"&keyword=" + escape($("#skey").val().replace(/(^\s*)|(\s*$)/g, ""));
//}

layui.use(['form'], function () {
    var form = layui.form();
    list();
    if (parent != 0) {
        tablelist(parent);
    }
    $("#add").click(function () {
        $("#divlb").attr("style", "display:none;");
        $("#divEdit").attr("style", "display:block;height:1000px;");
        window.editcolumn.location = "ColumnAdd.aspx?pages=" + nowpages + "&parent="+parent+"&keyword=" + escape($("#skey").val().replace(/(^\s*)|(\s*$)/g, ""));//"ColumnAdd.aspx";
        window.editcolumn.src = "ColumnAdd.aspx?pages=" + nowpages + "&parent=" + parent + "&keyword=" + escape($("#skey").val().replace(/(^\s*)|(\s*$)/g, "")); //"ColumnAdd.aspx";
        $("#pagetype").val('add');
    });
    $("#cx").click(function () {
        nowpages = 1;
        list();
        var pid = 0;
        if (treenode != null) {
            pid = treenode.id
        }
        tablelist(pid);
    });
    $("#del").click(function () {
        del();
    });
});

var treenode;
var lastnode;
function beforeClick(treeId, treeNode) {
    treenode = treeNode;
    loadpages = 1;
    if ($("#pagetype").val() == 'add') {
        //alert(treeNode.name)
        window.document.getElementById('editcolumn').contentWindow.document.getElementById('ParentID').value = treeNode.name;
        window.document.getElementById('editcolumn').contentWindow.document.getElementById('pid').value = treeNode.id;
        //alert(window.document.getElementById('editcolumn').contentWindow.document.getElementById('ParentID').value)
    } else {
        if (JSON.stringify(treeNode.children) != null) {//有子节点，展示列表
            $("#divlb").attr("style", "display:block;");
            $("#divEdit").attr("style", "display:none;");
            tablelist(treeNode.id);
        }
        else {//无子节点，显示编辑页
            edit(treeNode.id);
        }
    }
    if (treeNode.id != 0) {
        checkBtn();
    }
}
//判断左右移按钮显隐
function checkBtn() {
    //当前按钮的父按钮为0时，左移禁用
    //当前按钮排序第一个时，右移禁用
    var treeObj = $.fn.zTree.getZTreeObj("wztreelist");
    
    if (treenode != null) {
        //alert(111)
        if (treenode.pId != 0) {
            $("#left").removeClass(" layui-btn-disabled");
        } else {
            if (!$("#left").hasClass("layui-btn-disabled")) {
                $("#left").toggleClass("layui-btn-disabled");
            }
        }

        var treeindex = treeObj.getNodeIndex(treenode);
        if (treeindex != 0) {
            $("#right").removeClass(" layui-btn-disabled");
        } else {
            if (!$("#right").hasClass("layui-btn-disabled")) {
                $("#right").toggleClass("layui-btn-disabled");
            }
        }
        var pre = treenode.getPreNode();//上一个节点
    }
    else {
        //alert($("#left").hasClass("layui-btn-disabled"))
        if (!$("#left").hasClass("layui-btn-disabled")) {
            $("#left").toggleClass("layui-btn-disabled");
        }
        if (!$("#right").hasClass("layui-btn-disabled")) {
            $("#right").toggleClass("layui-btn-disabled");
        }
    }
}
function edit(id) {
    $("#divlb").attr("style", "display:none;");
    $("#divEdit").attr("style", "display:block;height:1000px;");
    window.editcolumn.location = "ColumnEdit.aspx?ID=" + id+"&pages=" + nowpages + "&parent="+parent+"&keyword=" + escape($("#skey").val().replace(/(^\s*)|(\s*$)/g, ""));;
    window.editcolumn.src = "ColumnEdit.aspx?ID=" + id + "&pages=" + nowpages + "&parent=" + parent + "&keyword=" + escape($("#skey").val().replace(/(^\s*)|(\s*$)/g, ""));;
    //window.editcolumn.location = "http://www.baidu.com";
    //$("#editcolumn").html("ColumnEdit.aspx?ID=" + id);
    $("#pagetype").val('edit');
}
function up() {
    var treeObj = $.fn.zTree.getZTreeObj("wztreelist");
    var pre = treenode.getPreNode();

    if (pre != null) {
        var json = treeObj.moveNode(pre, treenode, "prev");
        //alert('pre:' + pre.OrderID + ',treenode:' + treenode.OrderID)
        //移动成功后，将两个节点的orderID交换   
        if (lastnode != null) {
            //alert(lastnode.OrderID)
            pre["OrderID"] = lastnode.OrderID;
        }
        var oldid = pre.id;
        var oldorderid = pre.OrderID;
        var newid = treenode.id;
        var neworderid = treenode.OrderID;
             
        lastnode = pre;
        ChangeOid(oldid, oldorderid, newid, neworderid);
    }
}
function down() {
    var treeObj = $.fn.zTree.getZTreeObj("wztreelist");
    var nex = treenode.getNextNode();
    if (nex != null) {
        var json = treeObj.moveNode(nex, treenode, "next");
        if (lastnode != null) {
            nex["OrderID"] = lastnode.OrderID;
        }
        var oldid = nex.id;
        var oldorderid = nex.OrderID;
        var newid = treenode.id;
        var neworderid = treenode.OrderID;
        lastnode = nex;
        ChangeOid(oldid, oldorderid, newid, neworderid);
    }
}
function left() {
    if (!$("#left").hasClass("layui-btn-disabled")) {
        var treeObj = $.fn.zTree.getZTreeObj("wztreelist");
        var par = treenode.getParentNode();//父节点
        var parpar = par.getParentNode();

        if (par != null) {
            //alert(par)
            var moveid = treenode.id;
            treeObj.addNodes(parpar, 9999, treenode);//增加节点
            treeObj.removeNode(treenode);//删除节点
            ChangePid(moveid, par.pId);
            treenode = null;
            checkBtn();
        }
    }
}
function right() {
    if (!$("#right").hasClass("layui-btn-disabled")) {
        var treeObj = $.fn.zTree.getZTreeObj("wztreelist");
        var pre = treenode.getPreNode();

        if (pre != null) {
            treeObj.addNodes(pre, 9999, treenode);//增加节点
            treeObj.removeNode(treenode);//删除节点
            ChangePid(treenode.id, pre.id);//同时修改OrderID
            treenode = null;
            checkBtn();
        }
    }
}
function ChangeOid(oldid, oldorderid, newid, neworderid) {
    var change = {
        OldID: oldid,
        OldOrderID: oldorderid,
        NewID: newid,
        NewOrderID: neworderid
    }
    //JSON.stringify(change)
    //PostAjaxJson(ur + '/api/category/changeOrderID', JSON.stringify(change), OrderIDSuccessCallback, ErrorCallback);
    $.ajax({
        url: ur + "/api/category/changeOrderID",
        async: true,
        type: "POST",
        headers: { Authorization: GetCMSData().CMSToken },
        data: JSON.stringify(change),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.Success == "1") {
                var temp = treenode.OrderID;
                treenode["OrderID"] = oldorderid;
                lastnode["OrderID"] = temp;
                lastnode = null;
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
//function OrderIDSuccessCallback(data, index) {
//    layer.closeAll('loading');
//    if (data.Success == "1") {
//        var temp = treenode.OrderID;
//        treenode["OrderID"] = oldorderid;
//        lastnode["OrderID"] = temp;
//        lastnode = null;
//    }
//}
function ChangePid(id, newpid) {
    var change = {
        ID: id,
        ParentID: newpid
    }
    //PostAjaxJson(ur + '/api/category/changeParentID', JSON.stringify(change), ParentIDSuccessCallback, ErrorCallback);
    $.ajax({
        url: ur + "/api/category/changeParentID",
        async: true,
        type: "POST",
        headers: { Authorization: GetCMSData().CMSToken },
        data: JSON.stringify(change),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.Success == "1") {
                if (treenode != null) {
                    treenode["pId"] = newpid;
                }
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
//function ParentIDSuccessCallback(data, index) {
//    layer.closeAll('loading');
//    if (data.Success == "1") {
//        if (data.Success == "1") {
//            treenode["pId"] = newpid;
//        }
//    }
//}

//加载树菜单
function list() {
    crossDomainAjax(ur + "/api/category/treelist", TreeSuccessCallback, ErrorCallback);
    crossDomainAjax(geturl() + "/api/permission/navshowlist?NavCode=Column&UserID=" + CMSUserID, PerSuccessCallback, ErrorCallback);
}
function TreeSuccessCallback(data, index) {
    layer.closeAll('loading');
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
        async: {
            enable: true,
            url: ur + "/api/category/treelist",
            autoParam: ["id"],//JSON.stringify(change),
            contentType: "application/json; charset=utf-8",
            dataType: "text",
            type: "get",
            otherParam: ""
        },
        view: {
            //dblClickExpand: false, 
            selectedMulti: false,
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
}
function PerSuccessCallback(data, index) {
    layer.closeAll('loading');
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
//根据父id加载列表
function tablelist(parentid) {    
    //alert(parentid + ',' + escape($("#skey").val()) + ',' + loadpages)

    //$.ajax({
    //    url: ur + "/api/category/list",
    //    async: false,
    //    type: "GET",
    //    data: "parentID=" + parentid + "&pageIndex=1&keywords=" + $("#skey").val(),
    //    contentType: "application/json; charset=utf-8",
    //    timeout: 10000,
    //    dataType: "json",
    //    success: function (data) {
    //        //alert("Success:" + data.Success + ";PageCount:" + data.Result.pageCount + ";ToTalCount:" + data.Result.totalCount);
    //        $("#pagecount").val(data.Result.pageCount);
    //        $("#totalcount").val(data.Result.totalCount);
    //    },
    //    error: function (XMLHttpRequest, textStatus, errorThrown) {
    //        layer.msg("获取数据失败，消息：" + textStatus + "  " + errorThrown, { time: 1000 });
    //    }
    //});
    crossDomainAjax("/api/category/list?pageIndex=1&parentID=" + parentid + "&keywords=" + escape($("#skey").val()), SuccessCallback, ErrorCallback);
    var pages = $("#pagecount").html();
    //调用分页
    laypage({
        cont: 'laypages',
        pages: pages,
        curr:loadpages,
        skip: true, //是否开启跳页
        skin: 'molv',
        jump: function (obj) {
            parent = parentid;
            nowpages = obj.curr;
            crossDomainAjax("/api/category/list?pageIndex=" + obj.curr + "&parentID=" + parentid + "&keywords=" + escape($("#skey").val()), PageSuccessCallback, ErrorCallback);
            crossDomainAjax(geturl() + "/api/permission/navshowlist?NavCode=Column&UserID=" + CMSUserID, PerSuccessCallback, ErrorCallback);
            //$.ajax({
            //    url: ur + "/api/category/list",
            //    async: false,
            //    type: "GET",
            //    data: "parentID=" + parentid + "&pageIndex=" + obj.curr + "&keywords=" + $("#skey").val(),
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    success: function (data) {
            //        layer.close(index);
            //        var listdata = eval(data.Result.list);
            //        //alert(listdata.length)
            //        var msghtml = "";
            //        for (var i = 0; i < listdata.length; i++) {
            //            msghtml +=
            //            "<tr>" +
            //                "<td style='text-align:center;' name='choose'><input type='checkbox'  id='ck' ids='" + listdata[i].ID + "' name='ck' lay-skin='primary'></td>" +
            //                "<td style='text-align:center;'>" + listdata[i].ID + "</td>" +
            //                "<td style='text-align:center;'>" + listdata[i].Name + "</td>" +
            //                "<td style='text-align:center;' name='edit'><a onclick='edit(" + listdata[i].ID + ");'>编辑</a></td>" +
            //            "</tr>";
            //        }
            //        if (msghtml == "") {
            //            msghtml = "<tr>" +
            //                    "<td colspan='4' style='text-align: center;'>暂无数据</td>" +
            //                "</tr>";
            //        }
            //        $("#msg").empty().append(msghtml);
            //    }
            //});
            
            $("input[type='checkbox']").removeAttr("checked");
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

function SuccessCallback(data, index) {
    $("#pagecount").html(data.Result.pageCount);
    $("#totalcount").html(data.Result.totalCount);
    if (data.Result.pageCount <= 1) {
        $("#allpage").attr("style", "display:none;");
    } else {
        $("#allpage").attr("style", "display:block;text-align:center;");
    }
    layer.closeAll('loading');
}
function PageSuccessCallback(data, index) {
    var listdata = eval(data.Result.list);
    var msghtml = "";
    for (var i = 0; i < listdata.length; i++) {
        msghtml +=
        "<tr>" +
            "<td style='text-align:center;' name='choose'><input type='checkbox'  id='ck' ids='" + listdata[i].ID + "' name='ck' lay-skin='primary'></td>" +
            "<td style='text-align:center;'>" + listdata[i].ID + "</td>" +
            "<td style='text-align:center;'>" + listdata[i].Name + "</td>" +
            "<td style='text-align:center;' name='edit'><a onclick='edit(" + listdata[i].ID + ");'>编辑</a></td>" +
        "</tr>";
    }
    if (msghtml == "") {
        msghtml = "<tr>" +
                "<td colspan='4' style='text-align: center;'>暂无数据</td>" +
            "</tr>";
    }
    $("#msg").empty().append(msghtml);
    layer.closeAll('loading');
}
function ErrorCallback(data, index) {
    layer.msg("获取数据失败！");
    layer.closeAll('loading');
    return false;
}

//删除
function del() {
    //查出选择的记录
    if ($(".layui-table tbody input:checked").size() < 1) {
        layer.msg('对不起，请选中您要操作的记录！', { time: 1000 });
        layer.close(index);
        return false;
    }
    var ids = "";
    var checkObj = $(".layui-table tbody input:checked");
    for (var i = 0; i < checkObj.length; i++) {
        if (checkObj[i].checked && $(checkObj[i]).attr("disabled") != "disabled")
            ids += $(checkObj[i]).attr("ids") + ','; //如果选中，将value添加到变量idlist中    
    }

    layer.confirm("你确定要删除选中的栏目？", { icon: 3, title: '提示' }, function (index) {
        PostAjax(ur + '/api/category/delete', "ids=" + ids.substring(0, ids.length - 1) + "&userID=" + CMSUserID, delSuccessCallback, ErrorCallback);

        //    $.ajax({
        //        url: ur + "/api/category/delete",
        //        async: false,
        //        type: "POST",
        //        data: "ids=" + ids.substring(0, ids.length - 1) + "&userID=" + CMSUserID,
        //        success: function (data) {
        //            if (data.Success == "1") {
        //                layer.msg("删除成功！", { time: 1000 }, function () {
        //                    list();
        //                    tablelist(treenode.id);
        //                });
        //            } else {
        //                layer.msg(data.Result, { time: 1000 });
        //            }
        //        },
        //        error: function (XMLHttpRequest, textStatus, errorThrown) {
        //            layer.msg("获取数据失败，消息：" + textStatus + "  " + errorThrown, { time: 1000 });
        //        }
        //    });
    });
    layer.closeAll('loading');
}
    

function delSuccessCallback(data, index) {
    layer.close(index);
    if (data.Success == "1") {
        layer.msg("删除成功！", { time: 1000 }, function () {
            list();
            tablelist(treenode.id);
        });
    } else {
        layer.msg(data.Result, { time: 1000 });
    }
    layer.closeAll('loading');
}