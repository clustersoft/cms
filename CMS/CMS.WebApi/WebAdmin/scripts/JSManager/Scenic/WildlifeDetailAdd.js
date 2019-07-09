var ue = UE.getEditor('container', {
    autoHeightEnabled: true,

    autoFloatEnabled: true,
    //自定义工具
    toolbars: [['fullscreen', 'undo', 'redo', '|',
'bold', 'italic', 'underline', 'strikethrough', 'removeformat', '|', 'forecolor', 'backcolor', 'insertorderedlist', 'insertunorderedlist', 'selectall', 'cleardoc', '|',
'rowspacingtop', 'rowspacingbottom', 'lineheight', '|',
'customstyle', 'paragraph', 'fontfamily', 'fontsize', '|',
'justifyleft', 'justifycenter', 'justifyright', 'justifyjustify']]
});

var jsonData = [];
var CMSUserID = GetCMSData().CMSUserID;
var WildID = GetQueryString("WildID");
var select = GetQueryString("select");
var uploader2;
layui.use(['form'], function () {
    var form = layui.form();
    form.on('select(Type)', function (data) {
        if (data.value == "1") {
            $("#divfile").attr("style", "display:none");
            $("#divcontent").attr("style", "display:block");
        } else {
            $("#divfile").attr("style", "display:block");
            $("#divcontent").attr("style", "display:none");
            var UploadFormat = "", UploadFormatTypes = "";
            if (data.value == "2") {
                crossDomainAjax("/api/sysconfig/getInfo", FormatCallback, ErrorCallback);
                function FormatCallback(data, index) {
                    if (data.Success == "1") {
                        UploadFormat = data.Result.ImgFormat;
                        UploadFormatTypes = '.' + UploadFormat.replace(/,/g, ',.');
                    } else {
                        layer.msg("数据请求失败！");
                        return false;
                    }
                    layer.closeAll('loading');
                }
            }
            else if (data.value == "3") {
                crossDomainAjax("/api/sysconfig/getInfo", FormatCallback, ErrorCallback);
                function FormatCallback(data, index) {
                    if (data.Success == "1") {
                        UploadFormat = data.Result.SpeakFormat;
                        UploadFormatTypes = '.' + UploadFormat.replace(/,/g, ',.');
                    } else {
                        layer.msg("数据请求失败！");
                        return false;
                    }
                    layer.closeAll('loading');
                }
            }
            else if (data.value == "4") {
                crossDomainAjax("/api/sysconfig/getInfo", FormatCallback, ErrorCallback);
                function FormatCallback(data, index) {
                    if (data.Success == "1") {
                        UploadFormat = data.Result.VideoFormat;
                        UploadFormatTypes = '.' + UploadFormat.replace(/,/g, ',.');
                    } else {
                        layer.msg("数据请求失败！");
                        return false;
                    }
                    layer.closeAll('loading');
                }
            }
            $("#filePicker").empty().append("选择文件");
            uploader2 = WebUploader.create({
                auto: true,
                swf: '../scripts/webuploader/dist/Uploader.swf',
                server: geturl() + '/api/attach/upload',
                pick: '#filePicker',
                method: 'POST',
                resize: false,
                accept: {
                    title: 'intoTypes',
                    extensions: UploadFormat,
                    mimeTypes: UploadFormatTypes
                }
            });
            fuploader();
        }
    });
    crossDomainAjax("/api/wildlifemanager/getinfo?ID=" + WildID, SuccessCallback, ErrorCallback);
    var ddl = $("#Type");
    if (select != "") {
        removeAll();
        $("#divfile").attr("style", "display:block");
        $("#divcontent").attr("style", "display:none");

        if (select.indexOf('1') == -1) {
            ddl.append("<option value='1'>文字</option>");
            $("#divfile").attr("style", "display:none");
            $("#divcontent").attr("style", "display:block");
        }
        if (select.indexOf('2') == -1) {
            ddl.append("<option value='2'>图片</option>");
        }
        if (select.indexOf('3') == -1) {
            ddl.append("<option value='3'>语音</option>");
        }
        if (select.indexOf('4') == -1) {
            ddl.append("<option value='4'>视频</option>");
        }
    } else {
        $("#divfile").attr("style", "display:none");
        $("#divcontent").attr("style", "display:block");
    }

    var UploadFormat = "", UploadFormatTypes = "", MaxImgKB = "", MaxAttachKB = "";
    if ($("#Type").val() == "2") {
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
    }
    else if ($("#Type").val() == "3") {
        crossDomainAjax("/api/sysconfig/getInfo", FormatCallback, ErrorCallback);
        function FormatCallback(data, index) {
            if (data.Success == "1") {
                UploadFormat = data.Result.SpeakFormat;
                UploadFormatTypes = '.' + UploadFormat.replace(/,/g, ',.');
                MaxAttachKB = data.Result.MaxAttachKB;
            } else {
                layer.msg("数据请求失败！");
                return false;
            }
            layer.closeAll('loading');
        }
    }
    else if ($("#Type").val() == "4") {
        crossDomainAjax("/api/sysconfig/getInfo", FormatCallback, ErrorCallback);
        function FormatCallback(data, index) {
            if (data.Success == "1") {
                UploadFormat = data.Result.VideoFormat;
                UploadFormatTypes = '.' + UploadFormat.replace(/,/g, ',.');
                MaxAttachKB = data.Result.MaxAttachKB;
            } else {
                layer.msg("数据请求失败！");
                return false;
            }
            layer.closeAll('loading');
        }
    }
    $("#filePicker").empty().append("选择文件");
    //alert(MaxAttachKB)
    var Max = MaxImgKB == "" ? MaxAttachKB : MaxImgKB;
    uploader2 = WebUploader.create({
        auto: false,
        swf: '../scripts/webuploader/dist/Uploader.swf',
        server: geturl() + '/api/attach/upload',
        pick: '#filePicker',
        method: 'POST',
        resize: false,
        fileSingleSizeLimit: Max * 1024,
        //duplicate :true ,
        accept: {
            title: 'intoTypes',
            extensions: UploadFormat,
            mimeTypes: UploadFormatTypes
        }
    });
    if (MaxImgKB != "") {
        var max = (MaxImgKB / 1024).toFixed(1);
        if (max.substr(max.length - 1, 1) == "0") {
            max = (MaxImgKB / 1024).toFixed(0);
        }
        uploader2.on("error", function (type) {
            if (type == "F_EXCEED_SIZE") {
                layer.alert("所选图片大小不可超过" + max + "M！");
            }
        });
    } else if (MaxAttachKB != "") {
        var max = (MaxAttachKB / 1024).toFixed(1);
        if (max.substr(max.length - 1, 1) == "0") {
            max = (MaxAttachKB / 1024).toFixed(0);
        }
        uploader2.on("error", function (type) {
            if (type == "F_EXCEED_SIZE") {
                layer.alert("所选附件大小不可超过" + max + "M！");
            }
        });
    }
    fuploader();
    form.render('select');
    $("#submit").click(function () {
        formSubmit();
    });
});

