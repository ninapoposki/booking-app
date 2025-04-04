﻿using BookingApp.Domain.IRepositories;
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
    public class CheckPointRepository:ICheckPointRepository
    {
        private const string FilePath = "../../../Resources/Data/checkPoints.csv";

        private readonly Serializer<CheckPoint> serializer;

        private List<CheckPoint> checkPoints;
        public Subject subject;

        public CheckPointRepository()
        {
            serializer = new Serializer<CheckPoint>();
            checkPoints = serializer.FromCSV(FilePath);
            subject = new Subject();
        }

        public List<CheckPoint> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }

        public CheckPoint Add(CheckPoint checkPoint)
        {
            checkPoint.Id = NextId();
            checkPoints = serializer.FromCSV(FilePath);
            checkPoints.Add(checkPoint);
            serializer.ToCSV(FilePath, checkPoints);
            subject.NotifyObservers();
            return checkPoint;
        }

        public int NextId()
        {
            checkPoints = serializer.FromCSV(FilePath);
            if (checkPoints.Count < 1)
            {
                return 1;
            }
            return checkPoints.Max(c => c.Id) + 1;
        }

        public void Delete(CheckPoint checkPoint)
        {
            checkPoints = serializer.FromCSV(FilePath);
            CheckPoint founded = checkPoints.Find(c => c.Id == checkPoint.Id);
            checkPoints.Remove(founded);
            serializer.ToCSV(FilePath, checkPoints);
            subject.NotifyObservers();
        }

        public CheckPoint Update(CheckPoint checkPoint)
        {
            checkPoints = serializer.FromCSV(FilePath);
            CheckPoint current = checkPoints.Find(t => t.Id == checkPoint.Id);
            int index = checkPoints.IndexOf(current);
            checkPoints.Remove(current);
            checkPoints.Insert(index, checkPoint);       // keep ascending order of ids in file 
            serializer.ToCSV(FilePath, checkPoints);
            subject.NotifyObservers();
            return checkPoint;
        }

        public void Subscribe(IObserver observer)
        {
            subject.Subscribe(observer);
        }
        public List<CheckPoint> GetByTourId(int id)
        {
            checkPoints= serializer.FromCSV(FilePath);
           return checkPoints.FindAll(t => t.TourId ==id);
        }

        public CheckPoint GetById(int id)
        {
            checkPoints = serializer.FromCSV(FilePath);
            return checkPoints.FirstOrDefault(cp => cp.Id == id);
        }
    }
}

