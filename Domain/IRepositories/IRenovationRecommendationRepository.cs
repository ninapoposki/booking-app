using BookingApp.Domain.Model;
using BookingApp.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface IRenovationRecommendationRepository
    {
        List<RenovationRecommendation> GetAll();
        RenovationRecommendation Add(RenovationRecommendation recommendation);
        int NextId();
        void Delete(RenovationRecommendation recommendation);
        RenovationRecommendation Update(RenovationRecommendation recommendation);
        RenovationRecommendation GetById(int id);
        int GetCurrentId();
        void Subscribe(IObserver observer);
    }
}
