using BookingApp.Domain.Model;
using BookingApp.Observer;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface IForumCommentRepository
    {
         List<ForumComment> GetAll();
         ForumComment Add(ForumComment comment);
         ForumComment GetById(int id);
         int NextId();
         void Delete(ForumComment comment);
         ForumComment Update(ForumComment comment);

    }
}
