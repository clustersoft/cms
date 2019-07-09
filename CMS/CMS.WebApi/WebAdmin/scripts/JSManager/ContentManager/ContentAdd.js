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

            var index, PictureAttach = '', content = '';
            var jsonData = {
                fileList: []
            };
            var CMSUserID = GetCMSData().CMSUserID;
            var uploader, uploader2;
            var UploadFormat = "", UploadFormatTypes = "", MaxImgKB = "", MaxAttachKB = "", FileFormat = "", FileFortmatTypes = "";
            var angularjs = angular.module('myContent', []);
            angularjs.controller('contentController', function ($scope) {               
                $scope.LMcheck = function (Column) {
                    $scope.Column = $("#Column").val();
                }
            });
            layui.use(['form', 'layer'], function () {
                var form = layui.form(),
                    $ = layui.jquery;
                    //layer = layui.layer;
                $("#btnChoose").click(function () {
                    layeropen();
                });
                form.on('radio(pub)', function (data) {
                    if (data.value == 1)
                    { $("#PubTime").removeAttr("disabled"); $("#pub").val(1); }
                    else
                    { $("#PubTime").attr("disabled", true); $("#pub").val(0); }
                });
                form.on('radio(gq)', function (data) {
                    if (data.value == 1)
                    { $("#GQTime").removeAttr("disabled"); $("#gq").val(1); }
                    else
                    { $("#GQTime").attr("disabled", true); $("#gq").val(0); }
                });
                form.on('radio(content)', function (data) {
                    if (data.value == 1) {
                        $("#contentvalue").val(1);
                        $("#divpath").removeAttr("style");
                        $("#diveditor").attr("style", "display:none");              
                    }
                    else {
                        $("#divpath").attr("style", "display:none");
                        $("#diveditor").attr("style", "display:block");
                        $("#contentvalue").val(0);    
                    }
                });
                form.on('checkbox(issticktop)', function (data) {
                    if (data.elem.checked) {
                        $("#issticktop").val(1);
                    }
                    else { $("#issticktop").val(0); }
                });

                //用户内容来源
                crossDomainAjax("/api/user/getInfo?ID=" + CMSUserID, UserCallback, ErrorCallback);
                function UserCallback(data, index) {
                    $("#ContentSource").val(data.Result.UserSourceFrom);
                }

               
                crossDomainAjax("/api/sysconfig/getInfo", FormatCallback, ErrorCallback);
                function FormatCallback(data, index) {
                    if (data.Success == "1") {
                        UploadFormat = data.Result.ImgFormat;
                        UploadFormatTypes = '.' + UploadFormat.replace(/,/g, ',.');
                        FileFormat = data.Result.AttachFormat;
                        FileFortmatTypes = '.' + FileFormat.replace(/,/g, ',.');
                        MaxImgKB = data.Result.MaxImgKB;
                        MaxAttachKB = data.Result.MaxAttachKB;
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
                    fileSizeLimit: MaxImgKB*1024,
                    accept: {
                        title: 'intoTypes',
                        extensions: UploadFormat,
                        mimeTypes: UploadFormatTypes
                    }
                });
                uploader.on("error", function (type) {
                    if (type == "Q_EXCEED_SIZE_LIMIT") {
                        layer.alert("所选图片大小不可超过" + MaxImgKB / 1024 + "M！");
                    }
                });
                uploader2 = WebUploader.create({
                    auto: false,
                    swf: '../scripts/webuploader/dist/Uploader.swf',
                    server: geturl() + '/api/attach/upload',
                    pick: '#filePicker2',
                    method: 'POST',
                    fileSingleSizeLimit: MaxAttachKB * 1024,
                    resize: false,
                });
                var max = (MaxAttachKB / 1024).toFixed(1);
                if (max.substr(max.length - 1, 1) == "0") {
                    max = (MaxAttachKB / 1024).toFixed(0);
                }
                uploader2.on("error", function (type) {
                    if (type == "F_EXCEED_SIZE") {
                        layer.alert("所选附件大小不可超过" + max + "M！");
                    }
                });
                fuploader();
                layer.closeAll('loading');
            });
            function layeropen() {
                layer.open({ type: 2, title: '栏目选择', shade: 0.3, shadeClose: false, area: ['400px', '600px;'], content: 'ColumnSelect.aspx?hid=' + $("#hidColumn").val() });
            }
            //表单保存
            $("#submit").click(function () {
                formSubmit('submit');
            });

            $("#pauseSave").click(function () {
                formSubmit('pauseSave');
            });
            var status = 0;
            function PerSuccessCallback(data) {
                if (data.Result.IsAdmin != 1) {
                    if ((data.Result.ActionCode).indexOf('audit') > 0) {
                        status = 1;
                    }else
                    { status = 0;}
                }
                else {
                    status = 1;
                }
            }
            function formSubmit(btn) {
                if ($("#submit").hasClass('clicking')) {
                    return;
                }
                $("#submit").toggleClass("clicking");

                if ($("#contentvalue").val() == "0") {
                    $("#LinkPath").val('');
                    content = ue.getContent();
                    if (content == "") {
                        layer.msg("输入内容不能为空！");
                        ue.focus();
                        $("#submit").removeClass("clicking");
                        return;
                    }
                } else {
                    if ($("#LinkPath").val()=="") {
                        layer.msg("直链地址不能为空！");
                        $("#LinkPath").focus();
                        $("#submit").removeClass("clicking");
                        return;
                    }
                }
                var pubtime, gqtime;
                if ($("#pub").val() == "0" || $("#PubTime").val() == "") {
                    pubtime = getNowFormatDateHM();//"2017-04-12T16:17:20.339Z";//getNowFormatDate();//当前时间
                } else {
                    pubtime = $("#PubTime").val();
                }
                if ($("#gq").val() == "0" || $("#GQTime").val() == "") {
                    gqtime = '2099-12-31 23:59:59';//永不过期
                } else {
                    gqtime = $("#GQTime").val();
                }

                if (btn == 'pauseSave') {
                    status = -1;//暂存
                }
                else if (btn == 'submit') {
                    crossDomainAjax(geturl() + "/api/permission/navshowlist?NavCode=Article&UserID=" + CMSUserID, PerSuccessCallback, ErrorCallback);                    
                }
               
                var postdata = {
                    Title: $("#Title").val(),
                    LinkType: $("#contentvalue").val(),
                    LinkPath: $("#LinkPath").val(),
                    Summary: $("#Summary").val(),
                    ContentSource: $("#ContentSource").val(),
                    IsStickTop: $("#issticktop").val(),
                    PubTimeType: $("#pub").val(),
                    PubTime: pubtime,
                    ExpiredTime: gqtime,
                    ExpiredTimeType: $("#gq").val(),
                    OrderID: $("#OrderID").val(),
                    CategoryIDs: $("#hidColumn").val(),
                    Status: status,
                    AgreeIPField: '',
                    CreateUser: CMSUserID,
                    CoverImage: '',//CoverImage
                    Content: content,
                    PictureAttach: PictureAttach,
                    ArticleAttachs: jsonData.fileList
                }
                //alert(JSON.stringify(postdata));

                PostAjaxJson(geturl() + '/api/article/addInfo', JSON.stringify(postdata), AddSuccessCallback, ErrorCallback);
                //$.ajax({
                //    url: geturl() + "/api/article/addInfo",
                //    type: "POST",
                //    data: JSON.stringify(postdata),
                //    contentType: "application/json; charset=utf-8",   //内容类型
                //    success: function (data) {
                //        if (data.Success == "1") {
                //            layer.msg("新增文章成功！", { time: 1000 }, function () { location.href = "ContentListManager.aspx?backsource=1"; });
                //        } else {
                //            layer.msg("新增文章失败！", { time: 1000 });
                //            $("#submit").removeClass("clicking");
                //        }
                //    },
                //    error: function (XMLHttpRequest, textStatus, errorThrown) {
                //        layer.msg("请求失败，消息：" + textStatus + "  " + errorThrown, { time: 1000 });
                //    }
                //});
            }
            function AddSuccessCallback(data, index) {
                if (data.Success == "1") {
                    layer.msg("新增文章成功！", { time: 1000 }, function () { location.href = "ContentListManager.aspx?backsource=1"; });
                } else {
                    layer.msg("新增文章失败！", { time: 1000 });
                    $("#submit").removeClass("clicking");
                }
                layer.closeAll('loading');
            }
            function ErrorCallback(data, index) {
                layer.closeAll('loading');
                layer.msg("获取数据失败！");
                $("#submit").removeClass("clicking");
                return false;
            }

            function fuploader() {
                /*init webuploader*/
                var $list = $("#fileList");
                var $btn = $("#ctlBtn");   //开始上传  
                var thumbnailWidth = 100;   //缩略图高度和宽度 （单位是像素），当宽高度是0~1的时候，按照百分比计算
                var thumbnailHeight = 100;

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
                    //alert(percentage)
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
                    $("#deletePic").attr('style', 'display:inline-block;');
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

                //附件上传
                var $list2 = $("#fileList2");
                var $btn2 = $("#ctlBtn2");   //开始上传  
                var thumbnailWidth = 100;   //缩略图高度和宽度 （单位是像素），当宽高度是0~1的时候，按照百分比计算
                var thumbnailHeight = 100;

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
                    var fileEvent = {
                        HashValue: response.Result.HashValue,
                        AttachUrl: response.Result.AttachUrl,
                        AttachName: response.Result.AttachName,
                        AttachNewName: response.Result.AttachNewName,
                        AttachType: response.Result.AttachType,
                        AttachFormat: response.Result.AttachFormat,
                        AttachBytes: response.Result.AttachBytes
                    };
                    jsonData.fileList.push(fileEvent);
                    var $lipro = $('#' + file.id + '_pro');
                    $lipro.html("上传成功");
                    var $li = $('#' + file.id + '_do');
                    var dohtml = "<a href='" + fileEvent.AttachUrl + "' target='_blank' class='layui-btn layui-btn-small'>下载</a>" +
                                " <input type='button' value='删除' class='layui-btn layui-btn-small layui-btn-danger' onclick='getdelete(\"" + (jsonData.fileList.length - 1) + "\")'>";

                    $li.append(dohtml);
                });
                uploader2.on('uploadError', function (file) {
                    var $lipro = $('#' + file.id + '_pro');
                    $lipro.html("上传失败");
                });
                uploader2.on('uploadComplete', function (file) {
                    $('#' + file.id).find('.progress').remove();
                });
                uploader2.on('beforeFileQueued', function (file) {
                    if (FileFormat.indexOf(file.ext) == -1) {
                        layer.msg("系统禁止了" + file.ext + "格式的文件上传！", { time: 3000 });
                        return false;
                    }
                });
                $btn2.on('click', function () {
                    console.log("上传...");
                    uploader2.upload();
                });
                $("#deletePic").click(function () {
                    $("#fileList").html("");
                    PictureAttach = null;
                    //alert(JSON.stringify(attachImage))
                    $("#deletePic").css('display', 'none');
                });
            }

            function getdelete(oneFile) {
                layer.confirm("确定要删除该附件？", { icon: 3, title: '提示' }, function (index) {
                    jsonData.fileList.splice(oneFile, 1);
                    //队列重置
                    var file = uploader2.getFiles();
                    uploader2.reset();
                    for (var i = 0; i < file.length; i++) {
                        if (i != oneFile) {
                            uploader2.addFile(file[i]);
                        }
                    }
                    var msghtml = "";
                    for (var i = 0; i < jsonData.fileList.length; i++) {
                        msghtml += "<tr>" +
                        "<td style='text-align:center'>" + jsonData.fileList[i].AttachName + "</td>" +
                        "<td style='text-align:center'>已上传</td>"+
                        "<td style='text-align:center'><a href='" + jsonData.fileList[i].AttachUrl + "' target='_blank' class='layui-btn layui-btn-small'>下载</a>" +
                        //"<td><input type='button' value='下载' class='layui-btn layui-btn-small' onclick='getDown(\"" + jsonData[i].AttachUrl + "\")'>" +
                        " <input type='button' value='删除' class='layui-btn layui-btn-small layui-btn-danger' onclick='getdelete(\"" + i + "\")' /></td>" +
                        "</tr>";
                    }
                    $("#msg").empty().append(msghtml);

                    layer.closeAll('dialog');
                });
            }