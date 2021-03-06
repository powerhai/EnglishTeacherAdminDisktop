using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fool.Contracts;
using Fool.Models;
using RestSharp;
using RestSharp.Deserializers;
namespace Fool.Services
{
    class PublisherService : IPublisherService 
    {
        private readonly IConfigService mConfigService;
        public PublisherService(IConfigService configService)
        {
            mConfigService = configService;
        }
        public async Task<IEnumerable<Publisher>> GetPublishersAndBooks()
        { 
            var client = new RestClient(mConfigService.GetServerPath());
            var request = new RestRequest("api/publisher/GetPublishersAndBooks", Method.GET); 
            var response = await client.ExecuteAsync(request);
            var content = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Publisher>>(response.Content);
            return content;
        }
    }
}
