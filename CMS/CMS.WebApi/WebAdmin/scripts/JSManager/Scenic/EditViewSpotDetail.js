$("#refresh").click(function () {
    location.reload();
});
var CMSUserID = GetCMSData().CMSUserID;
var ViewSpotID = GetQueryString("ViewSpotID");
var DetailID = GetQueryString("DetailID");
var angularjs = angular.module('mySpot', []);
angularjs.controller('spotController', function ($scope) {

});
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
var HashValue = "", AttachUrl, AttachName, AttachNewName, AttachType, Remark, AttachFormat, AttachBytes, Type;
var jsonData = {
    fileList: []
};
var thumbnailWidth = 100;   //缩略图高度和宽度 （单位是像素），当宽高度是0~1的时候，按照百分比计算
var thumbnailHeight = 100;
var uploader2;
function fuploader() {

    uploader2.on('fileQueued', function (file) {
        $('#fileList2').css('display', 'block');
        var msghtml = "";
        msghtml += "<tr>" +
                    "<td>" + file.name + "</td>" +
                    "<td id='" + file.id + "_pro'></td>" +
                    "<td id='" + file.id + "_do'  style='text-align:center'></td>" +
                    "</tr>";
        $("#msg").append(msghtml);
    });
    uploader2.on('uploadProgress', function (file, percentage) {
        var $li = $('#' + file.id + '_pro'),
        $percent = $li.find('.layui-progress .layui-progress-bar');
        if (!$percent.length) {
            $percent = $('<div class="layui-progress layui-progress-big" lay-showpercent="true">' +
                '<div class="layui-progress-bar layui-bg-red" style="width: 0%;color:white;" id="' + file.id + '_per"></div>' +
                '</div>' +
            '</div>').appendTo($li).find('.layui-progress-bar');
        }
        $percent.css('width', percentage * 100 + '%');
        $("#" + file.id + "_per").html((percentage * 100).toFixed(0) + '%');
    });
    uploader2.on('uploadSuccess', function (file, response) {
        var fileEvent = {
            HashValue: response.Result.HashValue,
            AttachUrl: response.Result.AttachUrl,
            AttachName: response.Result.AttachName,
            AttachNewName: response.Result.AttachNewName,
            AttachType: response.Result.AttachType,
            AttachFormat: response.Result.AttachFormat,
            AttachBytes: response.Result.AttachBytes//,
        };
        jsonData.fileList.push(fileEvent);
        var $lipro = $('#' + file.id + '_pro');
        $lipro.empty().append("上传成功");
        var $li = $('#' + file.id + '_do');
        var dohtml = "<a href='" + fileEvent.AttachUrl + "' target='_blank' class='layui-btn layui-btn-small'>下载</a>" +
                    " <input type='button' value='删除' class='layui-btn layui-btn-small layui-btn-danger' onclick='getdelete(\"" + (jsonData.fileList.length - 1) + "\")'>";
        $li.append(dohtml);
    });
    uploader2.on('uploadError', function (file) {
        var $li = $('#' + file.id),
            $error = $li.find('div.error');
        if (!$error.length) {
            $error = $('<div class="error"></div>').appendTo($li);
        }
        $error.text('上传失败');
    });
    uploader2.on('uploadComplete', function (file) {
        $('#' + file.id).find('.progress').remove();
    });
}
crossDomainAjax("/api/viewspot/getinfo?ID=" + ViewSpotID+ "&ran=" + Math.random(), SuccessCallback, ErrorCallback);
crossDomainAjax("/api/viewspot/getdetailinfo?ID=" + DetailID+ "&ran=" + Math.random(), DetailCallback, ErrorCallback);
function SuccessCallback(data, index) {
    $("#SpotName").val(data.Result.Name);
}
function DetailCallback(data, index) {
    Type = data.Result.Type;
    if (data.Result.Type == "1") {
        $("#TypeName").val("文字");
        $('#files').css('display', 'none');
        ue.ready(function () { ue.setContent(data.Result.Content); });
    } else {
        if (data.Result.Type == "2") {
            $("#TypeName").val("图片");
        } else if (data.Result.Type == "3") {
            $("#TypeName").val("语音");
        } else if (data.Result.Type == "4") {
            $("#TypeName").val("视频");
        }
        var UploadFormat = "", UploadFormatTypes = "";
        if (data.Result.Type == 2) {
            crossDomainAjax("/api/sysconfig/getInfo?ran=" + Math.random(), FormatCallback, ErrorCallback);
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
        } else if (data.Result.Type == 3) {
            crossDomainAjax("/api/sysconfig/getInfo?ran=" + Math.random(), FormatCallback, ErrorCallback);
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
        } else if (data.Result.Type == 4) {
            crossDomainAjax("/api/sysconfig/getInfo?ran=" + Math.random(), FormatCallback, ErrorCallback);
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
        $("#filePicker2").empty().append("选择文件");
        uploader2 = WebUploader.create({
            auto: true,
            swf: '../scripts/webuploader/dist/Uploader.swf',
            server: geturl() + '/api/attach/upload',
            pick: '#filePicker2',
            method: 'POST',
            resize: false,
            accept: {
                title: 'intoTypes',
                extensions: UploadFormat,
                mimeTypes: UploadFormatTypes
            },
            duplicate: true
        });
        fuploader();
        var list = eval(data.Result.Attachs);
        var attachshtml = "";
        for (var i = 0; i < list.length; i++) {
            var data = {
                ID: list[i].ID,
                AttachUrl: list[i].AttachUrl,
                AttachName: list[i].AttachName,
                AttachNewName: list[i].AttachNewName
            }
            jsonData.fileList.push(data);
            attachshtml += "<tr>" +
               "<td>" + list[i].AttachName + "</td>" +
               "<td style='text-align:center'>已上传</td>" +
               "<td style='text-align:center'><a href='" + list[i].AttachUrl + "' target='_blank' class='layui-btn layui-btn-small'>下载</a> <input type='button' value='删除' class='layui-btn layui-btn-small layui-btn-danger' onclick='getdelete(\"" + i + "\",\"" + list[i].AttachName + "\")'></td>" +
               "</tr>";
        }
        $("#msg").append(attachshtml);
        $('#editor').css('display', 'none');
    }
}
function getdelete(oneFile, name) {
    layer.confirm("确定要删除该附件？", { icon: 3, title: '提示' }, function (index) {
        jsonData.splice(oneFile, 1);
        var msghtml = "";
        for (var i = 0; i < jsonData.length; i++) {
            msghtml += "<tr>" +
            "<td>" + jsonData[i].AttachName + "</td>" +
            "<td><a href='" + jsonData[i].AttachUrl + "' target='_blank' class='layui-btn layui-btn-small'>下载</a>" +
            " <input type='button' value='删除' class='layui-btn layui-btn-small layui-btn-danger' onclick='getdelete(\"" + i + "\")'></td>" +
            "</tr>";
        }
        $("#msg").empty().append(msghtml);
        layer.closeAll('dialog');
    });
}
function getdelete(oneFile) {
    layer.confirm("确定要删除该附件？", { icon: 3, title: '提示' }, function (index) {
        jsonData.fileList.splice(oneFile, 1);
        //重新加载
        var msghtml = "";
        for (var i = 0; i < jsonData.fileList.length; i++) {
            msghtml += "<tr>" +
            "<td>" + jsonData.fileList[i].AttachName + "</td>" +
            "<td style='text-align:center'>已上传</td>" +
            "<td style='text-align:center'><a href='" + jsonData.fileList[i].AttachUrl + "' target='_blank' class='layui-btn layui-btn-small'>下载</a>" +
            " <input type='button' value='删除' class='layui-btn layui-btn-small layui-btn-danger' onclick='getdelete(\"" + i + "\")'></td>" +
            "</tr>";
        }
        $("#msg").empty().append(msghtml);
        layer.closeAll('dialog');
    });
}
layui.use(['form'], function () {
    var form = layui.form(); //只有执行了这一步，部分表单元素才会修饰成功\
    form.on('select(InfoType)', function (data) {
        if (data.value != 1) {
            $('#files').css('display', 'block');
            $('#editor').css('display', 'none');
        } else {
            $('#editor').css('display', 'block');
            $('#files').css('display', 'none');
        }
    });
});
$("#submit").click(function () {
    var Attachs;
    if (Type == "1") {
        Attachs = [];
    } else {
        Attachs = jsonData.fileList;
    }
    var content = ue.getContent();
    if (Type == "1") {
        if (content == "") {
            layer.msg("请先输入文字信息！")
            return false;
        }
    } else {
        var list = eval(jsonData.fileList);
        if (list.length == 0) {
            layer.msg("请先上传附件！")
            return false;
        }
    }
    $("#submit").removeClass("layui-btn layui-btn");
    $("#submit").addClass("layui-btn layui-btn-disabled");
    $("#submit").attr("disabled", "disabled");
    var postdata = {
        ID: DetailID,
        ViewSpotID: ViewSpotID,
        Type: Type,
        Content: content,
        EditUser: CMSUserID,
        Attachs: Attachs
    };
    PostAjax('/api/viewspot/eidtdetail', postdata, SubmitSuccessCallback, PostErrorCallback);
});
function SubmitSuccessCallback(data, index) {
    if (data.Success == 1) {
        layer.msg("编辑景点详细信息成功！", { time: 1000 }, function () { location.href = "ViewSpotDetailManager.html?ViewSpotID=" + ViewSpotID + "&pages=" + GetQueryString("pages") + "&keyword=" + escape(GetQueryString("keyword")); });
        layer.closeAll('loading');
    } else {
        layer.msg("编辑景点详细信息失败！");
        $("#submit").removeClass("layui-btn layui-btn-disabled");
        $("#submit").addClass("layui-btn layui-btn");
        $("#submit").removeAttr("disabled");
        layer.closeAll('loading');
    }
}