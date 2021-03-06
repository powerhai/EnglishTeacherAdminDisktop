using System.Collections.ObjectModel;
using Prism.Mvvm;
namespace Fool.TextManagement.Models
{
    public class PublisherVm : BindableBase
    {
        private string mTitle;
        public int Id { get; set; }
        public string Title
        {
            get { return mTitle; }
            set
            {
                mTitle = value; 
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<BookVm> Books { get; set; } = new ObservableCollection<BookVm>();
        
    }
}