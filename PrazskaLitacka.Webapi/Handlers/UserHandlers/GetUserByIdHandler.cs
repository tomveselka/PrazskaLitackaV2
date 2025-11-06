using MediatR;
using Microsoft.AspNetCore.Mvc;
using PrazskaLitacka.Domain.Entities;
using PrazskaLitacka.Domain.Exceptions;
using PrazskaLitacka.Domain.Interfaces;
using static PrazskaLitacka.Webapi.Requests.UserRequests;

namespace PrazskaLitacka.Webapi.Handlers.UserHandlers;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, User>
{
    private readonly ILogger<GetUserByIdHandler> _logger;
    private readonly IUserRepository _userRepository;

    public GetUserByIdHandler(ILogger<GetUserByIdHandler> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("USER-BY-ID Requested user with id {id}", request.userId);
        var user = await _userRepository.GetById(request.userId);
        if (user == null)
        {
            _logger.LogWarning("USER-BY-ID No User found with id {id}", request.userId);
            throw new UserNotFoundException("id", request.userId.ToString());
        }
        _logger.LogInformation("USER-BY-ID Retrieved user with id {id} and email {email}", request.userId, user.Email);
        return user;

    }
}
