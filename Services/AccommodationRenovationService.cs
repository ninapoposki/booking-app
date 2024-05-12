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
        public void Delete(AccommodationRenovationDTO accommodationRenovationDTO)
        {
            accommodationRenovationRepository.Delete(accommodationRenovationDTO.ToAccommodationRenovation());
        }
        public void Add(AccommodationRenovationDTO accommodationRenovationDTO)
        {
            accommodationRenovationRepository.Add( accommodationRenovationDTO.ToAccommodationRenovation());
        }
    }
}
