﻿namespace RestaurantManagement.Commons
{
    public class Constants
    {
        public class AppSettingKeys
        {
            public const string DEFAULT_CONTROLLER_ROUTE = "api/[controller]/[action]";
            public const string DEFAULT_CONNECTION = "DefaultConnection";
            public const string JWT_SECRET = "JWT:Secret";
            public const string JWT_VALIDAUDIENCE = "JWT:ValidAudience";
            public const string JWT_VALIDISSUER = "JWT:ValidIssuer";
            public const string JWT_EXPIREMINUTES = "JWT:RefreshExpireMinutes";
            public const string TIMEZONE = "TimeZone";
            public const string PASSWORDSYS = "PasswordSystem";
            public const string URLAPI = "UrlAPI";
            public const string PERMISSIONS = "Permissions";
        }
        public class ExceptionMessage
        {
            public const string ALREADY_EXIST = "{0} already exist.";
            public const string NOT_FOUND = "{0} not found.";
            public const string FAILED = "{0} failed.";
            public const string SUCCESS = "{0} success.";
            public const string EMPTY = "{0} is empty.";
            public const string INVALID = "{0} invalid.";
            public const string PERMISSION_DENIED = "Permission denied";
            public const string NOT_HAVE_PERMISSION = "Not enough permissions";
            public const string ALREADY_EXIST_IN = "{0} already exist in {1}";
        }

        public class DefaultValue
        {
            public const int DEFAULT_PAGE_SIZE = 10;
            public const int DEFAULT_PAGE_NO = 1;
        }
        public class FileConst
        {
            public const string FILE_UPLOAD = "FileUpload";
            public const string FILE_NOT_FOUND = "File not found";
            public const string OCTET_STREAM = "application/octet-stream";
        }
        public class JobScheduleOptions
        {
            public const string NameSystemJob = "SystemJobs";
            public const string DeleteRefreshTokenJob = "DeleteRefreshTokenJob";
        }
    }
}
