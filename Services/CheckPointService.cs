using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.WPF.View.Guide;
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

        public CheckPointService(ICheckPointRepository checkPointRepository)
        {
            this.checkPointRepository = checkPointRepository;
        }
        public void Add(CheckPointDTO checkPoint,int tourId)
        {
            CheckPoint newCheckPoint = new CheckPoint(checkPoint.Name, tourId, checkPoint.Type);
            checkPointRepository.Add(newCheckPoint);
        }
        public string GetName(int id)
        {
            return checkPointRepository.GetAll().Find(c => c.Id == id).Name;
        } 
        public List<CheckPointDTO> GetByTourId(int id,int currentCheckPointId)
        {
            List<CheckPointDTO> checkPoints = new List<CheckPointDTO>();
            foreach (CheckPoint checkPoint in checkPointRepository.GetByTourId(id))
            {
                if (checkPoint.Id >= currentCheckPointId)
                {
                    checkPoints.Add(new CheckPointDTO(checkPoint));
                }
            }return checkPoints;
        }
    }
}
