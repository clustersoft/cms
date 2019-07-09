using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMSSystem.scripts.UEditor
{
    /// <summary>
    /// UEditorHandler 的摘要说明
    /// </summary>
    public class UEditorHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var result = context.Request["result"];
            //当然这里最好判断一下result是否安全，不要接收到内容就显示这样会被人利用。
            if (result != null)
                context.Response.Write(result.ToString());
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}