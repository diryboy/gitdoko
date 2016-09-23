using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gitdoko.Models
{
    public class LabelRef
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public Topic Topic { get; set; }
        public Label Label { get; set; }
    }
}
