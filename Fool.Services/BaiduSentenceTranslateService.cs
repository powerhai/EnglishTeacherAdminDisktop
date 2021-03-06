using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using Fool.Contracts;
using RestSharp;
namespace Fool.Services
{
    public class BaiduSentenceTranslateService : ISentenceTranslateService
    {
        private int mNumber = 0;
        private bool mLoaded = false;
        private string mServerPath = "";
        private string mAppID = "";
        private string mAppKEY = "";
        public BaiduSentenceTranslateService()
        {
            Init();
        }
        void Init()
        {
            if (mLoaded)
                return;
            var cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            this.mServerPath = Convert.ToString(cfa.AppSettings.Settings["BaiduTranslationApiUri"].Value);
            this.mAppID = Convert.ToString(cfa.AppSettings.Settings["BaiduTranslationApiAppID"].Value);
            this.mAppKEY = Convert.ToString(cfa.AppSettings.Settings["BaiduTranslationApiKey"].Value);
            mLoaded = true;
        }

 
         
        public   string Translate(string sentence)
        {
            this.mNumber++;
            var client = new RestClient(mServerPath);
            var request = new RestRequest(Method.POST);
            var ro = new System.Random();
            var s = this.mNumber.ToString().PadLeft(6,'0');
            var sign = EncryptWithMD5(mAppID + sentence +  s + mAppKEY);

            request.AddParameter("q", sentence);
            request.AddParameter("from", "en");
            request.AddParameter("to", "zh");
            request.AddParameter("appid", mAppID);
            request.AddParameter("salt", s);
            request.AddParameter("sign", sign);
            var response =   client.Execute<ResData>(request);
            if (!response.IsSuccessful)
            {
                return "";
            }
            if(response.Data.trans_result == null)
            {
                return "";
            }
            return response.Data.trans_result[0]?.Dst;
        }
        private   string EncryptWithMD5(string source)
        {
            byte[] sor = Encoding.UTF8.GetBytes(source);
            MD5 md5 = MD5.Create();
            byte[] result = md5.ComputeHash(sor);
            StringBuilder strbul = new StringBuilder(40);
            for (int i = 0; i < result.Length; i++) {
                strbul.Append(result[i].ToString("x2"));//加密结果"x2"结果为32位,"x3"结果为48位,"x4"结果为64位

            }
            return strbul.ToString();
        }
        class ResData
        {
            public List<SrcDstData> trans_result { get; set; }
        }

        class SrcDstData
        {
            public string src { get; set; }
            public string Dst { get; set; }
        }
    }
}