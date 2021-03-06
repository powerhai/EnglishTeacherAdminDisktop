using System.Runtime.Serialization;
namespace Fool.Models
{
    [DataContract]
    public class Book
    {
        public Book()
        {
            
        }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public Publisher Publisher { get; set; }
    }
}