using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fool.Contracts;
using Fool.Models;
using RestSharp;
namespace Fool.Services
{
    public class TextService : ITextService 
    {
        private readonly IConfigService mConfigService;
        public TextService(IConfigService configService)
        {
            mConfigService = configService;
        }
        public int CreateText(string book, string publisher, string title, string body )
        {
            var client = new RestClient(mConfigService.GetServerPath());

            var request = new RestRequest("api/text/CreateNewText", Method.POST);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("title", title);
            request.AddParameter("Publisher", publisher);
            request.AddParameter("book", book );
            request.AddParameter("body", body );  
            
            var response = client.Execute<int>(request);
            var content = response.Data;
            return content;
        }
        public IEnumerable<LightTextA> GetTexts()
        {
            var client = new RestClient(mConfigService.GetServerPath());
            var request = new RestRequest("api/text//GetAllTexts", Method.GET);
            var content = client.Execute<List<LightTextA>>(request);
            return content.Data;
        }

        public Text GeText(int textId)
        {
            var client = new RestClient(mConfigService.GetServerPath());
            var request = new RestRequest("api/text/GetText", Method.GET);
            request.AddParameter("Id", textId);
            var content = client.Execute<Text>(request);
            return content.Data;
        }
        public bool RemoveText(int id)
        {
            var client = new RestClient(mConfigService.GetServerPath());
            var request = new RestRequest("api/text/RemoveText", Method.POST);

            request.AlwaysMultipartFormData = true;
            request.AddParameter("Id", id);
            var content = client.Execute<bool>(request);
            return content.Data;
        }

        public void UpdateText(int id, string title, string body, string publisher, string book)
        {
            //var client = new RestClient("http://localhost:8888/api/text/UpdateText");
            var client = new RestClient(mConfigService.GetServerPath());

            var request = new RestRequest("api/text/UpdateText",Method.POST);

            request.AlwaysMultipartFormData = true;

            request.AddParameter("Id", id.ToString());
            request.AddParameter("Title", title);
            request.AddParameter("Text", body);
            request.AddParameter("Publisher", publisher);
            request.AddParameter("Book", book);
            var content = client.Execute(request);
            
        }
    }
}
