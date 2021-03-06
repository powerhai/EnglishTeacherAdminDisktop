using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Prism.Logging;
namespace EngManageDesktop
{
    public class HaiserLogger : ILoggerFacade
    {
        private readonly ILog mILogger = null;

        public HaiserLogger()
        {
            if (mILogger == null)
            {
                mILogger = LogManager.GetLogger("Logger");
                log4net.Config.XmlConfigurator.Configure();
            }
        }

        #region ILoggerFacade 成员

        public void Log(string message, Category category, Priority priority)
        {
            if (string.IsNullOrEmpty(message))
                return;

            switch (category)
            {
                case Category.Debug:
                    this.Debug(message);
                    break;
                case Category.Exception:
                    this.Error(message);
                    break;
                case Category.Info:
                    this.Info(message);
                    break;
                case Category.Warn:
                    this.Fatal(message);
                    break;
            }
        }

        #endregion


        void Debug(string message) { mILogger.Debug(message); }

        void Info(string message) { mILogger.Info(message); }

        void Error(string message) { mILogger.Error(message); }

        void Fatal(string message) { mILogger.Fatal(message); }
    }
}
