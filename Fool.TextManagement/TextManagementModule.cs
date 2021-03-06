using Prism.Modularity;
using Prism.Regions;
using System;
using System.Diagnostics;
using Fool.Domain;
using Fool.TextManagement.Models;
using Fool.TextManagement.Views;
using Prism.Events;
using Prism.Ioc;
using Prism.Logging;
namespace Fool.TextManagement
{
    public class TextManagementModule : IModule
    {
        private readonly ILoggerFacade mLogger;
        private readonly IEventAggregator mEventAggregator;
        private readonly IRegionManager mRegionManager;
        public TextManagementModule(ILoggerFacade logger, IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            mLogger = logger;
            mEventAggregator = eventAggregator;
            mRegionManager = regionManager;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<TextManageView>();
            containerRegistry.RegisterDialog<TextEditView>();
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            mLogger.Log($"TextManagementModule Module loaded", Category.Debug, Priority.High);
            ObjectRemap.Init();
            var region = containerProvider.Resolve<IRegionManager>();
            region.RegisterViewWithRegion(RegionNames.MAIN_BUTTONS, typeof(MainButton));
            // var evv = containerProvider.Resolve<IEventAggregator>();
            mEventAggregator.GetEvent<LoginEvent>().Subscribe(Login, true);
        }
        private void Login(bool obj)
        {
            mRegionManager.RequestNavigate(RegionNames.CONTENT, "TextManageView");
        }
    }
}