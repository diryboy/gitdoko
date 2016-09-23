using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gitdoko.Models
{
    public class Topic : IUserContent
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public Project Project { get; set; }
        public User Creator { get; set; }
        public int TopicNumber { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime LastEditedOn { get; set; } = DateTime.MinValue;
        public DateTime ClosedOn { get; set; } = DateTime.MinValue;
        public User LastEditedBy { get; set; }

        public string Summary { get; set; }
        public string Content { get; set; }

        public ICollection<LabelRef> Labels { get; set; }
        public ICollection<Discussion> Discussions { get; set; }
    }
}
