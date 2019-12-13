using System;
using System.IO;
using Amazon;
using Amazon.Runtime;
using Amazon.CloudFront;

namespace CloudFrontLibrary
{
    public class CloudFrontMain
    {
        private readonly AmazonCloudFrontClient _Client;        
        public CloudFrontMain(BasicAWSCredentials credentials, string awsregion)
        {            
            RegionEndpoint region = RegionEndpoint.GetBySystemName(awsregion);            
            _Client = new AmazonCloudFrontClient(credentials, region);
            
        }
    }
}
