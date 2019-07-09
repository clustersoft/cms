//var PTime = {
//    elem: '#PublishTime',
//    format: 'YYYY-MM-DD hh:mm:ss',
//    min: '1900-01-01 00:00:00',
//    max: '9999-12-31 23:59:59',
//    istime: true,
//    choose: function (datas) {
//        ETime.min = datas; //开始日选好后，重置结束日的最小日期
//        ETime.start = datas //将结束日的初始值设定为开始日
//    }
//}; laydate(PTime);
//var ETime = {
//    elem: '#ExpiredTime',
//    format: 'YYYY-MM-DD hh:mm:ss',
//    min: '1900-01-01 00:00:00',
//    max: '9999-12-31 23:59:59',
//    istime: true,
//    choose: function (datas) {
//        PTime.max = datas; //结束日选好后，重置开始日的最大日期
//    }
//}; laydate(ETime);
var AttachID, HashValue = "", AttachUrl, AttachName, AttachNewName, AttachType, Remark, AttachFormat, AttachBytes;
var id = GetQueryString("ID");//获取Url中的UserID值
var CMSUserID = GetCMSData().CMSUserID;
var UploadFormat = "", UploadFormatTypes = "";
function back() {
    location.href = "PictureManager.aspx?pages=" + GetQueryString("pages") + "&keyword=" +escape(GetQueryString("keyword")) + "&PublicityCateID=" + GetQueryString("PublicityCateID");
}
crossDomainAjax("/api/sysconfig/getInfo?ran=" + Math.random(), FormatCallback, ErrorCallback);
function FormatCallback(data, index) {
    if (data.Success == "1") {
        UploadFormat = data.Result.ImgFormat;
        UploadFormatTypes = 'image/' + UploadFormat.replace(/,/g, ',image/');
    } else {
        layer.msg("数据请求失败！");
        return false;
    }
    layer.closeAll('loading');
}
// 初始化Web Uploader
var uploader = WebUploader.create({
    auto: true,
    swf: '../scripts/webuploader/dist/Uploader.swf',
    server: apiurl + '/api/attach/upload',
    pick: {
        id: $("#filePicker"), // id
        multiple: false  // false  单选 
    },
    accept: {
        title: 'Images',
        extensions: UploadFormat,
        mimeTypes: UploadFormatTypes
    },
    duplicate: true
});// 当有文件添加进来的时候
uploader.on('fileQueued', function (file) {
    var $li = $(
            '<div id="' + file.id + '" class="file-item thumbnail">' +
                '<img>' +
                '<div class="info">' + file.name + '</div>' +
            '</div>'
            ),
        $img = $li.find('img');
    $("#fileList").html($li);
    uploader.makeThumb(file, function (error, src) {
        if (error) {
            $img.replaceWith('<span>不能预览</span>');
            return;
        }
        $img.attr('src', src);
    }, 100, 100);
});
uploader.on('uploadProgress', function (file, percentage) {
    var $li = $('#' + file.id),
        $percent = $li.find('.progress span');
    if (!$percent.length) {
        $percent = $('<p class="progress"><span></span></p>')
                .appendTo($li)
                .find('span');
    }
    $percent.css('width', percentage * 100 + '%');
});
// 文件上传成功，给item添加成功class, 用样式标记上传成功。
uploader.on('uploadSuccess', function (file, data) {
    $('#' + file.id).addClass('upload-state-done');
    $("#deletePic").css('display', 'block');
    AttachID = 0;
    HashValue = data.Result.HashValue;
    AttachUrl = data.Result.AttachUrl;
    AttachName = data.Result.AttachName;
    AttachNewName = data.Result.AttachNewName;
    AttachType = data.Result.AttachType;
    Remark = data.Result.Remark;
    AttachBytes = data.Result.AttachBytes;
    AttachFormat = data.Result.AttachFormat;
});
// 文件上传失败，显示上传出错。
uploader.on('uploadError', function (file) {
    var $li = $('#' + file.id),
        $error = $li.find('div.error');

    // 避免重复创建
    if (!$error.length) {
        $error = $('<div class="error"></div>').appendTo($li);
    }
    $error.text('上传失败');
});
// 完成上传完了，成功或者失败，先删除进度条。
uploader.on('uploadComplete', function (file) {
    $('#' + file.id).find('.progress').remove();
});
crossDomainAjax("/api/publicity/selpositionlist?PublicityTypesID=1", SuccessCallback, ErrorCallback);
function SuccessCallback(data, index) {
    var list = eval(data.Result);
    for (var i = 0; i < list.length; i++) {
        $("#PublicityCategoryID").append("<option value='" + list[i].ID + "'>" + list[i].PublicityCategoryName + "</option>");
    }
    layer.closeAll('loading');
    layui.use(['form'], function () {
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
function ErrorCallback(data, index) {
    layer.msg("获取数据失败！");
    return false;
}
crossDomainAjax("/api/publicity/getcontentInfo?ID=" + id, GetSuccessCallback, ErrorCallback);
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
        var img =
        '<div id="' + data.Result.Attach.ID + '" class="file-item thumbnail">' +
                '<img src="' + data.Result.Attach.AttachUrl + '">' +
                '<div class="info">' + data.Result.Attach.AttachName + '</div>' +
            '</div>';
        $("#fileList").html(img);
        AttachID = data.Result.Attach.ID;
        HashValue = "0";
        AttachUrl = data.Result.Attach.AttachUrl;
        AttachName = data.Result.Attach.AttachName;
        AttachNewName = data.Result.Attach.AttachNewName;
        AttachType = 0;
        Remark = 0;
        AttachFormat = 0;
        AttachBytes = 0;
        layer.closeAll('loading');
    } else {
        layer.msg("请求数据失败");
    }
}
var angularjs = angular.module('myPosition', []);
angularjs.controller('positionController', function ($scope) {
    $scope.PublicityName = $("#PublicityName").val();
    $scope.NavUrl = $("#NavUrl").val();
    $scope.orderid = $("#orderid").val();
});
$('.valid').css('display', 'block');
$("#deletePic").click(function () {
    $("#fileList").html("");
    HashValue = "";
    $("#deletePic").css('display', 'none');
});
$("#refresh").click(function () {
    location.reload();
});
$("#submit").click(function () {
    if (HashValue == "") {
        layer.msg("请先上传图片！");
        return false;
    }
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
        PublicityTypesID: 1,
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
        AttachUrl: AttachUrl,
        Attach: {
            ID: AttachID,
            HashValue: HashValue,
            AttachUrl: AttachUrl,
            AttachName: AttachName,
            AttachNewName: AttachNewName,
            AttachType: AttachType,
            Remark: "0",
            AttachFormat: AttachFormat,
            AttachBytes: AttachBytes
        }
    };
    PostAjax('/api/publicity/editcontentInfo', postdata, SubmitSuccessCallback, PostErrorCallback);
});
function SubmitSuccessCallback(data, index) {
    if (data.Success == 1) {
        layer.msg("图片链接编辑成功！", { time: 1000 }, function () { location.href = "PictureManager.aspx?pages=" + GetQueryString("pages") + "&keyword=" + escape(GetQueryString("keyword")) + "&PublicityCateID=" + GetQueryString("PublicityCateID"); });
    } else {
        layer.msg("图片链接编辑失败！");
        $("#submit").removeClass("layui-btn layui-btn-disabled");
        $("#submit").addClass("layui-btn layui-btn");
        $("#submit").removeAttr("disabled");
        layer.closeAll('loading');
    }
}