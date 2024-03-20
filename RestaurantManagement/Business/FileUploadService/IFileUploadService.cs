using RestaurantManagement.Data.Entities;

namespace RestaurantManagement.Business.FileUploadService
{
    public interface IFileUploadService
    {
        Task<bool> UploadAvatar(string userName, IFormFile file);
        Task<FileUpload> CreateNew(FileUpload fileUpload);
        Task<List<FileUpload>> CreateNewList(List<FileUpload> fileUploads);
        Task RemoveFileById(long id);
        IQueryable<FileUpload> GetFiles(string? key);
        Task<FileUpload> UploadFile(string folderName, IFormFile file);
        Task<List<FileUpload>> UploadMultipleFile(string folderName, IList<IFormFile> files);
    }
}
