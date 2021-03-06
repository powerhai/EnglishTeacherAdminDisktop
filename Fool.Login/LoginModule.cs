using Prism.Modularity;
using Prism.Regions;
using System;
using Fool.Domain;
namespace Fool.Login
{
    public class LoginModule : IModule
    {
        readonly IRegionManager mRegionManager;

        public LoginModule(IRegionManager regionManager)
        {
            mRegionManager = regionManager;
        }

        public void Initialize()
        {
            mRegionManager.RegisterViewWithRegion(RegionNames.MAIN, typeof(Views.LoginView));
        }
    }
}