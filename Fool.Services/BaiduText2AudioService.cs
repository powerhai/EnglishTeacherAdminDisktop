using System;
using System.IO;
using Fool.Contracts;
using RestSharp;
namespace Fool.Services
{
    public class BaiduText2AudioService : IText2AudioService
    {
        private readonly BaiduTokenService mBaiduTokenService; 
        private const string APP_ID = "20180527000167467";
        private const string KEY = "1cuzoRdGSExIdAWbecTd";
        private const string API_URI = "http://tsn.baidu.com/text2audio";
        public BaiduText2AudioService(BaiduTokenService baiduTokenService)
        {
            mBaiduTokenService = baiduTokenService;
        }
        
        public string GetAudioFile(string text)
        {
            var taken = mBaiduTokenService.GetToken();
            var client = new RestClient(API_URI);
            var request = new RestRequest(Method.POST);
            request.AddParameter("tex", text);
            request.AddParameter("tok", taken);
            request.AddParameter("cuid", "Haiser");
            request.AddParameter("ctp", "1");
            request.AddParameter("lan", "zh");
            try
            {
                var bytes = client.DownloadData(request);
                var file = System.IO.Path.GetTempFileName() + ".mp3";
                var fs = System.IO.File.Create(file);
                fs.Write(bytes,0, bytes.Length);
                fs.Close();
                return file;
            }
            catch(Exception)
            {
                return "";
            }

#pragma warning disable CS0162 // Unreachable code detected
            return "";
#pragma warning restore CS0162 // Unreachable code detected
        }
    }
}