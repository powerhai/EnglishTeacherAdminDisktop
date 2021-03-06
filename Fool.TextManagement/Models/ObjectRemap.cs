using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Fool.Models;
namespace Fool.TextManagement.Models
{
    public static class ObjectRemap
    {
        public static void Init()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<Publisher, PublisherVm>();
                cfg.CreateMap<Book, BookVm>();

            });
        }

    }
}
