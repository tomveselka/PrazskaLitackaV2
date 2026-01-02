using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PrazskaLitacka.Domain.DbContexts;
using PrazskaLitacka.Domain.Dto;
using PrazskaLitacka.Domain.Entities;
using PrazskaLitacka.Domain.Interfaces;
using PrazskaLitacka.Domain.Interfaces.ServiceInterfaces;
using PrazskaLitacka.Webapi.Constants;
using PrazskaLitacka.WebApi.Utils;
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
    private readonly IEmailValidator _emailValidator;
    private readonly ILogger<RegisterUserHandler> _logger;

    public RegisterUserHandler(IUserRepository userRepository, IEmailService emailService, IMapper mapper, IEmailValidator emailValidator, ILogger<RegisterUserHandler> logger)
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _mapper = mapper;
        _emailValidator = emailValidator;
        _logger = logger;
    }

    public async Task<RegisterUserResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("REGISTER-USER-START Began registering user with email {0}", request.dto.Email);
        if (!_emailValidator.ValidateEmail(request.dto.Email))
        {
            _logger.LogInformation("REGISTER-USER-INVALID-MAIL Email {0} is invalid", request.dto.Email);
            return new RegisterUserResponseDto
            {
                Result = UserConstants.InvalidEmail
            };
        }
        var existingUser = await _userRepository.GetByEmail(request.dto.Email);
        if (existingUser != null) 
        {
            if (existingUser.IsActive)
            {
                _logger.LogInformation("REGISTER-USER-EXISTS Email {0} already exists in database", request.dto.Email);
                return new RegisterUserResponseDto
                {
                    Result = UserConstants.AlreadyExistsActive
                };
            }
            _logger.LogInformation("REGISTER-USER-EXISTS Email {0} already exists in database but account is not active", request.dto.Email);
            return new RegisterUserResponseDto
             {
               Result = UserConstants.AlreadyExistsInactive
             };
        }

        var userForRegistration = _mapper.Map<User>(request.dto);
        RegisterUserResponseDto responseDto;
        try
        {
            var registerUser = await _userRepository.Add(userForRegistration);
            responseDto = _mapper.Map<RegisterUserResponseDto>(registerUser);
        }
        catch (Exception ex) 
        {
            _logger.LogError("REGISTER-USER-ERROR-REGISTRATION Registering user {0} failed with exeption  message {1} trace {2}", request.dto.Email, ex.Message, ex.StackTrace);

            return new RegisterUserResponseDto
            {
                Result = UserConstants.RegistrationFailed
            };
        }
        try
        {
            //TODO Mail
            var registrationEmailDto = new SendRegistrationEmailDto()
            {

            };
            await _emailService.SendRegistrationCompleteEmailAsync(registrationEmailDto);
            responseDto.Result = UserConstants.RegistrationSuccessfullMailSuccessfull;
            _logger.LogInformation("REGISTER-USER-SUCCESS User {0} successfully registered", request.dto.Email);
            return responseDto;
        }
        catch (Exception ex)
        {
            _logger.LogError("REGISTER-USER-ERROR-MAIL Sending email to {0} failed with exeption  message {1} trace {2}", request.dto.Email, ex.Message, ex.StackTrace);

            return new RegisterUserResponseDto
            {
                Result = UserConstants.RegistrationSuccessfullMailFailed
            };
        }
        

    }
}
