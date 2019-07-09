using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CMSSystem
{
    public partial class MainIndex : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public string menu()
        {
            string str = @"<ul class='layui-nav layui-nav-tree' lay-filter=''>
                        <li class='layui-nav-item'><a href='javascript:;'>后台首页</a></li>
                        <li class='layui-nav-item layui-nav-itemed'>
                            <a href='javascript:;'>内容管理</a>
                            <dl class='layui-nav-child'>
                                <dd><a href='main.html'>新闻</a></dd>
                                <dd><a href='javascript:;'>公告</a></dd>
                                <dd><a href='javascript:;'>吴江服务业</a></dd>
                            </dl>
                        </li>
                        <li class='layui-nav-item'>
                            <a href='javascript:;'>宣传管理 </a>
                            <dl class='layui-nav-child'>
                                <dd><a href='javascript:;'>政务 </a></dd>
                                <dd><a href='javascript:;'>办事公开</a></dd>
                                <dd><a href='javascript:;'>百件实事网上办 </a></dd>
                            </dl>
                        </li>
                    </ul>";
            return str;
        }
    }
}