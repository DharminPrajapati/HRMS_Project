using MVCProject.Api.Models;
using MVCProject.Api.Utilities;
using MVCProject.Common.Resources;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace MVCProject.Api.Controllers.Common
{
    public class UploadPrecController : BaseController
    {
        [HttpPost]
        public ApiResponse UploadFile()
        {
            
            string directoryPath = string.Empty;
            string originalFileName = string.Empty;

            //directoryPath= AppUtility.GetDirectoryPath(enumDirectoryPath, databaseName, false, FileURL);
            //save the fileoriginalFileName
            // Get Files from request
            string fileName = string.Empty;
            string filePath = HttpContext.Current.Server.MapPath("~"); ;
            directoryPath = filePath + "\\Attachments\\Temp";
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var httpPostedFile = HttpContext.Current.Request.Files["file"];
                originalFileName = httpPostedFile.FileName;
                fileName = Guid.NewGuid().ToString() + Path.GetExtension(originalFileName);
                filePath = Path.Combine(directoryPath, fileName);
                try
                {
                    // Delete file if exists
                    FileInfo fileInfo = new FileInfo(filePath);
                    if (fileInfo.Exists)
                    {
                        fileInfo.Delete();
                    }

                    // Save File
                    httpPostedFile.SaveAs(filePath);
                }
                catch (Exception ex)
                {
                  //  AppUtility.ElmahErrorLog(ex);
                    return this.Response(MessageTypes.Error, Resource.SomethingWrong);
                }
            }
                return this.Response(MessageTypes.Success, responseToReturn: null);
        }
    }
}
