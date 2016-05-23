using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gitdoko.Models
{
    public class Project : IUserContent
    {
        public Guid Id { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastEditedOn { get; set; }
        public string LastEditedBy { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string UserRights { get; set; }
        public string DefaultBranch { get; set; }
        public List<string> Branches { get; set; }
        public int NextTopicId { get; set; }
    }
}
