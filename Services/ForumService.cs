using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Repository;
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

        public ForumDTO AddNewForum(int guestId,int locationId)
        {
            Forum forum = new Forum
            {
                GuestId = guestId,
                LocationId = locationId,
                ActivationType = ActivationType.ACTIVE,
                ForumStatus = ForumStatusType.NEW
            };
            forumRepository.Add(forum);
            var forumDTO = new ForumDTO(forum);
            return forumDTO;

        }

        public List<ForumDTO> GetAll()
        {
            List<Forum> forums = forumRepository.GetAll();
            List<ForumDTO> forumDTOs = forums.Select(forum => new ForumDTO(forum)).ToList();
            return forumDTOs;
        }
       
        public void SetForumStatus(ForumDTO forumDTO)
        {
            forumDTO.ActivationType = ActivationType.DISABLED;
            forumRepository.Update(forumDTO.ToForum());
        }

        public bool IsDisabled(ForumDTO forum)
        {
            return forum.activationType == ActivationType.DISABLED;
        }
    }
}
