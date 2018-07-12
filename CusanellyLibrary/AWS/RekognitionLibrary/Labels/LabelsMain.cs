using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CusanellyLibrary.AWS.RekognitionLibrary.Labels
{

    class LabelsMain
    {
        private AmazonRekognitionClient _Client;
        public LabelsMain(AmazonRekognitionClient Client)
        {
            _Client = Client;
        }
        public async Task<DetectLabelsResponse> GetLabels(string image)
        {
            var imagebyte = Convert.FromBase64String(image);
            using (MemoryStream ms = new MemoryStream(imagebyte))
            {
                DetectLabelsRequest request = new DetectLabelsRequest
                {
                    Image = new Image()
                    {
                        Bytes = ms
                    }
                };
                var result = await GetImageLabels(request);
                return result;
            }
        }
        public async Task<DetectLabelsResponse> GetImageLabels(DetectLabelsRequest request)
        {
            DetectLabelsResponse response = await _Client.DetectLabelsAsync(request);
            if (response.HttpStatusCode.Equals(HttpStatusCode.OK) && response.Labels.Count > 0)
            {
                return response;
            }
            return null;
        }
        public async Task<DetectLabelsResponse> GetS3ImageLabels(DetectLabelsRequest request)
        {
            DetectLabelsResponse response = await _Client.DetectLabelsAsync(request);
            if (response.HttpStatusCode.Equals(HttpStatusCode.OK) && response.Labels.Count > 0)
            {
                return response;
            }
            return null;
        }
    }
}

