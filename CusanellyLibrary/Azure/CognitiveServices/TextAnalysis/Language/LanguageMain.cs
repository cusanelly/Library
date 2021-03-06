﻿using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CusanellyLibrary.Azure.CognitiveServices.TextAnalysis.Language
{
    class LanguageMain
    {
        public LanguageMain()
        {

        }
        internal async Task<LanguageBatchResult> DetectLangCall(ITextAnalyticsAPI client, List<Input> list)
        {
            return await client.DetectLanguageAsync(
                new BatchInput(list));
        }
    }
}
