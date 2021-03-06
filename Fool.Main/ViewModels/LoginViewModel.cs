using System;
using System.Windows;
using System.Windows.Input;
using Fool.Contracts;
using Fool.Domain;
using Prism.Commands;
using Prism.Events;
using Prism.Logging;
using Prism.Mvvm;
using Prism.Regions;
using Unity;
namespace Fool.Main.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private readonly ILoginService mLoginService;
        private readonly IEventAggregator mEventAggregator;
        private readonly IUnityContainer mContainer;
        private readonly IRegionManager mRegionManager;
        private readonly ILoggerFacade mLogger;
        public LoginViewModel(ILoginService loginService, IEventAggregator eventAggregator, IUnityContainer container,
            IRegionManager regionManager, ILoggerFacade logger)
        {
            mLoginService = loginService;
            mEventAggregator = eventAggregator;
            mContainer = container;
            mRegionManager = regionManager;
            mLogger = logger;
        }
        public ICommand LoginCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var evt = mEventAggregator.GetEvent<LoginEvent>();
                    // mLoginService.Login();
                    evt.Publish(true);
                });
            }
        }
    }
}