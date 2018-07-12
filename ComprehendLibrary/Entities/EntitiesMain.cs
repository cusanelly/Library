using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon.Comprehend;
using Amazon.Comprehend.Model;

namespace ComprehendLibrary.Entities
{
    internal class EntitiesMain
    {
        private AmazonComprehendClient _Client { get; set; }
        public EntitiesMain(AmazonComprehendClient Client)
        {
            _Client = Client;
        }
        public async Task<DetectEntitiesResponse> GetEntities(string text, DominantLanguage language) {
            var request = new DetectEntitiesRequest() {
                Text = text,
                LanguageCode = language.LanguageCode
            };
            var response = await _Client.DetectEntitiesAsync(request);            
            return response;
        }
    }
}
