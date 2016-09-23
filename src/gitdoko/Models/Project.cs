using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gitdoko.Models
{
    public class Project : IUserContent
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public User Creator { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime LastEditedOn { get; set; } = DateTime.MinValue;
        public User LastEditedBy { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string UserRights { get; set; }
        public string DefaultBranch { get; set; }
        public int NextTopicId { get; set; }

        public ICollection<Topic> Topics { get; set; }
        public ICollection<Label> Labels { get; set; }
        public ICollection<CIResult> CIResults { get; set; }
    }
}
