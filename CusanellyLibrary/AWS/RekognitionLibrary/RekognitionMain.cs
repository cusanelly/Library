using Amazon;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using CusanellyLibrary.AWS.RekognitionLibrary.Labels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CusanellyLibrary.AWS.RekognitionLibrary
{ 
    public class RekognitionMain
    {
        private AmazonRekognitionClient _Client;

        private string _AccessKey { get; set; }
        private string _AccessSecret { get; set; }
        private RegionEndpoint _Region { get; set; }

        public RekognitionMain()
        {

        }
        public RekognitionMain(string key, string secret, RegionEndpoint region)
        {
            _AccessKey = key;
            _AccessSecret = secret;
            _Region = region;
            _Client = new AmazonRekognitionClient(_AccessKey, _AccessSecret, _Region);
        }
        public List<Label> GetRekognition(string image)
        {
            var labels = new LabelsMain(_Client);
            var result = labels.GetLabels(image);
            return result.Result.Labels;
        }
    }
}

