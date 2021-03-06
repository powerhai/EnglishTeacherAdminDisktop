using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fool.Contracts;
using RestSharp;
namespace Fool.Services
{
    public class AudioService : IAudioService 
    {
        public void UploadSentenceAudio(string sentence, string audioFile)
        {
            var client = new RestClient("http://localhost:5000");
            var request = new RestRequest("api/Sentence/UploadAudio", Method.POST);
            request.AddQueryParameter("sentence", "This is a pen");
            request.AddFile("audioFile", audioFile);
            var postdata = new {
                sentence = "This is a pen."
            };
            var json = request.JsonSerializer.Serialize(postdata);
            request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);

            var response = client.Execute(request);
            var content = response.Content; 
        }
        
    }
}
