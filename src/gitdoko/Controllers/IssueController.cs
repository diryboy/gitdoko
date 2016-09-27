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
            => new Issue
            {
                Summary = viewModel.Summary,
                Content = viewModel.Content,
                CreatedOn = DateTime.UtcNow
            };

        protected override TopicEditViewModel CreateEditViewModelFromTopic( Topic topic )
            => new TopicEditViewModel
            {
                Summary = topic.Summary,
                Content = topic.Content
            };

        protected override void UpdateTopicFromViewModel( Issue topic, TopicEditViewModel viewModel )
        {
            topic.Summary = viewModel.Summary;
            topic.Content = viewModel.Content;
        }
    }
}
