using GalaSoft.MvvmLight;
using MagicBallAdvisor.Models;
using Parse;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MagicBallAdvisor.ViewModels
{
    public class TipViewModel : ViewModelBase
    {
        public static Expression<Func<Tip, TipViewModel>> FromModel
        {
            get
            {
                return model =>
                new TipViewModel()
                {
                    Text = model.Text
                };
            }
        }
        public string Text { get; set; }
    }
}
