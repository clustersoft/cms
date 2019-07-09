/** index.js By Beginner Emain:zheng_jinfan@126.com HomePage:http://www.zhengjinfan.cn */
layui.config({
    base: 'scripts/'
}).use(['element', 'layer', 'navbar', 'tab'], function () {
    var element = layui.element(),
		$ = layui.jquery,
		layer = layui.layer,
		navbar = layui.navbar(),
		tab = layui.tab({
		    elem: '.admin-nav-card', //设置选项卡容器
		    autoRefresh: true,
            //打开数量限制
		    //maxSetting: {
		    //    max: 5,
		    //    tipMsg: '只能打开五个。'
		    //}
		});
    //iframe自适应
    $(window).on('resize', function () {
        var $content = $('.admin-nav-card .layui-tab-content');
        $content.height($(this).height() - 107);
        $content.find('iframe').each(function () {
            $(this).height($content.height());
        });
    }).resize();
        
    //设置navbar
    navbar.set({
        spreadOne: true,
        elem: '#admin-navbar-side',
        cached: true,
        //url: 'datas/nav.js'
        data: navs,
        /*cached:true,
		url: 'datas/nav.json'
        type:get/post*/
    });
    //渲染navbar
    navbar.render();
    
    //添加tips		                  
    var li = $("#sidebar-side").find("li");
    var dd = $("#sidebar-side").find("dd");
    li.hover(function () {
        //alert(1);
        var minitips = $("#sidebar-side").hasClass("sidebar-mini");
        if (minitips) {
            //alert(2)
            var title = $(this).find("a").first().find("cite").text();
            layer.tips(title, this, {
                tips: 2,
                time: 1500
            });
        }
    });
    dd.hover(function () {
        var minitips = $("#sidebar-side").hasClass("sidebar-mini");
        if (minitips) {
            var title = $(this).find("a").find("cite").text();
            layer.tips(title, this, {
                tips: 2,
                time: 1500
            });
        }
    });
    //监听点击事件
    navbar.on('click(side)', function (data) {
        tab.tabAdd(data.field);
    });
    //左侧菜单收缩
    var foldNode = $('#sidebar');
    var sidebarNode = $('#sidebar-side');
    var headerNode = $('.header-admin');
    if (foldNode) {
        $(document).on("click", '#sidebar', function () {
            var toType = sidebarNode.hasClass("sidebar-mini") ? "full" : "mini";
            var sideWidth = sidebarNode.width();
            if (sideWidth == 200) {
                $('#admin-body').animate({
                    left: '70px'
                }); //admin-footer
                //$('.admin-footer').animate({
                //    left: '70px'
                //});
                sidebarNode.addClass('sidebar-mini');
                headerNode.addClass('header-mini');
                $('#sidebar').find('i').removeClass('fa-bars').addClass('fa-th-large');
            } else {
                $('#admin-body').animate({
                    left: '200px'
                });
                //$('.admin-footer').animate({
                //    left: '200px'
                //});
                sidebarNode.removeClass('sidebar-mini');
                headerNode.removeClass('header-mini');
                $('#sidebar').find('i').removeClass('fa-th-large').addClass('fa-bars');
            }
        });
    }

    //var globalActive = {
    //    doRefreshTable: function () {
    //        var index = layer.load(2, { shade: false }); //0代表加载的风格，支持0-2
    //        var t = setTimeout("layer.closeAll();", 300);
    //        $("#gridList").trigger("reloadGrid");
    //    },
    //    doRefresh: function () {
    //        var url = $(this).data('href');
    //        if (url) {
    //            location.href = url;
    //        }
    //        else {
    //            location.href = location.href;
    //        }
    //    },
    //    doGoTop: function () {
    //        $(this).click(function () {
    //            $('body,html').animate({ scrollTop: 0 }, 500);
    //            return false;
    //        });
    //    },
    //    doGoBack: function () {
    //        history.go(-1);
    //    }
    //};
    //$('.do-action').on('click', function (e) {
    //    var type = $(this).data('type');
    //    globalActive[type] ? globalActive[type].call(this) : '';
    //    layui.stope(e);//阻止冒泡事件
    //});
    //显示隐藏侧边导航
    //$('.admin-side-toggle').on('click', function () {
    //    var sideWidth = $('#admin-side').width();
    //    if (sideWidth === 200) {
    //        $('#admin-body').animate({
    //            left: '0'
    //        }); //admin-footer
    //        $('#admin-footer').animate({
    //            left: '0'
    //        });
    //        $('#sidebar-side').animate({
    //            width: '0'
    //        });
    //    } else {
    //        $('#admin-body').animate({
    //            left: '200px'
    //        });
    //        $('#admin-footer').animate({
    //            left: '200px'
    //        });
    //        $('#sidebar-side').animate({
    //            width: '200px'
    //        });
    //    }
    //});
    ////全屏功能
    //$('.admin-side-full').on('click', function () {
    //    var docElm = document.documentElement;
    //    //W3C  
    //    if (docElm.requestFullscreen) {
    //        docElm.requestFullscreen();
    //    }
    //        //FireFox  
    //    else if (docElm.mozRequestFullScreen) {
    //        docElm.mozRequestFullScreen();
    //    }
    //        //Chrome等  
    //    else if (docElm.webkitRequestFullScreen) {
    //        docElm.webkitRequestFullScreen();
    //    }
    //        //IE11
    //    else if (elem.msRequestFullscreen) {
    //        elem.msRequestFullscreen();
    //    }

    //    layer.msg('按Esc即可退出全屏');
    //});
});