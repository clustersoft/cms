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
        var PTime = {
            elem: '#starttime1',
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
            elem: '#endtime1',
            format: 'YYYY-MM-DD',
            min: '1900-01-01 00:00:00',
            max: '9999-12-31 23:59:59',
            istime: true,
            choose: function (datas) {
                PTime.max = datas; //结束日选好后，重置开始日的最大日期
            }
        }; laydate(ETime);
        var PTime = {
            elem: '#starttime2',
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
            elem: '#endtime2',
            format: 'YYYY-MM-DD',
            min: '1900-01-01 00:00:00',
            max: '9999-12-31 23:59:59',
            istime: true,
            choose: function (datas) {
                PTime.max = datas; //结束日选好后，重置开始日的最大日期
            }
        }; laydate(ETime);

        var ur = geturl();

        layui.use(['element', 'form'], function () {
            var $ = layui.jquery
            , element = layui.element()
            , form = layui.form();

            GetStartDate();
            //alert($("#startDate").html())
            var datenow = getNowFormatDate().substring(0, 10);
            var day = DateDiff($("#startDate").html(), datenow);
            $("#AnalyDay").html(day);
            
            zhbg();
            zhlist();

            NRCount();
            NRIP();
            NRlist();

            LMCount();
            LMIP();
            LMlist();
        });

        //计算天数差的函数，通用  
        function DateDiff(sDate1, sDate2) { 
            var aDate, oDate1, oDate2, iDays
            aDate = sDate1.split("-")
            oDate1 = new Date(aDate[1] + '-' + aDate[2] + '-' + aDate[0])    //转换为12-18-2006格式  
            aDate = sDate2.split("-")
            oDate2 = new Date(aDate[1] + '-' + aDate[2] + '-' + aDate[0])
            iDays = parseInt(Math.abs(oDate1 - oDate2) / 1000 / 60 / 60 / 24)    //把相差的毫秒数转换为天数  
            return iDays
        }

        function GetStartDate() {
            $.ajax({
                url: ur + "/api/analy/AnalyStartDate",
                async: false,
                type: "GET",
                headers: { Authorization: GetCMSData().CMSToken },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.Success == "1") {
                        //alert(data.Result)
                        $("#startDate").html(data.Result.substring(0, 10));
                    }
                    else {
                        $("#startDate").html(getNowFormatDate().substring(0, 10));
                    }
                }
            }).fail(function (xhr) {
    if (xhr.status == 401) {
        ReToken();
    } else {
        ErrorCallback(data, index);
    }
});
        }
        $("#cx").click(function () {
            zhbg();
        });
        $("#cx1").click(function () {
            NRCount();
            NRIP();
            NRlist();
        });
        $("#cx2").click(function () {
            LMCount();
            LMIP();
            LMlist();
        });
        //初始化echarts实例        
        var myChart = echarts.init(document.getElementById('divECharts'));
        //综合报表 --折线图
        function zhbg() {
            // 异步加载数据：综合报告访问量，IP
            var index = layer.load(2);
            $.ajax({
                url: ur + "/api/Analy/list",
                type: "GET",
                async: true,
                headers: { Authorization: GetCMSData().CMSToken },
                data: "StartDate=" + $("#starttime").val() + "&EndDate=" + $("#endtime").val(),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    layer.close(index);
                    if (data.Success == "1") {
                        var day = [];
                        var VisitCount = [];
                        var VisitIP = [];
                        for (var i = 0; i < data.Result.length; i++) {
                            day.push(data.Result[i].VisitDay);
                            VisitCount.push(data.Result[i].VisitCount);
                            VisitIP.push(data.Result[i].VisitIP);
                        }
                        // 填入数据
                        myChart.setOption({
                            title: {
                                text: '综合报告' + getNowFormatDateCN() + '，展示最近一个月'
                            },
                            tooltip: {},
                            legend: {
                                data: ['访问量', 'IP']
                            },
                            xAxis: {
                                data: day
                            },
                            yAxis: {},
                            series: [{
                                name: '访问量',
                                type: 'line',
                                data: VisitCount
                            }, {
                                name: 'IP',
                                type: 'line',
                                data: VisitIP
                            }
                            ]
                        });
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
        function zhlist() {
            var index = layer.load(2);
            $.ajax({
                url: ur + "/api/analy/getInfo",
                type: "GET",
                async: true,
                headers: { Authorization: GetCMSData().CMSToken },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    layer.close(index);
                    if (data.Success == "1") {
                        $("#TodayCount").html(data.Result.TodayCount);
                        $("#TodayIP").html(data.Result.TodayIP);
                        $("#yestodayCount").html(data.Result.yestodayCount);
                        $("#yestodayIP").html(data.Result.yestodayIP);
                        $("#averageCount").html(data.Result.averageCount);
                        $("#averageIP").html(data.Result.averageIP);
                        $("#TotalCount").html(data.Result.TotalCount);
                        $("#TotalIP").html(data.Result.TotalIP);
                        $("#HighestCount").html(data.Result.HighestCount);
                        $("#HighestIP").html(data.Result.HighestIP);
                        $("#hapendayCount").html(data.Result.hapendayCount);
                        $("#HapendayIP").html(data.Result.HapendayIP);
                        $("#monthCount").html(data.Result.MonthCount);
                        $("#monthIP").html(data.Result.MonthIP);
                    }
                }
            }).fail(function (xhr) {
                if (xhr.status == 401) {
                    ReToken();
                } else {
                    ErrorCallback(data, index);
                }
            });
        }

        //内容饼图
        var NRChartCount = echarts.init(document.getElementById('divEChartsNRCount'));
        var NRChartIP = echarts.init(document.getElementById('divEChartsNRIP'));
        function NRCount() {
            var index = layer.load(2);
            $.ajax({
                url: ur + "/api/Analy/NRCountlist",
                type: "GET",
                async: true,
                headers: { Authorization: GetCMSData().CMSToken },
                data: "StartDate=" + $("#starttime1").val() + "&EndDate=" + $("#endtime1").val(),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    layer.close(index)
                    if (data.Success == "1") {
                        var VisitData = [];
                        var NameData = [];
                        for (var i = 0; i < data.Result.length; i++) {
                            NameData.push(data.Result[i].Name);
                            var dat = {
                                value: data.Result[i].Count,
                                name: data.Result[i].Name
                            }
                            VisitData.push(dat);
                        }
                        if (VisitData != null && VisitData!="") {
                            // 填入数据
                            NRChartCount.setOption({
                                title: {//标题
                                    text: '访问量',
                                    subtext: '访问量',
                                    x: 'center'
                                },
                                tooltip: {//tips显示
                                    trigger: 'item',
                                    formatter: "{a} <br/>{b} : {c} ({d}%)"
                                },
                                legend: {//左侧显示
                                    orient: 'vertical',
                                    center: 'center',
                                    left: 'right',
                                    data: NameData
                                },
                                //color:['red','green' ],//颜色自定义
                                series: [//数据
                                    {
                                        name: '访问来源',
                                        type: 'pie',
                                        radius: '55%',
                                        center: ['41%', '60%'],
                                        data://数据填充
                                            VisitData,
                                        itemStyle: {//样式
                                            emphasis: {
                                                shadowBlur: 10,
                                                shadowOffsetX: 0,
                                                shadowColor: 'rgba(0, 0, 0, 0.5)'
                                            }
                                        }
                                    }
                                ]
                            });
                        }
                        else {
                            NRChartCount.setOption({
                                title: {//标题
                                    text: '访问量',
                                    subtext: '暂无数据',
                                    x: 'center'
                                },
                                tooltip: {//tips显示
                                    trigger: 'item',
                                    formatter: "{a} <br/>{b} : {c} ({d}%)"
                                },
                                legend: {//左侧显示
                                    orient: 'vertical',
                                    center: 'center',
                                    left: 'right',
                                    data: NameData
                                },
                                //color:['red','green' ],//颜色自定义
                                series: [//数据
                                    {
                                        name: '访问来源',
                                        type: 'pie',
                                        radius: '55%',
                                        center: ['41%', '60%'],
                                        data://数据填充
                                            VisitData,
                                        itemStyle: {//样式
                                            emphasis: {
                                                shadowBlur: 10,
                                                shadowOffsetX: 0,
                                                shadowColor: 'rgba(0, 0, 0, 0.5)'
                                            }
                                        }
                                    }
                                ]
                            });
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
        }
        function NRIP() {
            var index = layer.load(2);
            $.ajax({
                url: ur + "/api/Analy/NRIPlist",
                type: "GET",
                async: true,
                headers: { Authorization: GetCMSData().CMSToken },
                data: "StartDate=" + $("#starttime1").val() + "&EndDate=" + $("#endtime1").val(),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    layer.close(index);
                    if (data.Success == "1") {
                        var VisitData = [];
                        var NameData = [];
                        for (var i = 0; i < data.Result.length; i++) {
                            NameData.push(data.Result[i].Name);
                            var dat = {
                                value: data.Result[i].IP,
                                name: data.Result[i].Name
                            }
                            //console.log(JSON.stringify(dat));
                            VisitData.push(dat);
                        }
                        if (VisitData != null && VisitData != "") {
                            // 填入数据
                            NRChartIP.setOption({
                                title: {//标题
                                    text: 'IP数',
                                    subtext: 'IP数',
                                    x: 'center'
                                },
                                tooltip: {//tips显示
                                    trigger: 'item',
                                    formatter: "{a} <br/>{b} : {c} ({d}%)"
                                },
                                legend: {//左侧显示
                                    orient: 'vertical',
                                    center: 'center',
                                    left: 'right',
                                    data: NameData
                                },
                                //color:['red','green' ],//颜色自定义
                                series: [//数据
                                    {
                                        name: '访问来源',
                                        type: 'pie',
                                        radius: '55%',
                                        center: ['41%', '60%'],
                                        data://数据填充
                                            VisitData,
                                        itemStyle: {//样式
                                            emphasis: {
                                                shadowBlur: 10,
                                                shadowOffsetX: 0,
                                                shadowColor: 'rgba(0, 0, 0, 0.5)'
                                            }
                                        }
                                    }
                                ]
                            });
                        }
                        else {
                            NRChartIP.setOption({
                                title: {//标题
                                    text: 'IP数',
                                    subtext: '暂无数据',
                                    x: 'center'
                                },
                                tooltip: {//tips显示
                                    trigger: 'item',
                                    formatter: "{a} <br/>{b} : {c} ({d}%)"
                                },
                                legend: {//左侧显示
                                    orient: 'vertical',
                                    center: 'center',
                                    left: 'right',
                                    data: NameData
                                },
                                //color:['red','green' ],//颜色自定义
                                series: [//数据
                                    {
                                        name: '访问来源',
                                        type: 'pie',
                                        radius: '55%',
                                        center: ['50%', '60%'],
                                        data://数据填充
                                            VisitData,
                                        itemStyle: {//样式
                                            emphasis: {
                                                shadowBlur: 10,
                                                shadowOffsetX: 0,
                                                shadowColor: 'rgba(0, 0, 0, 0.5)'
                                            }
                                        }
                                    }
                                ]
                            });
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
        }
        function NRlist() {
            var index = layer.load(2);
            $.ajax({
                url: ur + "/api/analy/NRlist",
                type: "GET",
                async: true,
                headers: { Authorization: GetCMSData().CMSToken },
                data: "StartDate=" + $("#starttime1").val() + "&EndDate=" + $("#endtime1").val(),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    layer.close(index);
                    if (data.Success == "1") {                        
                        var listdata = eval(data.Result);
                        //alert(JSON.stringify(data.Result[0]))
                        var msghtml = "";
                        for (var i = 0; i < listdata.length; i++) {
                            msghtml +=
                            "<tr>" +
                                "<td style='text-align:center;'>" + listdata[i].内容名称 + "</td>" +
                                "<td style='text-align:center;'>" + listdata[i].访问量 + "</td>" +
                                 "<td style='text-align:center;'>" + listdata[i].访问量百分比 + "</td>" +
                                "<td style='text-align:center;'>" + listdata[i].IP数 + "</td>" +
                                 "<td style='text-align:center;'>" + listdata[i].IP数百分比 + "</td>" +
                            "</tr>";
                        }
                        if (msghtml == "") {
                            msghtml = "<tr>" +
                                    "<td colspan='5' style='text-align: center;'>暂无数据</td>" +
                                "</tr>";
                        }
                        $("#msgNR").empty().append(msghtml);
                    }
                }
            }).fail(function (xhr) {
                if (xhr.status == 401) {
                    ReToken();
                } else {
                    ErrorCallback(data, index);
                }
            });
        }

        //栏目饼图
        var LMChartCount = echarts.init(document.getElementById('divEChartsLMCount'));
        var LMChartIP = echarts.init(document.getElementById('divEChartsLMIP'));
        function LMCount() {
            var index = layer.load(2);
            $.ajax({
                url: ur + "/api/Analy/LMtbCountlist",
                type: "GET",
                async: true,
                headers: { Authorization: GetCMSData().CMSToken },
                data: "StartDate=" + $("#starttime2").val() + "&EndDate=" + $("#endtime2").val(),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    layer.close(index);
                    //alert(data.Success)
                    if (data.Success == "1") {
                        var VisitData = [];
                        var NameData = [];
                        for (var i = 0; i < data.Result.length; i++) {
                            NameData.push(data.Result[i].Name);
                            var dat = {
                                value: data.Result[i].Count,
                                name: data.Result[i].Name
                            }
                            //console.log(JSON.stringify(dat));
                            VisitData.push(dat);
                        }
                        if (VisitData != null && VisitData != "") {
                            // 填入数据
                            LMChartCount.setOption({
                                title: {//标题
                                    text: '访问量',
                                    subtext: '访问量',
                                    x: 'center'
                                },
                                tooltip: {//tips显示
                                    trigger: 'item',
                                    formatter: "{a} <br/>{b} : {c} ({d}%)"
                                },
                                legend: {//左侧显示
                                    orient: 'vertical',
                                    center: 'center',
                                    left: 'right',
                                    data: NameData
                                },
                                //color:['red','green' ],//颜色自定义
                                series: [//数据
                                    {
                                        name: '访问来源',
                                        type: 'pie',
                                        radius: '55%',
                                        center: ['50%', '60%'],
                                        data://数据填充
                                            VisitData,
                                        itemStyle: {//样式
                                            emphasis: {
                                                shadowBlur: 10,
                                                shadowOffsetX: 0,
                                                shadowColor: 'rgba(0, 0, 0, 0.5)'
                                            }
                                        }
                                    }
                                ]
                            });
                        }
                        else {
                            LMChartCount.setOption({
                                title: {//标题
                                    text: '访问量',
                                    subtext: '暂无数据',
                                    x: 'center'
                                },
                                tooltip: {//tips显示
                                    trigger: 'item',
                                    formatter: "{a} <br/>{b} : {c} ({d}%)"
                                },
                                legend: {//左侧显示
                                    orient: 'vertical',
                                    center: 'center',
                                    left: 'right',
                                    data: NameData
                                },
                                //color:['red','green' ],//颜色自定义
                                series: [//数据
                                    {
                                        name: '访问来源',
                                        type: 'pie',
                                        radius: '55%',
                                        center: ['50%', '60%'],
                                        data://数据填充
                                            VisitData,
                                        itemStyle: {//样式
                                            emphasis: {
                                                shadowBlur: 10,
                                                shadowOffsetX: 0,
                                                shadowColor: 'rgba(0, 0, 0, 0.5)'
                                            }
                                        }
                                    }
                                ]
                            });
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
        }
        function LMIP() {
            var index = layer.load(2);
            $.ajax({
                url: ur + "/api/Analy/LMtbIPlist",
                type: "GET",
                async: true,
                headers: { Authorization: GetCMSData().CMSToken },
                data: "StartDate=" + $("#starttime2").val() + "&EndDate=" + $("#endtime2").val(),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    layer.close(index);
                    if (data.Success == "1") {
                        var VisitData = [];
                        var NameData = [];
                        for (var i = 0; i < data.Result.length; i++) {
                            NameData.push(data.Result[i].Name);
                            var dat = {
                                value: data.Result[i].IP,
                                name: data.Result[i].Name
                            }
                            //console.log(JSON.stringify(dat));
                            VisitData.push(dat);
                        }
                        if (VisitData != null && VisitData != "") {
                            // 填入数据
                            LMChartIP.setOption({
                                title: {//标题
                                    text: 'IP数',
                                    subtext: 'IP数',
                                    x: 'center'
                                },
                                tooltip: {//tips显示
                                    trigger: 'item',
                                    formatter: "{a} <br/>{b} : {c} ({d}%)"
                                },
                                legend: {//左侧显示
                                    orient: 'vertical',
                                    center: 'center',
                                    left: 'right',
                                    data: NameData
                                },
                                //color:['red','green' ],//颜色自定义
                                series: [//数据
                                    {
                                        name: '访问来源',
                                        type: 'pie',
                                        radius: '55%',
                                        center: ['50%', '60%'],
                                        data://数据填充
                                            VisitData,
                                        itemStyle: {//样式
                                            emphasis: {
                                                shadowBlur: 10,
                                                shadowOffsetX: 0,
                                                shadowColor: 'rgba(0, 0, 0, 0.5)'
                                            }
                                        }
                                    }
                                ]
                            });
                        }
                        else {
                            LMChartIP.setOption({
                                title: {//标题
                                    text: 'IP数',
                                    subtext: '暂无数据',
                                    x: 'center'
                                },
                                tooltip: {//tips显示
                                    trigger: 'item',
                                    formatter: "{a} <br/>{b} : {c} ({d}%)"
                                },
                                legend: {//左侧显示
                                    orient: 'vertical',
                                    center: 'center',
                                    left: 'right',
                                    data: NameData
                                },
                                //color:['red','green' ],//颜色自定义
                                series: [//数据
                                    {
                                        name: '访问来源',
                                        type: 'pie',
                                        radius: '55%',
                                        center: ['50%', '60%'],
                                        data://数据填充
                                            VisitData,
                                        itemStyle: {//样式
                                            emphasis: {
                                                shadowBlur: 10,
                                                shadowOffsetX: 0,
                                                shadowColor: 'rgba(0, 0, 0, 0.5)'
                                            }
                                        }
                                    }
                                ]
                            });
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
        }
        function LMlist() {
            var index = layer.load(2);
            $.ajax({
                url: ur + "/api/Analy/LMlist",
                type: "GET",
                async: true,
                headers: { Authorization: GetCMSData().CMSToken },
                data: "StartDate=" + $("#starttime2").val() + "&EndDate=" + $("#endtime2").val(),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    layer.close(index);
                    if (data.Success == "1") {
                        var listdata = eval(data.Result);
                        //alert(listdata.length)
                        var msghtml = "";
                        for (var i = 0; i < listdata.length; i++) {
                            msghtml +=
                            "<tr>" +
                                "<td style='text-align:center;'>" + listdata[i].栏目名称 + "</td>" +
                                "<td style='text-align:center;'>" + listdata[i].访问量 + "</td>" +
                                 "<td style='text-align:center;'>" + listdata[i].访问量百分比 + "</td>" +
                                "<td style='text-align:center;'>" + listdata[i].IP数 + "</td>" +
                                 "<td style='text-align:center;'>" + listdata[i].IP数百分比 + "</td>" +
                            "</tr>";
                        }
                        if (msghtml == "") {
                            msghtml = "<tr>" +
                                    "<td colspan='5' style='text-align: center;'>暂无数据</td>" +
                                "</tr>";
                        }
                        $("#msgLM").empty().append(msghtml);
                    }
                }
            }).fail(function (xhr) {
                if (xhr.status == 401) {
                    ReToken();
                } else {
                    ErrorCallback(data, index);
                }
            });
        }