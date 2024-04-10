using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class CheckPointService
    {
        private ICheckPointRepository checkPointRepository;

        public CheckPointService()
        {
            checkPointRepository=Injector.Injector.CreateInstance<ICheckPointRepository>();
        }
        public void Add(CheckPointDTO checkPoint,int tourId)
        {
            CheckPoint newCheckPoint = new CheckPoint(checkPoint.Name, tourId, checkPoint.Type);
            checkPointRepository.Add(newCheckPoint);
        }
    }
}
