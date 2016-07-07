using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gitdoko.ViewModels
{
    public class TopicSearchLimits
    {
        public int Page { get; set; }
        public string Author { get; set; }
        public string Involved { get; set; }
    }
}
