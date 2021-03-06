using Prism.Modularity;
using Prism.Regions;
using System;
using Fool.AudioManagement.Views;
using Fool.Domain;
using Prism.Ioc;
namespace Fool.AudioManagement
{
    public class AudioManagementModule : IModule
    { 

        public AudioManagementModule( )
        { 
        }
 
        public void RegisterTypes(IContainerRegistry containerRegistry)
        { 
           containerRegistry.RegisterForNavigation<SentenceAudioManageView>();
           containerRegistry.RegisterForNavigation<SentenceAudioEditView>();
        }
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var region = containerProvider.Resolve<IRegionViewRegistry>(); 
            region.RegisterViewWithRegion(RegionNames.MAIN_BUTTONS, typeof(MainButtonsView)); 
        }
    }
}