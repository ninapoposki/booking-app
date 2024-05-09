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
    public class RenovationRecommendationRepository:IRenovationRecommendationRepository
    {
        private const string FilePath = "../../../Resources/Data/renovationRecommendation.csv";
        private readonly Serializer<RenovationRecommendation> serializer;
        private List<RenovationRecommendation> recommendations;
        public Subject subject;
        public RenovationRecommendationRepository()
        {
            serializer = new Serializer<RenovationRecommendation>();
            recommendations = serializer.FromCSV(FilePath);
            subject = new Subject();
        }
        public List<RenovationRecommendation> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }
        public RenovationRecommendation Add(RenovationRecommendation recommendation)
        {
            recommendation.Id = NextId();
            recommendations = serializer.FromCSV(FilePath);
            recommendations.Add(recommendation);
            serializer.ToCSV(FilePath, recommendations);
            subject.NotifyObservers();
            return recommendation;
        }
        public int NextId()
        {
            recommendations = serializer.FromCSV(FilePath);
            if (recommendations.Count < 1) { return 1; }
            return recommendations.Max(c => c.Id) + 1;
        }
        public void Delete(RenovationRecommendation recommendation)
        {
            recommendations = serializer.FromCSV(FilePath);
            RenovationRecommendation founded = recommendations.Find(c => c.Id == recommendation.Id);
            recommendations.Remove(recommendation);
            serializer.ToCSV(FilePath, recommendations);
            subject.NotifyObservers();
        }
        public RenovationRecommendation Update(RenovationRecommendation recommendation)
        {
            recommendations = serializer.FromCSV(FilePath);
            RenovationRecommendation current = recommendations.Find(t => t.Id == recommendation.Id);
            int index = recommendations.IndexOf(current);
            recommendations.Remove(current);
            recommendations.Insert(index, recommendation);
            serializer.ToCSV(FilePath, recommendations);
            subject.NotifyObservers();
            return recommendation;
        }

        public RenovationRecommendation GetById(int id)
        {
            recommendations = serializer.FromCSV(FilePath);
            return recommendations.Find(i => i.Id == id);
        }
        public int GetCurrentId()
        {
            recommendations = serializer.FromCSV(FilePath);
            int maxId = recommendations.Count > 0 ? recommendations.Max(t => t.Id) : 0;
            return maxId;
        }
        public void Subscribe(IObserver observer)
        {
            subject.Subscribe(observer);
        }
    }
}
