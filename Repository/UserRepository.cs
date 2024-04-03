﻿using BookingApp.Model;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Repository
{
    public class UserRepository
    {
        private const string FilePath = "../../../Resources/Data/users.csv";

        private readonly Serializer<User> _serializer;

        private List<User> _users;

        public UserRepository()
        {
            _serializer = new Serializer<User>();
            _users = _serializer.FromCSV(FilePath);
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
        public int GetCurrentUserId()
        {

            if (_users.Count == 0) return 1;
            return _users.Max(t => t.Id);

        }

       
    }
}
