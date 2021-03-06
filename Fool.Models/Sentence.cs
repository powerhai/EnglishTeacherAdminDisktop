namespace Fool.Models
{
 
    public class Sentence
    {
        public Sentence()
        {
            
        }
        public int Id { get; set; }
        public string Eng { get; set; }
        public string Chn { get; set; } 
        public string FileName { get; set; }
        public bool HasAudio { get; set; }
        
    }
}