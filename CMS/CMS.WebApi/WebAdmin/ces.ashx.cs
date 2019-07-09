using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CMS.WebApi.WebAdmin
{
    /// <summary>
    /// ces 的摘要说明
    /// </summary>
    public class ces : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //如果进行了分片
            if (context.Request.Form.AllKeys.Any(m => m == "chunk"))
            {
                string fileMD5 = context.Request.Form["fileMd5"].ToString();
                int chunk = Convert.ToInt32(context.Request.Form["chunk"]);//当前分片在上传分片中的顺序（从0开始）
                if (context.Request["filesize"]!=null)
                {
                    string filesize = context.Request["filesize"].ToString();

                    string folder = context.Server.MapPath("~/UpFile/" + fileMD5 + "/");
                    string folder2 = context.Server.MapPath("~/UpFile/" + fileMD5 + "/" + chunk + "");

                    if (!Directory.Exists(Path.GetDirectoryName(folder)))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    if (System.IO.File.Exists(folder2))
                    { 
                        System.IO.FileInfo objFI = new System.IO.FileInfo(folder2);
                        string len = objFI.Length.ToString();
                        if (len==filesize.ToString())
                        {
                            context.Response.Write("{\"isupload\" : true}");
                        }
                        else
                        {
                            context.Response.Write("{\"isupload\" : false}");
                        }
                    }
                    else
                    {
                        context.Response.Write("{\"isupload\" : false}");
                    }
                }
                else
                {
                    //取得chunk和chunks
                    int chunks = Convert.ToInt32(context.Request.Form["chunks"]);//总分片数
                                                                                 //根据GUID创建用该GUID命名的临时文件夹
                    string folder = context.Server.MapPath("~/UpFile/" + fileMD5 + "/");
                    string path = folder + chunk;

                    //建立临时传输文件夹
                    if (!Directory.Exists(Path.GetDirectoryName(folder)))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    FileStream addFile = new FileStream(path, FileMode.Append, FileAccess.Write);
                    BinaryWriter AddWriter = new BinaryWriter(addFile);
                    //获得上传的分片数据流
                    HttpPostedFile file = context.Request.Files[0];
                    Stream stream = file.InputStream;

                    BinaryReader TempReader = new BinaryReader(stream);
                    //将上传的分片追加到临时文件末尾
                    AddWriter.Write(TempReader.ReadBytes((int)stream.Length));
                    //关闭BinaryReader文件阅读器
                    TempReader.Close();
                    stream.Close();
                    AddWriter.Close();
                    addFile.Close();

                    TempReader.Dispose();
                    stream.Dispose();
                    AddWriter.Dispose();
                    addFile.Dispose();
                    string root = context.Server.MapPath("~/UpFile/");
                    string sourcePath = Path.Combine(root, fileMD5 + "/");//源数据文件夹
                    string targetPath = Path.Combine(root, Guid.NewGuid() + Path.GetExtension(file.FileName));//合并后的文件
                    if (chunk == (chunks - 1))
                    {
                        DirectoryInfo dicInfo = new DirectoryInfo(sourcePath);
                        if (Directory.Exists(Path.GetDirectoryName(sourcePath)))
                        {
                            FileInfo[] files = dicInfo.GetFiles();
                            foreach (FileInfo file2 in files.OrderBy(f => int.Parse(f.Name)))
                            {
                                FileStream addFile2 = new FileStream(targetPath, FileMode.Append, FileAccess.Write);
                                BinaryWriter AddWriter2 = new BinaryWriter(addFile2);

                                //获得上传的分片数据流
                                Stream stream2 = file2.Open(FileMode.Open);
                                BinaryReader TempReader2 = new BinaryReader(stream2);
                                //将上传的分片追加到临时文件末尾
                                AddWriter2.Write(TempReader2.ReadBytes((int)stream2.Length));
                                //关闭BinaryReader文件阅读器
                                TempReader2.Close();
                                stream2.Close();
                                AddWriter2.Close();
                                addFile2.Close();

                                TempReader2.Dispose();
                                stream2.Dispose();
                                AddWriter2.Dispose();
                                addFile2.Dispose();
                            }
                            //DeleteFolder(sourcePath);
                        }
                    }
                    context.Response.Write("{\"chunked\" : true, \"hasError\" : false, \"f_ext\" : \"" + Path.GetExtension(file.FileName) + "\"}");
                }
            }
            else//没有分片直接保存
            {
                context.Request.Files[0].SaveAs(context.Server.MapPath("~/UpFile/" + DateTime.Now.ToFileTime() + Path.GetExtension(context.Request.Files[0].FileName)));
                context.Response.Write("{\"chunked\" : false, \"hasError\" : false}");
            }
        }

        /// <summary>
        /// 删除文件夹及其内容
        /// </summary>
        /// <param name="dir"></param>
        private static void DeleteFolder(string strPath)
        {
            //删除这个目录下的所有子目录
            if (Directory.GetDirectories(strPath).Length > 0)
            {
                foreach (string fl in Directory.GetDirectories(strPath))
                {
                    Directory.Delete(fl, true);
                }
            }
            //删除这个目录下的所有文件
            if (Directory.GetFiles(strPath).Length > 0)
            {
                foreach (string f in Directory.GetFiles(strPath))
                {
                    System.IO.File.Delete(f);
                }
            }
            Directory.Delete(strPath, true);
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