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


        //mora accommodation da postoji
        //mora za selektovani accommodation a kontam da se negde mora povezati i selektovani accommodation sa ovim accommodationom u rezervaciji
        public bool isReservationValid(AccommodationReservation accommodationReservation,Accommodation accommodation)
        {
            //da li je br gostiju manji od max kapaciteta(manji ili jednak),da je br dana boravka veci od minimalnog broja dana -uradjeno
            //da je pocetni datum veci od krajnjeg datuma tj da je razlika izmedju pocetnog i krajnjeg datuma pozitivna-uradjeno
            //i provera da li je smestaj u tom periodu vec bukiran-tj ako su ti datumi zauzeti interval izmedju njih,onda ne moze da se bukira
            //ovo treba za bukiranje da bude posebna validacija,ovo ovde se odnosi samo na opste podatke
            //a druga validacija treba da se odnosi na to da li je vec rezervisano ili ne
            //vraca false inicijalno-fali jos provera da li su datumi zauzeti
            return accommodationReservation.DaysToStay < accommodation.MinStayDays || accommodationReservation.NumberOfGuests > accommodation.Capacity ||
                !isDateValid(accommodationReservation.InitialDate, accommodationReservation.EndDate);
                // accommodationReservation.InitialDate >= accommodationReservation.EndDate; //OVDE CE VRATITI FALSE
        }
        //ovo ti mozda ne treba,mozda mozes samo direktno da pooves za validnost godina meseca i dana
        public bool isDateValid(DateTime initialDate,DateTime endDate)
        {
            //provera validnosti datuma- vraca false inicijalno
            //vraca true inicijalno
            return initialDate.Month<endDate.Month || initialDate.Year<endDate.Year || areMonthsValid(initialDate,endDate) || areDaysValid(initialDate,endDate); 
        }

        public bool areMonthsValid(DateTime initialDate,DateTime endDate)
        {
            //ako su u istoj godini prvi datum mora biti manji od drugog
            //vraca true inicijalno
            return initialDate.Year==endDate.Year && initialDate.Month<=endDate.Month;
        }

        public bool areDaysValid(DateTime initialDate,DateTime endDate)
        {
            //ako su u istom mesecu prvi dan mora biti manji od drugog 
            //vraca true inicijalno
            return initialDate.Month==endDate.Month && initialDate.Day<endDate.Day;
        }


        //ne nzam da li mit reba da se ovde kao parametar prosledjuje accommodation samo,za selektovanu akomodaciju,jer meni ovde treba samo provera preklapanja datuma i dana boravka
        //treba mi accommodation da bih uporedila da li je selektovan smestaj jednak smestaju iz rezervacije
        public bool isAvailableForBooking(AccommodationReservation accommodationReservation,Accommodation accommodation)
        {
            //prvo prolazim kroz listu rezervisanih i gledam da li se poklapaju datumi sa onim datumima 
            //koje zelim da rezervisem
            foreach(AccommodationReservation reservation in accommodationReservations)
            {
                if (reservation.AccommodationId == accommodation.Id)
                {
                    //ili probaj bez ovoliko ifova-ALI TO POSLE SKONTAJ
                    if (reservation.InitialDate <= accommodationReservation.EndDate && reservation.EndDate >= accommodationReservation.InitialDate)
                    {
                        // Provera da li se dani boravka preklapaju
                        if (reservation.DaysToStay >= accommodationReservation.DaysToStay && accommodationReservation.DaysToStay > 0)
                        {
                            return false; // Vraćanje false ako postoji preklapanje datuma i dana boravka
                        }
                    }
                }
            }
            return true;
        }
        /*
        public void makeReservation(AccommodationReservation accommodationReservation,Accommodation accommodation)
        {
            //znaci to je inicijalno true
            if (isAvailableForBooking(accommodationReservation, accommodation))
            {
                accommodationReservations.Add(accommodationReservation);
                //dodajem u listu rezervisanih smestaja i dodajem taj datum u listu zauzetih datuma
                //ili da napravim listu slobodnih datuma?
                //SKONTAJ****************8
            }
           
            //List<Accommodation> reservedAccommodations=new List<Accommodation>();
           // reservedAccommodations.Add(accommodation); //na prvu ruku ali ne valja 
        }*/
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
            /*
            accommodationReservations = serializer.FromCSV(FilePath);
            AccommodationReservation current = accommodationReservations.Find(t => t.Id == accommodationReservation.Id);
            int index = accommodationReservations.IndexOf(current);
            accommodationReservations.Remove(current);
            accommodationReservations.Insert(index, accommodationReservation);     
            serializer.ToCSV(FilePath, accommodationReservations);
            subject.NotifyObservers();
            return accommodationReservation;*/
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
