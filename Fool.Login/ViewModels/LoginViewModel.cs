using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Fool.Contracts;
using Fool.Domain;
using Prism.Events;
namespace Fool.Login.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private readonly ILoginService mLoginService;
        private readonly IEventAggregator mEventAggregator;
        public LoginViewModel(ILoginService loginService,IEventAggregator eventAggregator)
        {
            mLoginService = loginService;
            mEventAggregator = eventAggregator;
        }
        public ICommand LoginCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                   // mLoginService.Login();

                    var evt = mEventAggregator.GetEvent<LoginEvent>();
                    mEventAggregator.GetEvent<LoginEvent>().Publish(true);
                });
            }
        }
    }
}
