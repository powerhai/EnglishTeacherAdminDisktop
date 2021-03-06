using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fool.Models;
namespace Fool.Contracts
{
    public interface IPublisherService
    {
        Task<IEnumerable<Publisher>> GetPublishersAndBooks();
    }
}
