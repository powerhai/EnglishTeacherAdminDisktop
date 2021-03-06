using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fool.Comm.Models
{
    public class NewSentenceModel
    {
        public int TextID { get; set; }
        public string Eng { get; set; }
        public string Chn { get; set; }
        public string AudioFile { get; set; }
    }
}
