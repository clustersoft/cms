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
            var id = GetQueryString("ID");//获取Url中的ID值
            var WildID = GetQueryString("WildID");
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
                    }
                });
                crossDomainAjax("/api/wildlifemanager/getinfo?ID=" + WildID, SuccessCallback, ErrorCallback);
                $.ajax({
                    url: geturl() + "/api/wildlifemanager/getdetailinfo",
                    type: "GET",
                    headers: { Authorization: GetCMSData().CMSToken },
                    data: "id=" + id,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        if (data.Success == "1") {
                            $("#Type").val(data.Result.TypeName);                            
                            $("#hidType").val(data.Result.Type);
                            if (data.Result.Type == "1") {
                                $("#divfile").attr("style", "display:none");
                                $("#divcontent").attr("style", "display:block");
                                ue.ready(function () { ue.setContent(data.Result.Content); });
                            } else {
                                $("#divfile").attr("style", "display:block");
                                $("#divcontent").attr("style", "display:none");

                                var UploadFormat = "", UploadFormatTypes = "", MaxImgKB = "", MaxAttachKB = "";
                                if (data.Result.Type == "2") {
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
                                else if (data.Result.Type == "3") {
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
                                else if (data.Result.Type == "4") {
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
                                var Max = MaxImgKB == "" ? MaxAttachKB : MaxImgKB;
                                uploader2 = WebUploader.create({
                                    auto: false,
                                    swf: '../scripts/webuploader/dist/Uploader.swf',
                                    server: geturl() + '/api/attach/upload',
                                    pick: '#filePicker',
                                    method: 'POST',
                                    fileSingleSizeLimit: Max * 1024,
                                    resize: false,
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

                                if (data.Result.Attachs != null && data.Result.Attachs != "") {
                                    var msghtml = "";
                                    var files = data.Result.Attachs;
                                    if (files.length > 0) {
                                        $("#tbl").attr("style", "block");

                                        for (var i = 0; i < files.length; i++) {
                                            var data = {
                                                ID: files[i].ID,
                                                AttachUrl: files[i].AttachUrl,
                                                AttachName: files[i].AttachName,
                                                AttachNewName: files[i].AttachNewName
                                            }
                                            jsonData.push(data);
                                            msghtml += "<tr>" +
                                                "<td style='text-align:center;'>" + files[i].AttachName + "</td>" +
                                                "<td style='text-align:center;'>已上传</td>" +
                                                "<td style='text-align:center;'><a href='" + files[i].AttachUrl + "' target='_blank' class='layui-btn layui-btn-small'>下载</a>" +
                                                " <input type='button' value='删除' class='layui-btn layui-btn-small layui-btn-danger' onclick='getdelete(\"" + i + "\",\"" + files[i].AttachName + "\")'></td>" +
                                                "</tr>";
                                        }
                                    }
                                    $("#msg").empty().append(msghtml);
                                }
                            }
                        }
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

                $("#submit").click(function () {
                    formSubmit();
                });
            });

            function SuccessCallback(data) {
                if (data.Success == "1") {
                    $("#Name").val(data.Result.Name);
                }
            }

            function formSubmit() {
                if ($("#submit").hasClass('clicking')) {
                    return;
                }
                $("#submit").toggleClass("clicking");

                var content = ue.getContent();

                if ($("#Type").val() == "文字") {
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
                    ID:id,
                    Content: content,
                    Type: $("#hidType").val(),
                    WildlifemanagerID: WildID,
                    EditUser: CMSUserID,
                    Attachs: jsonData
                }
                //alert(JSON.stringify(postdata))
                $.ajax({
                    url: geturl() + "/api/wildlifemanager/editdetail",
                    type: "POST",
                    headers: { Authorization: GetCMSData().CMSToken },
                    data: JSON.stringify(postdata),
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        if (data.Success == "1") {
                            layer.msg("编辑成功！", { time: 1000 }, function () { location.href = "WildlifeDetailManager.html?ID=" + WildID; });
                        } else {
                            layer.msg("编辑失败！", { time: 1000 });
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

            //$(function () {
            //    //附件上传
            //    /*init webuploader*/
            //    var $list2 = $("#fileList");
            //    var $btn2 = $("#ctlBtn");   //开始上传
            //    var thumbnailWidth = 100;   //缩略图高度和宽度 （单位是像素），当宽高度是0~1的时候，按照百分比计算
            //    var thumbnailHeight = 100;
            //    var uploader2 = WebUploader.create({
            //        auto: false,
            //        swf: '../scripts/webuploader/dist/Uploader.swf',
            //        server: geturl() + '/api/attach/upload',
            //        pick: '#filePicker',
            //        method: 'POST',
            //        resize: false
            //    });
            //    uploader2.on('fileQueued', function (file) {
            //        $("#tbl").attr("style", "block");
            //        //alert(file.name)
            //        var msghtml = "";
            //        msghtml += "<tr>" +
            //        "<td style='text-align:center'>" + file.name + "</td>" +
            //        "<td id='" + file.id + "_pro' style='text-align:center'></td>" +
            //        "<td id='" + file.id + "_do' style='text-align:center'></td>" +
            //        "</tr>";
            //        $("#msg").append(msghtml);
            //    });
            //    // 文件上传过程中创建进度条实时显示。
            //    uploader2.on('uploadProgress', function (file, percentage) {
            //        var $li = $('#' + file.id + '_pro'),
            //            $percent = $li.find('.layui-progress .layui-progress-bar');
            //        if (!$percent.length) {
            //            $percent = $('<div class="layui-progress layui-progress-big" lay-showpercent="true">' +
            //                '<div class="layui-progress-bar layui-bg-red" style="width: 0%;color:white;" id="per"></div>' +
            //                '</div>' +
            //            '</div>').appendTo($li).find('.layui-progress-bar');
            //        }
            //        $percent.css('width', percentage * 100 + '%');
            //        $("#per").html((percentage * 100).toFixed(0) + '%');
            //    });
            //    // 文件上传成功，给item添加成功class, 用样式标记上传成功。
            //    uploader2.on('uploadSuccess', function (file, response) {
            //        var $lipro = $('#' + file.id + '_pro');
            //        $lipro.html("上传成功");
            //        var fileEvent = {
            //            HashValue: response.Result.HashValue,
            //            AttachUrl: response.Result.AttachUrl,
            //            AttachName: response.Result.AttachName,
            //            AttachNewName: response.Result.AttachNewName,
            //            AttachType: response.Result.AttachType,
            //            AttachFormat: response.Result.AttachFormat,
            //            AttachBytes: response.Result.AttachBytes
            //        };
            //        //alert(jsonData.fileList.length)
            //        jsonData.push(fileEvent);
            //        var $lipro = $('#' + file.id + '_pro');
            //        $lipro.html("上传成功");
            //        var $li = $('#' + file.id + '_do');
            //        var dohtml = "<a href='" + fileEvent.AttachUrl + "' target='_blank' class='layui-btn layui-btn-small'>下载</a>" +
            //                    " <input type='button' value='删除' class='layui-btn layui-btn-small layui-btn-danger' onclick='getdelete(\"" + (jsonData.length - 1) + "\")'>";
            //        //alert(dohtml)
            //        $li.append(dohtml);
            //    });
                        //    // 文件上传失败，显示上传出错。
            //    uploader2.on('uploadError', function (file) {
            //        var $li = $('#' + file.id),
            //            $error = $li.find('div.error');
            //        // 避免重复创建
            //        if (!$error.length) {
            //            $error = $('<div class="error"></div>').appendTo($li);
            //        }
            //        $error.text('上传失败');
            //    });
            //    // 完成上传完了，成功或者失败，先删除进度条。
            //    uploader2.on('uploadComplete', function (file) {
            //        $('#' + file.id).find('.progress').remove();
            //    });
            //    $btn2.on('click', function () {
            //        console.log("上传...");
            //        uploader2.upload();
            //    });
            //});

            function fuploader() {
                var $list2 = $("#fileList");
                var $btn2 = $("#ctlBtn");   //开始上传
                uploader2.on('fileQueued', function (file) {
                    $("#tbl").attr("style", "block");
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
                    uploader2.upload();
                });
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