
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalProjectBackend.Data;
using FinalProjectBackend.Model;
using FinalProjectBackend.DTOModel;
using FinalProjectBackend.Logic;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using System.Text;

namespace FinalProjectBackend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UsersController : ControllerBase
    {
        private readonly FinalProjectBackendContext _context;
        private readonly IConfiguration _configuration;
        private readonly JWTSettings setting;
        public UsersController(FinalProjectBackendContext context, IConfiguration configuration, IOptions<JWTSettings> options)

        {
            _context = context;
            _configuration = configuration;
            setting = options.Value;
        }
        //public UsersController(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}
        // GET: api/Users



        //Get all users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUser()
        {
            RouteLogicUser RLU = new RouteLogicUser(_context);
            return await RLU.GetUsers();
        }
        //Get a user
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            RouteLogicUser RL = new(_context);
            var user = await _context.Users.FindAsync(id);
            if(user == null)
            {
                return BadRequest("User not found");
            }
            UserDTO UserDTOToReturn = RL.ConvertUserToDTO(user);
            return UserDTOToReturn;
        }

        //Update a user
        [Authorize]
        [HttpPost("update/{id}")]
        public async Task<IActionResult> UpdateUserInfo(int id, UserDTO userDTO)
        {
            Console.WriteLine("USERID"+userDTO.UserId);
            if (id != userDTO.UserId)
            {
                return BadRequest();
            }
            RouteLogicUser RL = new RouteLogicUser(_context);
            User UpdatedUser = RL.ConvertUserDTOToUser(userDTO);
            CreatePasswordHash(userDTO.Password, out byte[] passwordhash, out byte[] passwordSalt);
            UpdatedUser.PasswordHash = passwordhash;
            UpdatedUser.PasswordSalt = passwordSalt;
            _context.Entry(UpdatedUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();
         


            return NoContent();
        }
        //Create an account
        [HttpPost("createaccount")]
        public async Task<ActionResult<UserDTO>> CreateAccount(UserDTO userDTO)
        {
            RouteLogicUser RouteLogic = new RouteLogicUser(_context);
            if (RouteLogic.CheckIfInfoExists(userDTO))
            {
                return BadRequest("already exists");
            }
            else
            {

                User user = RouteLogic.ConvertUserDTOToUser(userDTO);
                CreatePasswordHash(userDTO.Password, out byte[] passwordhash, out byte[] passwordSalt);
                user.PasswordHash = passwordhash;
                user.PasswordSalt = passwordSalt;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return Ok(user);
            }

        }
        private void CreatePasswordHash(string password, out byte[] passwordhash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        // Delete an account
        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }


        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
        //Get all challenges for a user
        [Authorize]
        [HttpGet("getUserChallenges/{id}")]
        public async Task<ActionResult<List<ChallengeDTO>>> GetUserChallenges(int id)
        {
            var RLU = new RouteLogicUser(_context);
            var user = await _context.Users.Include(x => x.Challenges).Where(x => x.UserId == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }
            var challengesToReturn = user.Challenges.Select(x => new ChallengeDTO
            {
                ChallengeId = x.ChallengeId,
                ChallengeName = x.ChallengeName,
                ChallengeColor = x.ChallengeColor,
                ChallengeDescription = x.ChallengeDescription,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                DaysLeft = RLU.CalculateDaysLeft(x.EndDate)
            }); ;

            return challengesToReturn.ToList();
        }
        //GET 
        //Authenticate and login a user
        [HttpPost("login")]
        public async Task<ActionResult<JWTDTO>> Login(LoginDTO loginDTO)
        {
            RouteLogicUser RLU = new(_context);
            User user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == loginDTO.UserName);

            if (user == null)
            {
                return BadRequest("User not found");
            }
            if (!VerifyPasswordHash(loginDTO.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password");
            }
            //string token = CreateToken(user);
            ////UserDTO userDTOToSendBack = RLU.ConvertUserToDTO(user);
            //JWTDTO tokenToSend = new JWTDTO();
            //tokenToSend.token = token;
            //return tokenToSend;
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenkey = Encoding.UTF8.GetBytes(setting.Token);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(ClaimTypes.Name,user.UserId.ToString()),
                        }
               ),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha512)

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string finalToken = tokenHandler.WriteToken(token);

            JWTDTO tokenToReturn = new JWTDTO
            {
                token = finalToken
            };
            return Ok(tokenToReturn);

        }
        //Verify a hashed password
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] user)
        {
            using (var hmac = new HMACSHA512(user))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
