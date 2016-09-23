using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gitdoko.Models
{
    public interface IUserContent
    {
        User Creator { get; set; }
        DateTime CreatedOn { get; set; }
        DateTime LastEditedOn { get; set; }
        User LastEditedBy { get; set; }
        string Summary { get; set; }
    }
}
