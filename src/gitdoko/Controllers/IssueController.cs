using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gitdoko.Models;
using gitdoko.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace gitdoko.Controllers
{
    public class IssueController : TopicController<Issue, TopicEditViewModel, TopicEditViewModel>
    {
        public IssueController( AppDbContext db ) : base(db)
        {
        }

        protected override Issue CreateTopicFromViewMode( TopicEditViewModel viewModel )
        {
            throw new NotImplementedException();
        }

        protected override TopicEditViewModel CreateEditViewModelFromTopic( Topic topic )
        {
            throw new NotImplementedException();
        }

        protected override void UpdateTopicFromViewMode( Issue topic, TopicEditViewModel viewModel )
        {
            throw new NotImplementedException();
        }
    }
}
