using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
namespace Fool.AudioManagement.Models
{
    public class SentenceData : BindableBase
    {
        private long mStart = 0;
        private long mEnd = 0;
        public string Sentence { get; set; }
        public long Start
        {
            get { return mStart; }
            set
            {
                mStart = value; 
                RaisePropertyChanged();
            }
        }
        public long End
        {
            get { return mEnd; }
            set
            {
                mEnd = value; 
                RaisePropertyChanged();
            }
        }
        public string AudioFile { get; set; }

    }
}
