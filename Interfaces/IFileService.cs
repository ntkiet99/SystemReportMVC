using SystemReportMVC.Helpers;
using SystemReportMVC.Models;

namespace SystemReportMVC.Interfaces
{
    public interface IFileService
    {
        File GetById(int id);
        ResponseModel AddFile(AppUser user, string filePath, string fileName, string newFileName, string fileExt, int fileSize);
    }
}