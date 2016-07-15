using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gitdoko.Models;
using gitdoko.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace gitdoko.Controllers
{
    public class IssueController : TopicController<Issue, TopicEditViewModel>
    {
        public IssueController( AppDbContext db ) : base(db)
        {
        }

        protected override void CreateOrUpdateTopicFromViewMode( ref Issue topic, TopicEditViewModel viewModel )
        {
            throw new NotImplementedException();
        }
    }
}
