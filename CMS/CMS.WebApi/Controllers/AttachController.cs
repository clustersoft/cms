using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Hosting;
using System.Web.Http;
using CMS.Application.ArticleAttachs;
using CMS.Application.ArticleAttachs.Dto;
using CMS.Application.SystemConfigurations;
using CMS.Model;
using CMS.Util;
using Newtonsoft.Json;
using WebGrease.Activities;

namespace CMS.WebApi.Controllers
{
    [RoutePrefix("api/attach")]
    public class AttachController : BaseApiController
    {
        private readonly IArticleAttachService _articleAttachService;
        private readonly ISystemConfigurationService _systemConfigurationService;
        private string _moduleName = "附件";

        public AttachController(IArticleAttachService articleAttachService,ISystemConfigurationService systemConfigurationService)
        {
            _articleAttachService = articleAttachService;
            _systemConfigurationService = systemConfigurationService;
        }

        //public HttpResponseMessage Get(string fileName)
        //{
        //    HttpResponseMessage result = null;

        //    DirectoryInfo directoryInfo = new DirectoryInfo(HostingEnvironment.MapPath(CMSConst.UploadFolder));
        //    FileInfo foundFileInfo = directoryInfo.GetFiles().Where(x => x.Name == fileName).FirstOrDefault();
        //    if (foundFileInfo != null)
        //    {
        //        FileStream fs = new FileStream(foundFileInfo.FullName, FileMode.Open);

        //        result = new HttpResponseMessage(HttpStatusCode.OK);
        //        result.Content = new StreamContent(fs);
        //        result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
        //        result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
        //        result.Content.Headers.ContentDisposition.FileName = foundFileInfo.Name;
        //    }
        //    else
        //    {
        //        result = new HttpResponseMessage(HttpStatusCode.NotFound);
        //    }

        //    return result;
        //}


        [HttpPost]
        [Route("upload")]
        public ResponseInfoModel Upload()
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                string filename;
                string format;
                string newName;
                int contentlength;
                int fileType;
                string attachUrl;

                var systemConfig = _systemConfigurationService.FirstOrDefault();
                string uploadPath = systemConfig.Uploadpath;

                //如果目录不存在，则创建目录
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(uploadPath)))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(uploadPath));
                }

                HttpPostedFile f = HttpContext.Current.Request.Files[0];

                //获取文件MD5
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(f.InputStream);
                //f.InputStream.Close();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                string fileMD5 = sb.ToString();

                //查询数据库中有相同文件则不重新上传
                var SameHashFile = _articleAttachService.FirstOrDefault(a => a.HashValue == fileMD5);
                //判断文件在服务器上是否还存在
                if (SameHashFile != null&&IsExists(SameHashFile.AttachUrl))
                {
                    filename = f.FileName;
                    format = SameHashFile.AttachFormat;
                    contentlength = SameHashFile.AttachBytes;
                    newName = SameHashFile.AttachNewName;
                    fileType = SameHashFile.AttachType;
                    attachUrl = SameHashFile.AttachUrl;
                }
                else
                {
                    filename = f.FileName;
                    format = Path.GetExtension(filename).ToLower();
                    newName = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + format;
                    contentlength = f.ContentLength;
                    string[] imgTypes = systemConfig.ImgFormat.ToLower().Split(',');
                    fileType = imgTypes.Contains(format) ? 1 : 0;
                    attachUrl = uploadPath + newName;
                    f.SaveAs(HttpContext.Current.Server.MapPath(uploadPath) + newName);
                }

                var output = new UploadAttachOutput()
                {
                    AttachFormat = format,
                    AttachBytes = contentlength,
                    AttachName = filename,
                    AttachNewName = newName,
                    AttachType = fileType,
                    AttachUrl = attachUrl,
                    HashValue = fileMD5
                };
                json.Result = output;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/attach/upload", LocalizationConst.UploadFail);
            }
            return json;
        }


        [HttpPost]
        [Route("uploadWithPath")]
        public ResponseInfoModel UploadWithPath()
        {
            ResponseInfoModel json = new ResponseInfoModel() { Success = 1, Result = new object() };
            try
            {
                string filename;
                string format;
                string newName;
                int contentlength;
                int fileType;
                string attachUrl;

                var systemConfig = _systemConfigurationService.FirstOrDefault();
                string uploadPath = HttpContext.Current.Request["path"];

                //如果目录不存在，则创建目录
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(uploadPath)))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(uploadPath));
                }

                HttpPostedFile f = HttpContext.Current.Request.Files[0];

                //获取文件MD5
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(f.InputStream);
                //f.InputStream.Close();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                string fileMD5 = sb.ToString();

                //查询数据库中有相同文件则不重新上传
                var SameHashFile = _articleAttachService.FirstOrDefault(a => a.HashValue == fileMD5);
                //判断文件在服务器上是否还存在
                if (SameHashFile != null && IsExists(SameHashFile.AttachUrl))
                {
                    filename = f.FileName;
                    format = SameHashFile.AttachFormat;
                    contentlength = SameHashFile.AttachBytes;
                    newName = SameHashFile.AttachNewName;
                    fileType = SameHashFile.AttachType;
                    attachUrl = SameHashFile.AttachUrl;
                }
                else
                {
                    filename = f.FileName;
                    format = Path.GetExtension(filename).ToLower();
                    newName = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + format;
                    contentlength = f.ContentLength;
                    string[] imgTypes = systemConfig.ImgFormat.ToLower().Split(',');
                    fileType = imgTypes.Contains(format) ? 1 : 0;
                    attachUrl = uploadPath + newName;
                    f.SaveAs(HttpContext.Current.Server.MapPath(uploadPath) + newName);
                }

                var output = new UploadAttachOutput()
                {
                    AttachFormat = format,
                    AttachBytes = contentlength,
                    AttachName = filename,
                    AttachNewName = newName,
                    AttachType = fileType,
                    AttachUrl = attachUrl,
                    HashValue = fileMD5
                };
                json.Result = output;
            }
            catch (Exception e)
            {
                DisposeUserFriendlyException(e, ref json, "api/attach/uploadWithPath", LocalizationConst.UploadFail);
            }
            return json;
        }

        private bool IsExists(string path)
        {
          return File.Exists(HttpContext.Current.Server.MapPath(path));
        }
    }
}
