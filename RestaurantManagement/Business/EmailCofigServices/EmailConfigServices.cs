using MailKit.Security;
using MimeKit;
using RestaurantManagement.Commons;
using RestaurantManagement.Data;
using RestaurantManagement.Data.Entities;
using RestaurantManagement.Data.RequestModels.Email;

namespace RestaurantManagement.Business.EmailCofigServices
{
    public class EmailConfigServices : IEmailConfigServices
    {
        private readonly DataContext _context;
        private readonly ILogger<EmailConfigServices> _logger;
        private readonly IHttpContextAccessor _httpContext;
        public EmailConfigServices(DataContext context, IHttpContextAccessor httpContext, ILogger<EmailConfigServices> logger)
        {
            _context = context;
            _httpContext = httpContext;
            _logger = logger;
        }
        public async Task<bool> AddNewEmailConfig(EmailConfigRequestModel newEmail)
        {
            try
            {
                _context.Add(new EmailConfig
                {
                    Code = newEmail.Code,
                    Email = newEmail.Email,
                    Host = newEmail.Host,
                    Port = newEmail.Port,
                    Password = newEmail.Password,
                    Tittle = newEmail.Tittle,
                    Content = newEmail.Content
                });
                return _context.SaveChanges() > 0;
            }
            catch
            {
                throw new Exception(string.Format(Constants.ExceptionMessage.FAILED, nameof(newEmail.Tittle)));
            }
        }

        public async Task<bool> DeleteEmail(long id)
        {
            var deleteEmail = GetById(id).FirstOrDefault();
            if (deleteEmail != null)
            {
                deleteEmail.IsDeleted = true;
                _context.Update(deleteEmail);

                return _context.SaveChanges() > 0;
            }
            else
            {
                throw new Exception(string.Format(Constants.ExceptionMessage.NOT_FOUND, nameof(deleteEmail.Id)));
            }
        }

        public async Task<bool> UpdateEmailConfig(long id, EmailConfigRequestModel updateEmail)
        {
            var emailConfig = GetById(id).FirstOrDefault();
            if (emailConfig != null)
            {
                emailConfig.Code = updateEmail.Code;
                emailConfig.Email = updateEmail.Email;
                emailConfig.Host = updateEmail.Host;
                emailConfig.Port = updateEmail.Port;
                emailConfig.Password = updateEmail.Password;
                emailConfig.Tittle = updateEmail.Tittle;
                emailConfig.Content = updateEmail.Content;
                _context.Update(emailConfig);

                return _context.SaveChanges() > 0;
            }
            else
            {
                throw new Exception(string.Format(Constants.ExceptionMessage.NOT_FOUND, nameof(emailConfig.Email)));
            }
        }
        public async Task<bool> SendEmailForgotPasswordAsync(string fullNamse, string mail, string token)
        {
            var emailConfig = GetAll().FirstOrDefault();
            if (emailConfig == null)
            {
                throw new Exception(string.Format(Constants.ExceptionMessage.NOT_FOUND, ("emailConfig")));
            }
            string systemName = "HRM Management Software PhuongTrinh";
            string subjectEmail = "Reset Password";
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress(systemName, emailConfig?.Email));
            message.To.Add(MailboxAddress.Parse(mail));
            message.Subject = subjectEmail;

            var linkUrl = emailConfig?.ServerName + "/reset-password?email=" + mail + "&token=" + token;

            string textBody = RenderHtml(fullNamse, linkUrl, message.Subject);
            message.Body = new TextPart("html") { Text = textBody };

            var client = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                client.Connect(emailConfig?.Host, emailConfig.Port, emailConfig.UseSSL);
                client.Authenticate(emailConfig?.Email, emailConfig?.Password);

                await client.SendAsync(message);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error sending mail - " + mail);
                _logger.LogError(ex.Message);
                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }

        public async Task<bool> SendToEmailAsync(SendToEmailRequestModel emailModel)
        {
            var emailConfig = GetAll().FirstOrDefault();
            if (emailConfig == null)
            {
                throw new Exception(string.Format(Constants.ExceptionMessage.NOT_FOUND, ("emailConfig")));
            }
            string tieuDeEmail = string.IsNullOrEmpty(emailModel.Subject) ? emailConfig.Tittle : emailModel.Subject;
            var message = new MimeMessage();
            message.Sender = new MailboxAddress(tieuDeEmail, emailConfig?.Email);
            message.From.Add(new MailboxAddress(tieuDeEmail, emailConfig?.Email));
            message.To.Add(MailboxAddress.Parse(emailModel.ToEmail));
            message.Subject = tieuDeEmail;

            var builder = new BodyBuilder();
            builder.HtmlBody = string.IsNullOrEmpty(emailModel.Body) ? emailConfig?.Content : emailModel.Body;
            message.Body = builder.ToMessageBody();

            // dùng SmtpClient của MailKit
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                smtp.Connect(emailConfig.Host, emailConfig.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(emailConfig.Email, emailConfig.Password);
                await smtp.SendAsync(message);
            }
            catch (Exception ex)
            {
                // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
                //System.IO.Directory.CreateDirectory("mailssave");
                //var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                //await message.WriteToAsync(emailsavefile);

                _logger.LogInformation("Error sending email - " + emailModel.ToEmail);
                _logger.LogError(ex.Message);
                return false;
            }

            smtp.Disconnect(true);
            //_logger.LogInformation("send mail to: " + toEmail);
            return true;
        }

        private IQueryable<EmailConfig> GetAll()
        {
            return _context.EmailConfig.AsQueryable();
        }

        private IQueryable<EmailConfig> GetById(long id)
        {
            return GetAll().Where(x => x.Id == id);
        }

        private string RenderHtml(string displayName, string link, string subject)
        {
            return @"<!doctype html>
                <html>
                <head>
                    <style>
                        body {
                            font-family: Arial, sans-serif;
                            font-size: 14px;
                            line-height: 1.5;
                        }
                        h1 {                            
                            font-size: 18px;
                        }
                    </style>
                </head>
                <body>
                    <h1>Hi you " + displayName + @",</h1>
                    <p>You have sent a request to reset your password to access HRM Management Software PhuongTrinh. Please follow the link below to change your password.</p>                    
                    <a href=" + link + @" target=""_blank"">" + subject + @"</a>
                    <p>The link will expire after 1 day.</p>
                    <p>Thank you !<br/>
                        HRM Management Software PhuongTrinh
                    </p>
                </body>
                </html>
                ";
        }

    }
}
