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
    public class OwnerService
    {
        private IOwnerRepository ownerRepository;
        

        public OwnerService()
        {
            ownerRepository = Injector.Injector.CreateInstance<IOwnerRepository>();
        }

        public Owner GetByUserId(int userId)
        {
            return ownerRepository.GetByUserId(userId);
        }
        public Owner GetById(int id)
        {
            return ownerRepository.GetById(id);
        }
       
        public OwnerDTO UpdateOwner(int userId)
        {
              var owner = ownerRepository.GetByUserId(userId);
              var ownerDTO = new OwnerDTO(owner);
              return ownerDTO;
        }
        public OwnerDTO GetByIdDTO(int id)
        {
            var ownerDTO = new OwnerDTO(ownerRepository.GetByUserId(id));
            return ownerDTO;
        }

        public void UpdateOwnerRole(OwnerDTO ownerDTO, String Role)
        {
            ownerDTO.Role = Role;
             ownerRepository.Update(ownerDTO.ToOwner());
        }

    }
}
