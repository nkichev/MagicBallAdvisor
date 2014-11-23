using Parse;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagicBallAdvisor.Models
{
    [ParseClassName("Tip")]
    public class Tip : ParseObject
    {
        [ParseFieldName("text")]
        public string Text
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
    }
}
