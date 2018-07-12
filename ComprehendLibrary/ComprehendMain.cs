using Amazon;
using System;
using Amazon.Comprehend;
using Amazon.Comprehend.Model;
using ComprehendLibrary.Entities;
using ComprehendLibrary.KeyPhrase;
using ComprehendLibrary.Language;
using ComprehendLibrary.Sentiment;
using System.Threading.Tasks;

namespace ComprehendLibrary
{
    public class ComprehendMain
    {
        private string _AccessKey { get; set; }
        private string _AccessSecret { get; set; }
        private RegionEndpoint _Region { get; set; }
        private AmazonComprehendClient _Client { get; set; }
        public ComprehendMain()
        {

        }
        public ComprehendMain(AmazonComprehendClient Client) {
            _Client = Client;
        }
        public ComprehendMain(string key, string secret, RegionEndpoint region)
        {
            _AccessKey = key;
            _AccessSecret = secret;
            _Region = region;
            _Client = new AmazonComprehendClient(_AccessKey, _AccessSecret, _Region);
        }
        public async Task<DetectEntitiesResponse> GetEntities(string text, DominantLanguage language) {
            EntitiesMain entities = new EntitiesMain(_Client);
            return await entities.GetEntities(text, language);
        }
        public async Task<DetectKeyPhrasesResponse> GetKeyPhrase(string text, DominantLanguage language)
        {
            KeyPhraseMain keyphrase = new KeyPhraseMain(_Client);
            return await keyphrase.GetKeyPhrase(text, language);
        }
        public DetectDominantLanguageResponse GetLanguage(string text)
        {
            LanguageMain language = new LanguageMain(_Client);
            return language.GetLanguage(text).Result;
        }
        public async Task<DetectSentimentResponse> GetSentiment(string text, DominantLanguage language)
        {
            SentimentMain keyphrase = new SentimentMain(_Client);
            return await keyphrase.GetSentiment(text, language);
        }
    }
}
