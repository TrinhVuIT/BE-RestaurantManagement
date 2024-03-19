using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Commons;
using RestaurantManagement.Data;
using RestaurantManagement.Data.Entities;
using static RestaurantManagement.Commons.Constants;

namespace RestaurantManagement.Business.FileUploadService
{
    public class FileUploadService : IFileUploadService
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        public FileUploadService(DataContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<FileUpload> CreateNew(FileUpload fileUpload)
        {
            await _context.FileUpload.AddAsync(fileUpload);
            await _context.SaveChangesAsync();
            return fileUpload;
        }

        public async Task<List<FileUpload>> CreateNewList(List<FileUpload> fileUploads)
        {
            await _context.FileUpload.AddRangeAsync(fileUploads);
            await _context.SaveChangesAsync();
            return fileUploads;
        }

        public IQueryable<FileUpload> GetFiles(string? key)
        {
            var path = _context.FileUpload.Where(x => x.FileKey == key && !x.IsDeleted);
            return path;
        }

        public async Task RemoveFileById(long id)
        {
            var fileRemove = await _context.FileUpload.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);
            if (fileRemove == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(id)));

            fileRemove.IsDeleted = true;
            _context.FileUpload.Update(fileRemove);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UploadAvatar(string userName, IFormFile file)
        {
            if (file == null)
                throw new Exception(string.Format(FileConst.FILE_NOT_FOUND));

            var user = await _userManager.FindByNameAsync(userName);
            if(user == null)
                throw new Exception(string.Format(ExceptionMessage.NOT_FOUND, nameof(userName)));

            var folderChildUser = $"User";
            var folderChildName = $"Avatar";
            var absolutePath = $"{FileConst.FILE_UPLOAD}\\{folderChildUser}\\{userName}\\{folderChildName}";

            var uploadFile = await UploadFile(absolutePath, file);

            user.Avatar = uploadFile.FileKey;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<FileUpload> UploadFile(string folderName, IFormFile file)
        {
            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);

            var fileName = file.FileName.Replace("\"", "").Split(".");
            var newName = Guid.NewGuid().ToString().Replace("-", "");
            var destinationPath = Path.Combine(folderName, newName + "." + fileName[^1]);
            while (File.Exists(destinationPath))
            {
                destinationPath = Path.Combine(folderName, newName + "." + fileName[^1]);
            }
            using (var stream = new FileStream(destinationPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            FileUpload fileUpload = new FileUpload()
            {
                FileName = newName + "." + fileName[^1],
                OriginalName = file.FileName,
                FilePath = destinationPath,
                FileSize = new FileInfo(destinationPath).Length.ToString(),
                FileKey = newName,
                FileType = Path.GetExtension(file.FileName),
            };

            return await CreateNew(fileUpload);
        }

        public async Task<List<FileUpload>> UploadMultipleFile(string folderName, IList<IFormFile> files)
        {
            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);

            var fileUploads = new List<FileUpload>();
            foreach (var file in files)
            {
                var fileName = file.FileName.Replace("\"", "").Split(".");
                var newName = Guid.NewGuid().ToString().Replace("-", "");
                var destinationPath = Path.Combine(folderName, newName + "." + fileName[^1]);
                while (File.Exists(destinationPath))
                {
                    destinationPath = Path.Combine(folderName, newName + "." + fileName[^1]);
                }
                using (var stream = new FileStream(destinationPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                FileUpload fileUpload = new FileUpload()
                {
                    FileName = newName + "." + fileName[^1],
                    OriginalName = file.FileName,
                    FilePath = destinationPath,
                    FileSize = new FileInfo(destinationPath).Length.ToString(),
                    FileKey = newName,
                    FileType = Path.GetExtension(file.FileName),
                };
                fileUploads.Add(fileUpload);
            }
            return await CreateNewList(fileUploads);
        }
    }
}
