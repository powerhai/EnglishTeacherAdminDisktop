using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
namespace Fool.TextManagement.Models
{
    public class TextVm : BindableBase
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
    }
}
