using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Fool.AudioManagement.Models;
using Fool.Contracts;
using Microsoft.Win32;
using NAudio.Wave;
namespace Fool.AudioManagement.ViewModels
{
    public class SentenceAudioEditViewModel : BindableBase
    {
        private readonly IAudioService mAudioService;
        private WaveOutEvent mOutputDevice;
        private AudioFileReader mAudioReader;
        
        private DispatcherTimer mTimer = new DispatcherTimer();
        private bool mIsManual = false;

        private void Callback(object sender, EventArgs eventArgs)
        {
            if (mIsManual)
                return;
             
            if (this.mAudioReader != null && this.mOutputDevice?.PlaybackState == PlaybackState.Playing) {
                Current = this.mAudioReader.CurrentTime.TotalSeconds;
            }
        }
        public CollectionViewSource SentenceViewSource
        {
            get
            {
                if (mSentenceViewSource == null)
                    mSentenceViewSource = new CollectionViewSource() { Source = this.Sentences };
                return mSentenceViewSource;
            }
        }
        public ObservableCollection<SentenceData> Sentences { get; } = new ObservableCollection<SentenceData>();
        public void SetIsManual(bool isManual)
        {
            this.mIsManual = isManual;
        }
        public long Length
        {
            get { return mLength; }
            set
            {
                mLength = value;
                RaisePropertyChanged();
            }
        }
        public double Current
        {
            get { return mCurrent; }
            set
            {
                mCurrent = value;
                RaisePropertyChanged();
            }
        }
        public string Text
        {
            get { return mText; }
            set
            {
                mText = value;
                RaisePropertyChanged();
                OnPropertyChanged("CanAnalyse");
            }
        }
        private string mAudioFile;
        private bool mCanChooseAudioFile = true;
        private bool mCanPlay = false;
        private long mLength;
        private double mCurrent;
        private string mText;
        private CollectionViewSource mSentenceViewSource;
        private bool mCanSave;
        private bool mIsBusy;
        public string AudioFile
        {
            get { return mAudioFile; }
            set
            {
                mAudioFile = value;
                RaisePropertyChanged();
                RaisePropertyChanged("CanPlay");
            }
        }
        public bool CanChooseAudioFile
        {
            get { return mCanChooseAudioFile; }
            set
            {
                mCanChooseAudioFile = value;
                RaisePropertyChanged();
            }
        }
        public bool CanPause
        {
            get
            {
                return this.mOutputDevice?.PlaybackState == PlaybackState.Playing;
            }
        }
        public bool CanPlay
        {
            get
            {
                return !string.IsNullOrEmpty(this.AudioFile)  ;

            }

        }
        public bool CanAnalyse
        {
            get { return !string.IsNullOrEmpty(this.Text); }
        }
        public SentenceAudioEditViewModel(IAudioService audioService)
        {
            mAudioService = audioService;
            mTimer.Tick += new EventHandler(Callback);
            mTimer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            mTimer.Start();
        }
 
        public ICommand ChooseAudioFileCommand
        {
            get
            {
                return new DelegateCommand(ChooseAudioFile).ObservesCanExecute(() => CanChooseAudioFile);
            }
        }
        private void ChooseAudioFile()
        {
            CanChooseAudioFile = false;
            var dialog = new OpenFileDialog() {
                DefaultExt = ".mp3",
                Filter = "mp3 |*.mp3"
            };
            var rv = dialog.ShowDialog();
            if (rv != true)
                return;
            this.AudioFile = dialog.FileName;

        }
        private void Play()
        { 
            if (mOutputDevice == null) {
                mOutputDevice = new WaveOutEvent(); 
            }

            if (mAudioReader == null) {
                mAudioReader = new AudioFileReader(this.AudioFile); 
                mOutputDevice.Init(mAudioReader);
                Length = (long)mAudioReader.TotalTime.TotalSeconds;
            }
           
            mOutputDevice.Play();
            
            RaisePropertyChanged("CanPlay");
            RaisePropertyChanged("CanPause");
            RaisePropertyChanged("CanSave");
            InitRange();
        }
        private void Pause()
        {
            this.mOutputDevice.Pause();
        }
        private void AnalyseText()
        {
            Sentences.Clear();
            var reg = new Regex("[^.?!]{2,}[.?!]");
            var matchs = reg.Matches(this.Text);
            foreach (Match item in matchs) {
                this.Sentences.Add(new SentenceData() { Sentence = item.Value.Trim() });
            }
            InitRange();
            RaisePropertyChanged("CanSave");
        }

