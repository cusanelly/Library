using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AzureCognitiveLibrary.ComputerVision
{
    public class ComputerVisionMain
    {
        private string _VisionKey { get; set; }
        private AzureRegions _Region { get; set; }
        private ComputerVisionAPI _Client { get; set; }
        public ComputerVisionMain(string key, AzureRegions region)
        {
            _VisionKey = key;
            _Region = region;
            _Client = new ComputerVisionAPI(new ApiKeyServiceClientCredentials(_VisionKey));
            _Client.AzureRegion = _Region;
        }
        public async Task<ImageAnalysis> ComputerVisionUrlCall(string imageurl) {
            return await _Client.AnalyzeImageAsync(imageurl);            
        }
        public async Task<string> ComputerVisionBase64Call(byte[] image, string endpoint) {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _VisionKey);
            var parameters = "visualFeatures=Categories,Description,Tags,Faces,Adult,Color&details=Celebrities,Landmarks&language=en";
            using (ByteArrayContent content = new ByteArrayContent(image))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                var url = $"{endpoint}?{parameters}";
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
        }
    }
}
