using Amazon;
using Amazon.Rekognition.Model;
using System;
using System.Net;
using System.Threading.Tasks;
using Amazon.Rekognition;
using RekognitionLibrary.Labels;
using System.Collections.Generic;

namespace RekognitionLibrary
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
