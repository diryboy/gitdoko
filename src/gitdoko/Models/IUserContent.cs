using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gitdoko.Models
{
    public interface IUserContent
    {
        Guid CreatorId { get; set; }
        DateTime CreatedOn { get; set; }
        DateTime LastEditedOn { get; set; }
        string LastEditedBy { get; set; }
        string Summary { get; set; }
    }
}
