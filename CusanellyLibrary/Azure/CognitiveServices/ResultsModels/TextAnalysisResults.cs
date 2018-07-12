using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;

namespace CusanellyLibrary.Azure.CognitiveServices.ResultsModels
{
    public class TextAnalysisResults
    {
        public LanguageBatchResult LanguageResult { get; set; }
        public KeyPhraseBatchResult KeyPhraseResult { get; set; }
        public SentimentBatchResult SentimentResult { get; set; }
    }
}
