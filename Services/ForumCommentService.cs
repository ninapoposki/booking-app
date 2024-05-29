using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace BookingApp.Services
{
    public class ForumCommentService
    {
        private IForumCommentRepository forumCommentRepository;

        public ForumCommentService(IForumCommentRepository forumCommentRepository)
        {
            this.forumCommentRepository = forumCommentRepository;
        }

        public void AddNewForumComment(int guestId,int forumId,String newComment)
        {
            ForumComment forumComment = new ForumComment
            {
                ForumId = forumId,
                UserId = guestId,
                Comment = newComment,
                ReportNumber = 0,
                CreationDate = DateTime.Now,
            };
            forumCommentRepository.Add(forumComment);
        }

        public List<ForumCommentDTO> GetAll()
        {
            List<ForumComment> comments = forumCommentRepository.GetAll();
            List<ForumCommentDTO> commentDTOs = comments.Select(comm => new ForumCommentDTO(comm)).ToList();
            return commentDTOs;
        }

        public List<ForumCommentDTO> GetCommentsByForum(int forumId)
        {
            var allComments = GetAll();
            return allComments.Where(comm=> comm.ForumId==forumId).ToList();            

        }
        /* public List<ForumCommentDTO> GetCommentsByForum(int forumId)
         {
             var comments = forumCommentRepository.GetCommentsByForum(forumId);
             return comments.Select(c => new ForumCommentDTO(c)).ToList();
         }*/
        public int GuestCommentCount(int forumId, List<UserDTO> guests)
        {
            int count = 0;
            var guestIds = guests.Select(g => g.Id).ToHashSet();  // Skup ID-eva gostiju za brzu proveru
            foreach (var comment in GetCommentsByForum(forumId))
            {
                if (comment.ForumId == forumId && guestIds.Contains(comment.UserId))
                {
                    count++;
                }
            }
            return count;
        }

        public int OwnerCommentCount(int forumId, List<UserDTO> owners)
        {
            int count = 0;
            var ownerIds = owners.Select(o => o.Id).ToHashSet();  // Skup ID-eva vlasnika za brzu proveru
            foreach (var comment in GetCommentsByForum(forumId))
            {
                if (comment.ForumId == forumId && ownerIds.Contains(comment.UserId))
                {
                    count++;
                }
            }
            return count;
        }



    }
}
