﻿namespace FinalProjectBackend.Model
{
    public class User
    {
        public User()
        {
            Challenges = new List<Challenge>();
        }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Email { get; set; }
        public List<Challenge> Challenges { get; set; }
    }
}
