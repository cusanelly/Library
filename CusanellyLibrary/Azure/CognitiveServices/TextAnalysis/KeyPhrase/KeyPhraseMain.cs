using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CusanellyLibrary.Azure.CognitiveServices.TextAnalysis.KeyPhrase
{
    class KeyPhraseMain
    {
        public KeyPhraseMain()
        {

        }
        internal async Task<KeyPhraseBatchResult> GetKeyPhraseCall(List<Input> list,
            IList<LanguageBatchResultItem> documents,
            ITextAnalyticsAPI client)
        {
            string textid = "";
            string textlang = "";
            string textval = "";
            List<MultiLanguageInput> keyphraselist = new List<MultiLanguageInput>();
            for (int i = 0; i < documents.Count; i++)
            {
                textid = documents[i].Id;
                textlang = documents[i].DetectedLanguages[0].Iso6391Name;
                textval = list.Where(t => t.Id == i.ToString()).FirstOrDefault().Text;
                keyphraselist.Add(new MultiLanguageInput() { Id = textid, Text = textval, Language = textlang });
            }
            return await KeyPhraseCall(client, keyphraselist);
        }
        internal async Task<KeyPhraseBatchResult> KeyPhraseCall(ITextAnalyticsAPI client, List<MultiLanguageInput> list)
        {
            return await client.KeyPhrasesAsync(
                new MultiLanguageBatchInput(list));
        }
    }
}
