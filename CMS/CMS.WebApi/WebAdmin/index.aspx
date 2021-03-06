﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="CMSSystem.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" ng-app="myIndex">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>信息发布平台</title>
    <link href="scripts/layui/css/layui.css" rel="stylesheet" />
    <link href="css/global.css" rel="stylesheet" />
    <link href="css/font-awesome.min.css" rel="stylesheet" />
    <link href="css/cmsui.css" rel="stylesheet" />
    <link href="scripts/layui/css/modules/layer/default/layer.css" rel="stylesheet" />
    <style>
        body {
            margin: 0;
            overflow: hidden;
            background-color: #1c77ac;
        }

        .main {
            position: absolute; /*层漂浮*/
            top: 50%;
            left: 50%;
            width: 680px;
            height: 320px;
            margin-left: -340px; /*这里是DIV宽度的一半*/
            margin-top: -160px; /*注意这里必须是DIV高度的一半*/
            z-index: 1;
        }

        #code {
            font-family: Arial;
            font-style: italic;
            font-weight: bold;
            border: 0;
            letter-spacing: 2px;
            color: blue;
        }
        .valid {
            top: 0px; width: 100%; text-align: right; right: 0px; color: rgb(169, 68, 66); line-height: 38px; padding-right: 20px; padding-bottom: 0px; font-size: 12px; margin-top: 0px; display: block; position: absolute; z-index: 2; pointer-events: none; animation-duration: 0.2s;
        }
    </style>
