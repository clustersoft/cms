var CMSUserID = GetCMSData().CMSUserID;
var uploader;
var attachImage;
var UploadFormat = "", UploadFormatTypes = "", MaxImgKB = "";
var fs;

function backs() {
    location.href = "ModuleManager.aspx?pages=" + GetQueryString("pages") + "&keyword=" + GetQueryString("keyword");
}
function fuploader() {
    var $list = $("#fileList");
    var $btn = $("#ctlBtn");   //开始上传  
    var thumbnailWidth = 100;   //缩略图高度和宽度 （单位是像素），当宽高度是0~1的时候，按照百分比计算
    var thumbnailHeight = 100;

    uploader.on('fileQueued', function (file) {
        //alert(fs.length)
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
    uploader.on('uploadSuccess', function (file, response) {
        attachImage = {
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
        var $lipro = $('#' + file.id + '_pro');
        $lipro.html("上传失败");
        console.log("失败");
    });
    uploader.on('uploadComplete', function (file) {
        $('#' + file.id).find('.progress').remove();
    });
    //uploader.on('beforeFileQueued', function (file) {
    //    alert($("#Path").val())
    //    formData = { "path": $("#Path").val() };
    //});
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

        //uploader.option('formData', {
        //    path: $('#Path').val(),
        //})
        uploader.upload();
    });
    //$("#deletePic").click(function () {
    //    $("#msg").html("");
    //    attachImage = null;
    //    $("#deletePic").css('display', 'none');
    //});
}
        var angularjs = angular.module('myModule', []);
        angularjs.controller('moduleController', function ($scope) {
            //$scope.keyup = function (path) {
            //    if (path != "" && path!=null) {
            //        $("#divmodule").css("display", "block");

            //    } else {
            //        $("#divmodule").css("display", "none");
            //    }
            //}
        });
        layui.use(['form', 'layer'], function () {
            var form = layui.form(),
                layer = layui.layer;
            form.on('radio(UseAble)', function (data) {
                if (data.value == 1)
                { $("#UseAble").val(1); }
                else
                { $("#UseAble").val(0); }
            });
            
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
                server: geturl() + '/api/attach/uploadWithPath',//geturl() + '/api/attach/uploadWithPath',
                pick: '#filePicker',
                method: 'POST',
                formData: {  
                    path: ""
                },
                fileSizeLimit: MaxImgKB * 1024,
                resize: false,
                accept: {
                    title: 'intoTypes',
                    extensions: UploadFormat + ",html",
                    mimeTypes: UploadFormatTypes + ",.html"
                }
            });
            uploader.on("error", function (type) {
                if (type == "Q_EXCEED_SIZE_LIMIT") {
                    layer.alert("所选附件大小不可超过" + MaxImgKB / 1024 + "M！");
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
            //图片信息保存数组Attach
            var postdata = {
                Name: $("#Name").val(),
                UseAble: $("#UseAble").val(),
                OrderID: $("#OrderID").val(),
                CreateUser: CMSUserID,
                Path: $("#Path").val(),
                Image: '',//$("#Image").val(),
                Attach: attachImage
            }
            //console.log(JSON.stringify(postdata));
            //return;

            //数据插入
            PostAjaxJson(geturl() + '/api/template/addInfo', JSON.stringify(postdata), AddSuccessCallback, ErrorCallback);
            //$.ajax({
            //    url: geturl() + "/api/template/addInfo",
            //    type: "POST",
            //    data: JSON.stringify(postdata),
            //    contentType: "application/json; charset=utf-8",
            //    success: function (data) {
            //        if (data.Success == "1") {
            //            layer.msg("新增模版成功！", { time: 1000 }, function () { location.href = "ModuleManager.aspx"; });
            //        } else {
            //            layer.msg("新增模版失败！", { time: 1000 });
            //        }
            //    },
            //    error: function (XMLHttpRequest, textStatus, errorThrown) {
            //        layer.alert("获取数据失败，消息：" + textStatus + "  " + errorThrown, { time: 1000 });
            //    }
            //});
        });

        function AddSuccessCallback(data, index) {
            if (data.Success == "1") {
                layer.msg("新增模版成功！", { time: 1000 }, function () { backs(); });
            } else {
                layer.msg("新增模版失败！", { time: 1000 });
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
        function getdelete() {
            uploader.reset();
            $("#msg").html("");
            attachImage = {
                ID: ""
            };
        }
