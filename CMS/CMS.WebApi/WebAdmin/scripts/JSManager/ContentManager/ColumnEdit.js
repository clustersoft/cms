
function backs() {
    parent.location.href = "ColumnManager.aspx?pages=" + GetQueryString("pages") + "&parent=" + GetQueryString("parent") + "&keyword=" + escape(GetQueryString("keyword").replace(/(^\s*)|(\s*$)/g, ""));
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
    // 文件上传过程中创建进度条实时显示。
    uploader.on('uploadProgress', function (file, percentage) {
        var $li = $('#' + file.id),
                $percent = $li.find('.progress span');

        // 避免重复创建
        if (!$percent.length) {
            $percent = $('<p class="progress"><span></span></p>')
                    .appendTo($li)
                    .find('span');
        }

        $percent.css('width', percentage * 100 + '%');
    });
    // 文件上传成功，给item添加成功class, 用样式标记上传成功
    uploader.on('uploadSuccess', function (file, response) {
        $('#' + file.id).addClass('upload-state-done');

        attachImage = null;

        attachImage = {
            ID: 0,
            AttachName: response.Result.AttachName,
            AttachUrl: response.Result.AttachUrl,
            AttachNewName: response.Result.AttachNewName,
            AttachFormat: response.Result.AttachFormat,
            AttachBytes: response.Result.AttachBytes,
            AttachType: response.Result.AttachType,
            HashValue: response.Result.HashValue
        };
        $("#deletePic").attr('style', 'display:block;');
    });
    // 文件上传失败，显示上传出错。
    uploader.on('uploadError', function (file) {
        var $li = $('#' + file.id),
            $error = $li.find('div.error');

        // 避免重复创建
        if (!$error.length) {
            $error = $('<div class="error"></div>').appendTo($li);
        }

        $error.text('上传失败');
    });
    // 完成上传完了，成功或者失败，先删除进度条。
    uploader.on('uploadComplete', function (file) {
        $('#' + file.id).find('.progress').remove();
    });
    $btn.on('click', function () {
        console.log("上传...");
        uploader.upload();
    });
    $("#deletePic").click(function () {
        $("#fileList").html("");
        attachImage = null;
        //alert(JSON.stringify(attachImage))
        $("#deletePic").css('display', 'none');
    });
}
var CMSUserID = GetCMSData().CMSUserID;
var id = GetQueryString("ID");//获取Url中的ID值
var tempid = 0;
var uploader;
var attachImage = {
    ID: ""
};

