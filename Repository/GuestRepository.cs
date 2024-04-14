using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Observer;
using BookingApp.Serializer;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class GuestRepository:IGuestRepository
    {
        private const string FilePath = "../../../Resources/Data/guest.csv";

        private readonly Serializer<Guest> serializer;

        private List<Guest> guests;
        public Subject subject;

        public GuestRepository()
        {
            serializer = new Serializer<Guest>();
            guests = serializer.FromCSV(FilePath);
            subject = new Subject();
        }

        public List<Guest> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }

        public Guest Add(Guest guest)
        {
            guest.Id = NextId();
            guests = serializer.FromCSV(FilePath);
            guests.Add(guest);
            serializer.ToCSV(FilePath, guests);
            subject.NotifyObservers();
            return guest;
        }
        public int NextId()
        {
            guests = serializer.FromCSV(FilePath);
            if (guests.Count < 1)
            {
                return 1;
            }
            return guests.Max(c => c.Id) + 1;
        }

        public void Delete(Guest guest)
        {
            guests = serializer.FromCSV(FilePath);
            Guest founded = guests.Find(c => c.Id == guest.Id);
            guests.Remove(founded);
            serializer.ToCSV(FilePath, guests);
            subject.NotifyObservers();
        }

        public Guest Update(Guest guest)
        {
            guests = serializer.FromCSV(FilePath);
            Guest current = guests.Find(t => t.Id == guest.Id);
            int index = guests.IndexOf(current);
            guests.Remove(current);
            guests.Insert(index, guest);
            serializer.ToCSV(FilePath, guests);
            subject.NotifyObservers();
            return guest;
        }

    
        public Guest GetByUser(User user) 
        {
            guests = serializer.FromCSV(FilePath);
            return guests.FirstOrDefault(guest => guest.User.Id == user.Id);
        }


        public Guest GetById(int id)
        {
            
            guests = serializer.FromCSV(FilePath);
            return guests.Find(i => i.Id == id);
             
        }

        public int GetCurrentId()
        {
            guests = serializer.FromCSV(FilePath);
            int maxId = guests.Count > 0 ? guests.Max(t => t.Id) : 0;
            return maxId;
        }

        

        public void Subscribe(IObserver observer)
        {
            subject.Subscribe(observer);
        }

    }
}
