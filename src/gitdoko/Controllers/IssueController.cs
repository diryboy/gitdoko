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
    public class IssueController : TopicController<Issue, TopicEditViewModel, TopicEditViewModel>
    {
        public IssueController( AppDbContext db, UserManager<User> um ) : base(db, um)
        {
        }

        protected override Issue CreateTopicFromViewModel( TopicEditViewModel viewModel )
        {
            return new Issue
            {
                Summary = viewModel.Summary,
                Content = viewModel.Content,
                CreatedOn = DateTime.UtcNow
            };
        }

        protected override TopicEditViewModel CreateEditViewModelFromTopic( Topic topic )
        {
            throw new NotImplementedException();
        }

        protected override void UpdateTopicFromViewModel( Issue topic, TopicEditViewModel viewModel )
        {
            throw new NotImplementedException();
        }
    }
}
