using FinalProjectBackend.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinalProjectBackend.Model;
using FinalProjectBackend.DTOModel;
using System.Net.NetworkInformation;
namespace FinalProjectBackend.Logic
{
    public class RouteLogicUser
    {
        private readonly FinalProjectBackendContext _context;

        public RouteLogicUser(FinalProjectBackendContext context)
        {
            _context = context;
        }

        public async Task<List<UserDTO>> GetUsers()
        {
            return await _context.Users.Select(x => new UserDTO
            {
                UserId = x.UserId,
                UserName = x.UserName,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
              
          
            }).AsNoTracking().ToListAsync();
        }
       
       
        public bool CheckIfInfoExists(UserDTO userDTO)
        {
            bool CheckUserName = _context.Users.Any(x => x.UserName == userDTO.UserName);
            bool checkEmail = _context.Users.Any(X => X.Email == userDTO.Email);

            if(checkEmail || CheckUserName)
            {
               return true;
            } else
            {
                return false;
            }
           
        
        }
        public int CalculateDaysLeft(DateTime endDate)
        {
            var today = DateTime.Now;
            TimeSpan daysLeft = endDate - today;
            int days = daysLeft.Days;
            return days;
        }
        
       
       
        public User ConvertUserDTOToUser(UserDTO userDTO)
        {
            return new User
            {
                UserId = userDTO.UserId,
                UserName = userDTO.UserName,
                Email = userDTO.Email,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
              
              
            };
        }
      public UserDTO ConvertUserToDTO(User user)
        {
            return new UserDTO
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
              
               
            };
        }
    }
}
