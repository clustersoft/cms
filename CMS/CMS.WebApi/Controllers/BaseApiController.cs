using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CMS.Model;
using CMS.Util;
using CMS.Utils;

namespace CMS.WebApi.Controllers
{
    //需要验证的方法前加入 [Authorize]
    public class BaseApiController : ApiController
    {
        /// <summary>
        /// 模型验证
        /// </summary>
        protected virtual void CheckModelState()
        {
            if (!ModelState.IsValid)
            {
                throw new UserFriendlyException(LocalizationConst.FormIsNotValidMessage);
            }
        }

        /// <summary>
        /// 友好的异常处理
        /// </summary>
        /// <param name="e"></param>
        /// <param name="json"></param>
        /// <param name="loginfo"></param>
        /// <param name="jsonresult"></param>
        /// <returns></returns>
        protected virtual void DisposeUserFriendlyException(Exception e, ref ResponseInfoModel json, string loginfo, string jsonresult)
        {
            json.Success = 0;
            if (e is UserFriendlyException)
            {
                json.Result = e.Message;
            }
            else
            {
                LogHelper.ErrorLog(loginfo, e);
                json.Result = jsonresult;
            }
        }

        /// <summary>
        /// 字符串转换为数值数组
        /// </summary>
        /// <param name="srt"></param>
        /// <returns></returns>
        protected virtual int[] ConvertStringToIntArr(string srt)
        {
            int[] idInts = string.IsNullOrEmpty(srt)? new int[] { }: Array.ConvertAll<string, int>(srt.Split(','), delegate (string s) { return int.Parse(s); });
            return idInts;
        }

        /// <summary>
        /// 获取站点发布地址
        /// </summary>
        /// <returns></returns>
        protected virtual string GetPublishUrl()
        {
            return ConfigurationManager.AppSettings["PulishAddres"];
        }
    }
}
