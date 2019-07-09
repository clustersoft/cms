var CMSUserID = GetCMSData().CMSUserID;
var id = GetQueryString("ID");//获取Url中的UserID值
function back() {
    location.href = "TextManager.aspx?pages=" + GetQueryString("pages") + "&keyword=" + escape(GetQueryString("keyword")) + "&PublicityCateID=" + GetQueryString("PublicityCateID");
}
crossDomainAjax("/api/publicity/selpositionlist?PublicityTypesID=2&ran=" + Math.random(), SuccessCallback, ErrorCallback);
crossDomainAjax("/api/publicity/getcontentInfo?ID=" + id + "&ran=" + Math.random(), GetSuccessCallback, ErrorCallback);
function SuccessCallback(data, index) {
    var list = eval(data.Result);
    for (var i = 0; i < list.length; i++) {
        $("#PublicityCategoryID").append("<option value='" + list[i].ID + "'>" + list[i].PublicityCategoryName + "</option>");
    }

    layui.use('form', function () {
        var form = layui.form(); //只有执行了这一步，部分表单元素才会修饰成功
        form.on('radio(pub)', function (data) {
            if (data.value == 1)
            { $("#pubdiv").removeAttr("style"); }
            else
            { $("#pubdiv").attr("style", "display:none"); }
        });
        form.on('radio(exp)', function (data) {
            if (data.value == 1)
            { $("#expdiv").removeAttr("style"); }
            else
            { $("#expdiv").attr("style", "display:none"); }
        });
    });
}
function GetSuccessCallback(data, index) {
    if (data.Success == 1) {
        $("#PublicityName").val(data.Result.PublicityName);
        $("#PublicityCategoryID").val(data.Result.PublicityCategoryID);
        if (data.Result.NavType == "0") {
            document.getElementById("NavType0").checked = true;
        } else {
            document.getElementById("NavType1").checked = true;
        }
        $("#NavUrl").val(data.Result.NavUrl);
        $("#PublishTime").val(data.Result.PublishTime);
        if (data.Result.PublishType == "0") {
            document.getElementById("PublishType0").checked = true;
            //$("#PublishTime").val("");
        } else {
            document.getElementById("PublishType1").checked = true;
            $("#pubdiv").removeAttr("style");
        }
        $("#ExpiredTime").val(data.Result.ExpiredTime);
        if (data.Result.ExpiredType == "0") {
            document.getElementById("ExpiredType0").checked = true;
            $("#ExpiredTime").val("");
        } else {
            document.getElementById("ExpiredType1").checked = true;
            $("#expdiv").removeAttr("style");
        }
        if (data.Result.ShowType == "1") {
            document.getElementById("ShowType").checked = true;
        } else {
            document.getElementById("ShowType").checked = false;
        }
        $("#orderid").val(data.Result.OrderID);
        $("#remark").val(data.Result.Remark);
    } else {
        layer.msg("请求数据失败");
    }
    layer.closeAll('loading');
}
var angularjs = angular.module('myPosition', []);
angularjs.controller('positionController', function ($scope) {
    $scope.PublicityName = $("#PublicityName").val();
    $scope.NavUrl = $("#NavUrl").val();
    $scope.orderid = $("#orderid").val();
});
$('.valid').css('display', 'block');
$("#refresh").click(function () {
    location.reload();
});
$("#submit").click(function () {
    $("#submit").removeClass("layui-btn layui-btn");
    $("#submit").addClass("layui-btn layui-btn-disabled");
    $("#submit").attr("disabled", "disabled");
    var navType = 0;
    var publishType = 0;
    var expiredType = 0;
    var showType = "0";
    var publishTime = $("#PublishTime").val(), expiredTime = "9999-12-31 23:59:59";
    if (document.getElementById("NavType1").checked) {
        navType = 1;
    }
    if (document.getElementById("PublishType1").checked) {
        publishType = 1;
        if ($("#PublishTime").val() != "") {
            publishTime = $("#PublishTime").val();
        }
    }
    if (document.getElementById("ExpiredType1").checked) {
        expiredType = 1;
        if ($("#ExpiredTime").val() != "") {
            expiredTime = $("#ExpiredTime").val();
        }
    }
    if (document.getElementById("ShowType").checked) {
        showType = "1";
    }
    var postdata = {
        ID: id,
        PublicityTypesID: 2,
        PublicityName: $("#PublicityName").val(),
        PublicityCategoryID: $("#PublicityCategoryID").val(),
        NavType: navType,
        NavUrl: $("#NavUrl").val(),
        PublishTime: publishTime,
        PublishType: publishType,
        ExpiredTime: expiredTime,
        ExpiredType: expiredType,
        ShowType: showType,
        OrderID: $("#orderid").val(),
        Remark: $("#remark").val(),
        ModifyUser: CMSUserID,
        AttachUrl: "1",
        Attach: {
            ID: "",
            HashValue: "1",
            AttachUrl: "1",
            AttachName: "1",
            AttachNewName: "1",
            AttachType: 0,
            Remark: "0",
            AttachFormat: "0",
            AttachBytes: 0
        }
    };
    PostAjax('/api/publicity/editcontentInfo', postdata, SubmitSuccessCallback, PostErrorCallback);
});
function SubmitSuccessCallback(data, index) {
    if (data.Success == 1) {
        layer.msg("文本链接新增成功！", { time: 1000 }, function () { location.href = "TextManager.aspx?pages=" + GetQueryString("pages") + "&keyword=" +escape(GetQueryString("keyword")) + "&PublicityCateID=" + GetQueryString("PublicityCateID"); });
    } else {
        layer.msg("文本链接新增失败！");
        $("#submit").removeClass("layui-btn layui-btn-disabled");
        $("#submit").addClass("layui-btn layui-btn");
        $("#submit").removeAttr("disabled");
        layer.closeAll('loading');
    }
}