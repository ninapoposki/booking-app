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
    public class ForumRepository:IForumRepository
    {

        private const string FilePath = "../../../Resources/Data/forum.csv";
        private readonly Serializer<Forum> serializer;
        private List<Forum> forums;
        public Subject subject;
        public ForumRepository()
        {
            serializer = new Serializer<Forum>();
            forums = serializer.FromCSV(FilePath);
            subject = new Subject();
        }
        public List<Forum> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }
        public Forum Add(Forum forum)
        {
            forum.Id = NextId();
            forums = serializer.FromCSV(FilePath);
            forums.Add(forum);
            serializer.ToCSV(FilePath, forums);
            subject.NotifyObservers();
            return forum;
        }
        public int NextId()
        {
            forums = serializer.FromCSV(FilePath);
            if (forums.Count < 1) { return 1; }
            return forums.Max(c => c.Id) + 1;
        }
        public void Delete(Forum forum)
        {
            forums = serializer.FromCSV(FilePath);
            Forum founded = forums.Find(c => c.Id == forum.Id);
            forums.Remove(founded);
            serializer.ToCSV(FilePath, forums);
            subject.NotifyObservers();
        }
        public Forum Update(Forum forum)
        {
            forums = serializer.FromCSV(FilePath);
            Forum current = forums.Find(t => t.Id == forum.Id);
            int index = forums.IndexOf(current);
            forums.Remove(current);
            forums.Insert(index, forum);
            serializer.ToCSV(FilePath, forums);
            subject.NotifyObservers();
            return forum;
        }

        public Forum GetById(int id)
        {
            forums = serializer.FromCSV(FilePath);
            return forums.Find(i => i.Id == id);
        }
        public int GetCurrentId()
        {
            forums = serializer.FromCSV(FilePath);
            int maxId = forums.Count > 0 ? forums.Max(t => t.Id) : 0;
            return maxId;
        }
        public void Subscribe(IObserver observer)
        {
            subject.Subscribe(observer);
        }
    }
}
