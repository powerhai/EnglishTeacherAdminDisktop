using Prism.Modularity;
using Prism.Regions;
using System;
using Prism.Ioc;
namespace Fool.WordManagement
{
    public class WordManagementModule : IModule
    { 

        public WordManagementModule(IRegionManager regionManager)
        { 
        }
 
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
             
        }
        public void OnInitialized(IContainerProvider containerProvider)
        { 
        }
    }
}