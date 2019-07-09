var PTime = {
            elem: '#starttime',
            format: 'YYYY-MM-DD',
            min: '1900-01-01 00:00:00',
            max: '9999-12-31 23:59:59',
            istime: true,
            choose: function (datas) {
                ETime.min = datas; //开始日选好后，重置结束日的最小日期
                ETime.start = datas //将结束日的初始值设定为开始日
            }
        }; laydate(PTime);
        var ETime = {
            elem: '#endtime',
            format: 'YYYY-MM-DD',
            min: '1900-01-01 00:00:00',
            max: '9999-12-31 23:59:59',
            istime: true,
            choose: function (datas) {
                PTime.max = datas; //结束日选好后，重置开始日的最大日期
            }
        }; laydate(ETime);

        layui.use(['form'], function () {
            var form = layui.form;

            list(1);
        });

        $("#cx1").click(function () {
            list(1);
        });
        $("#cx2").click(function () {
            list(2);
        });

        function list(type) {
            var index = layer.load(2);
            var url;
            if (type == "1") {
                url = geturl() + "/api/Search/fblist";
            } else {
                url = geturl() + "/api/Search/sourelist";
            }
            $.ajax({
                url: url,
                async: false,
                type: "GET",
                headers: { Authorization: GetCMSData().CMSToken },
                data: "StartDate=" + $("#starttime").val() + "&EndDate=" + $("#endtime").val(),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var listdata = eval(data.Result.list);
                    var msghtml = "";
                    var headhtml = "";
                    if (type == "1") {
                        headhtml += "<tr>" +
                                "<th style='text-align: center;'>序号</th>" +
                                "<th style='text-align: center;'>发布者</th>" +
                                "<th style='text-align: center;'>发布数量</th>" +
                                "<th style='text-align: center;'>审核通过</th>" +
                                "<th style='text-align: center;'>未审核</th>" +
                            "</tr>";
                    } else {
                        headhtml += "<tr>" +
                                "<th style='text-align: center;'>序号</th>" +
                                "<th style='text-align: center;'>消息来源</th>" +
                                "<th style='text-align: center;'>发布数量</th>" +
                                "<th style='text-align: center;'>审核通过</th>" +
                                "<th style='text-align: center;'>未审核</th>" +
                            "</tr>";
                    }
                    for (var i = 0; i < listdata.length; i++) {
                        if (type == "1") {
                            msghtml +=                                
                            "<tr>" +
                                "<td style='text-align:center;'>" + (i + 1) + "</td>" +
                                "<td style='text-align:center;'>" + listdata[i].发布者 + "</td>" +
                                "<td style='text-align:center;'>" + listdata[i].发布数量 + "</td>" +
                                "<td style='text-align:center;'>" + listdata[i].审核 + "</td>" +
                                "<td style='text-align:center;'>" + listdata[i].未审核 + "</td>" +
                            "</tr>";
                        }
                        else {
                            msghtml +=                                
                            "<tr>" +
                                "<td style='text-align:center;'>" + (i + 1) + "</td>" +
                                "<td style='text-align:center;'>" + listdata[i].消息来源 + "</td>" +
                                "<td style='text-align:center;'>" + listdata[i].发布数量 + "</td>" +
                                "<td style='text-align:center;'>" + listdata[i].审核 + "</td>" +
                                "<td style='text-align:center;'>" + listdata[i].未审核 + "</td>" +
                            "</tr>";
                        }
                    }
                    if (msghtml == "") {
                        msghtml = "<tr>" +
                                "<td colspan='5' style='text-align: center;'>暂无数据</td>" +
                            "</tr>";
                    } else {
                        msghtml += "<tr>" +
                            "<td colspan='2' style='text-align: center;'>合计</td>" +
                             "<td style='text-align:center;'>" + data.Result.fbSum + "</td>" +
                            "<td style='text-align:center;'>" + data.Result.shSum + "</td>" +
                             "<td style='text-align:center;'>" + data.Result.wshSum + "</td>" +
                        "</tr>";
                    }
                    $(".head").empty().append(headhtml);
                    $(".msg").empty().append(msghtml);
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
            layer.close(index);
        }