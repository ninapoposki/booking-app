using BookingApp.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class ForumCommentService
    {
        private IForumCommentRepository forumCommentRepository;

        public ForumCommentService(IForumCommentRepository forumCommentRepository)
        {
            this.forumCommentRepository = forumCommentRepository;
        }
    }
}
