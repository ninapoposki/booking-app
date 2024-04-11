using BookingApp.Domain.Model;
using BookingApp.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface IUserRepository
    {

        User GetByUsername(string username);
        int GetCurrentGuestUserId();
        void SetCurrentUserId(int UserId);
      
        //stavila sve fje videcemo sta ide u servis?
    }
}
