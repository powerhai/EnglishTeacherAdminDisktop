using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Fool.Domain;
using Fool.TextManagement.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
namespace Fool.TextManagement.ViewModels
{
    public class MainButtonViewModel : BindableBase 
    {
        private readonly IRegionManager mRegionManager;
        public MainButtonViewModel(IRegionManager regionManager)
        {
            mRegionManager = regionManager;
        }
        public ICommand GoTextManageCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    mRegionManager.RequestNavigate(RegionNames.CONTENT, "TextManageView");
                });
            }
        }
    }
}