</head>
<body onresize="ResetLogintable();" ng-controller="indexController">
    <form name="myForm" class="layui-form layui-form-pane">
        <!--头-->
        <div style="width: 100%; height: 47px; background-color: #14547a;  line-height:47px;font-family:Microsoft YaHei; color:#fff; font-size:14px;">
            <span style=" margin-left:35px;">欢迎登录信息发布平台</span>
            <span style=" position:absolute; right:135px;">首页</span>
            <span style=" position:absolute; right:55px;">网信官网</span>
        </div>
        <div id="bgimg" style="width: 100%; background-color: #1c77ac; min-width: 1115px;">
            <img id="yun" class="yun" src="Images/yun.png" style="bottom: 47px; position: absolute; display: none;" />
        </div>
        <!--尾-->
        <div style="width: 100%; height: 47px; background-color: #448fbb; bottom: 0; position: absolute; text-align:center; line-height:47px; font-family:Microsoft YaHei; color:#fff; font-size:14px;">
            苏州网信信息科技有限公司 ©2017
        </div>
        <div class="main">
            <div style="width: 220px; height: 320px; float: left;">
                <img src="Images/lvyou.png" />
            </div>
            <div style="float: left; width: 460px; height: 320px; background-color: #fff;">
                <div style="margin: 33px 0 0 61px; font-size: 20px; font-weight: bold; font-family: Microsoft YaHei; color: #b68a3e;">
                    信息发布平台
                </div>
                <div style="margin: 30px 0 0 61px; float: left; width: 348px; border: solid 1px #e6e6e6;">
                    <div style="width: 32px; height: 38px; float: left">
                        <img src="Images/user.png" style=" margin:8px;" /></div>
                    <div style="width: 312px; height: 38px; float: left">
                        <div class="layui-input-inline">
                        <input id="loginname" name="loginname" type="text" placeholder="用户名" style="width: 310px; height: 36px; line-height: 36px; color: #939393; font-size: 16px; border: solid 0px #e6e6e6; padding-left: 5px;" ng-model='user.loginname' autocomplete="off" required />
                        <span class="valid" ng-show="myForm.loginname.$dirty &&myForm.loginname.$error.required">用户名不能为空</span>
                            </div>
                    </div>
                </div>
                <div style="margin: 15px 0 0 61px; float: left; width: 348px; border: solid 1px #e6e6e6;">
                    <div style="width: 32px; height: 38px; float: left">
                        <img src="Images/lock.png" style=" margin:8px;" /></div>
                    <div style="width: 312px; height: 38px; float: left">
                        
                        <div class="layui-input-inline">
                        <input id="password" name="password" type="password" placeholder="密码" style="width: 310px; height: 35px; font-size: 16px; color: #939393; border: solid 0px #e6e6e6; line-height: 35px;" ng-model='user.password' autocomplete="off" required />
                        <span class="valid" ng-show="myForm.password.$dirty &&myForm.password.$error.required">密码不能为空</span>
                            </div>
                    </div>
                </div>
                <div style="margin: 15px 0 0 61px; float: left; width: 348px;">
                    <div style="width: 245px; height: 38px; float: left; border: solid 1px #e6e6e6;">
                        <div style="width: 32px; height: 38px; float: left">
                            <img src="Images/lock.png" style=" margin:8px;" /></div>
                        <div style="width: 212px; height: 38px; float: left">
                        <div class="layui-input-inline">
                           <input type="text" id="input" name="input" placeholder="验证码" style="width: 208px; height: 35px; font-size: 16px; color: #939393; border: solid 0px #e6e6e6; line-height: 38px;" ng-model='user.input' autocomplete="off" required  />
                        <span class="valid" ng-show="myForm.input.$dirty &&myForm.input.$error.required">验证码不能为空</span>
                            </div>
                        </div>
                    </div>
                    <div style="">
                        <input type="button" id="code" style="width: 91px; height: 38px; font-size: 22px; margin: 1px 0 0 8px; font-weight: bold; color: #4985f0; background-color: #fff; border: solid 1px #e6e6e6;" onclick="createCode()" />
                    </div>
                </div>

                <div style="margin: 15px 0 0 0; float:left; width:100%;  text-align: center;">
                    <input id="submit" name="submit" type="button" value="立即登录" class="layui-btn layui-btn" data-ng-class="{true:'layui-btn-disabled',false:''}[myForm.$invalid]" data-ng-disabled="myForm.$invalid"  />
                </div>
            </div>
        </div>
    </form>
    <script src="scripts/angular.min.js"></script>
    <script src="scripts/jquery-1.10.2.min.js"></script>
    <script src="scripts/model/jquery.xdomainrequest.min.js"></script>
    <script src="scripts/model/CheckCode.js"></script>
    <script src="scripts/layui/lay/modules/layer.js"></script>
    <script src="scripts/layui/layui.js"></script>
    <script src="scripts/model/apiurl.js"></script>
    <script src="scripts/md5.js"></script>
    <script type="text/javascript">
        angular.module('myIndex', []).controller('indexController', function ($scope) { });
        function ResetLogintable() {
            $("#bgimg").height(document.documentElement.clientHeight - 94);
            $("#yun").width(document.documentElement.clientWidth);
            var sjwidth = $("#yun").width();
            var sjheight = sjwidth * 814 / 1920;
            var bodyheight = document.documentElement.clientHeight;
            if (bodyheight > sjheight) {
                sjwidth = 1920 * bodyheight / 814;
                $("#yun").width(sjwidth)
            }
            $("#yun").css('display', 'block');
        }
        ResetLogintable();
        var apiurl = geturl();
        if ($("#loginname").val().length == 0) {
            $("#loginname").focus();
        } else {
            if ($("#password").val().length==0) {
                $("#password").focus();
            } else {
                $("#input").focus();
            }
        }
        $("body").keydown(function () {
            if (event.keyCode == "13") {//keyCode=13是回车键
                if ($("#loginname").val().length > 0 && $("#password").val().length>0 && $("#input").val().length>0) {
                    $('#submit').click();
                }
            }
        });   

        $("#submit").click(function () {
            layer.load(2);
            //$("#submit").removeClass("layui-btn layui-btn");
            //$("#submit").addClass("layui-btn layui-btn-disabled");
            //$("#submit").attr("disabled", "disabled");
            var inputCode = document.getElementById("input").value.toUpperCase(); //取得输入的验证码并转化为大写      
            if (inputCode.length <= 0) { //若输入的验证码长度为0
                layer.msg("请输入验证码！"); //则弹出请输入验证码
                $("#input").focus();
                layer.closeAll('loading');
                return false;
            }
            else if (inputCode != code) { //若输入的验证码与产生的验证码不一致时
                layer.msg("验证码输入错误！"); //则弹出验证码输入错误
                createCode();//刷新验证码
                document.getElementById("input").value = "";//清空文本框
                $("#input").focus();
                layer.closeAll('loading');
                return false;
            } else {

                var tmp = hex_md5($("#password").val()).toUpperCase();
                var postdata = {
                    loginname: $("#loginname").val(),
                    password: tmp
                };

                $.ajax({
                    url: "/token",
                    ContentType: 'application/x-www-form-urlencoded;',
                    data: { grant_type: "password", username: $("#loginname").val(), password: tmp },
                    type: 'POST',
                    dataType: 'json',
                    async: false
                }).done(function (json) {
                    var token = json.token_type + " " + json.access_token;
                    var tokenRefresh = json.refresh_token;
                    $.ajax({
                        url: "/api/user/login",
                        ContentType: 'application/x-www-form-urlencoded;',
                        data: postdata,
                        type: 'POST',
                        dataType: 'json',
                        async: false
                    }).done(function (data) {
                        if (data.Success == 1) {
                            var cookie = {
                                CMSUserID: data.Result.ID,
                                CMSUserName: data.Result.UserName,
                                CMSUserSourceFrom: data.Result.UserSourceFrom,
                                CMSLastLoginTime: data.Result.LastLoginTime,
                                CMSLastLoginIp: data.Result.LastLoginIp,
                                CMSGrade: data.Result.Grade,
                                CMSToken: token,
                                CMSTokenRefresh: tokenRefresh
                            };
                            setCookie("CMSData", JSON.stringify(cookie), 1);
                            location.href = "MainIndex.aspx";
                        } else {
                            layer.msg("登录失败，用户名或密码错误！");
                            $("#loginname").focus();
                        }
                    });

                }).fail(function (xhr, error, errorThrown) {
                    layer.msg("登录失败，用户名或密码错误！");
                    $("#loginname").focus();
                });
            }
            layer.closeAll('loading');
        });
    </script>
</body>
</html>
