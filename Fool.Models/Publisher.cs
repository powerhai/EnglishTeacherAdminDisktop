using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Fool.Models
{
    [DataContract]
    public class Publisher
    {
        public Publisher()
        {
            
        }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public ICollection<Book> Books { get; set; }
    }
}
