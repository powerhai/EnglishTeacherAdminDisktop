using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Fool.AudioManagement.Views;
using Fool.Domain;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
namespace Fool.AudioManagement.ViewModels
{
    public class MainButtonsViewModel : BindableBase
    {
        private readonly IRegionManager mRegionManager;
        public MainButtonsViewModel(IRegionManager regionManager)
        {
            mRegionManager = regionManager;
        }
        public ICommand GoSentenceAudioMgmtCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    mRegionManager.RequestNavigate(RegionNames.CONTENT, typeof(SentenceAudioManageView).FullName);
                });
            }
        }

    }
}
