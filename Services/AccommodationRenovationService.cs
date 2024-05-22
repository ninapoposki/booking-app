using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class AccommodationRenovationService
    {
        public IAccommodationRenovationRepository accommodationRenovationRepository;
         
        public AccommodationRenovationService(IAccommodationRenovationRepository accommodationRenovationRepository) {
            this.accommodationRenovationRepository = accommodationRenovationRepository;
        }

        public List<AccommodationRenovationDTO> GetAll()
        {
            List<AccommodationRenovation> renovations = accommodationRenovationRepository.GetAll();
            List<AccommodationRenovationDTO> renovationsDTOs = renovations.Select(acc => new AccommodationRenovationDTO(acc)).ToList();
            return renovationsDTOs;
        }
        public List<AccommodationRenovationDTO> GetAllByAccommodationId(int accommodationId)
        {
            List<AccommodationRenovation> renovations = accommodationRenovationRepository.GetAll();
            List<AccommodationRenovationDTO> renovationsDTOs = renovations.Where(r => r.AccommodationId == accommodationId)
                .Select(acc => new AccommodationRenovationDTO(acc))
                .ToList();

            return renovationsDTOs;
        }
        public void Delete(AccommodationRenovationDTO accommodationRenovationDTO)
        {
            accommodationRenovationRepository.Delete(accommodationRenovationDTO.ToAccommodationRenovation());
        }
        public bool IsAboveFiveDays(AccommodationRenovationDTO accommodationRenovationDTO)
        {
            DateTime currentDate = DateTime.Now;
            DateTime endDate = accommodationRenovationDTO.EndDate;
            TimeSpan difference = endDate - currentDate;
            return difference.Days > 5;
        }
        public void Update(AccommodationRenovationDTO accommodationRenovationDTO)
        {
            accommodationRenovationRepository.Update(accommodationRenovationDTO.ToAccommodationRenovation());
        }
        public void Add(AccommodationRenovationDTO accommodationRenovationDTO)
        {
            accommodationRenovationRepository.Add( accommodationRenovationDTO.ToAccommodationRenovation());
        }
        public int GetCurrentId()
        {
            return accommodationRenovationRepository.GetCurrentId();
        }
        public AccommodationRenovationDTO GetByIdDTO(int id)
        {
            var accommodationDTO = new AccommodationRenovationDTO(accommodationRenovationRepository.GetById(id));
            return accommodationDTO;
        }
        public void UpdateData(string Description, int accommodationId)
        {
            var id = GetCurrentId() - 1;
            var accommodationRenovationDTO = GetByIdDTO(id);
            accommodationRenovationDTO.Description = Description;
            accommodationRenovationDTO.AccommodationId = accommodationId;
            accommodationRenovationRepository.Update(accommodationRenovationDTO.ToAccommodationRenovation());
        }
        
    }
}
