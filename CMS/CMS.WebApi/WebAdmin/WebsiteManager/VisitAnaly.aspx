<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VisitAnaly.aspx.cs" Inherits="CMSSystem.WebsiteManager.VisitAnaly" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>网站发布统计</title>
    <link href="../scripts/layui/css/layui.css" rel="stylesheet" />
    <link href="../css/cmsui.css" rel="stylesheet" />
    <link href="../css/font-awesome.min.css" rel="stylesheet" />
    <link href="../scripts/layui/css/modules/layer/default/layer.css" rel="stylesheet" />
    <link href="../scripts/layui/css/modules/laydate/laydate.css" rel="stylesheet" />
    <script src="../scripts/laydate/laydate.js"></script>
</head>
<body>
    <form id="form1" class="layui-form layui-form-pane">
        <blockquote class="layui-elem-quote">网站访问统计</blockquote>
        <div class="layui-tab  layui-tab-brief">
            <ul class="layui-tab-title">
                <li class="layui-this">综合分析</li>
                <li>内容分析</li>
                <li>栏目分析</li>
            </ul>
            <div class="layui-tab-content">
                <div class="layui-tab-item layui-show">
                    <div>
                        <fieldset class="layui-elem-field layui-field-title" style="margin-top: 20px;">
                            <legend>综合报告</legend>
                        </fieldset>
                        <div class="layui-input-inline">
                            <label class="layui-form-label">日期范围</label>
                            <div class="layui-input-inline dateWidth">
                                <input class="layui-input" placeholder="开始日" id="starttime" lay-filter="date">
                            </div>
                            <div class="layui-input-inline dateWidth">
                                <input class="layui-input" placeholder="截止日" id="endtime" lay-filter="date">
                            </div>
                        </div>
                        <a class="layui-btn layui-btn-normal" id="cx"><i class="fa fa-search"></i>&nbsp;查询</a>
                    </div>
                    <div class="layui-input-block">
                        <div id="divECharts" style="width: 100%; height: 400px; margin-top: 10px;"></div>
                    </div>
                    <div>
                        <fieldset class="layui-elem-field layui-field-title" style="margin-top: 20px;">
                            <legend>网站信息</legend>
                        </fieldset>
                        开始分析时间：<span id="startDate"></span>，已分析<span id="AnalyDay">1</span>天
                    </div>
                    <div>
                        <fieldset class="layui-elem-field layui-field-title" style="margin-top: 20px;">
                            <legend>网站流量</legend>
                        </fieldset>
                        <table class="layui-table" lay-skin="line">
                            <colgroup>
                                <col width="150">
                                <col width="250">
                                <col>
                            </colgroup>
                            <thead>
                                <tr>
                                    <td></td>
                                    <td>访问量</td>
                                    <td>IP数</td>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>今日</td>
                                    <td>
                                        <label id="TodayCount"></label>
                                    </td>
                                    <td>
                                        <label id="TodayIP"></label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>昨日</td>
                                    <td>
                                        <label id="yestodayCount"></label>
                                    </td>
                                    <td>
                                        <label id="yestodayIP"></label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>平均每日</td>
                                    <td>
                                        <label id="averageCount"></label>
                                    </td>
                                    <td>
                                        <label id="averageIP"></label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>本月合计</td>
                                    <td>
                                        <label id="monthCount"></label>
                                    </td>
                                    <td>
                                        <label id="monthIP"></label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>总量</td>
                                    <td>
                                        <label id="TotalCount"></label>
                                    </td>
                                    <td>
                                        <label id="TotalIP"></label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>最高访问</td>
                                    <td>
                                        <label id="HighestCount"></label>
                                    </td>
                                    <td>
                                        <label id="HighestIP"></label>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>发生在<label id="hapendayCount"></label></td>
                                    <td>发生在<label id="HapendayIP"></label></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <!--综合报告End-->

                <!--内容分析-->
                <div class="layui-tab-item">
                    <div>
                        <fieldset class="layui-elem-field layui-field-title" style="margin-top: 20px;">
                            <legend>图表</legend>
                        </fieldset>
                        <div class="layui-input-inline">
                            <label class="layui-form-label">日期范围</label>
                            <div class="layui-input-inline dateWidth">
                                <input class="layui-input" placeholder="开始日" id="starttime1" lay-filter="date" />
                            </div>
                            <div class="layui-input-inline dateWidth">
                                <input class="layui-input" placeholder="截止日" id="endtime1" lay-filter="date" />
                            </div>
                        </div>
                        <a class="layui-btn layui-btn-normal" id="cx1"><i class="fa fa-search"></i>&nbsp;查询</a>
                    </div>
                    <div id="divEChartsNRCount" style="width: 1250px; height: 400px; margin-top: 10px;"><%--margin-left:-15%;--%>
                    </div>
                    <div id="divEChartsNRIP" style="width: 1250px; height: 400px; margin-top: 10px;"></div>
                    <div style="clear: left;">
                        <fieldset class="layui-elem-field layui-field-title" style="margin-top: 20px;">
                            <legend>报表解读</legend>
                        </fieldset>
                    </div>
                    <table class="layui-table">
                        <colgroup>
                            <col >
                            <col width="15%">
                            <col width="15%">
                            <col width="15%">
                            <col width="15%" />
                        </colgroup>
                        <thead>
                            <tr>
                                <th style="text-align: center;">内容名称</th>
                                <th style="text-align: center;">访问量</th>
                                <th style="text-align: center;">访问量百分比</th>
                                <th style="text-align: center;">IP数</th>
                                <th style="text-align: center;">IP数百分比</th>
                            </tr>
                        </thead>
                        <tbody id="msgNR">
                            <tr>
                                <td colspan="5" style="text-align: center;">暂无数据</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <!--内容分析End-->
                <!--栏目分析-->
                <div class="layui-tab-item">
                    <div>
                        <fieldset class="layui-elem-field layui-field-title" style="margin-top: 20px;">
                            <legend>图表</legend>
                        </fieldset>
                        <div class="layui-input-inline">
                            <label class="layui-form-label">日期范围</label>
                            <div class="layui-input-inline dateWidth">
                                <input class="layui-input" placeholder="开始日" id="starttime2" lay-filter="date" />
                            </div>
                            <div class="layui-input-inline dateWidth">
                                <input class="layui-input" placeholder="截止日" id="endtime2" lay-filter="date" />
                            </div>
                        </div>
                        <a class="layui-btn layui-btn-normal" id="cx2"><i class="fa fa-search"></i>&nbsp;查询</a>
                    </div>
                    <div id="divEChartsLMCount" style="width: 1250px; height: 400px; margin-top: 10px; ">
                    </div>
                    <div id="divEChartsLMIP" style="width: 1250px; height: 400px; margin-top: 10px; "></div>
                    <div style="clear: left;">
                        <fieldset class="layui-elem-field layui-field-title" style="margin-top: 20px;">
                            <legend>报表解读</legend>
                        </fieldset>
                    </div>
                    <table class="layui-table">
                        <colgroup>
                            <col >
                            <col width="15%">
                            <col width="15%">
                            <col width="15%">
                            <col width="15%" />
                        </colgroup>
                        <thead>
                            <tr>
                                <th style="text-align: center;">栏目名称</th>
                                <th style="text-align: center;">访问量</th>
                                <th style="text-align: center;">访问量百分比</th>
                                <th style="text-align: center;">IP数</th>
                                <th style="text-align: center;">IP数百分比</th>
                            </tr>
                        </thead>
                        <tbody id="msgLM">
                            <tr>
                                <td colspan="5" style="text-align: center;">暂无数据</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <!--栏目分析End-->
            </div>
        </div>
    </form>
    <script src="../scripts/layui/layui.js"></script>
    <script src="../scripts/jquery-1.10.2.min.js"></script>
    <script src="../scripts/model/apiurl.js"></script>
    <script src="../scripts/datepicker.js"></script>
    <script src="../scripts/layer/layer.js"></script>
    <!-- 引入 ECharts 文件 -->
    <script src="../scripts/echarts.js"></script>
    <script src="../scripts/JSManager/WebsiteManager/VisitAnaly.js"></script>
</body>
</html>
