using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gitdoko.Models
{
    public class Label
    {
        public Guid Id { get; set; }
        public Project Project { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
    }
}
