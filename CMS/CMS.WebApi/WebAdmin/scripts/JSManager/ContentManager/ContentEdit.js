var content = '',uploader, uploader2,ue;
        var id = GetQueryString("ID");//获取Url中的ID值
        var from = GetQueryString("from");//获取来源：编辑/审核  from=1代表审核
        var source = GetQueryString("source");//获取来源：wshlist
        var jsonData = [];
        var PictureAttach = { ID: "" }, index, isupload = true;//控制上传多个附件
        var CMSUserID = GetCMSData().CMSUserID;
        var UploadFormat = "", UploadFormatTypes = "", MaxImgKB = "", MaxAttachKB = "", FileFormat = "", FileFortmatTypes = "";

        ue = UE.getEditor('container', {
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

        function fuploader() {
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
                $percent.css('width', percentage * 100 + '%');
            });
            uploader.on('uploadSuccess', function (file, response) {
                $('#' + file.id).addClass('upload-state-done');
                PictureAttach = null;
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
                //alert(JSON.stringify(PictureAttach));
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
                uploader.upload();
            });
            //附件上传
            var $list2 = $("#fileList2");
            var $btn2 = $("#ctlBtn2");   //开始上传  
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
                    ID: 0,
                    HashValue: response.Result.HashValue,
                    AttachUrl: response.Result.AttachUrl,
                    AttachName: response.Result.AttachName,
                    AttachNewName: response.Result.AttachNewName,
                    AttachType: response.Result.AttachType,
                    AttachFormat: response.Result.AttachFormat,
                    AttachBytes: response.Result.AttachBytes
                };
                jsonData.push(fileEvent);
                //alert(JSON.stringify(jsonData))

                var $lipro = $('#' + file.id + '_pro');
                $lipro.html("上传成功");
                var $li = $('#' + file.id + '_do');
                var dohtml = "<a href='" + fileEvent.AttachUrl + "' target='_blank' class='layui-btn layui-btn-small'>下载</a>" +
                            " <input type='button' value='删除' class='layui-btn layui-btn-small layui-btn-danger' onclick='getdelete(\"" + (jsonData.length - 1) + "\")'>";
                //alert(dohtml)
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
                uploader2.upload();
            });
            $("#deletePic").click(function () {
                $("#fileList").html("");
                PictureAttach = null;
                PictureAttach = { ID: "" };
                $("#deletePic").css('display', 'none');
            });
        }

        //返回
        function goback() {
            switch (source) {
                case "ArticleWshList":
                    window.location.href = 'ArticleWshList.aspx';
                    break;
                case "HomePage":
                    window.location.href = '../HomePage.aspx';
                    break;
                default:
                    window.location.href = 'ContentListManager.aspx?backsource=1';
                    break;
            }
        }

        layui.use(['element', 'form','layer'], function () {
            var element = layui.element(),
                form = layui.form(),
                layer = layui.layer,
                $ = layui.jquery;

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
            form.render();
                        
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
                fileSizeLimit: MaxImgKB*1024,
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
                    layer.alert("所选附件总大小不可超过" + max + "M！");
                }
            });
            fuploader();
        });

        //删除所有节点
        function removeAll() {
            var obj = document.getElementById('ColumnID');
            obj.options.length = 0;
        }
        //表单保存
        $("#submit").click(function () {
            formSubmit('submit');
        });

        $("#pauseSave").click(function () {
            formSubmit('pauseSave');
        });

        $("#shtg").click(function () {
            formSubmit('shtg');
        });

        $("#takeback").click(function () {
            formSubmit('takeback');
        });

        var status = 0;
        function PerSuccessCallback(data) {
            if (data.Result.IsAdmin != 1) {
                if ((data.Result.ActionCode).indexOf('audit') > 0) {
                    status = 1;
                } else { status = 0; }
            }
            else {
                status = 1;
            }
        }
        function formSubmit(btn) {
            if ($("#submit").hasClass('clicking')) { return;  }
            $("#submit").toggleClass("clicking");
            if ($("#contentvalue").val() == "0") {
                $("#LinkPath").val('');
                content = ue.getContent();
            } else { content = '';  }
            var pubtime, gqtime;
            if ($("#pub").val() == "0") {
                if ($("#oldpub").val() == "0") {
                    pubtime = $("#oldPubTime").val();//时间不变
                } else {
                    pubtime = getNowFormatDateHM();//当前时间 
                }
            } else {
                if ($("#PubTime").val() != "") {
                    pubtime = $("#PubTime").val();
                }
                else {
                    pubtime = getNowFormatDateHM();//当前时间 
                }
            }
            if ($("#gq").val() == "0") {
                gqtime = '2099-12-31 23:59:59';//永不过期
            } else {
                if ($("#GQTime").val() != "") {
                    gqtime = $("#GQTime").val();
                }
                else {
                    gqtime = '2099-12-31 23:59:59';//永不过期
                }
            }
            switch (btn) {
                case 'pauseSave'://暂存
                    status = $("#hidstatus").val();
                    break;
                case 'shtg'://审核通过
                    status = 1;
                    break;
                case 'takeback'://撤回
                    status = 3;
                    break;
                default://提交
                    //if ($("#hidstatus").val() != "-1") {
                    //    status = $("#hidstatus").val();
                    //} else {
                        if (btn != 'pauseSave') { 
                            crossDomainAjax(geturl() + "/api/permission/navshowlist?NavCode=Article&UserID=" + CMSUserID, PerSuccessCallback, ErrorCallback);
                        }
                    //}
                    break;
            }

            var postdata = {
                ID: id,
                Title: $("#Title").val(),
                LinkType: $("#contentvalue").val(),
                LinkPath: $("#LinkPath").val(),
                CoverImage: '',//ConverImage
                Summary: $("#Summary").val(),
                ContentSource: $("#ContentSource").val(),
                IsStickTop: $("#issticktop").val(),
                PubTimeType: $("#pub").val(),
                PubTime: pubtime,
                ExpiredTime: gqtime,
                ExpiredTimeType: $("#gq").val(),
                OrderID: $("#OrderID").val(),
                Status: status,
                AgreeIPField: '',
                ModifyUser: CMSUserID,
                CategoryIDs: $("#hidColumn").val(),
                Content: content,
                ArticleAudit: $("#ArticleAudit").val(),
                PictureAttach: PictureAttach,
                UpdateArticleAttachs: jsonData
            }
            //alert(JSON.stringify(postdata));
            //数据插入文章表Articles
            var url = geturl() + "/api/article/editInfo";
            if (btn == "pauseSave") {
                url = geturl() + "/api/article/editInfopause";
            }
            //alert(url)
            PostAjaxJson(url, JSON.stringify(postdata), AddSuccessCallback, ErrorCallback);
            //$.ajax({
            //    url: url,
            //    type: "POST",
            //    data: JSON.stringify(postdata),
            //    contentType: "application/json; charset=utf-8",
            //    success: function (data) {
            //        if (data.Success == "1") {
            //            layer.msg("编辑文章成功！", { time: 1000 }, function () {
            //                goback();
            //            });
            //        } else {
            //            layer.msg("编辑文章失败！", { time: 1000 });
            //            $("#submit").removeClass("clicking");
            //        }
            //    },
            //    error: function (XMLHttpRequest, textStatus, errorThrown) {
            //        layer.msg("请求失败，消息：" + textStatus + "  " + errorThrown, { time: 1000 });
            //        $("#submit").removeClass("clicking");
            //    }
            //});
        }
        function AddSuccessCallback(data, index) {
            if (data.Success == "1") {
                layer.msg("编辑文章成功！", { time: 1000 }, function () {
                    goback();
                });
            } else {
                layer.msg("编辑文章失败！", { time: 1000 });
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

        function layeropen() {
            layer.open({ type: 2, title: '栏目选择', shade: 0.3, shadeClose: false, area: ['400px', '85%'], content: 'ColumnSelect.aspx?ID=1&&hid=' + $("#hidColumn").val() });
        }

        $.ajax({
            url: geturl() + "/api/article/getinfo",
            type: "GET",
            headers: { Authorization: GetCMSData().CMSToken },
            data: "id=" + id,
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data.Success == "1") {
                    $("#Title").val(data.Result.Title);
                    if (data.Result.IsStickTop == "1") {
                        $("input[name='issticktop']").attr("checked", "checked");
                        $("input[name='issticktop']").val(1);
                    }
                    $("#issticktop").val(data.Result.IsStickTop);
                    if (data.Result.PubTimeType == "1") {
                        $("input[name='Pub']").eq(1).attr("checked", 'checked');
                        $("#PubTime").val(data.Result.PubTime);
                        $("#PubTime").removeAttr("disabled");
                        $("#pub").val(1);
                    }
                    else {
                        $("#oldPubTime").val(data.Result.PubTime);
                        $("#oldpub").val(0);
                    }
                    if (data.Result.ExpiredTimeType == "1") {
                        $("input[name='guoqi']").eq(1).attr("checked", 'checked');
                        $("#GQTime").val(data.Result.ExpiredTime);
                        $("#GQTime").removeAttr("disabled");
                        $("#gq").val(1);
                    }
                    $("#OrderID").val(data.Result.OrderID);
                    $("#Summary").val(data.Result.Summary);
                    $("#ContentSource").val(data.Result.ContentSource);
                    $("#hidstatus").val(data.Result.Status);
                    if (data.Result.Status == -1) {//暂存
                        //$("#pauseSave").attr("style", "display:inline-block");
                        $("#shtg").attr("style", "display:none");
                        $("#takeback").attr("style", "display:none");
                        $("#divArticleAudit").attr("style", "display:none");
                    }
                    else if (data.Result.Status == 1 || from != 1) {//已审核通过，或者不是审核进入
                        if (data.Result.Status != 4 && data.Result.Status != 3) {
                            $("#submit").attr("style", "display:none");
                        }
                        $("#shtg").attr("style", "display:none");
                        $("#takeback").attr("style", "display:none");
                        $("#divArticleAudit").attr("style", "display:none");
                    }
                    else if (from == 1) {//审核进入
                        $("#submit").attr("style", "display:none");
                    }
                    //多个栏目读取ColumnID
                    $("#hidColumn").val(data.Result.CategoryIDs);
                    $("#Column").val(data.Result.CategoryNames);
                    var uevalue = data.Result.Content;
                    $("#ViewNums").html(data.Result.ViewNums);
                    $("#contentvalue").val(data.Result.LinkType);
                    if (data.Result.LinkType == "1") {
                        $("input[name='content']").eq(1).attr("checked", 'checked');
                        $("#divpath").removeAttr("style");
                        $("#diveditor").attr("style", "display:none");

                        $("#LinkPath").val(data.Result.LinkPath);
                    }
                    else {
                        ue.ready(function () { ue.setContent(uevalue); });
                    }

                    //内容详细列表
                    if (data.Result.ArticleAuditList != null && data.Result.ArticleAuditList != "") {
                        var detail = data.Result.ArticleAuditList;
                        if (detail.length > 0) {
                            $("#divArticleAuditList").attr("style", "display:block");
                            var msgdetail = "";
                            for (var i = 0; i < detail.length; i++) {
                                msgdetail += "<tr>" +
                                    "<td>" + (i + 1) + "</td>" +
                                    "<td>" + detail[i].AuditUser + "</td>" +
                                    "<td>" + detail[i].AuditStatus + "</td>" +
                                    "<td>" + detail[i].AuditTime + "</td>" +
                                    "<td>" + detail[i].AuditReason + "</td>" +
                                    "</tr>";
                            }
                            $("#nrdetail").empty().append(msgdetail);
                        }
                    }

                    if (data.Result.PictureAttach != null && data.Result.PictureAttach != "") {
                        var img =
                            '<div id="' + data.Result.PictureAttach.ID + '" class="file-item thumbnail">' +
                                '<img src="' + data.Result.PictureAttach.AttachUrl + '" width="100" height="100">' +
                            '</div>';
                        $("#fileList").html(img);

                        //展示图片及附件 
                        PictureAttach = {
                            ID: data.Result.PictureAttach.ID,
                            AttachName: data.Result.PictureAttach.AttachName,
                            AttachUrl: data.Result.PictureAttach.AttachUrl
                        };
                    }

                    if (data.Result.AttachLists != null && data.Result.AttachLists != "") {
                        //循环展示
                        var msghtml = "";
                        var files = data.Result.AttachLists;

                        if (files.length > 0) {
                            $("#tbl").attr("style", "block");

                            for (var i = 0; i < files.length; i++) {
                                var data = {
                                    ID: files[i].AttachID,
                                    AttachUrl: files[i].AttachUrl,
                                    AttachName: files[i].AttachName,
                                    AttachNewName: files[i].AttachNewName
                                }
                                jsonData.push(data);

                                msghtml += "<tr>" +
                                    "<td style='text-align:center'>" + files[i].AttachName + "</td>" +
                                    "<td style='text-align:center'>已上传</td>" +
                                    "<td style='text-align:center'><a href='" + files[i].AttachUrl + "' target='_blank' class='layui-btn layui-btn-small'>下载</a>" +
                                    //"<td><input type='button' value='下载' class='layui-btn layui-btn-small' onclick='getDown(\"" + jsonData[i].AttachUrl + "\")'>" +
                                    " <input type='button' value='删除' class='layui-btn layui-btn-small layui-btn-danger' onclick='getdelete(\"" + i + "\",\"" + files[i].AttachName + "\")'></td>" +
                                    "</tr>";
                            }
                        }
                        $("#msg").empty().append(msghtml);
                    }
                } else {
                    layer.msg("获取数据失败！", { time: 1000 });
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

        var angularjs = angular.module('myContent', []);
        angularjs.controller('contentController', function ($scope) {
            $scope.Title = $("#Title").val();
            $scope.Column = $("#Column").val();

            $scope.LMcheck = function (Column) {
                $scope.Column = $("#Column").val();
            }
        });

        function getDown(fileURL) {
            var oPop = window.open(fileURL, "", "width=1, height=1, top=5000, left=5000");
            for (; oPop.document.readyState != "complete";) {
                if (oPop.document.readyState == "complete") break;
            }
            oPop.document.execCommand("SaveAs");
            oPop.close();
        }
        function getdelete(oneFile, name) {
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
                var msghtml = "";
                for (var i = 0; i < jsonData.length; i++) {
                    msghtml += "<tr>" +
                    "<td style='text-align:center'>" + jsonData[i].AttachName + "</td>" +
                    "<td style='text-align:center'>已上传</td>"+
                    "<td style='text-align:center'><a href='" + jsonData[i].AttachUrl + "' target='_blank' class='layui-btn layui-btn-small'>下载</a>" +
                    //"<td><input type='button' value='下载' class='layui-btn layui-btn-small' onclick='getDown(\"" + jsonData[i].AttachUrl + "\")'>" +
                    " <input type='button' value='删除' class='layui-btn layui-btn-small layui-btn-danger' onclick='getdelete(\"" + i + "\")'></td>" +
                    "</tr>";
                }
                
                $("#msg").empty().append(msghtml);
                layer.closeAll('dialog');
            });
        }