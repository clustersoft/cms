var CMSUserID = GetCMSData().CMSUserID;
var id = GetQueryString("ID");//获取Url中的ID值
var uploader;
var attachImage = {
    ID: ""
};
function backs() {
    location.href = "ModuleManager.aspx?pages=" + GetQueryString("pages") + "&keyword=" + GetQueryString("keyword");
}
layui.use(['form'], function () {
    var form = layui.form();

    form.on('radio(UseAble)', function (data) {
        if (data.value == 1)
        { $("#UseAble").val(1); }
        else
        { $("#UseAble").val(0); }
    });

    var UploadFormat = "", UploadFormatTypes = "", MaxImgKB = "";
    crossDomainAjax("/api/sysconfig/getInfo", FormatCallback, ErrorCallback);
    function FormatCallback(data, index) {
        if (data.Success == "1") {
            UploadFormat = data.Result.ImgFormat;
            UploadFormatTypes = '.' + UploadFormat.replace(/,/g, ',.');
            MaxImgKB = data.Result.MaxImgKB;
        } else {
            layer.msg("数据请求失败！");
            return false;
        }
        layer.closeAll('loading');
    }
    uploader = WebUploader.create({
        auto: false,
        swf: '../scripts/webuploader/dist/Uploader.swf',
        server: geturl() + '/api/attach/uploadWithPath',
        pick: '#filePicker',
        formData: {
            path: ""
        },
        fileSizeLimit: MaxImgKB * 1024,
        accept: {
            title: 'Images',
            extensions: UploadFormat,
            mimeTypes: UploadFormatTypes
        },
        method: 'POST',
        resize: false
    });
    uploader.on("error", function (type) {
        if (type == "Q_EXCEED_SIZE_LIMIT") {
            layer.alert("所选图片大小不可超过" + MaxImgKB / 1024 + "M！");
        }
    });
    fuploader();
});

//表单保存
$("#submit").click(function () {
    if ($("#submit").hasClass('clicking')) {
        return;
    }
    $("#submit").toggleClass("clicking");

    //图片信息保存数组Image
    var postdata = {
        ID: id,
        Name: $("#Name").val(),
        UseAble: $("#UseAble").val(),
        OrderID: $("#OrderID").val(),
        ModifyUser: CMSUserID,
        Path: $("#Path").val(),
        Attach: attachImage
    }
    //alert(JSON.stringify(postdata))
    //数据插入
    PostAjaxJson(geturl() + '/api/template/editInfo', JSON.stringify(postdata), AddSuccessCallback, ErrorCallback);
    //$.ajax({
    //    url: geturl() + "/api/template/editInfo",
    //    type: "POST",
    //    data: JSON.stringify(postdata),
    //    contentType: "application/json; charset=utf-8",
    //    success: function (data) {
    //        if (data.Success == "1") {
    //            layer.msg("编辑模版成功！", { time: 1000 }, function () { location.href = "ModuleManager.aspx"; });
    //        } else {
    //            layer.msg("编辑模版失败！", { time: 1000 });
    //        }
    //    },
    //    error: function (XMLHttpRequest, textStatus, errorThrown) {
    //        layer.alert("获取数据失败，消息：" + textStatus + "  " + errorThrown, { time: 1000 });
    //    }
    //});
});
function AddSuccessCallback(data, index) {
    if (data.Success == "1") {
        layer.msg("编辑模版成功！", { time: 1000 }, function () { backs(); });
    } else {
        layer.msg("编辑模版失败！", { time: 1000 });
        $("#submit").removeClass("clicking");
    }
    layer.closeAll('loading');
}

function ErrorCallback(data, index) {
    layer.msg("获取数据失败！");
    $("#submit").removeClass("clicking");
    layer.closeAll('loading');
    return false;
}

