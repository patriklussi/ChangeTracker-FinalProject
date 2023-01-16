using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalProjectBackend.Data;
using FinalProjectBackend.Model;
using FinalProjectBackend.DTOModel;

namespace FinalProjectBackend.Logic
{
    public class RouteLogicChallenge
    {
        private readonly FinalProjectBackendContext _context;

        public RouteLogicChallenge(FinalProjectBackendContext context)
        {
            _context = context;
        }

        public async  Task<ActionResult<IEnumerable<ChallengeDTO>>> Test()
        {
            return await _context.Challenge.Select(x => new ChallengeDTO
            {
                ChallengeId = x.ChallengeId,
                ChallengeName = x.ChallengeName,
                ChallengeDescription = x.ChallengeDescription,
                ChallengeColor = x.ChallengeColor,
                StartDate = x.StartDate,
             

            }).AsNoTracking().ToListAsync();
        }
        public bool CheckChallengeCount(int id)
        {
            User user = _context.Users.Include(x => x.Challenges).FirstOrDefault(x => x.UserId == id);
            if (user == null)
            {
                return false;
            }
            if (user.Challenges.Count < 3)
            {
                return true;

            }
            else
            {
                return false;
            }
        }

    }
}
