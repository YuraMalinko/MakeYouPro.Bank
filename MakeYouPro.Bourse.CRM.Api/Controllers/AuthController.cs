using AutoMapper;
using FluentValidation;
using MakeYouPro.Bourse.CRM.Api.Models.Users.Request;
using MakeYouPro.Bourse.CRM.Api.Models.Users.Response;
using MakeYouPro.Bourse.CRM.Auth.Bll.IServices;
using MakeYouPro.Bourse.CRM.Auth.Bll.Models;
using MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace MakeYouPro.Bourse.CRM.Api.Controllers
{
    [Authorize]
    [Route("/[controller]")]
    [ApiController]
    [SwaggerResponse((int)HttpStatusCode.OK)]
    [SwaggerResponse((int)HttpStatusCode.BadRequest)]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    [SwaggerResponse((int)HttpStatusCode.Forbidden)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    [SwaggerResponse((int)HttpStatusCode.PreconditionFailed)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IValidator<UserUpdateRequest> _validator;

        public AuthController(IMapper mapper, IAuthService authService, IValidator<UserUpdateRequest> validator)
        {
            _mapper = mapper;
            _authService = authService;
            _validator = validator;
        }

        [AllowAnonymous]
        [HttpGet("Authenticate", Name = "Authenticate")]
        public async Task<ActionResult> Authenticate([FromQuery] UserBaseRequest user)
        {

            var userBll = _mapper.Map<User>(user);
            var response = _mapper.Map<AuthResultResponse>(await _authService.Authenticate(userBll));

            if (response == null)
            {
                throw new AuthorizationException(" ", "Authorization failed");
            }

            SetTokenCookie(response.TokenRefresh);

            return Ok(response.Token);
        }

        [AllowAnonymous]
        [HttpGet("RefreshToken", Name = "RefreshToken")]
        public async Task<ActionResult> RefreshToken()
        {
            var tokenRefresh = Request.Cookies["TokenRefresh"];

            if (tokenRefresh == null)
            {
                throw new RefreshTokenException("Refresh token does not exist ");
            }

            var response = _mapper.Map<AuthResultResponse>(await _authService.RefreshToken(tokenRefresh!));

            if (response == null)
            {
                throw new AuthorizationException(" ", "Authorization failed");
            }

            SetTokenCookie(response.TokenRefresh);

            return Ok(response.Token);
        }

        [HttpDelete("RefreshToken", Name = "RevokeRefreshToken")]
        public async Task<ActionResult> RevokeRefreshToken()
        {
            var tokenRefresh = Request.Cookies["TokenRefresh"];

            if (tokenRefresh == null)
            {
                throw new RefreshTokenException("Refresh token does not exist ");
            }

            var response = await _authService.RevokeRefreshToken(tokenRefresh!);

            return Ok(response);
        }

        [HttpPatch("Password", Name = "UpdatePasswordToken")]
        public async Task<ActionResult> UpdatePasswordToken([FromQuery] UserUpdateRequest user)
        {
            var validateUser = await _validator.ValidateAsync(user);

            if (!validateUser.IsValid)
            {
                throw new AccountArgumentException(string.Join($" | ", validateUser.Errors));
            }

            var userUpdateBll = _mapper.Map<User>(user);

            var response = _mapper.Map<AuthResultResponse>(await _authService.UpdatePassword(userUpdateBll));

            if (response == null)
            {
                throw new AuthorizationException(" ", "Authorization failed");
            }

            SetTokenCookie(response.TokenRefresh);

            return Ok(response.Token);
        }

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
            };
            Response.Cookies.Append("TokenRefresh", token, cookieOptions);
        }
    }
}
