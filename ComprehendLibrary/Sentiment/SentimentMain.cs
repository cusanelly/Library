using Amazon.Comprehend;
using Amazon.Comprehend.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ComprehendLibrary.Sentiment
{
    internal class SentimentMain
    {
        private AmazonComprehendClient _Client { get; set; }
        public SentimentMain(AmazonComprehendClient Client)
        {
            _Client = Client;
        }
        public async Task<DetectSentimentResponse> GetSentiment(string text, DominantLanguage language)
        {
            var request = new DetectSentimentRequest()
            {
                Text = text,
                LanguageCode = language.LanguageCode
            };
            var response = await _Client.DetectSentimentAsync(request);
            return response;
        }
    }
}
