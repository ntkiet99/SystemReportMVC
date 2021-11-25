using System;
using System.Linq;
using SystemReportMVC.Data;
using SystemReportMVC.Helpers;
using SystemReportMVC.Interfaces;
using SystemReportMVC.Models;

namespace SystemReportMVC.Services
{
    public class FileService : IFileService
    {
        private DataContext _context;
        public string EntityName = "Tệp tin";

        public FileService(DataContext context)
        {
            _context = context;
        }

        public File GetById(int id)
        {
            return _context.Files.Where(x => x.Id == id).FirstOrDefault();
        }

        public ResponseModel AddFile(AppUser user, string filePath, string fileName, string newFileName, string fileExt, int fileSize)
        {
            try
            {
                var entity = new File();
                entity.FileName = fileName;
                entity.SaveName = newFileName;
                entity.Path = filePath;
                entity.Ext = fileExt;
                entity.Size = fileSize;
                entity.CreateAt = DateTime.Now;
                entity.IsDeleted = false;
                entity.AuditTs = DateTime.Now;
                //entity.AuditUserId = user.Id;

                _context.Files.Add(entity);
                _context.SaveChanges();
                return new ResponseModel()
                {
                    Status = "success",
                    Message = "Tải tệp tin thành công.",
                    Data = new { entity }
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    Status = "false",
                    Message = $"Tải tệp tin thất bại",
                    Data = { }
                };
            }
        }
    }
}