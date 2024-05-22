using BookingApp.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class ForumService
    {
        private IForumRepository forumRepository;
        public ForumService(IForumRepository forumRepository)
        {
            this.forumRepository = forumRepository;

        }
    }
}
