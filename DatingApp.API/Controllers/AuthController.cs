using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


// controlador con metodos de autenticacion
namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IauthRepository _repo;
         public IConfiguration _config { get; }

        public AuthController(IauthRepository repo, IConfiguration  config)
        {
            _repo = repo;
            _config = config;
        }

       [AllowAnonymous]
        [HttpPost("Login")]   
        public async Task<IActionResult> Login(LoginUserForDTO  User)
        {
            var user =await _repo.Login(User.codigoPaciente,User.identificacion);

            if (user.client_cli==null)
            {
                return Unauthorized();
            }

            var claims= new []
            {
                    new Claim(ClaimTypes.NameIdentifier,user.client_cli),
                    new Claim(ClaimTypes.Name,user.razon__cli)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds= new SigningCredentials(key,SecurityAlgorithms.HmacSha256Signature );
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject=new ClaimsIdentity(claims),
                Expires=DateTime.Now.AddDays(1),
                SigningCredentials=creds


            };
            var tokenHandler = new  JwtSecurityTokenHandler();
            var token= tokenHandler.CreateToken(tokenDescriptor);

            return Ok (new {
                    token = tokenHandler.WriteToken(token)

            });




        }
        

    }
}