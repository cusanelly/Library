using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using System;
using System.Net;
using System.Threading.Tasks;

namespace CognitoLibrary
{
    public class CognitoMain
    {
        private string _AccessKey { get; set; }
        private string _AccessSecret { get; set; }
        private RegionEndpoint _Region { get; set; }
        private AmazonCognitoIdentityProviderClient _Client { get; set; }  
        

        public CognitoMain()
        {

        }
        public CognitoMain(AmazonCognitoIdentityProviderClient client)
        {
            _Client = client;
        }
        public CognitoMain(string accesskey, string accesssecret, string region)
        {
            _AccessKey = accesskey;
            _AccessSecret = accesssecret;
            _Region = RegionEndpoint.GetBySystemName(region);
            _Client = new AmazonCognitoIdentityProviderClient(_AccessKey, _AccessSecret, _Region);
        }

        public async Task<SignUpResponse> SignUp(string username, string password)
        {
            var request = new SignUpRequest
            {
                Username = username,
                Password = password
            };
            var response = await _Client.SignUpAsync(request);
            return (response.HttpStatusCode.Equals(HttpStatusCode.OK)) ? response : null;
        }
        public async Task SignIn(CognitoUser user, string password) {
            
        }
    }
}
