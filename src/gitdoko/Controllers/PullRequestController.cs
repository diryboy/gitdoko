using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gitdoko.Models;
using gitdoko.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace gitdoko.Controllers
{
    public class PullRequestController : TopicController<PullRequest, PullRequestCreateViewModel, TopicEditViewModel>
    {
        public PullRequestController( AppDbContext db, UserManager<User> um ) : base(db, um)
        {
        }

        protected override PullRequest CreateTopicFromViewModel( PullRequestCreateViewModel viewModel )
        {
            throw new NotImplementedException();
        }

        protected override TopicEditViewModel CreateEditViewModelFromTopic( Topic topic )
        {
            throw new NotImplementedException();
        }

        protected override void UpdateTopicFromViewModel( PullRequest topic, TopicEditViewModel viewModel )
        {
            throw new NotImplementedException();
        }
    }
}
