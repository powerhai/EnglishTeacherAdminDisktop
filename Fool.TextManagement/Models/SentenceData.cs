using Prism.Mvvm;
namespace Fool.TextManagement.Models
{
    public class SentenceData : BindableBase
    {
        private long mStart = 0;
        private long mEnd = 0;
        private string mChinese;
        private bool mIsBaiduAudio = false;
        private AudioStatus mAudioStatus;
        public string Sentence { get; set; }
        public string Chinese
        {
            get { return mChinese; }
            set
            {
                mChinese = value;
                RaisePropertyChanged();
            }
        }
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
 
        public bool IsBaiduAudio
        {
            get { return mIsBaiduAudio; }
            set
            {
                mIsBaiduAudio = value;
                RaisePropertyChanged();
            }
        }
        public string AudioFile { get; set; }
        public AudioStatus AudioStatus
        {
            get => mAudioStatus;
            set => SetProperty(ref mAudioStatus, value);
        }
    }

    public enum AudioStatus
    {
        HasAudio,
        NoneAudio,
        Modify,
        ReadyUpload,
        Uploaded
    }
}
