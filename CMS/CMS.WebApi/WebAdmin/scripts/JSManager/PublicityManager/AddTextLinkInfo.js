var CMSUserID = GetCMSData().CMSUserID;
function back() {
    location.href = "TextManager.aspx?pages=" + GetQueryString("pages") + "&keyword=" + escape(GetQueryString("keyword")) + "&PublicityCateID=" + GetQueryString("PublicityCateID");
}
var angularjs = angular.module('myPosition', []);
angularjs.controller('positionController', function ($scope) {

});
var PTime = {
    elem: '#PublishTime',
    format: 'YYYY-MM-DD hh:mm:ss',
    min: '1900-01-01 00:00:00',
    max: '9999-12-31 23:59:59',
    istime: true,
    choose: function (datas) {
        ETime.min = datas; //开始日选好后，重置结束日的最小日期
        ETime.start = datas //将结束日的初始值设定为开始日
    }
}; laydate(PTime);
var ETime = {
    elem: '#ExpiredTime',
    format: 'YYYY-MM-DD hh:mm:ss',
    min: '1900-01-01 00:00:00',
    max: '9999-12-31 23:59:59',
    istime: true,
    choose: function (datas) {
        PTime.max = datas; //结束日选好后，重置开始日的最大日期
    }
}; laydate(ETime);

crossDomainAjax("/api/publicity/selpositionlist?PublicityTypesID=2&ran=" + Math.random(), SuccessCallback, ErrorCallback);
function SuccessCallback(data, index) {
    var list = eval(data.Result);
    if (list.length == 0) {
        layer.msg("未获取到链接位置，请先到位置管理中添加位置！", { time: 3000 }, function () {
            location.href = "TextManager.aspx";
        });
    } else {
        for (var i = 0; i < list.length; i++) {
            $("#PublicityCategoryID").append("<option value='" + list[i].ID + "'>" + list[i].PublicityCategoryName + "</option>");
        }
        layer.closeAll('loading');
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
            //$("#PublishTime").val(getNowFormatDate());
            //$("#ExpiredTime").val("9999-12-31 23:59:59")
        });
    }
}
$('.valid').css('display', 'block');
$("#submit").click(function () {
    $("#submit").removeClass("layui-btn layui-btn");
    $("#submit").addClass("layui-btn layui-btn-disabled");
    $("#submit").attr("disabled", "disabled");
    var navType = 0;
    var publishType = 0;
    var expiredType = 0;
    var showType = "0";
    var publishTime = getNowFormatDate().toString(), expiredTime = "9999-12-31 23:59:59";
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
        CreateUser: CMSUserID,
        Attach: {
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
    PostAjax('/api/publicity/addcontentInfo', postdata, SubmitSuccessCallback, PostErrorCallback);
});
function SubmitSuccessCallback(data, index) {
    if (data.Success == 1) {
        layer.msg("文本链接新增成功！", { time: 1000 }, function () { location.href = "TextManager.aspx?pages=" + GetQueryString("pages") + "&keyword=" + escape(GetQueryString("keyword")) + "&PublicityCateID=" + GetQueryString("PublicityCateID"); });
    } else {
        layer.msg("文本链接新增失败！");
        $("#submit").removeClass("layui-btn layui-btn-disabled");
        $("#submit").addClass("layui-btn layui-btn");
        $("#submit").removeAttr("disabled");
        layer.closeAll('loading');
    }
}
function getNowFormatDate() {
    var date = new Date();
    var seperator1 = "-";
    var seperator2 = ":";
    var month = date.getMonth() + 1;
    var strDate = date.getDate();
    if (month >= 1 && month <= 9) {
        month = "0" + month;
    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate
            + " " + date.getHours() + seperator2 + date.getMinutes()
            + seperator2 + (date.getSeconds().toString().length == 1 ? date.getSeconds() + "0" : date.getSeconds());
    return currentdate;
}