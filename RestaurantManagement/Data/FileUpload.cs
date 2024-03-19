using RestaurantManagement.Commons;

namespace RestaurantManagement.Data
{
    public class FileUpload : BaseEntityCommons
    {
        public string FileName { get; set; }
        public string OriginalName { get; set; }
        public string FileType { get; set; }
        public string FileSize { get; set; }
        public string FilePath { get; set; }
        public string FileKey { get; set; }
        public string FileDescription { get; set; }
    }
}
