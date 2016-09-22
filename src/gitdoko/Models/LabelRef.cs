using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gitdoko.Models
{
    public class LabelRef
    {
        public Guid Id { get; set; }
        public Topic Topic { get; set; }
        public Label Label { get; set; }
    }
}