layui.use(['form', 'layer'], function () {
    var form = layui.form(), //只有执行了这一步，部分表单元素才会修饰成功
        layer = layui.layer;

    form.on('switch(isLink)', function (data) {
        if (data.elem.checked == true) {
            $("#linkclose").attr("style", "display:none");
            $("#linkopen").attr("style", "display:block");
            $("input[name='linktype']").val(1);
        }
        else {
            $("#linkclose").attr("style", "display:block");
            $("#linkopen").attr("style", "display:none");
            $("input[name='linktype']").val(0);
        }
    });
    form.on('switch(AddArticle)', function (data) {
        if (data.elem.checked == true) {
            $("input[name='AddArticlePermissions']").val(1);
        }
        else {
            $("input[name='AddArticlePermissions']").val(0);
        }
    });
    form.on('switch(state)', function (data) {
        if (data.elem.checked == true) {
            $("input[name='state']").val(1);
        }
        else {
            $("input[name='state']").val(0);
        }
    });

    //加载页面模版
    crossDomainAjax(geturl() + "/api/template/selectlist", ModuleSuccessCallback, ErrorCallback);
    function ModuleSuccessCallback(data, index) {
        layer.closeAll('loading');
        var ddl = $("#TemplateID");
        //转成Json对象
        var result = eval(data.Result);
        //删除节点
        removeAlltemp();
        ddl.append("<option value=''>请选择</option>");
        //循环遍历 下拉框绑定
        $(result).each(function (key) {
            var opt = $("<option></option>").text(result[key].Name).val(result[key].ID);
            ddl.append(opt);
        });
        form.render('select'); //刷新select选择框渲染
    }
    //$.ajax({
    //    url: geturl() + "/api/template/selectlist",
    //    type: "GET",
    //    data: "",
    //    async: false,
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: function (data) {
    //        //alert(1);
    //        var ddl = $("#TemplateID");
    //        //转成Json对象
    //        var result = eval(data.Result);
    //        //删除节点
    //        removeAlltemp();
    //        ddl.append("<option value=''>请选择</option>");
    //        //循环遍历 下拉框绑定
    //        $(result).each(function (key) {
    //            var opt = $("<option></option>").text(result[key].Name).val(result[key].ID);
    //            ddl.append(opt);
    //        });
    //    },
    //    error: function (XMLHttpRequest, textStatus, errorThrown) {
    //        layer.msg("获取数据失败，消息：" + textStatus + "  " + errorThrown, { time: 1000 });
    //    }
    //});

    $("#TemplateID").val(tempid);
    form.render();

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
        fileSizeLimit: MaxImgKB * 1024,
        accept: {
            title: 'Images',
            extensions: UploadFormat,
            mimeTypes: UploadFormatTypes
        },
        method: 'POST',
        resize: false
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

    
});

//获取数据
$.ajax({
    url: geturl() + "/api/category/getInfo",
    type: "GET",
    headers: { Authorization: GetCMSData().CMSToken },
    data: "ID=" + id,
    async: false,
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (data) {
        if (data.Success == "1") {
            $("#Name").val(data.Result.Name);
            $("#RefNo").val(data.Result.RefNo);
            $("#ParentID").val(data.Result.ParentName);
            $("#pid").val(data.Result.ParentID);
            $("#linktype").val(data.Result.LinkType);
            if (data.Result.LinkType == 1) {
                $("input[name='linktype']").attr("checked", 'true');
                $("#linkclose").attr("style", "display:none");
                $("#linkopen").attr("style", "display:block");
                $("#LinkPath").val(data.Result.LinkPath);
                $("input[name='linktype']").val(1);
            }
            else {
                $("#linkclose").attr("style", "display:block");
                $("#linkopen").attr("style", "display:none");
            }
            $("#TemplateID").val(data.Result.TemplateId);
            tempid = data.Result.TemplateId;
            if (data.Result.AddArticlePermissions == 1) {
                $("input[name='AddArticlePermissions']").attr("checked", 'true');
                $("input[name='AddArticlePermissions']").val(1);
            }
            if (data.Result.State == 1) {
                $("input[name='state']").attr("checked", 'true');
                $("input[name='state']").val(1);
            }
            $("#remark").val(data.Result.Remark);
            if (data.Result.Attach != null && data.Result.Attach != "") {
                var img =
                    '<div id="' + data.Result.Attach.ID + '" class="file-item thumbnail">' +
                        '<img src="' + data.Result.Attach.AttachUrl + '">' +
                    '</div>';
                $("#fileList").html(img);
                attachImage = {
                    ID: data.Result.Attach.ID,
                    AttachName: data.Result.Attach.AttachName,
                    AttachUrl: data.Result.Attach.AttachUrl
                }
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

function removeAlltemp() {
    var obj = document.getElementById('TemplateID');
    obj.options.length = 0;
}

//验证
var angularjs = angular.module('columnyz', []);
angularjs.controller('columnbodyyz', function ($scope) {
    $scope.Namelm = $("#Name").val();
    $scope.RefNo = $("#RefNo").val();

    $scope.isDup = function (name) {
        //验证栏目名称是否重复
        $.ajax({
            url: geturl() + "/api/category/valid",
            type: "GET",
            headers: { Authorization: GetCMSData().CMSToken },
            async: false,
            data: "ID=" + id + "&Name=" + name,
            success: function (data) {
                //alert(data.Result)
                if (data.Success == "0") {
                    $scope.myForm.Namelm.$setValidity("Namelm", false);
                } else {
                    $scope.myForm.Namelm.$setValidity("Namelm", true);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layer.msg("获取数据失败，消息：" + textStatus + "  " + errorThrown);
            }
        }).fail(function (xhr) {
            if (xhr.status == 401) {
                ReToken();
            } else {
                ErrorCallback(data, index);
            }
        });
    }
    $scope.isDupNo = function (no) {
        //验证栏目标识码是否重复
        $.ajax({
            url: geturl() + "/api/category/novalid",
            headers: { Authorization: GetCMSData().CMSToken },
            type: "GET",
            async: false,
            data: "ID=" + id + "&RefNo=" + no,
            success: function (data) {
                //alert(data.Success)
                if (data.Success == "0" && no != "") {
                    $scope.myForm.RefNo.$setValidity("RefNo", false)
                } else {
                    $scope.myForm.RefNo.$setValidity("RefNo", true);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layer.msg("获取数据失败，消息：" + textStatus + "  " + errorThrown);
            }
        }).fail(function (xhr) {
            if (xhr.status == 401) {
                ReToken();
            } else {
                ErrorCallback(data, index);
            }
        });
    }
});

$("#submit").click(function () {
    if ($("#submit").hasClass('clicking')) {
        return;
    }
    $("#submit").toggleClass("clicking");

    if ($("input[name='linktype']").val() == "1")//是直连
    {
        $("#TemplateID").val("");
    }
    else {
        $("#LinkPath").val("");
    }

    var postdata = {
        ID: id,
        ParentID: $("#pid").val(),
        Name: $("#Name").val(),
        RefNo: $("#RefNo").val(),
        Remark: $("#remark").val(),
        LinkType: $("input[name='linktype']").val(),
        LinkPath: $("#LinkPath").val(),
        AddArticlePermissions: $("input[name='AddArticlePermissions']").val(),
        BeCategory: 0,
        State: $("input[name='state']").val(),
        TemplateId: $("#TemplateID").val(),
        TemplatePreview: '',
        ModifyUser: CMSUserID,
        Attach: attachImage
    }
    //alert(JSON.stringify(postdata));
    PostAjaxJson(geturl() + '/api/category/editInfo', JSON.stringify(postdata), AddSuccessCallback, ErrorCallback);
    //$.ajax({
    //    url: geturl() + "/api/category/editInfo",
    //    type: "POST",
    //    data: JSON.stringify(postdata),
    //    contentType: "application/json; charset=utf-8",
    //    success: function (data) {
    //        //alert(data.Success)
    //        if (data.Success == "1") {
    //            layer.msg("编辑栏目成功！", { time: 1000 }, function () { parent.location.href = "ColumnManager.aspx"; });
    //        } else {
    //            layer.msg("编辑栏目失败！", { time: 1000 });
    //            $("#submit").removeClass("clicking");
    //        }
    //    },
    //    error: function (XMLHttpRequest, textStatus, errorThrown) {
    //        layer.msg("获取数据失败，消息：" + textStatus + "  " + errorThrown, { time: 1000 });
    //        $("#submit").removeClass("clicking");
    //    }
    //});
});

function AddSuccessCallback(data, index) {
    if (data.Success == "1") {
        layer.msg("编辑栏目成功！", { time: 1000 }, function () { backs(); });
    } else {
        layer.msg("编辑栏目失败！", { time: 1000 });
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