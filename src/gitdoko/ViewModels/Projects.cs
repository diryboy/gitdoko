using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace gitdoko.ViewModels
{
    public class CreateProjectViewModel
    {
        [Required]
        public string Name { get; set; }

        public string Summary { get; set; }

        public string Description { get; set; }
    }
}
