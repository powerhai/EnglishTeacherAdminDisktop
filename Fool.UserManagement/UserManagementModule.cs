using Prism.Modularity;
using Prism.Regions;
using System;
using Prism.Ioc;
namespace Fool.UserManagement
{
    public class UserManagementModule : IModule
    {
        IRegionManager _regionManager;

        public UserManagementModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }
        public void RegisterTypes(IContainerRegistry containerRegistry)
        { 
        }
        public void OnInitialized(IContainerProvider containerProvider)
        { 
        }
    }
}