using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fool.Contracts;
using Fool.Domain;
using Prism.Events;
namespace Fool.Services
{
    public class LoginService : ILoginService 
    {
        private readonly IEventAggregator mEventAggregator;
        public LoginService(IEventAggregator eventAggregator)
        {
            mEventAggregator = eventAggregator;
            Console.WriteLine(mEventAggregator.GetHashCode());
        }
        public void Login()
        {
            var evt = mEventAggregator.GetEvent<LoginEvent>();
            mEventAggregator.GetEvent<LoginEvent>().Publish(true);
        }
        
    }
}
