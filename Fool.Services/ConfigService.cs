using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Fool.Contracts;
namespace Fool.Services
{
    public class ConfigService : IConfigService
    {
        private string mServerPath = "";
        public ConfigService()
        {
            Init();
        }

        private bool mLoaded = false;
        void Init()
        {
            if (mLoaded)
                return;
            var cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            this.mServerPath = Convert.ToString(cfa.AppSettings.Settings["ServerPath"].Value);
            mLoaded = true;
        }
        public string GetServerPath()
        {
            return mServerPath;
        }
    }
}


