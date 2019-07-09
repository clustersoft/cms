var CMSUserID = GetCMSData().CMSUserID;
var ViewSpotID = GetQueryString("ViewSpotID");
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
var HashValue = "", AttachUrl, AttachName, AttachNewName, AttachType, Remark, AttachFormat, AttachBytes, fileMd5;
var jsonData = {
    fileList: []
};
var thumbnailWidth = 100;   //缩略图高度和宽度 （单位是像素），当宽高度是0~1的时候，按照百分比计算
var thumbnailHeight = 100;
var uploader2;
crossDomainAjax("/api/viewspot/getinfo?ID=" + ViewSpotID+ "&ran=" + Math.random(), SuccessCallback, ErrorCallback);
crossDomainAjax("/api/viewspot/typelist?ViewSpotID=" + ViewSpotID+ "&ran=" + Math.random(), TypeCallback, ErrorCallback);
function TypeCallback(data, index) {
    var list = eval(data.Result);
    var word = 0, pic = 0, speak = 0, video = 0;
    for (var i = 0; i < list.length; i++) {
        if (list[i].Type == "图片") {
            pic = 1;
        } else if (list[i].Type == "文字") {
            word = 1;
        } else if (list[i].Type == "语音") {
            speak = 1;
        } else if (list[i].Type == "视频") {
            video = 1;
        }
    }
    if (pic == 0) {
        $("#InfoType").append("<option value='2'>图片</option>");
    }
    if (speak == 0) {
        $("#InfoType").append("<option value='3'>语音</option>");
    }
    if (video == 0) {
        $("#InfoType").append("<option value='4'>视频</option>");
    }
    if (word == 0) {
        $("#InfoType").append("<option value='1'>文字</option>");
    }
    if ($("#InfoType").val() == "1") {
        $('#editor').css('display', 'block');
        $('#files').css('display', 'none');

    } else {
        $('#files').css('display', 'block');
        $('#editor').css('display', 'none');
        var UploadFormat = "", UploadFormatTypes = "";
        if ($("#InfoType").val() == 2) {
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
        } else if ($("#InfoType").val() == 3) {
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
        } else if ($("#InfoType").val() == 4) {
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
            server: '/api/attach/upload',
            pick: '#filePicker2',
            method: 'POST',
            ////是否要分片处理大文件上传。
            //chunked: true,
            //// 如果要分片，分多大一片？ 默认大小为5M.
            //chunkSize: 5*1024*1024,
            resize: false,
            accept: {
                title: 'intoTypes',
                extensions: UploadFormat,
                mimeTypes: UploadFormatTypes
            },
            duplicate: true
        });
        fuploader();
    }
}
function start() {
    uploader2.upload();
    $('#btnupload').attr("onclick", "stop()");
    $('#btnupload').removeClass("layui-btn layui-btn-primary ");
    $('#btnupload').addClass("layui-btn layui-btn-danger");
    $('#btnupload').text("取消上传");
}

