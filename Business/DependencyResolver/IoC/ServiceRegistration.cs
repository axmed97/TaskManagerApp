﻿using Business.ValidationRules.FluenValidation;
using Core.Entities.Concrete;
using DataAccess.Concrete.EntityFramework;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Business.DependencyResolver.IoC
{
    public static class ServiceRegistration
    {
        public static void AddBusinessService(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            //services.Configure<IdentityOptions>(options =>
            //{
            //    options.Tokens.EmailConfirmationTokenProvider = "Default";
            //    options.Tokens.ProviderMap.Add("Default",
            //        new TokenProviderDescriptor(typeof(IUserTwoFactorTokenProvider<AppUser>)));
            //});

            //services.Configure<DataProtectionTokenProviderOptions>(options =>
            //{
            //    options.TokenLifespan = TimeSpan.FromDays(1); // Set the token lifespan to 1 day
            //});

            services.AddHttpClient();

            ValidatorOptions.Global.LanguageManager = new CustomLanguageManager();

            // FluentEmail
            #region FluentEmail
            var emailSettings = configuration.GetSection("EmailSettings");
            var defaultFromEmail = emailSettings["DefaultFromEmail"];
            var defaultFromName = emailSettings["DefaultFromName"];
            var host = emailSettings["Host"];
            var port = emailSettings.GetValue<int>("Port");
            var username = emailSettings["Username"];
            var password = emailSettings["Password"];
            services.AddFluentEmail(defaultFromEmail, "Task Manager")
                .AddSmtpSender(host, port, username, password);
            #endregion
        }
    }
}