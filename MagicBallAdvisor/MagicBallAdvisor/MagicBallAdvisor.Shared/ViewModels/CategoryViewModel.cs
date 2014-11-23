using Parse;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Linq;
using GalaSoft.MvvmLight;

namespace MagicBallAdvisor.ViewModels
{
    public class CategoryViewModel : ViewModelBase
    {
        public CategoryViewModel()
        {
            this.Answers = new List<string>();
        }
        public List<string> Answers { get; set; }
    }
}