function removeAll() {
    var obj = document.getElementById('Type');
    obj.options.length = 0;
}

function formSubmit() {
    if ($("#submit").hasClass('clicking')) {
        return;
    }
    $("#submit").toggleClass("clicking");

    var content = ue.getContent();
    if ($("#Type").val() == "1") {
        if (content == "") {
            layer.msg("请输入内容！");
            $("#submit").removeClass("clicking");
            return;
        }
    } else {
        if (jsonData.length == 0) {
            layer.msg("请上传附件！");
            $("#submit").removeClass("clicking");
            return;
        }
    }
    var postdata = {
        Content: content,
        Type: $("#Type").val(),
        WildlifemanagerID: WildID,
        CreatUser: CMSUserID,
        Attachs: jsonData
    }
    //alert(JSON.stringify(postdata))
    $.ajax({
        url: geturl() + "/api/wildlifemanager/adddetail",
        type: "POST",
        headers: { Authorization: GetCMSData().CMSToken },
        data: JSON.stringify(postdata),
        contentType: "application/json; charset=utf-8",   //内容类型
        success: function (data) {
            //alert(data.Success);
            if (data.Success == "1") {
                layer.msg("新增成功！", { time: 1000 }, function () { location.href = "WildlifeDetailManager.html?ID=" + WildID; });
            } else {
                layer.msg("新增失败！", { time: 1000 });
                $("#submit").removeClass("clicking");
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            layer.msg("请求失败，消息：" + textStatus + "  " + errorThrown, { time: 1000 });
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
    //附件上传
    /*init webuploader*/
    var $list2 = $("#fileList");
    var $btn2 = $("#ctlBtn");   //开始上传  
    var thumbnailWidth = 100;   //缩略图高度和宽度 （单位是像素），当宽高度是0~1的时候，按照百分比计算
    var thumbnailHeight = 100;

    uploader2.on('fileQueued', function (file) {
        $("#tbl").attr("style", "block");
        //alert(file.name)
        var msghtml = "";
        msghtml += "<tr>" +
        "<td style='text-align:center'>" + file.name + "</td>" +
        "<td id='" + file.id + "_pro' style='text-align:center'></td>" +
        "<td id='" + file.id + "_do' style='text-align:center'></td>" +
        "</tr>";
        $("#msg").append(msghtml);
    });
    uploader2.on('uploadProgress', function (file, percentage) {
        var $li = $('#' + file.id + '_pro'),
                    $percent = $li.find('.layui-progress .layui-progress-bar');
        if (!$percent.length) {
            $percent = $('<div class="layui-progress layui-progress-big" lay-showpercent="true">' +
                '<div class="layui-progress-bar layui-bg-red" style="width: 0%;color:white;" id="per"></div>' +
                '</div>' +
            '</div>').appendTo($li).find('.layui-progress-bar');
        }
        $percent.css('width', percentage * 100 + '%');
        $("#per").html((percentage * 100).toFixed(0) + '%');
    });
    uploader2.on('uploadSuccess', function (file, response) {
        var $lipro = $('#' + file.id + '_pro');
        $lipro.html("上传成功");

        var fileEvent = {
            HashValue: response.Result.HashValue,
            AttachUrl: response.Result.AttachUrl,
            AttachName: response.Result.AttachName,
            AttachNewName: response.Result.AttachNewName,
            AttachType: response.Result.AttachType,
            AttachFormat: response.Result.AttachFormat,
            AttachBytes: response.Result.AttachBytes
        };
        jsonData.push(fileEvent);

        var $lipro = $('#' + file.id + '_pro');
        $lipro.html("上传成功");
        var $li = $('#' + file.id + '_do');
        var dohtml = "<a href='" + fileEvent.AttachUrl + "' target='_blank' class='layui-btn layui-btn-small'>下载</a>" +
                    " <input type='button' value='删除' class='layui-btn layui-btn-small layui-btn-danger' onclick='getdelete(\"" + (jsonData.length - 1) + "\")'>";

        $li.append(dohtml);
    });
    uploader2.on('uploadError', function (file) {
        var $lipro = $('#' + file.id + '_pro');
        $lipro.html("上传失败");
    });
    uploader2.on('uploadComplete', function (file) {
        $('#' + file.id).find('.progress').remove();
    });
    $btn2.on('click', function () {
        console.log("上传...");
        uploader2.upload();
    });
}
function SuccessCallback(data) {
    if (data.Success == "1") {
        $("#Name").val(data.Result.Name);
    }
}

function getdelete(oneFile) {
    layer.confirm("确定要删除该附件？", { icon: 3, title: '提示' }, function (index) {
        jsonData.splice(oneFile, 1);
        //队列重置
        var file = uploader2.getFiles();
        uploader2.reset();
        for (var i = 0; i < file.length; i++) {
            if (i != oneFile) {
                uploader2.addFile(file[i]);
            }
        }
        //重新加载
        var msghtml = "";
        for (var i = 0; i < jsonData.length; i++) {
            msghtml += "<tr>" +
            "<td style='text-align:center'>" + jsonData[i].AttachName + "</td>" +
            "<td style='text-align:center'>已上传</td>" +
            "<td style='text-align:center'><a href='" + jsonData[i].AttachUrl + "' target='_blank' class='layui-btn layui-btn-small'>下载</a>" +
            " <input type='button' value='删除' class='layui-btn layui-btn-small layui-btn-danger' onclick='getdelete(\"" + i + "\")'></td>" +
            "</tr>";
        }
        $("#msg").empty().append(msghtml);
        layer.closeAll('dialog');
    });
}