﻿using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using ErrorOr;
using MediatR;

namespace BuberDinner.Application.Authentication.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public LoginQueryHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        if (_userRepository.GetUserByEmail(query.Email) is not User user)
        {
            return Task.FromResult<ErrorOr<AuthenticationResult>>(Errors.Authentication.InvalidEmail);
        }
     
        if (user.Password != query.Password)
        {
            return Task.FromResult<ErrorOr<AuthenticationResult>>(new[] {Errors.Authentication.InvalidPassword});
        }
        
        var token = _jwtTokenGenerator.GenerateToken(user);
        
        return Task.FromResult<ErrorOr<AuthenticationResult>>(new AuthenticationResult
        (
            user,
            token
        ));
    }
}