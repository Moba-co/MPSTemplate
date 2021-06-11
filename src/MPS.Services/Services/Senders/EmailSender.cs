using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moba.Common.DTOs;
using Moba.Common.ViewModels.Base;
using Moba.Services.Interfaces.Senders;

namespace Moba.Services.Services.Senders
{
    public class EmailSender :  ISender
    {
        private readonly IOptionsSnapshot<SiteSettings> _emailSettings;
        private readonly ILogger _logger;
        public EmailSender(IOptionsSnapshot<SiteSettings> emailSettings, ILogger logger)
        {
            _emailSettings = emailSettings;
            _logger = logger;
        }
        
        public async Task<ReturnMessageDto> SendManyAsync(IEnumerable<string> to, string subject, string message, bool isMessageHtml = false)
        {
            try
            {
                await SendEmail(to, subject, message, isMessageHtml).ConfigureAwait(false);
                return new ReturnMessageDto("Done",true,200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ReturnMessageDto(ex.Message,false,200);
            }
        }

        public async Task<ReturnMessageDto> SendAsync(string to, string subject, string message, bool isMessageHtml = false)
        {
            try
            {
                await SendEmail(new List<string>(){to}, subject, message, isMessageHtml);
                return new ReturnMessageDto("Done",true,200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new ReturnMessageDto(ex.Message,false,200);
            }
        }

        private async Task<bool> SendEmail(IEnumerable<string> to, string subject, string message, bool isMessageHtml = false)
        {
            try
            {
                using var client = new SmtpClient();
                var email = _emailSettings.Value.EmailSetting.Email;
                var username = _emailSettings.Value.EmailSetting.Username;
                var passWord = _emailSettings.Value.EmailSetting.Password;
                var host = _emailSettings.Value.EmailSetting.Host;
                var port = _emailSettings.Value.EmailSetting.Port;

                var credentials = new NetworkCredential()
                {
                    UserName = username, // without @gmail.com
                    Password = passWord
                };
                client.UseDefaultCredentials = true;
                client.Credentials = credentials;
                client.Host = host;
                client.Port = port;
                client.EnableSsl = true;
                using var emailMessage = new MailMessage()
                {
                    From = new MailAddress(email), // with @gmail.com
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = isMessageHtml
                };
                foreach (var items in to)
                {
                    emailMessage.To.Add(items);
                }
                await client.SendMailAsync(emailMessage).ConfigureAwait(false);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}