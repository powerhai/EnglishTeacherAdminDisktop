using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Fool.Comm.Models;
using Fool.Contracts;
using Fool.Models;
using RestSharp;
namespace Fool.Services
{
    public class SentenceService : ISentenceService
    {
        private readonly IConfigService mConfigService;
        public SentenceService(IConfigService configService)
        {
            mConfigService = configService;
        }
        public IEnumerable<Sentence> GetSentencesOfText(int textId)
        {
            var client = new RestClient(mConfigService.GetServerPath());
            var request = new RestRequest("api/sentence/GetSentencesOfText", Method.GET);
            request.AddParameter("textId", textId);
            var response = client.Execute<List<Sentence>>(request);
            return response.Data; 
        }
        public async Task<bool> CreateSentence(NewSentenceModel sentence)
        {
            var client = new RestClient(mConfigService.GetServerPath());
            var request = new RestRequest("api/sentence/AddOrUpdateSentence", Method.POST);
            request.AlwaysMultipartFormData = true;
            if(!string.IsNullOrEmpty( sentence.AudioFile))
            {
                request.AddFile("Audio", sentence.AudioFile);
            }

            request.AddParameter("Eng", sentence.Eng);
            request.AddParameter("Chn",sentence.Chn);
            request.AddParameter("TextId", sentence.TextID);

            request.AddParameter("AudioFileName", "abc.mp3");
            var rvbool = await Task.Run(() =>
            {
                var rv = client.Execute(request);
                return rv.IsSuccessful && rv.StatusCode == HttpStatusCode.OK ;
            });
            return rvbool;
        }

        public string GetSentenceAudioFile(string sentence)
        {
            var client = new RestClient(mConfigService.GetServerPath());
            var request = new RestRequest("api/sentence/ReadAudio", Method.POST);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("sentence", sentence);
            var ds = client.DownloadData(request);
            string tempFile = $"avi_{DateTime.Now.ToFileTime()}.avi";
            using(var writer = File.OpenWrite(tempFile))
            {
                writer.Write(ds,0, ds.Length);
            }


            return tempFile;
        }
    }

}