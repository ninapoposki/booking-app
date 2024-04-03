using BookingApp.Model;
using BookingApp.Observer;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class OwnerRepository
    {

        private const string FilePath = "../../../Resources/Data/owner.csv";

        private readonly Serializer<Owner> serializer;

        private List<Owner> owners;
        public Subject subject;

        public OwnerRepository()
        {
            serializer = new Serializer<Owner>();
            owners = serializer.FromCSV(FilePath);
            subject = new Subject();
        }

        public List<Owner> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }

        public Owner Add(Owner owner)
        {
            owner.Id = NextId();
            owners = serializer.FromCSV(FilePath);
            owners.Add(owner);
            serializer.ToCSV(FilePath, owners);
            subject.NotifyObservers();
            return owner;
        }
        public int NextId()
        {
            owners = serializer.FromCSV(FilePath);
            if (owners.Count < 1)
            {
                return 1;
            }
            return owners.Max(c => c.Id) + 1;
        }

        public void Delete(Owner owner)
        {
            owners = serializer.FromCSV(FilePath);
            Owner founded = owners.Find(c => c.Id == owner.Id);
            owners.Remove(founded);
            serializer.ToCSV(FilePath, owners);
            subject.NotifyObservers();
        }

        public Owner Update(Owner owner)
        {
            owners = serializer.FromCSV(FilePath);
            Owner current = owners.Find(t => t.Id == owner.Id);
            int index = owners.IndexOf(current);
            owners.Remove(current);
            owners.Insert(index, owner);
            serializer.ToCSV(FilePath, owners);
            subject.NotifyObservers();
            return owner;
        }


        public Owner GetByUser(User user)
        {
            owners = serializer.FromCSV(FilePath);
            return owners.FirstOrDefault(guest => guest.User.Id == user.Id);
        }


        public Owner GetById(int id)
        {

            owners = serializer.FromCSV(FilePath);
            return owners.Find(i => i.Id == id);

        }

        public int GetCurrentId()
        {
            owners = serializer.FromCSV(FilePath);
            int maxId = owners.Count > 0 ? owners.Max(t => t.Id) : 0;
            return maxId;
        }

        public Owner GetByUserId(int userId)
        {
            // Pretraga vlasnika na osnovu ID-ja korisnika
            return owners.FirstOrDefault(owner => owner.UserId == userId);
        }


        public void Subscribe(IObserver observer)
        {
            subject.Subscribe(observer);
        }

    }
}
