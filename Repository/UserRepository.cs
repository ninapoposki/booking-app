using BookingApp.Domain.Model;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Repository
{
    public class UserRepository
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
        /* public int GetCurrentUserId()
           {

               if (_users.Count == 0) return 1;
               return _users.Max(t => t.Id);


         }*/
        public void SetCurrentUserId(int userId)
        {
            currentUserId = userId;
        }
        public int GetCurrentUserId()
        {
            // Provera da li je neki korisnik trenutno ulogovan
            if (currentUserId != 0)
            {
                // Vraća ID trenutno ulogovanog korisnika
                return currentUserId;
            }
            else
            {
                // Ako nijedan korisnik nije ulogovan, možete vratiti neku vrednost koja to označava
                // Na primer, -1 ili baciti izuzetak
                return -1; // ili throw new Exception("Nema ulogovanog korisnika.");
            }
        }

       
    }
}
