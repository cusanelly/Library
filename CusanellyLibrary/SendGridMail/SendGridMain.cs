#region Autor
/*
    Author: David Cusanelly
    Email: cuentacus@gmail.com
    Date: 20180401
    Nothing else but Navi...
*/
#endregion
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CusanellyLibrary.Mail
{
    public interface ISendGridMain
    {
        Task<Response> SendEmail(string from, string subject, string body, string htmlcontent = null, string to = null, List<string> CcEmails = null);
        Task<Response> SendEmail(SendGridMessage message);
    }
    public class SendGridMain : ISendGridMain
    {
        readonly string _ApiKey;
        readonly SendGridClient _Client;
        public SendGridMain(string key)
        {
            _ApiKey = key;
            _Client = new SendGridClient(_ApiKey);
        }
        public async Task<Response> SendEmail(string from, string subject,
            string body,
            string htmlcontent = "",
            string to = null,
            List<string> CcEmails = null)
        {
            //var from = new EmailAddress(from);
            var msg = MailHelper.CreateSingleEmail(new EmailAddress(from),
                new EmailAddress(to),
                subject,
                body, htmlcontent);
            var cc = new List<EmailAddress>();
            if (CcEmails.Count > 0)
            {
                CcEmails.ForEach(t => {
                    cc.Add(new EmailAddress(t));
                });
                msg.AddCcs(cc);
            }
            return await Send(msg);
        }
        public async Task<Response> SendEmail(SendGridMessage message)
        {
            return await Send(message);
        }
        private async Task<Response> Send(SendGridMessage message)
        {
            return await _Client.SendEmailAsync(message);
        }
    }
}
