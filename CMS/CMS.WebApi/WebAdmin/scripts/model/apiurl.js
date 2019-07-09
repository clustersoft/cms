var apiurl = "";
function geturl() {
    return apiurl;
}
//获取url中的参数
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}
function setCookie(name, value,days) {
    var exp = new Date();
    exp.setTime(exp.getTime() + days * 24 * 60 * 60 * 1000);
    document.cookie = name + "=" + escape(value) + ";path=/;expires=" + exp.toGMTString();
}
function delCookie(name) {
    var exp = new Date();
    exp.setTime(exp.getTime() - 1);
    var cval = getCookie(name);
    if (cval != null)
        document.cookie = name + "=" + cval + ";path=/;expires=" + exp.toGMTString();
}
function loginout() {
    delCookie("CMSData");
    //var CMSData = new Array();
    //CMSData = JSON.parse(getCookie("CMSData"));
    //alert(CMSData.CMSUserID);
    parent.window.location.href = "../../WebAdmin/index.aspx";
}
function getCookie(name) {
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)"); //正则匹配
    if (arr = document.cookie.match(reg)) {
        return unescape(arr[2]);
    }
    else {
        parent.window.location.href = "../../WebAdmin/index.aspx";
        return false;
    }
}
//获取cookie
function GetCMSData() {
    var CMSData = new Array();
    CMSData = JSON.parse(getCookie("CMSData")); //从cookie中还原数组
    return CMSData;
}
function ErrorCallback(data, index) {
    layer.msg("获取数据失败！");
    return false;
}

function PostErrorCallback(data, index) {
    layer.msg("获取数据失败！");
    $("#submit").removeClass("layui-btn layui-btn-disabled");
    $("#submit").addClass("layui-btn layui-btn");
    $("#submit").removeAttr("disabled");
    layer.closeAll('loading');
    return false;
}
//再次请求token
function ReToken() {
    $.ajax({
        url: "/token",
        ContentType: 'application/x-www-form-urlencoded;',
        data: { grant_type: "refresh_token", refresh_token: GetCMSData().CMSTokenRefresh },
        type: 'POST',
        dataType: 'json',
        async: false
    }).done(function (json) {
        var token = json.token_type + " " + json.access_token;
        var tokenRefresh = json.refresh_token;
        var cookie = {
            CMSUserID: GetCMSData().CMSUserID,
            CMSUserName: GetCMSData().CMSUserName,
            CMSUserSourceFrom: GetCMSData().CMSUserSourceFrom,
            CMSLastLoginTime: GetCMSData().CMSLastLoginTime,
            CMSLastLoginIp: GetCMSData().CMSLastLoginIp,
            CMSGrade: GetCMSData().CMSGrade,
            CMSToken: token,
            CMSTokenRefresh: tokenRefresh
        };
        setCookie("CMSData", JSON.stringify(cookie), 1);
        //location.reload();
    }).fail(function () {
        layer.msg("授权失效，请重新登录！");
        setTimeout(function () { parent.window.location.href = "../../WebAdmin/index.aspx"; }, 1000);
        return false;
    });
}
///导航层级显示
function lays(lay) {
    var empty = "";
    for (var i = 1; i < lay; i++) {
        empty += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
    }
    return empty + "<i class='layui-icon' style='color: #c2c2c2;'>&#xe61a;</i>&nbsp;";
}
function GetNavs(nodes) {
    var source = NavsSourData(nodes);
    var result = NavsGetList(source, 0);
    return result;
}
//转化数据源
function NavsSourData(nodes) {
    var targetArr = [];

    for (var i = 0; i < nodes.length; i++) {
        var model = {
            id: nodes[i].ID,
            parentId: nodes[i].ParentID,
            title: nodes[i].NavTitle,
            icon: nodes[i].IconUrl,
            href: nodes[i].LinkUrl,
            spread: false,
            children: []
        };
        targetArr.push(model);
    }
    return targetArr;
}
//将子节点添加到当前项中
function NavsGetList(arr, parentId) {
    var sourceArr = NavsGetInTree(arr, parentId);
    for (var i = 0; i < sourceArr.length; i++) {
        var tempId = sourceArr[i].id;
        var tempArr = NavsGetList(arr, tempId);
        sourceArr[i].children = (tempArr);
    }
    return sourceArr;
}
//获取子项
function NavsGetInTree(sourceArr, parentId) {
    var targetArr = [];
    for (var i = 0; i < sourceArr.length; i++) {
        if (sourceArr[i].parentId == parentId) {
            targetArr.push(sourceArr[i]);
        }
    }
    return targetArr;
}
var dynamicLoading = {
    css: function (path) {
        if (!path || path.length === 0) {
            throw new Error('argument "path" is required !');
        }
        var head = document.getElementsByTagName('head')[0];
        var link = document.createElement('link');
        link.href = path;
        link.rel = 'stylesheet';
        link.type = 'text/css';
        head.appendChild(link);
    },
    js: function (path) {
        if (!path || path.length === 0) {
            throw new Error('argument "path" is required !');
        }
        var head = document.getElementsByTagName('head')[0];
        var script = document.createElement('script');
        script.src = path;
        script.type = 'text/javascript';
        head.appendChild(script);
    }
}

function crossDomainAjax(url, successCallback, errorCallback) {
    layer.closeAll('loading');
    var index = layer.load(2);
    $.ajax({
        headers: { Authorization: GetCMSData().CMSToken },
        url: url,
        cache: false,
        dataType: 'json',
        type: 'GET',
        async: false
    }).done(function (data) {
        successCallback(data, index);
    }).fail(function (xhr) {
        if (xhr.status == 401) {
            ReToken();
        } else {
            errorCallback(data, index);
        }
    });
    layer.closeAll('loading');
}



function PostAjax(url,postdata, successCallback, PostErrorCallback) {
    layer.closeAll('loading');
    var index = layer.load(2);
    $.ajax({
        headers: { Authorization: GetCMSData().CMSToken },
        url: url,
        data: postdata,
        cache: false,
        dataType: 'json',
        type: 'POST',
        async: false
    }).done(function (data) {
        successCallback(data, index);
    }).fail(function (xhr) {
        if (xhr.status == 401) {
            ReToken();
        } else {
            PostErrorCallback(data, index);
        }
    });
    layer.closeAll('loading');
}

function PostAjaxJson(url, postdata, successCallback, PostErrorCallback) {
    layer.closeAll('loading');
    var index = layer.load(2);
    $.ajax({
        headers: { Authorization: GetCMSData().CMSToken },
        url: url,
        data: postdata,
        contentType: "application/json; charset=utf-8",
        cache: false,
        type: 'POST',
        async: false
    }).done(function (data) {
        successCallback(data, index);
    }).fail(function (xhr) {
        if (xhr.status == 401) {
            ReToken();
        } else {
            PostErrorCallback(data, index);
        }
    });
    layer.closeAll('loading');
}


