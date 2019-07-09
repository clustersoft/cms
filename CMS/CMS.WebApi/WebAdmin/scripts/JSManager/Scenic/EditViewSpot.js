$("#refresh").click(function () {
    location.reload();
});
function back() {
    location.href = "ViewSpotManager.html?pages=" + GetQueryString("pages") + "&keyword=" + escape(GetQueryString("keyword"));
}
var CMSUserID = GetCMSData().CMSUserID;
var id = GetQueryString("ID");
var ue = UE.getEditor('introduce', {
    autoHeightEnabled: true,
    autoFloatEnabled: true,
    //自定义工具
    toolbars: [['fullscreen', 'undo', 'redo', '|',
'bold', 'italic', 'underline', 'strikethrough', 'removeformat', '|', 'forecolor', 'backcolor', 'insertorderedlist', 'insertunorderedlist', 'selectall', 'cleardoc', '|',
'rowspacingtop', 'rowspacingbottom', 'lineheight', '|',
'customstyle', 'paragraph', 'fontfamily', 'fontsize', '|',
'justifyleft', 'justifycenter', 'justifyright', 'justifyjustify']]

});
var ue2 = UE.getEditor('charge', {
    autoHeightEnabled: true,
    autoFloatEnabled: true,
    toolbars: [['fullscreen', 'undo', 'redo', '|',
'bold', 'italic', 'underline', 'strikethrough', 'removeformat', '|', 'forecolor', 'backcolor', 'insertorderedlist', 'insertunorderedlist', 'selectall', 'cleardoc', '|',
'rowspacingtop', 'rowspacingbottom', 'lineheight', '|',
'customstyle', 'paragraph', 'fontfamily', 'fontsize', '|',
'justifyleft', 'justifycenter', 'justifyright', 'justifyjustify']]

});
var ue3 = UE.getEditor('openingtime', {
    autoHeightEnabled: true,
    autoFloatEnabled: true,
    toolbars: [['fullscreen', 'undo', 'redo', '|',
'bold', 'italic', 'underline', 'strikethrough', 'removeformat', '|', 'forecolor', 'backcolor', 'insertorderedlist', 'insertunorderedlist', 'selectall', 'cleardoc', '|',
'rowspacingtop', 'rowspacingbottom', 'lineheight', '|',
'customstyle', 'paragraph', 'fontfamily', 'fontsize', '|',
'justifyleft', 'justifycenter', 'justifyright', 'justifyjustify']]

});
var AttachID, HashValue = "", AttachUrl, AttachName, AttachNewName, AttachType, Remark, AttachFormat, AttachBytes;
var uploader = WebUploader.create({
    // 选完文件后，是否自动上传。
    auto: true,
    // swf文件路径
    swf: '../scripts/webuploader/dist/Uploader.swf',
    // 文件接收服务端。
    server: apiurl + '/api/attach/upload',
    //server: 'http://10.37.10.163:116/api/attach/upload',
    // 选择文件的按钮。可选。
    // 内部根据当前运行是创建，可能是input元素，也可能是flash.
    pick: {
        id: $("#filePicker"), // id
        multiple: false  // false  单选 
    },
    // 只允许选择图片文件。
    accept: {
        title: 'Images',
        extensions: 'gif,jpg,jpeg,bmp,png',
        mimeTypes: 'image/jpg,image/jpeg,image/bmp,image/png,image/gif'
    }
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
$('.valid').css('display', 'block');
crossDomainAjax("/api/viewspot/getinfo?ID=" + id + "&ran=" + Math.random(), SuccessCallback, ErrorCallback);
function SuccessCallback(data, index) {
    $("#SpotName").val(data.Result.Name);
    $("#Jd").val(data.Result.Jd);
    $("#Wd").val(data.Result.Wd);
    $("#Radius").val(data.Result.Radius);
    ue.ready(function () { ue.setContent(data.Result.Introduce); });
    ue2.ready(function () { ue2.setContent(data.Result.Charge); });
    ue3.ready(function () { ue3.setContent(data.Result.OpeningTime); });
    $("#orderid").val(data.Result.OrderID);
    if (data.Result.Attach != null) {

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
    } else {
        $("#deletePic").css('display', 'none');
    }
}
var angularjs = angular.module('mySpot', []);
angularjs.controller('spotController', function ($scope) {
    $scope.SpotName = $("#SpotName").val();
    $scope.orderid = $("#orderid").val();
    $scope.Jd = $("#Jd").val();
    $scope.Wd = $("#Wd").val();
    $scope.Radius = $("#Radius").val();
});
$("#deletePic").click(function () {
    $("#fileList").html("");
    HashValue = "";
    AttachID = null;
    $("#deletePic").css('display', 'none');
});
$("#submit").click(function () {
    $("#submit").removeClass("layui-btn layui-btn");
    $("#submit").addClass("layui-btn layui-btn-disabled");
    $("#submit").attr("disabled", "disabled");
    var postdata = {
        ID: id,
        Name: $("#SpotName").val(),
        Jd: $("#Jd").val(),
        Wd: $("#Wd").val(),
        Radius: $("#Radius").val(),
        Introduce: ue.getContent(),
        Charge: ue2.getContent(),
        OpeningTime: ue3.getContent(),
        OrderID: $("#orderid").val(),
        EditUser: CMSUserID,
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
    PostAjax('/api/viewspot/edit', postdata, SubmitSuccessCallback, PostErrorCallback);
    });
    function SubmitSuccessCallback(data, index) {
        if (data.Success == 1) {
            layer.msg("编辑景点成功！", { time: 1000 }, function () { location.href = "ViewSpotManager.html?pages=" + GetQueryString("pages") + "&keyword=" + escape(GetQueryString("keyword")); });
            layer.closeAll('loading');
        } else {
            layer.msg("编辑景点失败！");
            $("#submit").removeClass("layui-btn layui-btn-disabled");
            $("#submit").addClass("layui-btn layui-btn");
            $("#submit").removeAttr("disabled");
            layer.closeAll('loading');
        }
    }