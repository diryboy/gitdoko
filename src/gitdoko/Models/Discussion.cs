using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gitdoko.Models
{
    public class Discussion : IUserContent
    {
        public Guid Id { get; set; }
        public Guid TopicId { get; set; }
        public Guid CreatorId { get; set; }
        public string CommitId { get; set; }
        public string Diff { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastEditedOn { get; set; }
        public string LastEditedBy { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
    }
}
