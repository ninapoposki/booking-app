using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Observer;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class ForumCommentRepository : IForumCommentRepository
    {
        private const string FilePath = "../../../Resources/Data/forumComments.csv";

        private readonly Serializer<ForumComment> serializer;

        private List<ForumComment> forumComments;
        public Subject subject;
        public ForumCommentRepository()
        {
            serializer = new Serializer<ForumComment>();
            forumComments = serializer.FromCSV(FilePath);
            subject = new Subject();
        }

        public List<ForumComment> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }

        public ForumComment Add(ForumComment comment)
        {
            comment.Id = NextId();
            forumComments = serializer.FromCSV(FilePath);
            forumComments.Add(comment);
            serializer.ToCSV(FilePath, forumComments);
            return comment;
        }
        public ForumComment GetById(int id)
        {
            forumComments = serializer.FromCSV(FilePath);
            return forumComments.Find(i => i.Id == id);
        }

        public int NextId()
        {
            forumComments = serializer.FromCSV(FilePath);
            if (forumComments.Count < 1)
            {
                return 1;
            }
            return forumComments.Max(c => c.Id) + 1;
        }

        public void Delete(ForumComment comment)
        {
            forumComments = serializer.FromCSV(FilePath);
            ForumComment founded = forumComments.Find(c => c.Id == comment.Id);
            forumComments.Remove(founded);
            serializer.ToCSV(FilePath, forumComments);
        }

        public ForumComment Update(ForumComment comment)
        {
            forumComments = serializer.FromCSV(FilePath);
            ForumComment current = forumComments.Find(c => c.Id == comment.Id);
            int index = forumComments.IndexOf(current);
            forumComments.Remove(current);
            forumComments.Insert(index, comment);       // keep ascending order of ids in file 
            serializer.ToCSV(FilePath, forumComments);
            return comment;
        }

        public List<ForumComment> GetCommentsByForum(int forumId)
        {
            return forumComments.Where(fc => fc.ForumId == forumId).ToList();
        }
        private void WriteToFile()
        {
            serializer.ToCSV(FilePath, forumComments);
        }
        public void Subscribe(IObserver observer)
        {
            subject.Subscribe(observer);
        }

    }
}
