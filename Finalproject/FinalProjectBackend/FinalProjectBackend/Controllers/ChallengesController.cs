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
using FinalProjectBackend.Logic;
using Microsoft.AspNetCore.Authorization;

namespace FinalProjectBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChallengesController : ControllerBase
    {
        private readonly FinalProjectBackendContext _context;

        public ChallengesController(FinalProjectBackendContext context)
        {
            _context = context;
        }

        // Get all challenges
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChallengeDTO>>> GetChallenge()
        {
            RouteLogicChallenge RL = new RouteLogicChallenge(_context);
            return await RL.Test();
        }
        //Get a single challenge
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ChallengeDTO>> GetChallenge(int id)
        {
            DateTime today = DateTime.Now;
            var challenge = await _context.Challenge.FindAsync(id);
            if (challenge == null)
            {
                return NotFound();
            }
            var endDate = challenge.EndDate;
            var chalDTO = ConvertChallengeToChallengeDTO(challenge);
            var daysLeft = (endDate - today).Days;

            chalDTO.DaysLeft = daysLeft;
            return chalDTO;
        }
        //UPDATE Challenge
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutChallenge(int id, ChallengeDTO challengeDTO)
        {

            if (id != challengeDTO.ChallengeId)
            {
                return BadRequest();
            }
            Challenge challenge = ConvertChallengeDTOToChallenge(challengeDTO);
            _context.Entry(challenge).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChallengeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        // Add challenge
        [HttpPost("addchallenge/{id}")]
        public async Task<ActionResult<ChallengeDTO>> PostChallenge(int id, ChallengeDTO ChallengeDTO)
        {
            Console.WriteLine(id);
            var user = await _context.Users.FindAsync(id);
            var RouteLogic = new RouteLogicChallenge(_context);
            if(user == null)
            {
                return BadRequest("No account man");
            }
            if (RouteLogic.CheckChallengeCount(id))
            {
                Challenge challenge = ConvertChallengeDTOToChallenge(ChallengeDTO);
                challenge.EndDate = ChallengeDTO.StartDate.AddDays(90);
                user.Challenges.Add(challenge);
                await _context.SaveChangesAsync();
                return Ok();
            } else
            {
                return BadRequest("Challengecount is 3 ");
            }

            //_context.Entry(challenge).State = EntityState.Added;
            //Check user with a function then add it to another user instance.
            //var user = await _context.Users.Include(x => x.Challenges).FirstOrDefaultAsync(x => x.UserId == id);
        }
        //DeleteChallenge
        [HttpPost("deletechallenge/{id}")]
        public async Task<IActionResult> DeleteChallenge(int id)
        {
            var challenge = await _context.Challenge.FindAsync(id);
            if (challenge == null)
            {
                return NotFound();
            }

            _context.Challenge.Remove(challenge);
            await _context.SaveChangesAsync();

            return Ok();
        }



        private bool ChallengeExists(int id)
        {
            return _context.Challenge.Any(e => e.ChallengeId == id);
        }

        //Add daily message
        [HttpPost("addmsg/{id}")]
        public async Task<ActionResult<DailyMsg>> AddDailyMsg(int id, DailyMsgDTO dailyMsg)
        {
            DailyMsg dailyMsgToSave = ConvertDailyMSGDTOToDailyMsg(dailyMsg);
            Challenge challenge = await _context.Challenge
                .Include(x => x.DailyMsg).FirstOrDefaultAsync(x => id == x.ChallengeId);

            if (challenge == null)
            {
                return BadRequest();
            }

            challenge.DailyMsg.Add(dailyMsgToSave);
            _context.SaveChanges();
            return Ok();
        }
        [HttpDelete("{id}")]
        

           
        //Get all messages for an challenge
        [HttpGet("getDailyMsgs/{id}")]
        public async Task<ActionResult<List<DailyMsgDTO>>> GetDailyMsgs(int id)
        {
            Challenge challenge = await _context.Challenge.Include(x => x.DailyMsg).FirstOrDefaultAsync(x => id == x.ChallengeId);
            if(challenge == null)
            {
                return BadRequest("Cannot find any daily messages ");

            }
            var messagesToReturn = challenge.DailyMsg.Select(x => new DailyMsgDTO
            {
                DailyMsgId = x.DailyMsgId,
                Description = x.Description,
                DateOfWriting = x.DateOfWriting
            });
            return messagesToReturn.ToList();
        }
        //Delete a message
        [HttpDelete("deleteMsg/{id}")]
        public async Task<ActionResult> DeleteMsg(int id)
        {
            //Id == message id.
            var MsgToDelete = await _context.DailyMsgs.FindAsync(id);
            _context.DailyMsgs.Remove(MsgToDelete);
            return Ok("Deleted");
        }
        // Update a message
        [HttpPost("updateMsg/{id}")]
        public async Task<ActionResult> UpdateMsg(int id,DailyMsgDTO dailyMsgDTO)
        {

            if(id != dailyMsgDTO.DailyMsgId)
            {
                return BadRequest();
            }
            DailyMsg dailyMsg = ConvertDailyMSGDTOToDailyMsg(dailyMsgDTO);
            _context.Entry(dailyMsg).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChallengeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }


        private static DailyMsg ConvertDailyMSGDTOToDailyMsg(DailyMsgDTO dailyMsgDTO)
        {
            return new DailyMsg
            {
                DailyMsgId = dailyMsgDTO.DailyMsgId,
                Description = dailyMsgDTO.Description,
                DateOfWriting = dailyMsgDTO.DateOfWriting,

            };
        }
        private static DailyMsgDTO ConvertMSGToMSGDTO(DailyMsg dailyMsg)
        {
            return new DailyMsgDTO
            {
                DailyMsgId = dailyMsg.DailyMsgId,
                Description = dailyMsg.Description,
               DateOfWriting = dailyMsg.DateOfWriting,

            };
        }
        private static Challenge ConvertChallengeDTOToChallenge(ChallengeDTO x)
        {
            return new Challenge
            {
                ChallengeId = x.ChallengeId,
                ChallengeName = x.ChallengeName,
                ChallengeDescription = x.ChallengeDescription,
                ChallengeColor = x.ChallengeColor,
                StartDate = x.StartDate,


            };
        }
        private static ChallengeDTO ConvertChallengeToChallengeDTO(Challenge x)
        {
            return new ChallengeDTO
            {
                ChallengeId = x.ChallengeId,
                ChallengeName = x.ChallengeName,
                ChallengeDescription = x.ChallengeDescription,
                ChallengeColor = x.ChallengeColor,
                StartDate = x.StartDate,
                EndDate = x.EndDate,

            };
        }
    }
}
