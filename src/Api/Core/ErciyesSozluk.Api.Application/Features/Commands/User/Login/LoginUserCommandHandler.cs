﻿using AutoMapper;
using ErciyesSozluk.Api.Application.Interfaces.Repositories;
using ErciyesSozluk.Common.Infrastructure.Exceptions;
using ErciyesSozluk.Common.Infrastructure;
using ErciyesSozluk.Common.Models.Queries;
using ErciyesSozluk.Common.Models.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ErciyesSozluk.Api.Application.Features.Commands.User.Login
{
    //loginusercommand alır, loginuserviewmodel döndürür
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserViewModel>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        //appsettings.json'dan secretkey cekilecegi icin iconfiguration kullanilir

        public LoginUserCommandHandler(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        public async Task<LoginUserViewModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var dbUser = await userRepository.GetSingleAsync(i => i.EmailAddress == request.EmailAddress);
            if (dbUser == null)
            {
                throw new DatabaseValidationException("User not found");
            }

            var pass = PasswordEncryptor.Encrypt(request.Password);
            if (pass != dbUser.Password)
            {
                throw new DatabaseValidationException("Password is wrong");
            }

            if (!dbUser.EmailConfirmed)
            {
                throw new DatabaseValidationException("Email address is not confirmed yet");
            }

            var result = mapper.Map<LoginUserViewModel>(dbUser);

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, dbUser.Id.ToString()),
                new Claim(ClaimTypes.Email, dbUser.EmailAddress),
                new Claim(ClaimTypes.Name, dbUser.UserName),
                //new Claim(ClaimTypes.GivenName, dbUser.FirstName),
                //new Claim(ClaimTypes.Surname, dbUser.LastName),
            };

            result.Token = GenerateToken(claims);
            return result;
        }

        private string GenerateToken(Claim[] claims)
        {
            //secret webapi'de appsettings.json'dan alınır
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AuthConfig:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(10);

            var token = new JwtSecurityToken(claims: claims,
                                             expires: expiry,
                                             signingCredentials: creds,
                                             notBefore: DateTime.Now);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
