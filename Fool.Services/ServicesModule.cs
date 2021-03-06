using Prism.Modularity;
using Prism.Regions;
using System;
using System.ComponentModel;
using Fool.Contracts; 
using Prism.Ioc;
using Prism.Logging;
using Unity;
namespace Fool.Services
{
    public class ServicesModule : IModule
    {
        private readonly IUnityContainer mContainer;
        IRegionManager mRegionManager;
        private readonly ILoggerFacade mLogger;
        public ServicesModule(IUnityContainer container,  IRegionManager regionManager, ILoggerFacade logger)
        {
            mContainer = container;
            mRegionManager = regionManager;
            mLogger = logger;
        }
         
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            mContainer.RegisterInstance(typeof(IConfigService), mContainer.Resolve<ConfigService>());
            mContainer.RegisterInstance(typeof(ILoginService), mContainer.Resolve<LoginService>());
            mContainer.RegisterInstance(typeof(IAudioService), mContainer.Resolve<AudioService>());
            mContainer.RegisterInstance(typeof(IPublisherService), mContainer.Resolve<PublisherService>());
            mContainer.RegisterInstance(typeof(ISentenceTranslateService), mContainer.Resolve<BaiduSentenceTranslateService>());
            mContainer.RegisterInstance(typeof(IText2AudioService), mContainer.Resolve<BaiduText2AudioService>());
            mContainer.RegisterInstance(typeof(BaiduTokenService), mContainer.Resolve<BaiduTokenService>());
            mContainer.RegisterInstance(typeof(ITextService), mContainer.Resolve<TextService>());
            mContainer.RegisterInstance(typeof(ISentenceService), mContainer.Resolve<SentenceService>());
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            mLogger.Log($"ServicesModule Module loaded", Category.Debug, Priority.High);

        }
    }
}