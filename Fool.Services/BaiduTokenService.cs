using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
namespace Fool.Services
{
    public class BaiduTokenService 
    {
        private const string API_URI = "https://openapi.baidu.com/oauth/2.0/token";
        private const string CLIENT_ID = "QUvZVAgyj6VpZtrypGFvW800";
        private const string CLIENT_SECRET = "jrvTbtO1CEtQs3ji0xdCVxa6mYVxMPY8";
        private string mToken = "";
        public string GetToken()
        {
            if(!string.IsNullOrEmpty(mToken))
                return mToken;
            try
            {
                var client = new RestClient(API_URI);
                var request = new RestRequest(Method.POST);
                request.AddParameter("grant_type", "client_credentials");
                request.AddParameter("client_id", CLIENT_ID);
                request.AddParameter("client_secret", CLIENT_SECRET);
                var rs = client.Execute<rsdata>(request);
                this.mToken = rs.Data.access_token;
                return rs.Data.access_token;
            }
            catch(Exception e)
            { 
                throw e;
            }
#pragma warning disable CS0162 // Unreachable code detected
            return mToken;
#pragma warning restore CS0162 // Unreachable code detected
        }
        class rsdata
        {
            public string access_token { get; set; }
        }
    }
}
