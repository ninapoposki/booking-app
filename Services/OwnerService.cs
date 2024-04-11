using BookingApp.Domain.IRepositories;
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
    }
}
