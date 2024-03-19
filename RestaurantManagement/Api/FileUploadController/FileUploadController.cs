using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Business.FileUploadService;
using RestaurantManagement.Data;
using System.ComponentModel.DataAnnotations;
using System.IO.Compression;
using System.Security.Claims;
using static RestaurantManagement.Commons.Constants;

namespace RestaurantManagement.Api.FileUploadController
{
    [Route(AppSettingKeys.DEFAULT_CONTROLLER_ROUTE)]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IFileUploadService _uploadService;
        public FileUploadController(IFileUploadService uploadService)
        {
            _uploadService = uploadService;
        }
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadAvatar([Required] IFormFile file)
        {
            var userName = User.Claims.FirstOrDefault(x => string.Equals(x.Type, ClaimTypes.Name))!.Value;
            return Ok(await _uploadService.UploadAvatar(userName, file));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadMeida([FromForm] IList<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return Problem(detail: "Invalid File", statusCode: 400);

            var dayReceive = $"{DateTime.Now.Year}\\{DateTime.Now.Month}\\{DateTime.Now.Day}";
            var absolutePath = $"{FileConst.FILE_UPLOAD}\\{dayReceive}";

            var result = await _uploadService.UploadMultipleFile(absolutePath, files);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadAsync([FromQuery] string key)
        {
            var path = _uploadService.GetFiles(key).ToList();
            if (path == null || path.Count == 0)
                return new BadRequestObjectResult(FileConst.FILE_NOT_FOUND);

            if (!System.IO.File.Exists(path[0].FilePath))
                return new BadRequestObjectResult(FileConst.FILE_NOT_FOUND);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path[0].FilePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, FileConst.OCTET_STREAM, Path.GetFileName(path[0].FilePath));
        }

        [HttpGet]
        public async Task<IActionResult> DownloadZipAsync([FromRoute] string key)
        {
            var fileDownloads = new List<FileUpload>();
            var path = _uploadService.GetFiles(key).ToList();
            if (path == null || path.Count == 0)
                return new BadRequestObjectResult(FileConst.FILE_NOT_FOUND);

            if (!System.IO.File.Exists(path[0].FilePath))
                return new BadRequestObjectResult(FileConst.FILE_NOT_FOUND);


            fileDownloads.Add(new FileUpload
            {
                FilePath = path[0].FilePath,
                FileName = Path.GetFileName(path[0].FilePath),
                OriginalName = path[0].OriginalName
            });

            var memory = new MemoryStream();
            using (var zipArchive = new ZipArchive(memory, ZipArchiveMode.Create, true))
            {
                foreach (var file in fileDownloads)
                {
                    var enntry = zipArchive.CreateEntry(file.OriginalName);
                    using var entryStream = enntry.Open();
                    using var fileStream = new FileStream(file.FilePath, FileMode.Open);
                    await fileStream.CopyToAsync(entryStream);
                }
            }
            memory.Position = 0;

            return File(memory, FileConst.OCTET_STREAM, $"{path[0].OriginalName}.zip");
        }
    }
}
