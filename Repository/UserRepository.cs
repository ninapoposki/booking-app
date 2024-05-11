using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Input;

namespace BookingApp.Repository
{
    public class UserRepository : IUserRepository
    {
        private const string FilePath = "../../../Resources/Data/users.csv";
        private List<User> _users;
        private int currentUserId=-1;
        private readonly Serializer<User> _serializer;
        private static UserRepository instance;
        public UserRepository()
        {
            _serializer = new Serializer<User>();
            _users = _serializer.FromCSV(FilePath);
          
        }
        public static UserRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserRepository();
                }
                return instance;
            }
        }
        public User GetByUsername(string username)
        {
            _users = _serializer.FromCSV(FilePath);
            return _users.FirstOrDefault(u => u.Username == username);
        }
        public int GetCurrentGuestUserId()
        {
            if (_users.Count == 0) return 1;
            return _users[1].Id;
        }
        public void SetCurrentUserId(int userId)
        {
            currentUserId = userId;
        }
        public User? GetById(int id)
        {
            _users = _serializer.FromCSV(FilePath);
            return _users.Find(u => u.Id == id);
        }
        public int GetCurrentUserId()
       {
            if (currentUserId != 0) { 

                return currentUserId;
            }
            else
            {
                return -1; 
            }
        }
    }
}