function stop() {
    uploader2.stop(true);
    $('#btnupload').attr("onclick", "start()");
    $('#btnupload').removeClass("layui-btn layui-btn-danger");
    $('#btnupload').addClass("layui-btn layui-btn-primary");
    $('#btnupload').text("继续上传");
}
function SuccessCallback(data, index) {
    $("#SpotName").val(data.Result.Name);
}
function getdelete(oneFile) {
    layer.confirm("确定要删除该附件？", { icon: 3, title: '提示' }, function (index) {
        jsonData.fileList.splice(oneFile, 1);
        //重新加载
        var msghtml = "";
        for (var i = 0; i < jsonData.fileList.length; i++) {
            msghtml += "<tr>" +
            "<td>" + jsonData.fileList[i].AttachName + "</td>" +
            "<td style='text-align:center'>上传成功</td>" +
            "<td style='text-align:center'><a href='" + jsonData.fileList[i].AttachUrl + "' target='_blank' class='layui-btn layui-btn-small'>下载</a>" +
            " <input type='button' value='删除' class='layui-btn layui-btn-small layui-btn-danger' onclick='getdelete(\"" + i + "\")'></td>" +
            "</tr>";
        }
        $("#msg").empty().append(msghtml);
        layer.closeAll('dialog');
    });
}
var GUID = WebUploader.Base.guid();
layui.use(['form'], function () {
    var form = layui.form(); //只有执行了这一步，部分表单元素才会修饰成功\
    form.on('select(InfoType)', function (data) {
        if (data.value != 1) {
            $('#files').css('display', 'block');
            $('#editor').css('display', 'none');
            var UploadFormat = "", UploadFormatTypes = "";
            if (data.value == 2) {
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
            } else if (data.value == 3) {
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
            } else if (data.value == 4) {
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
                server: '/api/attach/upload',
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
            //uploader2 = WebUploader.create({
            //    auto: true,
            //    swf: '../scripts/webuploader/dist/Uploader.swf',
            //    server: '../ces.ashx',
            //    method: 'POST',
            //    pick: '#filePicker2',
            //    chunked: true,
            //    chunkSize: 5 * 1024 * 1024,
            //    threads: 1,
            //    chunkRetry: 3,
            //    // 上传本分片时预处理下一分片
            //    prepareNextFile: true,
            //    formData: { guid: GUID },
            //    accept: {
            //        title: 'intoTypes',
            //        extensions: UploadFormat,
            //        mimeTypes: UploadFormatTypes
            //    }
            //});
            fuploader();
        } else {
            $('#editor').css('display', 'block');
            $('#files').css('display', 'none');
        }
    });
});
function fuploader() {
    uploader2.on('fileQueued', function (file) {
        //fileMd5 = hex_md5(file.name + file.size);
        
        $('#fileList2').css('display', 'block');
        var msghtml = "";
        msghtml += "<tr>" +
                    "<td>" + file.name + "</td>" +
                    "<td id='" + file.id + "_pro'></td>" +
                    "<td id='" + file.id + "_do'  style='text-align:center'></td>" +
                    "</tr>";
        $("#msg").append(msghtml);
        $("#btnupload").show();
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
        $("#"+ file.id +"_per").html((percentage * 100).toFixed(0) + '%');
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
        //alert(1);


        var $lipro = $('#' + file.id + '_pro');
        $lipro.empty().append("上传成功");
        var $li = $('#' + file.id + '_do');
        var dohtml = "<a href='" + fileEvent.AttachUrl + "' target='_blank' class='layui-btn layui-btn-small'>下载</a>" +
                    " <input type='button' value='删除' class='layui-btn layui-btn-small layui-btn-danger' onclick='getdelete(\"" + (jsonData.fileList.length - 1) + "\")'>";
        $li.empty().append(dohtml);
    });
    uploader2.on('uploadError', function (file) {
        var $li = $('#' + file.id + '_pro');
        $li.empty().append("上传成功");
    });
    uploader2.on('uploadComplete', function (file) {
        $("#btnupload").hide();
    });

    //WebUploader.Uploader.register({
    //    "before-send-file": "beforeSendFile",  // 整个文件上传前
    //    'before-send': 'beforeSend'
    //}, {
    //    beforeSendFile: function (file) { 
    //        this.owner.options.formData.fileMd5 = fileMd5;
    //    },
    //    beforeSend: function (file) {
    //        var deferred = WebUploader.Deferred();
    //        var me = this, owner = this.owner;
    //        var filesize = file.end - file.start;
    //        $.ajax({
    //            url: "../ces.ashx",
    //            dataType: 'json',
    //            type: "POST",
    //            data: {
    //                chunk: file.chunk,
    //                filesize: filesize,
    //                fileMd5: fileMd5
    //            },
    //            success: function (response) {
    //                 //如果验证已经上传过
    //                 if (response.isupload) {
    //                    deferred.reject();
    //                    alert('分片已传，跳过');
    //                 } else {
    //                     alert(2);
    //                    deferred.resolve();
    //                }
    //            }
    //        });
    //        return deferred.promise();
    //    }
    //});

}
$("#submit").click(function () {
    var type = $("#InfoType").val(), content = ue.getContent();
    if (type == "1") {
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
        ViewSpotID: ViewSpotID,
        Type: type,
        Content: content,
        CreatUser: CMSUserID,
        Attachs: jsonData.fileList
    };
    PostAjax('/api/viewspot/adddetail', postdata, SubmitSuccessCallback, PostErrorCallback);
});
function SubmitSuccessCallback(data, index) {
    if (data.Success == 1) {
        layer.msg("新增景点详细信息成功！", { time: 1000 }, function () { location.href = "ViewSpotDetailManager.html?ViewSpotID=" + ViewSpotID + "&pages=" + GetQueryString("pages") + "&keyword=" + escape(GetQueryString("keyword")); });
        layer.closeAll('loading');
    } else {
        layer.msg("新增景点详细信息失败！");
        $("#submit").removeClass("layui-btn layui-btn-disabled");
        $("#submit").addClass("layui-btn layui-btn");
        $("#submit").removeAttr("disabled");
        layer.closeAll('loading');
    }
}