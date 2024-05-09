using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class TourRequestService
    {
        private ITourRequestRepository tourRequestRepository;

        public TourRequestService(ITourRequestRepository tourRequestRepository)
        {
            this.tourRequestRepository = tourRequestRepository;
        }
        public List<TourRequest> GetAll()
        {
            return tourRequestRepository.GetAll();
        }
    }
}
