using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PrazskaLitacka.Domain.DbContexts;
using PrazskaLitacka.Domain.Dto;
using PrazskaLitacka.Domain.Entities;
using PrazskaLitacka.Domain.Interfaces;
using PrazskaLitacka.Domain.Interfaces.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PrazskaLitacka.Webapi.Requests.UserRequests;

namespace PrazskaLitacka.WebApi.Handlers.UserHandlers;
public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, RegisterUserResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<RegisterUserHandler> _logger;

    public RegisterUserHandler(IUserRepository userRepository, IEmailService emailService, IMapper mapper, ApplicationDbContext dbContext, ILogger<RegisterUserHandler> logger)
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _mapper = mapper;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<RegisterUserResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByEmail(request.dto.Email);
        if (existingUser != null) 
        {
            return new RegisterUserResponseDto
            {
                Result = "already_exists"
            };
        }

        var userForRegistration = _mapper.Map<User>(request.dto);

        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var registerUser = await _userRepository.Add(userForRegistration);
            var registrationEmailDto = new SendRegistrationEmailDto()
            {

            };
            await _emailService.SendRegistrationCompleteEmailAsync(registrationEmailDto);
            await transaction.CommitAsync();

            var responseDto = _mapper.Map<RegisterUserResponseDto>(registerUser);
            return responseDto;
        }
        catch (Exception ex) 
        {
            await transaction.RollbackAsync();

            return new RegisterUserResponseDto
            {
                Result = "error_registering_user"
            };
        }

    }
}
