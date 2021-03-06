using Prism.Modularity;
using Prism.Regions;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using Fool.Domain;
using Prism.Events;
using Prism.Ioc;
using Prism.Logging;
using Unity;
namespace Fool.Main
{
    public class MainModule : IModule
    {
        private readonly IEventAggregator mEventAggregator;
        private readonly ILoggerFacade mLogger;
        public MainModule(IEventAggregator eventAggregator, ILoggerFacade logger)
        {
            mEventAggregator = eventAggregator;
            mLogger = logger;
            Debug.WriteLine("event a : " + eventAggregator.GetHashCode());
        }
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.RegisterForNavigation<Views.WorkPanel>();
            //containerRegistry.RegisterForNavigation<Views.LoginView>();
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            mLogger.Log($"Main Module loaded", Category.Debug, Priority.High);
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionNames.MAIN, typeof(Views.LoginView));

            mEventAggregator.GetEvent<LoginEvent>().Subscribe((b) =>
            {
                mLogger.Log("Login Error A", Category.Debug, Priority.High);
                var view = containerProvider.Resolve<Views.WorkPanel>();
                var main = regionManager.Regions[RegionNames.MAIN];
                regionManager.RegisterViewWithRegion(RegionNames.MAIN, () => view);
                main.Activate(view);
            }, true);
        }
    }
}