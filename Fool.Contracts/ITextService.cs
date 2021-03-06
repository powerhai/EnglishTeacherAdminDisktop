using System.Collections.Generic;
using Fool.Models;
namespace Fool.Contracts
{
    public interface ITextService
    {
        int CreateText(string book, string publisher, string title, string body);
        IEnumerable<LightTextA> GetTexts();
        Text GeText(int textId);
        bool RemoveText(int id);
        void UpdateText(int id, string title, string body, string publisher, string book);
    }
    public class LightTextA
    {
        public LightTextA()
        {
            
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public int BookId { get; set; }
     
    }
}