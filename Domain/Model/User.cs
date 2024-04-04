using BookingApp.Serializer;
using System;

namespace BookingApp.Domain.Model
{
    public enum UserType { OWNER, GUEST, GUIDE,TOURIST };
    public class User : ISerializable
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public UserType UserType { get; set; }
        public User() { }

        public User(string username, string password, UserType userType)
        {
            Username = username;
            Password = password;
            UserType = userType;
        }

        public string[] ToCSV()
        {
            string[] csvValues = 
            {
                Id.ToString(),
                Username, 
                Password,
                UserType.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Username = values[1];
            Password = values[2];
            if (values[3] == "OWNER")
            {
                UserType= UserType.OWNER;
            }
            else if (values[3] == "GUEST")
            {
                UserType= UserType.GUEST;
            }
            else if (values[3] == "TOURIST")
            {
                UserType = UserType.TOURIST;
            }
            else
            {
                UserType = UserType.GUIDE;
            }
        }
    }
}
