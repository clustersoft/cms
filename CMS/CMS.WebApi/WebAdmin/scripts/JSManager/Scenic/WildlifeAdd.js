//实例化编辑器
var ue = UE.getEditor('container', {
    autoHeightEnabled: true,
    autoFloatEnabled: true,
    //自定义工具
    toolbars: [['fullscreen', 'source', '|', 'undo', 'redo', '|',
'bold', 'italic', 'underline', 'fontborder', 'strikethrough', 'superscript', 'subscript', 'removeformat', 'formatmatch', 'autotypeset', 'blockquote', 'pasteplain', '|', 'forecolor', 'backcolor', 'insertorderedlist', 'insertunorderedlist', 'selectall', 'cleardoc', '|',
'rowspacingtop', 'rowspacingbottom', 'lineheight', '|',
'customstyle', 'paragraph', 'fontfamily', 'fontsize', '|',
'directionalityltr', 'directionalityrtl', 'indent', '|',
'justifyleft', 'justifycenter', 'justifyright', 'justifyjustify', '|', 'touppercase', 'tolowercase', '|',
'link', 'unlink', 'anchor', '|', 'imagenone', 'imageleft', 'imageright', 'imagecenter', '|',
'insertimage', 'emotion', 'attachment', 'pagebreak', 'background', '|',
'horizontal', 'date', 'time', 'spechars', 'snapscreen', 'wordimage', '|',
'inserttable', 'deletetable', 'insertparagraphbeforetable', 'insertrow', 'deleterow', 'insertcol', 'deletecol', 'mergecells', 'mergeright', 'mergedown', 'splittocells', 'splittorows', 'splittocols', 'charts', '|',
'print', 'preview', 'searchreplace', 'drafts']]
});

var CMSUserID = GetCMSData().CMSUserID;
var PictureAttach = "";
var uploader;

function backs() {
    location.href = "WildlifeManager.html?pages=" + GetQueryString("pages") + "&keyword=" + GetQueryString("keyword");
}

var angularjs = angular.module('myWildlife', []);
angularjs.controller('WildlifeController', function ($scope) {
});

layui.use(['form'], function () {
    var form = layui.form();
    form.on('select(Type)', function (data) {
        $.ajax({
            url: geturl() + "/api/wildlifeclass/selectlist",
            type: "GET",
            headers: { Authorization: GetCMSData().CMSToken },
            data: "Type=" + data.value,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //alert(1);
                var ddl = $("#CategoryID");
                var result = eval(data.Result);
                removeAll();
                ddl.append("<option value=''>请选择</option>");
                $(result).each(function (key) {
                    var opt = $("<option></option>").text(result[key].CateName).val(result[key].ID);
                    ddl.append(opt);
                });
                form.render('select'); //刷新select选择框渲染
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

    $.ajax({
        url: geturl() + "/api/wildlifeclass/selectlist",
        type: "GET",
        headers: { Authorization: GetCMSData().CMSToken },
        data: "Type=1",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var ddl = $("#CategoryID");
            var result = eval(data.Result);
            removeAll();
            ddl.append("<option value=''>请选择</option>");
            $(result).each(function (key) {
                var opt = $("<option></option>").text(result[key].CateName).val(result[key].ID);
                ddl.append(opt);
            });
            form.render('select'); //刷新select选择框渲染
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

    form.render('select');

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
        auto: true,
        swf: '../scripts/webuploader/dist/Uploader.swf',
        server: geturl() + '/api/attach/upload',
        pick: '#filePicker',
        method: 'POST',
        resize: false,
        fileSizeLimit: MaxImgKB * 1024,
        accept: {
            title: 'intoTypes',
            extensions: UploadFormat,
            mimeTypes: UploadFormatTypes
        }
    });
    uploader.on("error", function (type) {
        var max = (MaxImgKB / 1024).toFixed(1);
        if (max.substr(max.length - 1, 1) == "0") {
            max = (MaxImgKB / 1024).toFixed(0);
        }
        if (type == "Q_EXCEED_SIZE_LIMIT") {
            layer.alert("所选图片大小不可超过" + max + "M！");
        }
    });
    fuploader();

    $("#submit").click(function () {
        formSubmit();
    });
});

function removeAll() {
    var obj = document.getElementById('CategoryID');
    obj.options.length = 0;
}

function formSubmit() {
    if ($("#submit").hasClass('clicking')) {
        return;
    }
    $("#submit").toggleClass("clicking");

    var content = ue.getContent();

    var postdata = {
        Type: $("#Type").val(),
        Name: $("#Name").val(),
        WildlifemanagerID: $("#CategoryID").val(),
        Introduce: content,
        OrderID: $("#OrderID").val(),
        CreatUser: CMSUserID,
        Attach: PictureAttach
    }
    //alert(JSON.stringify(postdata))
    $.ajax({
        url: geturl() + "/api/wildlifemanager/add",
        type: "POST",
        headers: { Authorization: GetCMSData().CMSToken },
        data: JSON.stringify(postdata),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.Success == "1") {
                layer.msg("新增成功！", { time: 1000 }, function () { backs(); });
            } else {
                layer.msg("新增失败！", { time: 1000 });
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
}

function fuploader() {
    /*init webuploader*/
    var $list = $("#fileList");
    var $btn = $("#ctlBtn");   //开始上传  
    var thumbnailWidth = 100;   //缩略图高度和宽度 （单位是像素），当宽高度是0~1的时候，按照百分比计算
    var thumbnailHeight = 100;

    //var uploader = WebUploader.create({
    //    auto: true,
    //    swf: '../scripts/webuploader/dist/Uploader.swf',
    //    server: geturl() + '/api/attach/upload',
    //    pick: '#filePicker',
    //    accept: {
    //        title: 'Images',
    //        extensions: 'jpg,jpeg,png',
    //        mimeTypes: 'image/jpg,image/jpeg,image/png'
    //    },
    //    method: 'POST',
    //    resize: false
    //});
    uploader.on('fileQueued', function (file) {
        var $li = $(
                '<div id="' + file.id + '" class="file-item thumbnail">' +
                    '<img>' +
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
    uploader.on('uploadSuccess', function (file, response) {
        $('#' + file.id).addClass('upload-state-done');
        PictureAttach = {
            HashValue: response.Result.HashValue,
            AttachUrl: response.Result.AttachUrl,
            AttachName: response.Result.AttachName,
            AttachNewName: response.Result.AttachNewName,
            AttachType: response.Result.AttachType,
            AttachFormat: response.Result.AttachFormat,
            AttachBytes: response.Result.AttachBytes
        };

        $("#deletePic").attr('style', 'display:block;');
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
    $btn.on('click', function () {
        console.log("上传...");
        uploader.upload();
    });
}