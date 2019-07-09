<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ColumnSelect.aspx.cs" Inherits="CMSSystem.ContentManager.ColumnSelect" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>选择栏目</title>
    <link href="../scripts/zTree/css/zTreeStyle/zTreeStyle.css" rel="stylesheet" />
    <link href="../scripts/layui/css/layui.css" rel="stylesheet" />
    <link href="../css/cmsui.css" rel="stylesheet" />
</head>
<body>
    <div class="treeset" style="margin-top:10px;border-bottom:0px; ">
        <ul id="wztreelist" class="ztree"></ul>
        <div style="height:230px;"></div>
    </div>
    <form class="layui-form layui-form-pane" style="width: 95%; height:200px;position: fixed; bottom: 0;">
        <%--<div class="layui-form-item layui-form-text" >
            <label class="layui-form-label">已选栏目</label>
            <div class="layui-input-block">
                <textarea id="selected" name="selected" placeholder="" class="layui-textarea" readonly="readonly"></textarea>
            </div>
        </div>--%>
        <table class="layui-table" style="">
            <thead>
                <tr>
                    <td>已选栏目</td>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td style="height:40px;" id="selected">
                        
                    </td>
                </tr>
            </tbody>
        </table>
        <div style="margin-top:-10px;background-color:white;width:100%;height:100%;">
            <div class="layui-input-block" style="padding-top:10px;">
                <input type="button" id="submit" class="layui-btn" value="确认" onclick="javascript: ReturnDialogResult();" />
                <input type="button" id="close" class="layui-btn" value="关闭" onclick=" winclose();" />
            </div>
        </div>
        <input type="hidden" id="hidselect" />
    </form>

    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/zTree/js/jquery.ztree.core.js"></script>
    <script src="../scripts/zTree/js/jquery.ztree.exedit.js"></script>
    <script src="../scripts/zTree/js/jquery.ztree.excheck.js"></script>
    <script src="../scripts/layui/layui.js"></script>
    <script src="../scripts/model/apiurl.js"></script>
    <script type="text/javascript">
        var index = parent.layer.getFrameIndex(window.name);
        var CMSUserID = GetCMSData().CMSUserID;

        var lis = [];
        $(function () {
            var hid = GetQueryString("hid");
            $("#hidselect").val(hid);
            list();
        });

        function ReturnDialogResult() {
            parent.$('#Column').val($("#selected").html());
            
            parent.$('#hidColumn').val($("#hidselect").val());
            parent.$('#Column').focus();
            winclose();
        }

        function winclose() {
            parent.layer.close(index);
        }
        function onCheck(event, treeId, treeNode) {
            add(treeNode, 2);
        }

        function add(treeNode, type) {
            var msg = $("#selected").html();
            var hid = $("#hidselect").val();
            //alert(msg)
            if (msg.indexOf(treeNode.name) == -1) {
                //增加
                if ((type == "2" && treeNode.checked == true) || type == "1") {
                    //alert(treeNode.checked)
                    if (msg == "") {
                        $("#selected").html(treeNode.name);
                        $("#hidselect").val(treeNode.id);
                    }
                    else {
                        $("#selected").html(msg + ',' + treeNode.name);
                        $("#hidselect").val(hid + ',' + treeNode.id);
                    }
                }
            }
            else {//撤销
                msg = msg.replace(treeNode.name + ',', '').replace(','+treeNode.name, '');
                hid = hid.replace(treeNode.id + ',', '').replace(',' + treeNode.id, '');
                //alert(msg)
                //alert(hid+'|'+treeNode.id)
                if (msg.indexOf(treeNode.name) > -1) {
                    msg = msg.replace(',' + treeNode.name, '');
                    hid = hid.replace(',' + treeNode.id, '');
                    //alert(hid)
                    if (msg.indexOf(treeNode.name) > -1) {

                        msg = msg.replace(treeNode.name, '');
                        hid = hid.replace(treeNode.id, '');
                    }
                }
                //alert(msg)
                $("#selected").html(msg);
                $("#hidselect").val(hid);
            }
        }

        function list() {
            $.ajax({
                url: geturl() + "/api/article/treelist",
                type: "GET",
                headers: { Authorization: GetCMSData().CMSToken },
                data: "userID=" + CMSUserID,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var ls = eval(data.Result);                    
                    var tes = [];
                    var strname = "";
                    var selecthid = ',' + $("#hidselect").val() + ',';
                    for (var i = 0; i < ls.length; i++) {
                        if (ls[i].ParentID == 0) {
                            if (ls[i].AddArticlePermissions == 0) {
                                tes = {
                                    id: ls[i].ID,
                                    name: ls[i].Name,
                                    pId: ls[i].ParentID,
                                    OrderID: ls[i].OrderID,
                                    open: false,
                                    nocheck: true
                                }
                            } else {
                                if ($("#hidselect").val() != "" && selecthid.indexOf(',' + ls[i].ID + ',') > -1) {
                                    tes = {
                                        id: ls[i].ID,
                                        name: ls[i].Name,
                                        pId: ls[i].ParentID,
                                        OrderID: ls[i].OrderID,
                                        open: true,
                                        checked: true
                                    }
                                    strname += ls[i].Name + ',';
                                } else {
                                    tes = {
                                        id: ls[i].ID,
                                        name: ls[i].Name,
                                        pId: ls[i].ParentID,
                                        OrderID: ls[i].OrderID,
                                        open: true,
                                    }
                                }
                            }
                        }
                        else {                            
                            if (ls[i].AddArticlePermissions == 0) {
                                tes = {
                                    id: ls[i].ID,
                                    name: ls[i].Name,
                                    pId: ls[i].ParentID,
                                    OrderID: ls[i].OrderID,
                                    open: false,
                                    nocheck: true
                                }
                            } else {
                                //if ($("#hidselect").val() != "") {
                                //    var hidselect = $("#hidselect").val().split(',');
                                //    console.log(hidselect[0],hidselect[1]);
                                    
                                //}
                                if ($("#hidselect").val() != "" && selecthid.indexOf(',' + ls[i].ID + ',') > -1) {
                                    tes = {
                                        id: ls[i].ID,
                                        name: ls[i].Name,
                                        pId: ls[i].ParentID,
                                        OrderID: ls[i].OrderID,
                                        open: true,
                                        checked: true
                                    }
                                    strname += ls[i].Name + ',';
                                } else {
                                    tes = {
                                        id: ls[i].ID,
                                        name: ls[i].Name,
                                        pId: ls[i].ParentID,
                                        OrderID: ls[i].OrderID,
                                        open: true,
                                    }
                                }
                            }
                        }
                        lis.push(tes);
                    }
                    $("#selected").html(strname.substring(0,strname.length-1));

                    var setting = {
                        check: {
                            enable: true,
                            chkStyle: "checkbox",
                            chkboxType: { "Y": "", "N": "" },
                        },
                        view: {
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
                        callback: {     
                            //beforeClick: beforeClick,
                            onCheck: onCheck
                        }
                    };

                    $(document).ready(function () {//初始化ztree对象     
                        var zTree = $.fn.zTree.init($("#wztreelist"), setting, lis);
                    });
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
    </script>
</body>
</html>
