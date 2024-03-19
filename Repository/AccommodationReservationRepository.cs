using BookingApp.Model;
using BookingApp.Observer;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.Repository
{
    public class AccommodationReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationreservation.csv";

        private readonly Serializer<AccommodationReservation> serializer;

        //ili samo prebaci na public
        //da li ovo uospte sme tu ili mora lista u AccommodationReservation
        private List<AccommodationReservation> accommodationReservations; //lista rezervisanih smestaja 
        public Subject subject; 

        public AccommodationReservationRepository()
        {
            serializer = new Serializer<AccommodationReservation>();
            accommodationReservations = serializer.FromCSV(FilePath);
            subject = new Subject();
        }

        //izdvajam rezervacije samo za selektovani smestaj,da bih rpoverila da li postoji u bazi
        public List<AccommodationReservation> GetReservationsForAccommodation(int accommodationId)
        {
            return accommodationReservations.Where(r => r.AccommodationId == accommodationId).ToList();
        }
        public bool IsCapacityValid(int numberOfGuests, int maxGuests)
        {
            return numberOfGuests <= maxGuests;
        }
        public bool AreDatesValid(DateTime initialDate, DateTime endDate)
        {
            return initialDate < endDate;
        }

        public bool AreDatesAvailable(DateTime initialDate, DateTime endDate, int accommodationId)
        {
            List<AccommodationReservation> reservations = GetReservationsForAccommodation(accommodationId);

            foreach (AccommodationReservation reservation in reservations)
            {
                if (IsOverlap(initialDate, endDate, reservation.InitialDate, reservation.EndDate))
                {
                    return false; // Postoji preklapanje datuma sa postojećom rezervacijom
                }
            }

            return true; // Nema preklapanja datuma, datumi su dostupni za rezervaciju
        }

        private bool IsOverlap(DateTime startDate1, DateTime endDate1, DateTime startDate2, DateTime endDate2)
        {
            return startDate1 <= endDate2 && endDate1 >= startDate2;
        }



        //provera dana boravka
        public bool AreStayDaysValid(int daysToStay, int minimumStayDays)
        {
            //TimeSpan stayDuration = endDate - initialDate; //ili samo prosledi days to stay
            return daysToStay >= minimumStayDays; //inicijalno je true
        }
        //provera broja gostiju

        public bool isValid(AccommodationReservation reservation,Accommodation accommodation)
        {
            return IsCapacityValid(reservation.NumberOfGuests, accommodation.Capacity) &&
                AreStayDaysValid(reservation.DaysToStay, accommodation.MinStayDays) && //moze samo preko stay days,alid a se osiguramo,ako korisnik slucajno pogresan br unese
                AreDatesValid(reservation.InitialDate, reservation.EndDate); //vraca true ako je sve tacno

        }
        /* public List<(DateTime,DateTime)> FindAvailableDates(DateTime startDate, DateTime endDate, int accommodationId, int requiredStayDays)
         {
             List<DateTime> availableDates = new List<DateTime>();
             List<AccommodationReservation> reservations = GetReservationsForAccommodation(accommodationId);

             for (DateTime date = startDate; date <= endDate.AddDays(-requiredStayDays); date = date.AddDays(1))
             {
                 // Provera dostupnosti datuma koristeći već postojeću funkciju
                 if (areDatesAvailable(date, date.AddDays(requiredStayDays - 1), accommodationId))
                 {
                     availableDates.Add(date);
                 }
             }

             return availableDates;
         }*/
        /* public List<(DateTime, DateTime)> FindAvailableDates(DateTime startDate, DateTime endDate, int accommodationId, int requiredStayDays)
         {
             List<(DateTime, DateTime)> availableDates = new List<(DateTime, DateTime)>();
             List<AccommodationReservation> reservations = GetReservationsForAccommodation(accommodationId);

             for (DateTime date = startDate; date <= endDate.AddDays(-requiredStayDays); date = date.AddDays(1))
             {
                 // Provera dostupnosti datuma koristeći već postojeću funkciju
                 if (areDatesAvailable(date, date.AddDays(requiredStayDays - 1), accommodationId))
                 {
                     availableDates.Add((date, date.AddDays(requiredStayDays - 1)));
                 }
             }

             return availableDates;
         }*/

        /* public List<(DateTime, DateTime)> FindAvailableDates(DateTime startDate, DateTime endDate, int accommodationId, int requiredStayDays)
         {
             List<(DateTime, DateTime)> availableDates = new List<(DateTime, DateTime)>();
             List<AccommodationReservation> reservations = GetReservationsForAccommodation(accommodationId);

             for (DateTime date = startDate; date <= endDate.AddDays(-requiredStayDays); date = date.AddDays(1))
             {
                 // Provera dostupnosti datuma koristeći već postojeću funkciju
                 if (areDatesAvailable(date, date.AddDays(requiredStayDays - 1), accommodationId))
                 {
                     availableDates.Add((date, date.AddDays(requiredStayDays - 1)));
                 }
             }

             // Provera da li postoje alternativni slobodni datumi van zadatog opsega
             if (availableDates.Count == 0)
             {
                 DateTime alternativeStartDate = endDate.AddDays(1);
                 DateTime alternativeEndDate = alternativeStartDate;

                 while (alternativeEndDate <= endDate && !areDatesAvailable(alternativeStartDate, alternativeEndDate, accommodationId))
                 {
                     alternativeEndDate = alternativeEndDate.AddDays(1);
                 }

                 // Provera da li je pronađen validan alternativni datum
                 if (alternativeEndDate <= endDate && (alternativeEndDate - alternativeStartDate).Days >= requiredStayDays)
                 {
                     availableDates.Add((alternativeStartDate, alternativeEndDate));
                 }
             }

             return availableDates;
         }*/
        /* public List<(DateTime, DateTime)> FindAvailableDates(DateTime startDate, DateTime endDate, int accommodationId, int requiredStayDays)
         {
             List<(DateTime, DateTime)> availableDates = new List<(DateTime, DateTime)>();
             List<AccommodationReservation> reservations = GetReservationsForAccommodation(accommodationId);

             reservations.Sort((r1, r2) => r1.InitialDate.CompareTo(r2.InitialDate)); // Sortiranje rezervacija po početnom datumu

             DateTime today = DateTime.Today;
             DateTime alternativeStartDate = today;
             DateTime alternativeEndDate = today.AddDays(requiredStayDays);

             foreach (AccommodationReservation reservation in reservations)
             {
                 if (isOverlap(alternativeStartDate, alternativeEndDate, reservation.InitialDate, reservation.EndDate))
                 {
                     alternativeStartDate = reservation.EndDate.AddDays(1); // Nastavljamo traženje slobodnih datuma od kraja poslednje rezervacije
                     alternativeEndDate = alternativeStartDate.AddDays(requiredStayDays);
                 }
             }

             // Dodavanje slobodnih datuma van opsega unetih datuma
             while (alternativeEndDate <= endDate)
             {
                 availableDates.Add((alternativeStartDate, alternativeEndDate));
                 alternativeStartDate = alternativeStartDate.AddDays(1);
                 alternativeEndDate = alternativeStartDate.AddDays(requiredStayDays);
             }

             return availableDates;
         }*/
        /* public List<(DateTime, DateTime)> FindAvailableDates(DateTime startDate, DateTime endDate, int accommodationId, int daysToStay)
         {
             List<(DateTime, DateTime)> availableDateRanges = new List<(DateTime, DateTime)>();
             List<AccommodationReservation> reservations = GetReservationsForAccommodation(accommodationId);

             // Sortiranje rezervacija po datumima
             reservations.Sort((r1, r2) => r1.InitialDate.CompareTo(r2.InitialDate));

             DateTime currentStartDate = startDate;
             DateTime currentEndDate = startDate.AddDays(daysToStay - 1);
             bool isWithinRange = false;

             foreach (AccommodationReservation reservation in reservations)
             {
                 // Provera da li postoji preklapanje datuma sa rezervacijom
                 if (currentEndDate >= reservation.InitialDate && currentStartDate <= reservation.EndDate)
                 {
                     isWithinRange = true;
                     currentStartDate = reservation.EndDate.AddDays(1);
                     currentEndDate = currentStartDate.AddDays(daysToStay - 1);
                 }
                 else if (currentEndDate < reservation.InitialDate)
                 {
                     // Dodavanje raspona slobodnih datuma sa razmakom od daysToStay dana
                     availableDateRanges.Add((currentStartDate, currentStartDate.AddDays(daysToStay - 1)));
                     currentStartDate = reservation.EndDate.AddDays(1);
                     currentEndDate = currentStartDate.AddDays(daysToStay - 1);
                 }
             }

             // Dodavanje poslednjeg raspona slobodnih datuma
             if (!isWithinRange && currentEndDate <= endDate)
             {
                 availableDateRanges.Add((currentStartDate, currentStartDate.AddDays(daysToStay - 1)));
             }

             return availableDateRanges;
         }*/
        public List<(DateTime, DateTime)> FindAvailableDates(DateTime startDate, DateTime endDate, int accommodationId, int daysToStay)
        {
            List<(DateTime, DateTime)> availableDateRanges = new List<(DateTime, DateTime)>();
            List<AccommodationReservation> reservations = GetReservationsForAccommodation(accommodationId);

            // Sortiranje rezervacija po datumima početka
            reservations = reservations.OrderBy(r => r.InitialDate).ToList();

            DateTime currentStartDate = startDate;
            DateTime currentEndDate = startDate.AddDays(daysToStay - 1);

            foreach (AccommodationReservation reservation in reservations)
            {
                // Provera da li postoji preklapanje datuma sa rezervacijom
                if (currentEndDate >= reservation.InitialDate && currentStartDate <= reservation.EndDate)
                {
                    currentStartDate = reservation.EndDate.AddDays(1);
                    currentEndDate = currentStartDate.AddDays(daysToStay - 1);
                }
                else if (currentEndDate < reservation.InitialDate)
                {
                    // Izračunavanje listi slobodnih datuma
                    List<DateTime> freeDates = new List<DateTime>();
                    for (DateTime date = currentStartDate; date <= currentEndDate; date = date.AddDays(1))
                    {
                        freeDates.Add(date);
                    }

                    // Dodavanje raspona slobodnih datuma u listu
                    availableDateRanges.Add((freeDates.First(), freeDates.Last()));

                    // Postavljanje novog raspona datuma
                    currentStartDate = reservation.EndDate.AddDays(1);
                    currentEndDate = currentStartDate.AddDays(daysToStay - 1);
                }
            }

            // Dodavanje poslednjeg raspona slobodnih datuma
            if (currentEndDate <= endDate)
            {
                List<DateTime> freeDates = new List<DateTime>();
                for (DateTime date = currentStartDate; date <= currentEndDate; date = date.AddDays(1))
                {
                    freeDates.Add(date);
                }

                availableDateRanges.Add((freeDates.First(), freeDates.Last()));
            }

            return availableDateRanges;
        }








        public List<AccommodationReservation> GetAll()
        {
            return serializer.FromCSV(FilePath);
          
        }

        public AccommodationReservation Add(AccommodationReservation accommodationReservation)
        {
            accommodationReservation.Id = NextId();
           // accommodationReservations = serializer.FromCSV(FilePath);
            accommodationReservations.Add(accommodationReservation);
            // serializer.ToCSV(FilePath, accommodationReservations);
            WriteToFile();

            subject.NotifyObservers();
            return accommodationReservation;
        }
        private void WriteToFile()
        {
            serializer.ToCSV(FilePath, accommodationReservations);
        }

        public int NextId()
        {
            accommodationReservations = serializer.FromCSV(FilePath);
            if (accommodationReservations.Count < 1)
            {
                return 1;
            }
            return accommodationReservations.Max(c => c.Id) + 1;
        }

        public void Delete(AccommodationReservation accommodationReservation)
        {
            accommodationReservations = serializer.FromCSV(FilePath);
            AccommodationReservation founded = accommodationReservations.Find(c => c.Id == accommodationReservation.Id);
            serializer.ToCSV(FilePath, accommodationReservations);
            subject.NotifyObservers();
        }

        public AccommodationReservation Update(AccommodationReservation accommodationReservation)
        {
            var existing = accommodationReservations.FindIndex(a => a.Id == accommodationReservation.Id);
            if (existing != -1)
            {
                accommodationReservations[existing] = accommodationReservation;
                WriteToFile();
                subject.NotifyObservers();
            }
            return accommodationReservation;
        }
        public bool IsOverFiveDays(AccommodationReservation accommodationReservation)
        {
            
            DateTime currentDate = DateTime.Now;
            DateTime endDate = accommodationReservation.EndDate;
            TimeSpan difference = currentDate - endDate;
            return difference.Days < 5 && difference.Days > 0;
        }
        
     
    public void Subscribe(IObserver observer)
        {
            subject.Subscribe(observer);
        }
    }
}
