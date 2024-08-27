﻿using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Azure.Core;
using Business.Abstract;
using Business.Utilities.StatusMessages.Abstract;
using Business.ValidationRules.FluenValidation.AuthValidations;
using Core.Aspects.Autofac.Validation;
using Core.Entities.Concrete;
using Core.Utilities.Message.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using Core.Utilities.Security.Abstract;
using Entities.Common;
using Entities.DTOs.AuthDTOs;
using FluentEmail.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly ILocalizationService _localizationService;
        private readonly IEmailService _emailService;
        public AuthManager(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, ILocalizationService localizationService, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _localizationService = localizationService;
            _emailService = emailService;
        }

        public async Task<IResult> ConfirmEmail(string userId, string token)
        {
            var langCode = Thread.CurrentThread.CurrentUICulture.Name;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new ErrorResult(_localizationService.GetLocalizedString("UserNotFound", langCode), 
                    HttpStatusCode.NotFound);
            
            token = token.Replace(" ", "+");
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if(result.Succeeded)
                return new SuccessResult(HttpStatusCode.OK);
            else
            {
                string responseMessage = string.Empty;
                foreach (var error in result.Errors)
                    responseMessage += $"{error.Description}. ";
                return new ErrorResult(message: responseMessage, HttpStatusCode.BadRequest);
            }
        }

        public IDataResult<List<UserDTO>> GetAll()
        {
            var result = _userManager.Users.OfType<User>().ToList();

            List<UserDTO> userDTO = result.Select(x => new UserDTO()
            {
                Username = x.UserName,
                Email = x.Email,
                Id = x.Id
            }).ToList();

            return new SuccessDataResult<List<UserDTO>>(data: userDTO, statusCode: HttpStatusCode.OK);
        }

        [ValidationAspect(typeof(LoginValidator))]
        public async Task<IDataResult<Token>> LoginAsync(LoginDTO model)
        {
            var langCode = Thread.CurrentThread.CurrentUICulture.Name;
            var user = await _userManager.FindByNameAsync(model.EmailOrUsername);

            if (user == null)
                return new ErrorDataResult<Token>(_localizationService.GetLocalizedString("UserNotFound", langCode), HttpStatusCode.NotFound);

            if (!user.EmailConfirmed)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = $"https://localhost:7040/api/v1/Auth/ConfirmEmail?userId={user.Id}&token={token}";

                EmailMetadata emailMetadata =
                    new(subject: "Email veritification",
                    body: $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>;.",
                    toAddress: user.Email);
                await _emailService.SendEmailAsync(emailMetadata);
                return new ErrorDataResult<Token>(statusCode: HttpStatusCode.BadRequest, message: _localizationService.GetLocalizedString("EmailNotConfirmed", langCode));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            var roles = await _userManager.GetRolesAsync(user);

            if (result.Succeeded)
            {
                Token token = await _tokenService.CreateAccessToken(user, [.. roles]);
                var response = await UpdateRefreshToken(refreshToken: token.RefreshToken, user);
                if (response.Success)
                    return new SuccessDataResult<Token>(data: token, statusCode: HttpStatusCode.OK, message: response.Message);
                else
                    return new ErrorDataResult<Token>(statusCode: HttpStatusCode.BadRequest, message: response.Message);
            }
            else
                return new ErrorDataResult<Token>(statusCode: HttpStatusCode.BadRequest, message: _localizationService.GetLocalizedString("UserNotFound", langCode));
        }

        public async Task<IResult> LogOutAsync(string userId)
        {
            var langCode = Thread.CurrentThread.CurrentUICulture.Name;
            var findUser = await _userManager.FindByIdAsync(userId);
            if (findUser == null)
                return new ErrorResult(statusCode: HttpStatusCode.NotFound, message: _localizationService.GetLocalizedString("UserNotFound", langCode));
            findUser.RefreshToken = null;
            findUser.RefreshTokenExpiredDate = null;
            var result = await _userManager.UpdateAsync(findUser);
            if (result.Succeeded)
            {
                return new SuccessResult(statusCode: HttpStatusCode.OK);
            }
            else
            {
                string responseMessage = string.Empty;
                foreach (var error in result.Errors)
                {
                    responseMessage += error + ". ";
                };
                return new ErrorDataResult<Token>(statusCode: HttpStatusCode.BadRequest, message: responseMessage);
            }
        }

        public async Task<IDataResult<Token>> RefreshTokenLoginAsync(RefreshTokenDTO refreshToken)
        {
            var langCode = Thread.CurrentThread.CurrentUICulture.Name;
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken.RefreshToken);
            var roles = await _userManager.GetRolesAsync(user);

            if (user != null && user?.RefreshTokenExpiredDate > DateTime.UtcNow.AddHours(4))
            {
                Token token = await _tokenService.CreateAccessToken(user, roles.ToList());
                token.RefreshToken = refreshToken.RefreshToken;
                return new SuccessDataResult<Token>(data: token, statusCode: HttpStatusCode.OK);
            }
            else
                return new ErrorDataResult<Token>(statusCode: HttpStatusCode.BadRequest, message: _localizationService.GetLocalizedString("UserNotFound", langCode));
        }

        [ValidationAspect(typeof(RegisterValidator))]
        public async Task<IResult> RegisterAsync(RegisterDTO model)
        {
            var langCode = Thread.CurrentThread.CurrentUICulture.Name;

            var checkEmail = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
            if (checkEmail != null)
                return new ErrorResult(statusCode: HttpStatusCode.BadRequest, 
                    message: _localizationService.GetLocalizedString("EmailAlreadyExists", langCode));

            User newUser = new()
            {
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Email = model.Email,
                UserName = model.Email,
            };

            IdentityResult identityResult = await _userManager.CreateAsync(newUser, model.Password);

            if (identityResult.Succeeded)
            {

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                var confirmationLink = $"https://localhost:7040/api/v1/Auth/ConfirmEmail?userId={newUser.Id}&token={token}";

                EmailMetadata emailMetadata = 
                    new(subject: "Email veritification", 
                    body: $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>;.", 
                    toAddress: newUser.Email);
                await _emailService.SendEmailAsync(emailMetadata);
                return new SuccessResult(message: _localizationService.GetLocalizedString("RegistrationSuccess", langCode), statusCode: HttpStatusCode.Created);
            }
            else
            {
                string responseMessage = string.Empty;
                foreach (var error in identityResult.Errors)
                    responseMessage += $"{error.Description}. ";
                return new ErrorResult(message: responseMessage, HttpStatusCode.BadRequest);
            }
        }

        public async Task<IResult> RemoveUserAsync(string userId)
        {
            var langCode = Thread.CurrentThread.CurrentUICulture.Name;

            var findUser = await _userManager.FindByIdAsync(userId);
            if (findUser == null)
                return new ErrorResult(statusCode: HttpStatusCode.BadRequest, message: _localizationService.GetLocalizedString("UserNotFound", langCode));

            var result = await _userManager.DeleteAsync(findUser);

            if (result.Succeeded)
                return new SuccessResult(HttpStatusCode.OK);
            else
            {
                string response = string.Empty;
                foreach (var error in result.Errors)
                {
                    response += error.Description + ". ";
                }
                return new ErrorResult(message: response, HttpStatusCode.BadRequest);
            }
        }

        public async Task<IDataResult<string>> UpdateRefreshToken(string refreshToken, AppUser user)
        {
            var langCode = Thread.CurrentThread.CurrentUICulture.Name;

            if (user is not null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiredDate = DateTime.UtcNow.AddMonths(1);

                IdentityResult identityResult = await _userManager.UpdateAsync(user);

                if (identityResult.Succeeded)
                    return new SuccessDataResult<string>(statusCode: HttpStatusCode.OK, data: refreshToken);
                else
                {
                    string responseMessage = string.Empty;
                    foreach (var error in identityResult.Errors)
                        responseMessage += $"{error.Description}. ";
                    return new ErrorDataResult<string>(message: responseMessage, HttpStatusCode.BadRequest);
                }
            }
            else
                return new ErrorDataResult<string>(_localizationService.GetLocalizedString("UserNotFound", langCode), HttpStatusCode.NotFound);
        }

        private string GenerateOtp()
        {
            byte[] data = new byte[4];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(data);
            }
            int value = BitConverter.ToInt32(data, 0);
            return Math.Abs(value % 900000).ToString("D6");
        }
    }
}
