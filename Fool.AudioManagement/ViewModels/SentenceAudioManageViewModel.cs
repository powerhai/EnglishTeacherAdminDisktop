using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Fool.AudioManagement.Views;
using Fool.Domain;
using Prism.Regions;
namespace Fool.AudioManagement.ViewModels
{
    public class SentenceAudioManageViewModel : BindableBase
    {
        private readonly IRegionManager mRegionManager;
        public SentenceAudioManageViewModel(IRegionManager regionManager)
        {
            mRegionManager = regionManager;
        }
        public ICommand GoAudioEditCommand
        {
            get
            {
                return new DelegateCommand(() =>
                  {
                      mRegionManager.RequestNavigate(RegionNames.CONTENT, typeof(SentenceAudioEditView).FullName);
                  });
            }
        }
    }
}
