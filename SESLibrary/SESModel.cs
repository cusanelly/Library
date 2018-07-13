using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SESLibrary
{
    public interface ISESModel
    {
        Task<bool> CreateTemplate(string templatename, string subject, string body, string html);
        Task<List<TemplateMetadata>> ListTemplates();
        Task<Template> GetTemplate(string templatename);
        Task<bool> DeleteTemplate(string templatename);
        Task SendEmail(string from, string to, string subject, string body, List<string> cc = null);
    }
    public class SESModel : ISESModel, IDisposable
    {
        private readonly AmazonSimpleEmailServiceClient _Client;
        private CreateTemplateRequest _templaterequest;
        private CreateTemplateResponse _templateresponse;
        private ListTemplatesRequest _listtemprequest;
        private ListTemplatesResponse _listtempresponse;
        private GetTemplateRequest _gettemprequest;
        private GetTemplateResponse _gettempresponse;
        private DeleteTemplateRequest _deletetemprequest;
        private DeleteTemplateResponse _deletetempresponse;       
        private SendTemplatedEmailRequest _sendtemplaterequest;
        private SendTemplatedEmailResponse _sendtemplateresponse;

        public SESModel(string ClientKey, string ClientSecret, string Region)
        {
            RegionEndpoint region = RegionEndpoint.GetBySystemName(Region);
            _Client = new AmazonSimpleEmailServiceClient(ClientKey, ClientSecret, region);
        }

        public async Task<ListIdentitiesResponse> ListEmails() {
            var request = new ListIdentitiesRequest { IdentityType = IdentityType.EmailAddress };
            return await _Client.ListIdentitiesAsync(request);
        }
        public async Task AddEmail(string email) {
            VerifyEmailIdentityRequest request = new VerifyEmailIdentityRequest {
                EmailAddress = email
            };
            ListIdentitiesRequest req = new ListIdentitiesRequest{
                IdentityType = IdentityType.EmailAddress                
            };
            var res = await _Client.ListIdentitiesAsync(req);
            
            var response = await _Client.VerifyEmailIdentityAsync(request);           
        }
        public async Task<bool> CreateTemplate(string templatename, string subject, string body, string html)
        {
            Template template = new Template()
            {
                TemplateName = templatename,
                SubjectPart = subject,
                TextPart = body,
                HtmlPart = html
            };
            _templaterequest = new CreateTemplateRequest
            {
                Template = template
            };
            _templateresponse = await _Client.CreateTemplateAsync(_templaterequest);
            if (_templateresponse.HttpStatusCode.ToString() == "OK")
            {
                return true;
            }
            return false;
        }

        public async Task<List<TemplateMetadata>> ListTemplates()
        {
            List<TemplateMetadata> list = new List<TemplateMetadata>();
            _listtemprequest = new ListTemplatesRequest();
            try
            {
                _listtempresponse = await _Client.ListTemplatesAsync(_listtemprequest);
                if (_listtempresponse.HttpStatusCode.ToString() == "OK")
                {
                    list = _listtempresponse.TemplatesMetadata;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Data: {ex.Data}");
                Console.WriteLine($"Source: {ex.Source}");
                Console.WriteLine($"Message: {ex.Message}");
            }
            return list;
        }

        public async Task<Template> GetTemplate(string templatename)
        {
            Template template = new Template();
            _gettemprequest = new GetTemplateRequest() { TemplateName = templatename };
            _gettempresponse = await _Client.GetTemplateAsync(_gettemprequest);
            if (_gettempresponse.HttpStatusCode.ToString() == "OK" && _gettempresponse.Template != null)
            {
                template = _gettempresponse.Template;
            }
            return template;
        }

        public void Dispose()
        {
            _Client.Dispose();
            //throw new NotImplementedException();
        }

        public async Task<bool> DeleteTemplate(string templatename)
        {
            _deletetemprequest = new DeleteTemplateRequest()
            {
                TemplateName = templatename
            };
            _deletetempresponse = await _Client.DeleteTemplateAsync(_deletetemprequest);
            if (_deletetempresponse.HttpStatusCode.ToString() == "OK")
            {
                return true;
            }
            return false;
            //throw new NotImplementedException();
        }     

        public async Task SendEmail(string from, 
            string to, 
            string subject, 
            string body, 
            List<string> cc = null)
        {
            var request = new SendEmailRequest() {
                Source = from,                
                Message = new Message {
                    Subject = new Content(subject),
                    Body = new Body(new Content(body))
                }
            };
            var destination = new Destination { ToAddresses = new List<string> { to } };
            if (cc.Count > 0)
            {
                destination.CcAddresses = cc;
            }
            request.Destination = destination;
            await _Client.SendEmailAsync(request);            
        }
        public async Task<SendTemplatedEmailResponse> SendTemplate(string templatename, 
            string name, 
            string url, 
            List<string> to,
            string arn,
            string sourceemail)
        {
            _sendtemplaterequest = new SendTemplatedEmailRequest()
            {
                SourceArn = arn,
                Source = sourceemail,
                Template = templatename,
                TemplateData = $"{{ \"username\":\"{name}\", \"urlinfo\": \"{url}\" }}",
                Destination = new Destination
                {
                    ToAddresses = to
                }
            };            
            try
            {
                _sendtemplateresponse = await _Client.SendTemplatedEmailAsync(_sendtemplaterequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Data: {ex.Data}");
                Console.WriteLine($"Source: {ex.Source}");
                Console.WriteLine($"Message: {ex.Message}");
            }

            return _sendtemplateresponse;
        }
    }    
}
