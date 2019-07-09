var HashValue = "", AttachUrl, AttachName, AttachNewName, AttachType, Remark, AttachFormat, AttachBytes;
var UploadFormat = "", UploadFormatTypes = "";
function back() {
    location.href = "PictureManager.aspx?pages=" + GetQueryString("pages") + "&keyword=" + escape(GetQueryString("keyword")) + "&PublicityCateID=" + GetQueryString("PublicityCateID");
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
var CMSUserID = GetCMSData().CMSUserID;
// 初始化Web Uploader
var uploader = WebUploader.create({
    // 选完文件后，是否自动上传。
    auto: true,
    // swf文件路径
    swf: '../scripts/webuploader/dist/Uploader.swf',
    // 文件接收服务端。
    server: '/api/attach/upload',
    // 选择文件的按钮。可选。
    // 内部根据当前运行是创建，可能是input元素，也可能是flash.
    pick: {
        id: $("#filePicker"), // id
        multiple: false  // false  单选 
    },
    // 只允许选择图片文件。
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
    // $list为容器jQuery实例
    $("#fileList").html($li);
    // 创建缩略图
    // 如果为非图片文件，可以不用调用此方法。
    // thumbnailWidth x thumbnailHeight 为 100 x 100
    uploader.makeThumb(file, function (error, src) {
        if (error) {
            $img.replaceWith('<span>不能预览</span>');
            return;
        }
        $img.attr('src', src);
    }, 100, 100);
});
// 文件上传过程中创建进度条实时显示。
uploader.on('uploadProgress', function (file, percentage) {
    var $li = $('#' + file.id),
        $percent = $li.find('.progress span');

    // 避免重复创建
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
    //$('#' + file.id).append("<div class=\"delete\"><span class=\"cancel\">删除</span></div>");
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
var angularjs = angular.module('myPosition', []);
angularjs.controller('positionController', function ($scope) {

});
crossDomainAjax("/api/publicity/selpositionlist?PublicityTypesID=1&ran=" + Math.random(), ListSuccessCallback, ErrorCallback);
function ListSuccessCallback(data, index) {
    var list = eval(data.Result);
    if (list.length == 0) {
        layer.msg("未获取到链接位置，请先到位置管理中添加位置！", { time: 3000 }, function () {
            location.href = "PictureManager.aspx";
        });
    } else {
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
}
$('.valid').css('display', 'block');
$("#deletePic").click(function () {
    $("#fileList").html("");
    HashValue = "";
    $("#deletePic").css('display', 'none');
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
        CreateUser: CMSUserID,
        Attach: {
            HashValue: HashValue,
            AttachUrl: AttachUrl,
            AttachName: AttachName,
            AttachNewName: AttachNewName,
            AttachType: AttachType,
            Remark: "",
            AttachFormat: AttachFormat,
            AttachBytes: AttachBytes
        }
    };
    PostAjax('/api/publicity/addcontentInfo', postdata, SubmitSuccessCallback, PostErrorCallback);
});
function SubmitSuccessCallback(data, index) {
    if (data.Success == 1) {
        layer.msg("图片链接新增成功！", { time: 1000 }, function () { location.href = "PictureManager.aspx?pages=" + GetQueryString("pages") + "&keyword=" + escape(GetQueryString("keyword")) + "&PublicityCateID=" + GetQueryString("PublicityCateID"); });
    } else {
        layer.msg("图片链接新增失败！");
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