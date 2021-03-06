using System.Collections.ObjectModel;
using Prism.Mvvm;
namespace Fool.TextManagement.Models
{
    public class BookVm : BindableBase 
    {
        private string mTitle;
        private string mPublisher;
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
        public string Publisher
        {
            get { return mPublisher; }
            set
            {
                mPublisher = value; 
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<TextVm> Texts { get; set; } = new ObservableCollection<TextVm>();
    }
}