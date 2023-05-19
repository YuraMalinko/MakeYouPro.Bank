using AutoMapper;
using MakeYouPro.Bank.Api.Auth.Models.Requests;
using MakeYouPro.Bank.Dal.Auth.Models;
using MakeYouPro.Bank.Service.Auth.Models;
using MakeYouPro.Bank.Service.Auth.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MakeYouPro.Bank.Api.Auth.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public AuthController(IMapper mapper, IAuthService authService)
        {
            _mapper = mapper;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task <IActionResult> RegisterAsync(UserRegisterRequest request)
        {
            var user =  _mapper.Map<User>(request);
            _authService.RegisterUserAsync(user);
            return Ok();
        }
    }
}
