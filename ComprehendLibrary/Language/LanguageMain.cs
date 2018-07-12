using Amazon.Comprehend;
using Amazon.Comprehend.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ComprehendLibrary.Language
{
    internal class LanguageMain
    {
        private AmazonComprehendClient _Client { get; set; }
        public LanguageMain(AmazonComprehendClient Client)
        {
            _Client = Client;
        }
        public async Task<DetectDominantLanguageResponse> GetLanguage(string text)
        {
            var request = new DetectDominantLanguageRequest()
            {
                Text = text
            };
            var response = await _Client.DetectDominantLanguageAsync(request);
            return response;
        }
    }
}
