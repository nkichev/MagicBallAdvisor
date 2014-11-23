using Parse;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagicBallAdvisor.Models
{
    [ParseClassName("Category")]
    public class Category : ParseObject
    {
        [ParseFieldName("title")]
        public string Title 
        { 
            get 
            {
                return GetProperty<string>();
            }
            set
            {
                SetProperty<string>(value);
            }
        }

        [ParseFieldName("answers")]
        public IEnumerable<string> Answers
        {
            get
            {
                return GetProperty<IEnumerable<string>>();
            }
            set
            {
                SetProperty<IEnumerable<string>>(value);
            }
        }
    }
}
