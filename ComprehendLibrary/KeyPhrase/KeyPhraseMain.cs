using Amazon.Comprehend;
using Amazon.Comprehend.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ComprehendLibrary.KeyPhrase
{
    internal class KeyPhraseMain
    {
        private AmazonComprehendClient _Client { get; set; }
        public KeyPhraseMain(AmazonComprehendClient Client)
        {
            _Client = Client;
        }
        public async Task<DetectKeyPhrasesResponse> GetKeyPhrase(string text, DominantLanguage language)
        {
            var request = new DetectKeyPhrasesRequest()
            {
                Text = text,
                LanguageCode = language.LanguageCode
            };
            var response = await _Client.DetectKeyPhrasesAsync(request);
            return response;
        }
    }
}
