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
    public class GuestRepository
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

        /*public List<Guest> GetByUser(User user)
        {
            guests = serializer.FromCSV(FilePath);
            return guests.FindAll(t => t.User.Id == user.Id);
        }*/
        //ili treba samo jedan gost da bude tog tipa?
        public Guest GetByUser(User user) //ILI int userId
        {
            guests = serializer.FromCSV(FilePath);
            return guests.FirstOrDefault(guest => guest.User.Id == user.Id);
        }
      
        public Guest GetById(int id)
        {
            guests = serializer.FromCSV(FilePath);
            return guests.Find(i => i.Id == id);
        }

        public Guest GetById(int id)
        {
            //ostavicu ovaj red da vidim da li ce raditi da bude sazetije
            guests = serializer.FromCSV(FilePath);
            return guests.Find(i => i.Id == id);
             //return guests.FirstOrDefault(guest => guest.Id == id);
        }


        public void Subscribe(IObserver observer)
        {
            subject.Subscribe(observer);
        }

    }
}
