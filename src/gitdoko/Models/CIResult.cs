using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gitdoko.Models
{
    public enum CIStatus
    {
        Pending,
        Running,
        Succeed,
        Failed,
    }

    public class CIResult
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public Project Project { get; set; }
        public string CommitId { get; set; }
        public CIStatus Status { get; set; }
        public string Message { get; set; }
    }
}
