using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gitdoko.Models;
using gitdoko.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace gitdoko.Controllers
{
    public class PullRequestController : TopicController<PullRequest, PullRequestEditViewModel>
    {
        public PullRequestController( AppDbContext db ) : base(db)
        {
        }

        protected override void CreateOrUpdateTopicFromViewMode( ref PullRequest topic, PullRequestEditViewModel viewModel )
        {
            throw new NotImplementedException();
        }
    }
}
