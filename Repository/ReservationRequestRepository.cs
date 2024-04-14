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

    public  class ReservationRequestRepository : IReservationRequestRepository

    {
        private const string FilePath = "../../../Resources/Data/reservationRequests.csv";

        private readonly Serializer<ReservationRequest> serializer;
        

        private List<ReservationRequest> reservationRequests;
        public Subject subject;

        public ReservationRequestRepository()
        {
            serializer = new Serializer<ReservationRequest>();
            reservationRequests = serializer.FromCSV(FilePath); 
            subject = new Subject();
        }

        public List<ReservationRequest> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }
        
        public ReservationRequest Add(ReservationRequest reservationRequest)
        {
            reservationRequest.Id = NextId();
            reservationRequests.Add(reservationRequest);

            WriteToFile();

            subject.NotifyObservers();
            return reservationRequest;
        }
       


        public int GetCurrentId()
        {
            
            reservationRequests = serializer.FromCSV(FilePath);
            int maxId = reservationRequests.Count > 0 ? reservationRequests.Max(t => t.Id) : 0;
            return maxId + 1;
        }

        public ReservationRequest GetById(int id)
        {

            reservationRequests = serializer.FromCSV(FilePath);
            return reservationRequests.Find(i => i.Id == id);
        }

        public int NextId()
        {
            reservationRequests = serializer.FromCSV(FilePath);
            if (reservationRequests.Count < 1)
            {
                return 1;
            }
            return reservationRequests.Max(c => c.Id) + 1;
        }
        public List<(DateTime, DateTime)> GenerateNewDateRange(DateTime startDate, int daysToStay)
        {
            List<(DateTime, DateTime)> dates = new List<(DateTime, DateTime)>();

            DateTime endDate = startDate.AddYears(1); // Računa krajnji datum

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                dates.Add((date, date.AddDays(daysToStay - 1))); // Dodaje par datuma sa željenom razlikom
            }

            return dates;
        }

        public (DateTime, DateTime) GetInitialDateRange()
        {
            DateTime today = DateTime.Today;
            DateTime oneYearFromNow = today.AddYears(1);
            return (today, oneYearFromNow);
        }

        public void Delete(ReservationRequest reservationRequest)
        {
            reservationRequests = serializer.FromCSV(FilePath);
            ReservationRequest founded = reservationRequests.Find(c => c.Id == reservationRequest.Id);
            reservationRequests.Remove(founded);
            serializer.ToCSV(FilePath, reservationRequests);
            subject.NotifyObservers();
        }

        public ReservationRequest Update(ReservationRequest reservationRequest)
        {
            reservationRequests = serializer.FromCSV(FilePath);
            ReservationRequest current = reservationRequests.Find(i => i.Id == reservationRequest.Id);
            int index = reservationRequests.IndexOf(current);
            reservationRequests.Remove(current);
            reservationRequests.Insert(index, reservationRequest);       
            serializer.ToCSV(FilePath, reservationRequests);
            subject.NotifyObservers();
            return reservationRequest;
        }

        public void UpdateStatus(int accommodationReservationId, RequestStatus status, string Comment)
        {
            List<ReservationRequest> requests = serializer.FromCSV(FilePath);

            ReservationRequest reservationRequest = requests.Find(r => r.Id == accommodationReservationId);
            if (reservationRequest != null)
            {
                reservationRequest.RequestStatus = status;
                reservationRequest.Comment = Comment;
                serializer.ToCSV(FilePath, requests);
                subject.NotifyObservers();
            }
            else
            {
                throw new Exception("Cannot find request");
            }
        }
        private void WriteToFile()
        {
            serializer.ToCSV(FilePath, reservationRequests);
        }


        public void Subscribe(IObserver observer)
        {
            subject.Subscribe(observer);
        }
    }
}
