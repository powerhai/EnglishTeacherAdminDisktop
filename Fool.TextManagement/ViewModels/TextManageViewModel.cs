using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Fool.Contracts;
using Fool.Domain;
using Fool.TextManagement.Models;
using Fool.TextManagement.Views;
using Prism;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
namespace Fool.TextManagement.ViewModels
{
    public class TextManageViewModel : BindableBase , INavigationAware 
    {
        #region Fields
        private readonly IPublisherService mPublisherService;
        private readonly IRegionManager mRegionManager;
        private readonly ITextService mTextService;
        private readonly IDialogService mDialogService;
        private CollectionViewSource mBooksView; 
        private ICommand mDeleteTextCommand;
        private ICommand mEditTextCommand;
        private ICommand mNewTextCommand; 
        private bool mLoaded;
        private bool mIsBusy;
        #endregion
        #region Properties
        public ObservableCollection<BookVm> Books { get; } = new ObservableCollection<BookVm>();
        public BookVm CurrentBook
        {
            get
            {
                return  BooksView.View.CurrentItem as BookVm;
                //return mCurrentBook;
            }
 
        }
        public bool IsBusy
        {
            get => mIsBusy;
            set => SetProperty(ref mIsBusy,value);
        }
        public CollectionViewSource BooksView
        {
            get
            {
                if(mBooksView == null)
                    mBooksView = new CollectionViewSource
                    {
                        Source = Books,
                        GroupDescriptions =
                        {
                            new PropertyGroupDescription("Publisher")
                        },
                        SortDescriptions =
                        {
                            new SortDescription("Publisher", ListSortDirection.Ascending)
                        }
                    };
                return mBooksView;
            }
        }
 
        public ICommand NewTextCommand
        {
            get
            {
                return mNewTextCommand ??
                       (mNewTextCommand =
                           new DelegateCommand(
                               () =>
                               { 
                                   var parms = new DialogParameters();
                                   parms.Add("publisher", CurrentBook.Publisher);
                                   parms.Add("book", CurrentBook.Title);
                                   mDialogService.ShowDialog("TextEditView", parms, async result =>
                                   {
                                       if (result.Result == ButtonResult.OK)
                                       {
                                           var p = result.Parameters.GetValue<string>("publisher");
                                           var b = result.Parameters.GetValue<string>("book");
                                           var t = result.Parameters.GetValue<string>("title");
                                           var i = result.Parameters.GetValue<int>("id");

                                           if (!p.Equals(CurrentBook.Publisher) || !b.Equals(CurrentBook.Title))
                                           {
                                               await Load();
                                           }
                                           else
                                           {
                                               CurrentBook.Texts.Add(new TextVm()
                                               {
                                                   Id = i, Title = t 
                                               });
                                           }
                                       }
                                   });
                                   //mRegionManager.RequestNavigate(RegionNames.CONTENT, typeof(TextEditView).FullName);
                               }));
            }
        }
        public ICommand DeleteTextCommand
        {
            get
            {
                return mDeleteTextCommand ??
                       (mDeleteTextCommand = new DelegateCommand<TextVm>(text =>
                       {
                           var b = mTextService.RemoveText(text.Id);
                           if(b)
                           {
                               CurrentBook.Texts.Remove(text); 
                           } 
                       }));
            }
        }
        public ICommand EditTextCommand
        {
            get
            {
                return mEditTextCommand ?? (mEditTextCommand = new DelegateCommand<TextVm>(text =>
                       {
                           var parms = new DialogParameters();
                           parms.Add("Id", text.Id);
                           parms.Add("publisher", CurrentBook.Publisher);
                           parms.Add("book", CurrentBook.Title); 
                          
                           mDialogService.ShowDialog("TextEditView", parms, async result =>
                           {
                               if(result.Result == ButtonResult.OK)
                               {
                                   var p = result.Parameters.GetValue<string>("publisher");
                                   var b = result.Parameters.GetValue<string>("book");
                                   var t = result.Parameters.GetValue<string>("title");
                                   text.Title = t;
                                   if(!p.Equals(CurrentBook.Publisher) || !b.Equals(CurrentBook.Title))
                                   {
                                       await Load();
                                   }
                               }
                           });

                       }));
            }
        }
        #endregion
        #region Constructors
        public TextManageViewModel(IRegionManager regionManager, IPublisherService publisherService,
            ITextService textService, IDialogService dialogService)
        {
            mRegionManager = regionManager;
            mPublisherService = publisherService;
            mTextService = textService;
            mDialogService = dialogService;
        }
        #endregion
        public async void OnNavigatedTo(NavigationContext navigationContext)
        {

            if(mLoaded)
            {
                return;
            }
            Books.Clear();
           
            await Load();
           
            mLoaded = true;
        }
        private async Task Load()
        {
            IsBusy = true;
            var books = await mPublisherService.GetPublishersAndBooks();
            if(books != null)
            {
                foreach(var p in books)
                {
                   
                    foreach(var b in p.Books)
                    {
                        var book = new BookVm
                        {
                            Title = b.Title,
                            Id = b.Id,
                            Publisher = p.Title
                        };
                        Books.Add(book);
                    }
                }

                var texts = mTextService.GetTexts();
                foreach(var text in texts)
                {
                    var book = Books.FirstOrDefault(a => a.Id == text.BookId);
                    book?.Texts.Add(new TextVm
                    {
                        Id = text.Id,
                        Title = text.Title
                    });
                }
            }
            IsBusy = false;

        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }
        public void OnNavigatedFrom(NavigationContext navigationContext)
        { 

        }
    }
}