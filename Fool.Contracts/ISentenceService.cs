using System.Collections.Generic;
using System.Threading.Tasks;
using Fool.Comm.Models;
using Fool.Models;
namespace Fool.Contracts
{
    public interface ISentenceService
    {
        IEnumerable<Sentence> GetSentencesOfText(int textId);
        Task<bool> CreateSentence(NewSentenceModel md);
        string GetSentenceAudioFile(string sentence);
    }
}