        private void CleanText()
        {
            string text = "";
            //去除双引号
            {
                var reg = new Regex("[\"“”。]");
                text = reg.Replace(this.Text, ".");
            }
            // 去除行首的发言人
            {
                var reg = new Regex(@"^.*:", RegexOptions.Multiline);
                text = reg.Replace(text, "");
            }
            //去除行首空格
            {
                var reg = new Regex(@"^\s*", RegexOptions.Multiline);
                text = reg.Replace(text, "");
            }
            //去除行尾空格
            {
                var reg = new Regex(@"\s*$", RegexOptions.Multiline);
                text = reg.Replace(text, "");
            }
            //去除多余空格
            {
                var reg = new Regex(@" {2,}");
                text = reg.Replace(text, " ");
            }
            this.Text = text;
        }
        public bool IsBusy
        {
            get { return mIsBusy; }
            set
            {
                mIsBusy = value; 
                RaisePropertyChanged();
            }
        }
        private void InitRange()
        {
            if(this.Length<= 0 || this.Sentences.Count <=0)
                return;
            var unitLen = this.Length / this.Sentences.Count;
            long start = 0;
            for(var i = 0; i < this.Sentences.Count; i++)
            {
                var cur = this.Sentences[i];
                cur.Start = start;
                cur.End = start + unitLen;
                start += unitLen;
            }
        }
        private void RestRange()
        {
            var cur = this.SentenceViewSource.View.CurrentItem as SentenceData;
            var curIdx = this.Sentences.IndexOf(cur);
         
            var alllen = this.Length - cur.End;
            var unitlen = alllen / (this.Sentences.Count - curIdx - 1);
            var start = cur.End;

            for (var i = curIdx + 1; i < this.Sentences.Count; i++) {
                var sen = this.Sentences[i];
                sen.Start = start;
                sen.End = start + unitlen;
                start += unitlen;
            }
        }
        private void PlaySentence()
        {
            this.Pause();
            var cur = this.SentenceViewSource.View.CurrentItem as SentenceData;
            if(cur == null)
                return;

            var start = TimeSpan.FromSeconds(cur.Start);
            var end = TimeSpan.FromSeconds(cur.End);
            var len = end - start;
            var trimmed = new AudioFileReader(this.AudioFile).Skip(start).Take(len);
            var outputDevice = new WaveOutEvent();
            outputDevice.Init(trimmed);
            outputDevice.Play();
        }
        public void JumpTo(double bt)
        {
            var position = (Length * bt);
            mAudioReader.CurrentTime = new TimeSpan((long)position);
        }
        private void Save()
        {
            IsBusy = true;
            LoadAudio(this.SentenceViewSource.View.CurrentItem as SentenceData);
            IsBusy = false;
        }

        private void LoadAudio(SentenceData data)
        { 

            var start = TimeSpan.FromSeconds(data.Start);
            var end = TimeSpan.FromSeconds(data.End);
            var len = end - start;
            var trimmed = new AudioFileReader(this.AudioFile).Skip(start).Take(len);
            var path = "abc.avi";
            WaveFileWriter.CreateWaveFile16(path, trimmed);
            mAudioService.UploadSentenceAudio(data.Sentence, path);
        }
        public ICommand PlayCommand
        {
            get
            {
                return new DelegateCommand(Play).ObservesCanExecute(() => CanPlay);
            }
        }
        public ICommand AnalyseTextCommand
        {
            get
            {
                return new DelegateCommand(AnalyseText).ObservesCanExecute(() => CanAnalyse);
            }
        }
        public ICommand CleanTextCommand
        {
            get
            {
                return new DelegateCommand(CleanText).ObservesCanExecute(() => CanAnalyse);
            }
        }
        public ICommand PauseCommand
        {
            get
            {
                return new DelegateCommand(Pause).ObservesCanExecute(() => CanPause);
            }
        }
        public ICommand PlaySentenceCommand
        {
            get
            {
                return new DelegateCommand(PlaySentence);
            }
        }
        public ICommand ResetRangeCommand
        {
            get
            {
                return new DelegateCommand(RestRange);
            }
        }
        public ICommand SaveCommand
        {
            get
            {
                return  new DelegateCommand(Save).ObservesCanExecute(()=> CanSave);
            }
        }
        public bool CanSave
        {
            get { return Length>0 && this.Sentences.Count>0 ; } 
        }
    }
}