function fuploader() {
    /*init webuploader*/
    var $list = $("#fileList");
    var $btn = $("#ctlBtn");   //开始上传  
    var thumbnailWidth = 100;   //缩略图高度和宽度 （单位是像素），当宽高度是0~1的时候，按照百分比计算
    var thumbnailHeight = 100;

    //var uploader = WebUploader.create({
    //    // 选完文件后，是否自动上传。  
    //    auto: true,
    //    // swf文件路径  
    //    swf: '../scripts/webuploader/dist/Uploader.swf',
    //    // 文件接收服务端。
    //    //server: '../ContentManager/test.ashx',
    //    //server: 'http://10.37.10.163:116/api/attach/upload',
    //    server: geturl() + '/api/attach/upload',
    //    // 选择文件的按钮。可选.
    //    // 内部根据当前运行是创建，可能是input元素，也可能是flash.  
    //    pick: '#filePicker',
    //    // 只允许选择图片文件。  
    //    accept: {
    //        title: 'Images',
    //        extensions: 'jpg,jpeg,png',
    //        mimeTypes: 'image/jpg,image/jpeg,image/png'
    //    },
    //    method: 'POST',
    //    // 不压缩image, 默认如果是jpeg，文件上传前会压缩一把再上传！
    //    resize: false
    //});
    // 当有文件添加进来的时候  
    // webuploader事件.当选择文件后，文件被加载到文件队列中，触发该事件。等效于 uploader.onFileueued = function(file){...} ，类似js的事件定义。  
    uploader.on('fileQueued', function (file) {
        //$("#deletePic").css('display', 'none');
        $("#tbl").attr("style", "block");
        var msghtml = "";
        msghtml += "<tr>" +
        "<td style='text-align:center;' class='border'>" + file.name + "</td>" +
        "<td id='" + file.id + "_pro' style='text-align:center'></td>" +
        "<td id='" + file.id + "_do' style='text-align:center'></td>" +
        "</tr>";
        $("#msg").html(msghtml);
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
    uploader.on('uploadSuccess', function (file, response) {
        $('#' + file.id).addClass('upload-state-done');

        attachImage = null;

        attachImage = {
            ID: 0,
            HashValue: response.Result.HashValue,
            AttachUrl: response.Result.AttachUrl,
            AttachName: response.Result.AttachName,
            AttachNewName: response.Result.AttachNewName,
            AttachType: response.Result.AttachType,
            AttachFormat: response.Result.AttachFormat,
            AttachBytes: response.Result.AttachBytes
        };
        var $lipro = $('#' + file.id + '_pro');
        $lipro.html("上传成功");
        var $li = $('#' + file.id + '_do');
        var dohtml = "<a href='" + attachImage.AttachUrl + "' target='_blank' class='layui-btn layui-btn-small'>下载</a>" +
                    " <input type='button' value='删除' class='layui-btn layui-btn-small layui-btn-danger' onclick='getdelete()'>";
        //alert(dohtml)
        $li.append(dohtml);

        //$("#deletePic").attr('style', 'display:block;');
    });
    uploader.on('uploadError', function (file) {
        var $li = $('#' + file.id),
            $error = $li.find('div.error');
        if (!$error.length) {
            $error = $('<div class="error"></div>').appendTo($li);
        }
        $error.text('上传失败');
    });
    uploader.on('uploadComplete', function (file) {
        $('#' + file.id).find('.progress').remove();
    });
    uploader.on('uploadBeforeSend', function (obj, data, headers) {
        data.formData = { "path": $("#Path").val() };
    });
    $btn.on('click', function () {
        if ($("#Path").val() == "") {
            layer.msg("请填写要上传的路径");
            $("#Path").focus();
            return;
        }

        uploader.options.formData = { path: $("#Path").val() };
        uploader.upload();
    });
    //$("#deletePic").click(function () {
    //    $("#fileList").html("");
    //    attachImage = null;
    //    attachImage = {
    //        ID: ""
    //    };
    //    $("#deletePic").css('display', 'none');
    //});
}

var angularjs = angular.module('myModule', []);
angularjs.controller('moduleController', function ($scope) {
    $scope.modName = $("#Name").val();
    $scope.Path = $("#Path").val();
});
//获取数据
$.ajax({
    url: geturl() + "/api/template/getinfo",
    type: "GET",
    headers: { Authorization: GetCMSData().CMSToken },
    data: "ID=" + id,
    async: false,
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (data) {        
        if (data.Success == "1") {
            $("#Name").val(data.Result.Name);
            $("#OrderID").val(data.Result.OrderID);
            $("#Useable").val(data.Result.Useable);
            $("#Path").val(data.Result.Path);
            if (data.Result.Useable == "1") {
                $("input[name='UseAble']").eq(1).attr("checked", 'checked');
                $("#UseAble").val(data.Result.Useable);

                //alert(data.Result.Useable)
            }
            if (data.Result.Attach != null && data.Result.Attach != "") {
                //展示图片
                //var img =
                //    '<div id="' + data.Result.Attach.ID + '" class="file-item thumbnail">' +
                //        '<img src="' + data.Result.Attach.AttachUrl + '">' +
                //    '</div>';
                //$("#fileList").html(img);
                
                $("#tbl").attr("style", "block");
                var msghtml = "";
                msghtml += "<tr>" +
                        "<td style='text-align:center'>" + data.Result.Attach.AttachName + "</td>" +
                        "<td style='text-align:center'>已上传</td>" +
                        "<td style='text-align:center'><a href='" + data.Result.Attach.AttachUrl + "' target='_blank' class='layui-btn layui-btn-small'>下载</a>" +
                        "<input type='button' value='删除' class='layui-btn layui-btn-small layui-btn-danger' onclick='getdelete()'></td>" +
                        "</tr>";
                //alert(msghtml)
                $("#msg").empty().append(msghtml);

                attachImage = {
                    ID: data.Result.Attach.ID,
                    AttachName: data.Result.Attach.AttachName,
                    AttachUrl: data.Result.Attach.AttachUrl
                }
            }
            layer.closeAll('loading');
        } else {
            layer.msg("获取数据失败！", { time: 1000 });
            $("#submit").removeClass("clicking");
        }
    },
    error: function (XMLHttpRequest, textStatus, errorThrown) {
        layer.msg("获取数据失败，消息：" + textStatus + "  " + errorThrown, { time: 1000 });
        $("#submit").removeClass("clicking");
    }
}).fail(function (xhr) {
    if (xhr.status == 401) {
        ReToken();
    } else {
        ErrorCallback(data, index);
    }
});

function getdelete() {
    uploader.reset();
    $("#msg").html("");
    attachImage = {
        ID: ""
    };
}