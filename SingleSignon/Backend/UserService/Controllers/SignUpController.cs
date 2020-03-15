﻿using JPProject.Domain.Core.Bus;
using JPProject.Domain.Core.Notifications;
using JPProject.Sso.Application.Interfaces;
using JPProject.Sso.Application.ViewModels.UserViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace UserService.Controllers
{
    [Route("sign-up"), AllowAnonymous]
    public class SignUpController : ApiController
    {
        private readonly IUserAppService _userAppService;

        public SignUpController(
            IUserAppService userAppService,
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator) : base(notifications, mediator)
        {
            _userAppService = userAppService;
        }

        [HttpPost, Route("")]
        public async Task<ActionResult<RegisterUserViewModel>> Register([FromBody] RegisterUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return ModelStateErrorResponseError();
            }

            if (model.ContainsFederationGateway())
                await _userAppService.RegisterWithProvider(model);
            else
                await _userAppService.Register(model);

            model.ClearSensitiveData();
            return ResponsePost("UserData", "Account", null, model);
        }


        [HttpGet, Route("check-username/{suggestedUsername}")]
        public async Task<ActionResult<bool>> CheckUsername(string suggestedUsername)
        {
            var exist = await _userAppService.CheckUsername(suggestedUsername);

            return ResponseGet(exist);
        }

        [HttpGet, Route("check-email/{givenEmail}")]
        public async Task<ActionResult<bool>> CheckEmail(string givenEmail)
        {
            var exist = await _userAppService.CheckUsername(givenEmail);

            return ResponseGet(exist);
        }
    }
}