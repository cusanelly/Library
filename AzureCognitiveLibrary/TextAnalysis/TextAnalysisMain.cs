using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AzureCognitiveLibrary.TextAnalysis.Language;
using AzureCognitiveLibrary.TextAnalysis.KeyPhrase;
using AzureCognitiveLibrary.TextAnalysis.Sentiment;
using AzureCognitiveLibrary.ResultsModels;

namespace AzureCognitiveLibrary.TextAnalysis
{
    public class TextAnalysisMain
    {
        private string _TextKey { get; set; }
        private AzureRegions _Region { get; set; }
        private TextAnalyticsAPI _Client { get; set; }
        protected TextAnalysisResults Results { get; set; }
        public TextAnalysisMain(string textkey, AzureRegions region)
        {
            _TextKey = textkey;
            _Region = region;
            _Client = new TextAnalyticsAPI { SubscriptionKey = _TextKey, AzureRegion = _Region };
            Results = new TextAnalysisResults();
        }
        public TextAnalysisMain(TextAnalyticsAPI client)
        {
            _Client = client;
            Results = new TextAnalysisResults();
        }
        public async Task<TextAnalysisResults> TextAnalyticsCall(string text)
        {
            List<Input> list = BreakText(text);
            var language = new LanguageMain();
            var keyphrase = new KeyPhraseMain();
            var sentiment = new SentimentMain();
            try
            {
                Results.LanguageResult = await language.DetectLangCall(_Client, list);
                if (Results.LanguageResult.Documents.Count > 0)
                {
                    Results.KeyPhraseResult = await keyphrase.GetKeyPhraseCall(list, Results.LanguageResult.Documents, _Client);
                    Results.SentimentResult = await sentiment.GetSentimentCall(list, Results.LanguageResult.Documents, _Client);
                }
                return Results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<Input> BreakText(string text)
        {
            List<Input> result = new List<Input>();
            int count = 0;
            do
            {
                string texttemp = (text.Length < 5000) ? text : text.Substring(0, 5000);
                result.Add(new Input(count.ToString(), texttemp));
                text = text.Remove(0, texttemp.Length);
                count++;
            } while (text.Length > 0);
            return result;
        }
    }
}
