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
            'insertimage', 'emotion', 'attachment', 'pagebreak',  'background', '|',
            'horizontal', 'date', 'time', 'spechars', 'snapscreen', 'wordimage', '|',
            'inserttable', 'deletetable', 'insertparagraphbeforetable', 'insertrow', 'deleterow', 'insertcol', 'deletecol', 'mergecells', 'mergeright', 'mergedown', 'splittocells', 'splittorows', 'splittocols', 'charts', '|',
            'print', 'preview', 'searchreplace', 'drafts']]
            });

            var CMSUserID = GetCMSData().CMSUserID;
            var PictureAttach = { ID: "" };
            var id = GetQueryString("ID");//获取Url中的ID值
            var uploader;
            //alert(GetQueryString("pages"))
            function backs() {
                //alert(GetQueryString("keyword"))
                location.href = "WildlifeManager.html?pages=" + GetQueryString("pages") + "&keyword=" + escape(GetQueryString("keyword").replace(/(^\s*)|(\s*$)/g, ""));
            }

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

                form.render();
                $("#filePicker").empty().append("选择文件");
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
                var max = (MaxImgKB / 1024).toFixed(1);
                if (max.substr(max.length - 1, 1) == "0") {
                    max = (MaxImgKB / 1024).toFixed(0);
                }
                uploader.on("error", function (type) {
                    if (type == "Q_EXCEED_SIZE_LIMIT") {
                        layer.alert("所选图片大小不可超过" + max + "M！");
                    }
                });

                fuploader();

                $("#submit").click(function () {
                    formSubmit();
                });
            });

            $.ajax({
                url: geturl() + "/api/wildlifemanager/getinfo",
                type: "GET",
                headers: { Authorization: GetCMSData().CMSToken },
                async: false,
                data: "id=" + id,
                contentType: "application/json; charset=utf-8",   //内容类型
                dataType: "json",
                success: function (data) {
                    if (data.Success == "1") {
                        $("#Name").val(data.Result.Name);
                        $("#OrderID").val(data.Result.OrderID);
                        $("#Type").val(data.Result.Type);
                        $.ajax({
                            url: geturl() + "/api/wildlifeclass/selectlist",
                            type: "GET",
                            headers: { Authorization: GetCMSData().CMSToken },
                            async: false,
                            data: "Type=" + data.Result.Type,
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
                        $("#CategoryID").val(data.Result.WildlifeCategoryID);
                        ue.ready(function () { ue.setContent(data.Result.Introduce); });

                        if (data.Result.Attach != null && data.Result.Attach != "") {
                            var img =
                                '<div id="' + data.Result.Attach.ID + '" class="file-item thumbnail">' +
                                    '<img src="' + data.Result.Attach.AttachUrl + '" width="100" height="100">' +
                                '</div>';
                            $("#fileList").html(img);

                            //展示图片及附件 
                            PictureAttach = {
                                ID: data.Result.Attach.ID,
                                AttachName: data.Result.Attach.AttachName,
                                AttachUrl: data.Result.Attach.AttachUrl
                            };
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

            var angularjs = angular.module('myWildlife', []);
            angularjs.controller('WildlifeController', function ($scope) {
                $scope.Name = $("#Name").val();
            });

            function removeAll() {
                var obj = document.getElementById('CategoryID');
                obj.options.length = 0;
            }

            function formSubmit() {
                //alert(1)
                if ($("#submit").hasClass('clicking')) {
                    return;
                }
                $("#submit").toggleClass("clicking");

                var content = ue.getContent();

                var postdata = {
                    ID:id,
                    Type: $("#Type").val(),
                    Name: $("#Name").val(),
                    WildlifemanagerID: $("#CategoryID").val(),
                    Introduce: content,
                    OrderID: $("#OrderID").val(),
                    EditUser: CMSUserID,
                    Attach: PictureAttach
                }
                //alert(JSON.stringify(postdata))

                $.ajax({
                    url: geturl() + "/api/wildlifemanager/edit",
                    type: "POST",
                    headers: { Authorization: GetCMSData().CMSToken },
                    data: JSON.stringify(postdata),
                    contentType: "application/json; charset=utf-8",   //内容类型
                    success: function (data) {
                        //alert(data.Success);
                        if (data.Success == "1") {
                            layer.msg("编辑成功！", { time: 1000 }, function () { backs(); });
                        } else {
                            layer.msg("编辑失败！", { time: 1000 });
                            $("#submit").removeClass("clicking");
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
                        ID: 0,
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