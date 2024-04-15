﻿using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Observer;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.Repository
{
    public class AccommodationRepository : IAccommodationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodation.csv";

        private readonly Serializer<Accommodation> serializer;
       // private readonly OwnerRepository ownerRepository;

        private List<Accommodation> accommodations;
        public Subject subject;

        public AccommodationRepository()
        {
            serializer = new Serializer<Accommodation>();
            accommodations = serializer.FromCSV(FilePath);
           // ownerRepository = new OwnerRepository(); 
            subject = new Subject();
        }

        public List<Accommodation> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }

        public Accommodation Add(Accommodation accommodation)
        {
            accommodation.Id = NextId();
             accommodations.Add(accommodation);
            
            WriteToFile();
            
            subject.NotifyObservers();
            return accommodation;
        }
       /* public Owner GetOwnerForAccommodation(int accommodationId)
        {
            Accommodation accommodation = GetById(accommodationId);
            if (accommodation != null)
            {
                int ownerId = accommodation.OwnerId;
                return ownerRepository.GetById(ownerId);
            }
            return null; // Ako smeštaj nije pronađen ili vlasnik nije pronađen
        }*/


        public int GetCurrentId()
        {
            /*   if (accommodations.Count == 0) return 1;
               return accommodations.Max(t => t.Id);*/
            accommodations = serializer.FromCSV(FilePath);
            int maxId = accommodations.Count > 0 ? accommodations.Max(t => t.Id) : 0;
            return maxId + 1;
        }

        public Accommodation GetById(int id)
        {

            accommodations = serializer.FromCSV(FilePath);
            return accommodations.Find(i => i.Id == id);
        }

        public int NextId()
        {
            accommodations = serializer.FromCSV(FilePath);
            if (accommodations.Count < 1)
            {
                return 1;
            }
            return accommodations.Max(c => c.Id) + 1;
        }

        public void Delete(Accommodation accommodation)
        {
            accommodations = serializer.FromCSV(FilePath);
            Accommodation founded = accommodations.Find(c => c.Id == accommodation.Id);
            accommodations.Remove(founded);
            serializer.ToCSV(FilePath, accommodations);
            subject.NotifyObservers();
        }

        public Accommodation Update(Accommodation accommodation)
        {
            /* accommodations = serializer.FromCSV(FilePath);
             Accommodation current = accommodations.Find(t => t.Id == accommodation.Id);
             int index = accommodations.IndexOf(current);
             accommodations.Remove(current);
             accommodations.Insert(index, accommodation);       // keep ascending order of ids in file 
             serializer.ToCSV(FilePath, accommodations);
             subject.NotifyObservers();
             return accommodation;*/

            var existing = accommodations.FindIndex(a => a.Id == accommodation.Id);
            if (existing != -1)
            {
                accommodations[existing] = accommodation;
                WriteToFile();
                subject.NotifyObservers();
            }
            return accommodation;
        }
        private void WriteToFile()
        {
            serializer.ToCSV(FilePath, accommodations);
        }


        public void Subscribe(IObserver observer)
        {
            subject.Subscribe(observer);
        }
    }
}
