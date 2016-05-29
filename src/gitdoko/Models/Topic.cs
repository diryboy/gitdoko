using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gitdoko.Models
{
    public class Topic : IUserContent
    {
        public Guid Id { get; set; }
        public Project Project { get; set; }
        public User Creator { get; set; }
        public int TopicNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastEditedOn { get; set; }
        public string LastEditedBy { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }

        public ICollection<Discussion> Discussions { get; set; }
    }
}
