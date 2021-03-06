using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using AutoMapper;
using Fool.Comm.Models;
using Fool.Contracts;
using Fool.TextManagement.Models;
using Microsoft.Win32;
using NAudio.Wave;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
namespace Fool.TextManagement.ViewModels
{
    public class TextEditViewModel : BindableBase, IDialogAware
    {
        #region Fields
        private readonly IAudioService mAudioService;
        private readonly bool mIsManual = false;
        private readonly IPublisherService mPublisherService;
        private readonly ISentenceTranslateService mSentenceTranslateService;
        private readonly IText2AudioService mText2AudioService;
        private readonly ITextService mTextService;
        private readonly ISentenceService mSentenceService;
        private readonly DispatcherTimer mTimer = new DispatcherTimer();
        private string mAudioFile;
        private AudioFileReader mAudioReader;
        private string mBook;
        private double mCurrent;
        private FlowDocument mDocument;
        private ICommand mFormatTextCommand;
        private bool mIsBusy;
        private bool mIsDocumentDisplay;
        private IRegionNavigationJournal mJournal;
        private long mLength;
        private WaveOutEvent mOutputDevice;
        private string mPublisher;
        private List<PublisherVm> mPublishers = new List<PublisherVm>();
        private CollectionViewSource mSentenceViewSource;
        private List<string> mHasAudioSentences = new List<string>();
        private bool mUpdateAudio = true;
        private string mText;
        private string mTitle;
        private bool mIsEditMode;
        #endregion
        #region Properties
        public bool UpdateAudio
        {
            get => mUpdateAudio;
            set => SetProperty(ref mUpdateAudio, value);
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
        public int Id { get; set; }
        public bool CanCloseDialog()
        {
            return true;
        }
        public void OnDialogClosed()
        {
            
        }
        public void OnDialogOpened(IDialogParameters parameters)
        {
            int id = 0;
            var hv = parameters.TryGetValue<int>("Id", out id);
            Publisher = parameters.GetValue<string>("publisher");
            Book = parameters.GetValue<string>("book");  
            if (hv)
            {
                Id = id;
                IsEditMode = true;
                LoadText();

            }
            else
            {
                IsEditMode = false;
            }
        }
        public string Title
        {
            get { return mTitle; }
            set
            {
                mTitle = value;
                RaisePropertyChanged();
            }
        }
        public event Action<IDialogResult> RequestClose;
        public string AudioFile
        {
            get { return mAudioFile; }
            set
            {
                mAudioFile = value;
                RaisePropertyChanged();
            }
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
        public bool IsDocumentDisplay
        {
            get { return mIsDocumentDisplay; }
            set
            {
                mIsDocumentDisplay = value;
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
            }
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
        public List<PublisherVm> Publishers
        {
            get { return mPublishers; }
            set
            {
                mPublishers = value;
                RaisePropertyChanged();
            }
        }
        public IEnumerable<BookVm> Books
        {
            get
            {
                var books = new List<BookVm>();
                var p = Publishers?.FirstOrDefault(a => a.Title.Equals(Publisher));
                return p?.Books;
            }
        }
        public string Book
        {
            get { return mBook; }
            set
            {
                mBook = value;
                RaisePropertyChanged();
            }
        }
        public string Publisher
        {
            get { return mPublisher; }
            set
            {
                mPublisher = value;
                RaisePropertyChanged();
                RaisePropertyChanged("Books");
            }
        }
        public CollectionViewSource SentenceViewSource
        {
            get
            {
                if(mSentenceViewSource == null)
                    mSentenceViewSource = new CollectionViewSource
                    {
                        Source = Sentences
                    };
                return mSentenceViewSource;
            }
        }
        public ObservableCollection<SentenceData> Sentences { get; } = new ObservableCollection<SentenceData>();
        public FlowDocument Document
        {
            get { return mDocument; }
            set
            {
                mDocument = value;
                RaisePropertyChanged();
            }
        }
        public bool IsEditMode
        {
            get { return mIsEditMode; }
            set
            {
                mIsEditMode = value;
                RaisePropertyChanged();
            }
        }
        #endregion
        #region Container
        public TextEditViewModel(IPublisherService publisherService, IAudioService audioService,
            ISentenceTranslateService sentenceTranslateService, IText2AudioService text2AudioService , ITextService textService, ISentenceService sentenceService)
        {
            mPublisherService = publisherService;
            mAudioService = audioService;
            mSentenceTranslateService = sentenceTranslateService;
            mText2AudioService = text2AudioService;
            mTextService = textService;
            mSentenceService = sentenceService;
            mTimer.Tick += Callback;
            mTimer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            mTimer.Start();
            Init();
        }
        #endregion
        #region Commands
        public ICommand ChooseAudioFileCommand
        {
            get { return new DelegateCommand(ChooseAudioFile); }
        }
        public ICommand ResetRangeCommand
        {
            get { return new DelegateCommand(RestRange); }
        }
        public ICommand GoBackCommand
        {
            get { return new DelegateCommand(() => { this.RequestClose( new DialogResult()); }); }
        }
        public ICommand GetAudioCommand
        {
            get { return new DelegateCommand(GetAudios); }
        }
        public ICommand PauseCommand
        {
            get { return new DelegateCommand(Pause); }
        }
        public ICommand SaveCommand
        {
            get { return new DelegateCommand(SaveAsync); }
        }
        public ICommand PlayCommand
        {
            get { return new DelegateCommand(Play); }
        }
        public ICommand DisplayDocumentCommand
        {
            get { return new DelegateCommand(() => { IsDocumentDisplay = true; }); }
        }
        public ICommand ResetBooksCommand
        {
            get { return new DelegateCommand<object>(o => { }); }
        }
        public ICommand FormatTextCommand
        {
            get { return mFormatTextCommand ?? (mFormatTextCommand = new DelegateCommand(FormatText)); }
        }
        public ICommand AnalyseTextCommand
        {
            get { return new DelegateCommand(AnalyseText); }
        }
        public ICommand TranslateCommand
        {
            get { return new DelegateCommand(TranslateSentencesAsync); }
        }
        public ICommand PlaySentenceCommand
        {
            get { return new DelegateCommand<SentenceData>(PlaySentence); }
        }
        public ICommand DisplaySourceCommand
        {
            get { return new DelegateCommand(() => { IsDocumentDisplay = false; }); }
        }
        #endregion
        #region Public Methods

 
        public void JumpTo(double br)
        {
            
        }
        public void SetIsManual(bool b) {}
        #endregion
        #region Private or Protect Methods  
        /// <summary>
        /// load text from server
        /// </summary>
        private void LoadText()
        {
            var txt = mTextService.GeText(Id);
            this.Title = txt.Title;
            this.Text = txt.Body;
            var sens = mSentenceService.GetSentencesOfText(Id);
            foreach(var sen in sens)
            {
                var s = new SentenceData()
                {
                    Chinese = sen.Chn,
                    Sentence = sen.Eng,
                    AudioStatus = sen.HasAudio ? AudioStatus.HasAudio : AudioStatus.NoneAudio,
                    IsBaiduAudio = false
                };
                if(s.AudioStatus == AudioStatus.HasAudio)
                {
                    mHasAudioSentences.Add(s.Sentence);
                }
                this.Sentences.Add(s);
            }
        }
        private async void Init()
        {
            var publishers = await mPublisherService.GetPublishersAndBooks();
            Publishers = Mapper.Map<List<PublisherVm>>(publishers);
        }
        private void AnalyseText()
        {
            IsBusy = true;
            Sentences.Clear();
            var text = Text.Replace("\r\n", "\r");
            // 去除行首的发言人
            {
                var reg = new Regex(@"^.*:", RegexOptions.Multiline);
                text = reg.Replace(text, "");
            }
            var fkf = new[] {'.', '\"', '\r', '?', '!'};
            var cs = text.Split(fkf);
            foreach(var l in cs)
            {
                var s = l.Trim();
                if(string.IsNullOrEmpty(s))
                    continue;
                Sentences.Add(new SentenceData
                {
                    Sentence = l.Trim(), AudioStatus = AudioStatus.NoneAudio
                });
            }
            InitRange();
            RaisePropertyChanged("CanSave");
            FindSign();
            MakeUtfText();
            IsBusy = false;
        }
        private void Callback(object sender, EventArgs eventArgs)
        {
            if(mIsManual)
                return;
            if((mAudioReader != null) && (mOutputDevice?.PlaybackState == PlaybackState.Playing))
                Current = mAudioReader.CurrentTime.TotalSeconds;
        }
        private void ChooseAudioFile()
        {
            var dialog = new OpenFileDialog
            {
                DefaultExt = ".mp3",
                Filter = "mp3 |*.mp3"
            };
            var rv = dialog.ShowDialog();
            if(rv != true)
                return;
            AudioFile = dialog.FileName;
        }
        private void FindSign()
        {
            var curIndex = 0;
            for(var j = 0; j < Sentences.Count; j++)
            {
                var s = Sentences[j];
                var i = Text.IndexOf(s.Sentence, curIndex, StringComparison.Ordinal);
                var inx = i + s.Sentence.Length;
                if(inx >= Text.Length)
                    continue;
                var ch = Text[i + s.Sentence.Length];
                s.Sentence += ch;
                curIndex = i + s.Sentence.Length;
            }
        }
        private void FormatText()
        {
            var text = Text;
            //去除双引号
            //{
            //    var reg = new Regex("[\"“”。]");
            //    text = reg.Replace(this.Text, ".");
            //}
            // 去除行首的发言人
            //{
            //    var reg = new Regex(@"^.*:", RegexOptions.Multiline);
            //    text = reg.Replace(text, "");
            //}
            text = text.Replace('，', ',');
            text = text.Replace('。', '.');
            text = text.Replace(';', '.');
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
            {
                text = text.Replace(" ,", ",");
                text = text.Replace(" .", ".");
                text = text.Replace(" !", "!");
            }
            Text = text;
        }
        private void GetAudios()
        {
            IsBusy = true;
            foreach(var sen in Sentences)
            {
                sen.AudioFile = mText2AudioService.GetAudioFile(sen.Sentence);
                sen.IsBaiduAudio = true;
              
            }
            IsBusy = false;
        }
        private void InitRange()
        {
            if((Length <= 0) || (Sentences.Count <= 0))
                return;
            var unitLen = Length / Sentences.Count;
            long start = 0;
            for(var i = 0; i < Sentences.Count; i++)
            {
                var cur = Sentences[i];
                cur.Start = start;
                cur.End = start + unitLen;
                start += unitLen;
                cur.AudioStatus = AudioStatus.HasAudio;
                cur.IsBaiduAudio = false;
            }
        }
        private void LoadAudio(SentenceData data)
        {
            var start = TimeSpan.FromSeconds(data.Start);
            var end = TimeSpan.FromSeconds(data.End);
            var len = end - start;
            var trimmed = new AudioFileReader(AudioFile).Skip(start).Take(len);
            var path = "abc.avi";
            WaveFileWriter.CreateWaveFile16(path, trimmed);
            mAudioService.UploadSentenceAudio(data.Sentence, path);
        }
        private void MakeUtfText()
        {
            var doc = new FlowDocument();
            var section = new Paragraph();
            doc.Blocks.Add(section);
            var curIndex = 0;
            for(var j = 0; j < Sentences.Count; j++)
            {
                var s = Sentences[j];
                var i = Text.IndexOf(s.Sentence, curIndex, StringComparison.Ordinal);
                if(i >= curIndex)
                {
                    var str = Text.Substring(curIndex, i - curIndex);
                    section.Inlines.Add(new Run(str));
                    var rk = new Run($"{j + 1}");
                    rk.Background = Brushes.Coral;
                    section.Inlines.Add(rk);
                    var rse = new Run(s.Sentence);
                    rse.Background = Brushes.PowderBlue;
                    section.Inlines.Add(rse);
                    curIndex = i + s.Sentence.Length;
                }
                if(j == Sentences.Count - 1)
                {
                    var str = Text.Substring(curIndex);
                    section.Inlines.Add(new Run(str));
                }
            }
            Document = doc;
        }
        private void Pause()
        {
            mOutputDevice?.Pause();
        }
        private void Play()
        {
            if(mOutputDevice == null)
                mOutputDevice = new WaveOutEvent();
            if (!System.IO.File.Exists(AudioFile))
                return;
            if(mAudioReader == null)
            {
                mAudioReader = new AudioFileReader(AudioFile);
                mOutputDevice.Init(mAudioReader);
                Length = (long)mAudioReader.TotalTime.TotalSeconds;
            }
            mOutputDevice.Play();
            RaisePropertyChanged("CanPlay");
            RaisePropertyChanged("CanPause");
            RaisePropertyChanged("CanSave");
            InitRange();
        }
        private void Play(string file)
        {
             
            
             var   outputDevice = new WaveOutEvent();
            if (!System.IO.File.Exists(file))
                return;

            var audioReader = new AudioFileReader(file);
            outputDevice.Init(audioReader);

            outputDevice.Play();
        }
        private void CreateAudioFile(SentenceData sen)
        {
            var start = TimeSpan.FromSeconds(sen.Start);
            var end = TimeSpan.FromSeconds(sen.End);
            var len = end - start;

            if(string.IsNullOrEmpty(AudioFile))
            {
                return;
            }
            var trimmed = new AudioFileReader(AudioFile).Skip(start).Take(len);
            var path = $"abc{DateTime.Now.ToFileTime()}.avi";
            WaveFileWriter.CreateWaveFile16(path, trimmed);
            sen.AudioFile = path;
        }
        private void PlaySentence(SentenceData data)
        {
            Pause();
            var cur = data;
            if(cur == null)
                return;
            if(cur.AudioStatus == AudioStatus.NoneAudio)
                return;
            if(cur.IsBaiduAudio)
            {
                CreateAudioFile(cur);

            }
            else
            {
                cur.AudioFile =  mSentenceService.GetSentenceAudioFile(cur.Sentence);             
            }
            Play(cur.AudioFile);

        }
        private void RestRange()
        {
            var cur = SentenceViewSource.View.CurrentItem as SentenceData;
            var curIdx = Sentences.IndexOf(cur);
            var alllen = Length - cur.End;
            var cot = (Sentences.Count - curIdx - 1);
            cot = cot != 0 ? cot : 1;
            var unitlen = alllen / cot;
            var start = cur.End;
            for(var i = curIdx + 1; i < Sentences.Count; i++)
            {
                var sen = Sentences[i];
                sen.Start = start;
                sen.End = start + unitlen;
                start += unitlen;
            }
        }
        private async Task SaveText  ()
        {
            
                // mTextService.UpdateText(this.Id, this.Title, this.Text, this.Publisher, this.Book);
            if(this.Id <= 0)
            {
                    this.Id = mTextService.CreateText(this.Book, this.Publisher, this.Title, this.Text);
            }
            else
            {
                mTextService.UpdateText(this.Id, this.Title, this.Text, this.Publisher, this.Book);
            }
                
                foreach(var sen in this.Sentences)
                {
                    var md = new NewSentenceModel() { TextID = this.Id, Chn = sen.Chinese, Eng = sen.Sentence };
                    if(UpdateAudio)
                    {
                        if(sen.AudioStatus == AudioStatus.HasAudio && sen.IsBaiduAudio == false)
                        {
                            CreateAudioFile(sen);
                        }

                        md.AudioFile = sen.AudioFile;

                    }
                    var  sAudioSaved = await  mSentenceService.CreateSentence(md);
                }
                  
        }
        private async void SaveAsync()
        {
            IsBusy = true;
            await SaveText();
            //LoadAudio(SentenceViewSource.View.CurrentItem as SentenceData);
            IsBusy = false;
            var prs = new DialogParameters();
            prs.Add("publisher", this.Publisher);
            prs.Add("book", this.Book);
            prs.Add("title", this.Title);
            prs.Add("id", this.Id);
            RequestClose(new DialogResult(ButtonResult.OK, prs ));


        }
        private async void TranslateSentencesAsync()
        {
            IsBusy = true;
            await Task.Run(() => 
            {
                foreach(var sen in Sentences)
                {
                    sen.Chinese = mSentenceTranslateService.Translate(sen.Sentence);
                    Thread.Sleep(1000);
                }
            });

               
            IsBusy = false;
        }
        #endregion
    }
}