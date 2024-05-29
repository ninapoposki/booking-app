using BookingApp.Domain.Model;
using BookingApp.DTO;
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
        User? GetById(int id);
        int GetCurrentUserId();
        List<User> GetByRole(UserType role);

    }
}
