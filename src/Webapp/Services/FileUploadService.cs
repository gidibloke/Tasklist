using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Webapp.Interfaces;
using Webapp.Models;

namespace Webapp.Services
{
    public class FileUploadService : IFileUplaod
    {
        private readonly ApplicationDbContext _context;

        public FileUploadService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> FileUpload(HttpPostedFileBase file, string UserId)
        {
            var user = await _context.Users.Where(x => x.Id == UserId).SingleOrDefaultAsync();
            if (user != null)
            {
                string[] ext = { ".jpeg", ".png", ".jpg" };
                var fileExt = Path.GetExtension(file.FileName);
                if (!ext.Contains(fileExt.ToLower()))
                {
                    return string.Empty;
                }
                //string userFolder = Path.Combine(HttpRuntime.AppDomainAppPath, "Content", "Profilepictures", UserId);
                //var files = Directory.GetFiles(userFolder);
                //foreach
                string appdatafolder = Path.Combine(HttpRuntime.AppDomainAppPath, "Content", "Profilepictures", UserId, UserId + fileExt);
                file.SaveAs(appdatafolder);
                return file.FileName;
            }
            return string.Empty;
        }
    }
}