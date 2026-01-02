using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using PrazskaLitacka.Domain.DbContexts;
using PrazskaLitacka.Domain.Dto;
using PrazskaLitacka.Domain.Entities;
using PrazskaLitacka.Domain.Interfaces;
using PrazskaLitacka.Domain.Interfaces.ServiceInterfaces;
using PrazskaLitacka.Webapi.Constants;
using PrazskaLitacka.Webapi.Mappers;
using PrazskaLitacka.WebApi.Handlers.UserHandlers;
using PrazskaLitacka.WebApi.Tests.Extensions;
using PrazskaLitacka.WebApi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PrazskaLitacka.Webapi.Requests.UserRequests;

namespace PrazskaLitacka.WebApi.Tests.Handlers.UserHandlers;
public class RegisterUserHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<RegisterUserHandler>> _loggerMock;
    private readonly Mock<IEmailValidator> _emailValidatorMock;
    private readonly RegisterUserHandler _sut;

    public RegisterUserHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _emailServiceMock = new Mock<IEmailService>();
        _loggerMock = new Mock<ILogger<RegisterUserHandler>>();
        _mapper = MapperExtension.CreateMapper();
        _emailValidatorMock = new Mock<IEmailValidator>();
        
        _sut = new RegisterUserHandler(_userRepositoryMock.Object, _emailServiceMock.Object, _mapper, _emailValidatorMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task RegisterUser_ReturnsUserExists_WhenEmailExistsInDatabaseAndAccountIsActive()
    {
        //Arrange
        _userRepositoryMock
            .Setup(x => x.GetByEmail(It.IsAny<string>()))
            .ReturnsAsync(new User { Id = 1, Email = "test@test.cz", Login = "Login", Name = "Name", Password = "password", Role = "user", IsActive = true });

        _emailValidatorMock
            .Setup(x => x.ValidateEmail(It.IsAny<string>()))
            .Returns(true);

        var request = new RegisterUserCommand(new RegisterUserRequestDto() { Email = "test@test.cz", Login = "Login", Name = "Name", Password = "password", Role = "user" });

        //Act
        var result = await _sut.Handle(request, CancellationToken.None);

        //Assert
        Assert.IsType<RegisterUserResponseDto>(result);
        Assert.Equal(UserConstants.AlreadyExistsActive, result.Result);

        _userRepositoryMock.Verify(x => x.GetByEmail(It.IsAny<string>()), Times.Once);
        _emailValidatorMock.Verify(x => x.ValidateEmail(It.IsAny<string>()), Times.Once);
        _userRepositoryMock.Verify(x => x.Add(It.IsAny<User>()), Times.Never);
        _emailServiceMock.Verify(x => x.SendRegistrationCompleteEmailAsync(It.IsAny<SendRegistrationEmailDto>()), Times.Never);

        _loggerMock.VerifyLogStartsWith(
          LogLevel.Information,
          "REGISTER-USER-START Began registering user with email test@test.cz",
          Times.Once()
         );
        _loggerMock.VerifyLogStartsWith(
          LogLevel.Information,
          "REGISTER-USER-EXISTS Email test@test.cz already exists in database",
          Times.Once()
          );
    }


    [Fact]
    public async Task RegisterUser_ReturnsUserExists_WhenEmailExistsInDatabaseAndAccountIsInactive()
    {
        //Arrange
        _userRepositoryMock
            .Setup(x => x.GetByEmail(It.IsAny<string>()))
            .ReturnsAsync(new User { Id = 1, Email = "test@test.cz", Login = "Login", Name = "Name", Password = "password", Role = "user", IsActive = false });

        _emailValidatorMock
            .Setup(x => x.ValidateEmail(It.IsAny<string>()))
            .Returns(true);

        var request = new RegisterUserCommand(new RegisterUserRequestDto() { Email = "test@test.cz", Login = "Login", Name = "Name", Password = "password", Role = "user" });

        //Act
        var result = await _sut.Handle(request, CancellationToken.None);

        //Assert
        Assert.IsType<RegisterUserResponseDto>(result);
        Assert.Equal(UserConstants.AlreadyExistsInactive, result.Result);

        _userRepositoryMock.Verify(x => x.GetByEmail(It.IsAny<string>()), Times.Once);
        _emailValidatorMock.Verify(x => x.ValidateEmail(It.IsAny<string>()), Times.Once);
        _userRepositoryMock.Verify(x => x.Add(It.IsAny<User>()), Times.Never);
        _emailServiceMock.Verify(x => x.SendRegistrationCompleteEmailAsync(It.IsAny<SendRegistrationEmailDto>()), Times.Never);

        _loggerMock.VerifyLogStartsWith(
          LogLevel.Information,
          "REGISTER-USER-START Began registering user with email test@test.cz",
          Times.Once()
         );
        _loggerMock.VerifyLogStartsWith(
          LogLevel.Information,
          "REGISTER-USER-EXISTS Email test@test.cz already exists in database but account is not active",
          Times.Once()
          );
    }

    [Fact]
    public async Task RegisterUser_ReturnsInvalidEmail_WhenMailIsInvalid()
    {
        //Arrange
        _emailValidatorMock
            .Setup(x => x.ValidateEmail(It.IsAny<string>()))
            .Returns(false);

        var request = new RegisterUserCommand(new RegisterUserRequestDto() { Email = "test@test.cz", Login = "Login", Name = "Name", Password = "password", Role = "user" });

        //Act
        var result = await _sut.Handle(request, CancellationToken.None);

        //Assert
        Assert.IsType<RegisterUserResponseDto>(result);
        Assert.Equal(UserConstants.InvalidEmail, result.Result);

        _loggerMock.VerifyLogStartsWith(
          LogLevel.Information,
          "REGISTER-USER-START Began registering user with email test@test.cz",
          Times.Once()
         );
        _loggerMock.VerifyLogStartsWith(
          LogLevel.Information,
          "REGISTER-USER-INVALID-MAIL Email test@test.cz is invalid",
          Times.Once()
          );
    }

    [Fact]
    public async Task RegisterUser_ReturnsSuccess_WhenRegistrationSucceeded()
    {
        //Arrange
        _userRepositoryMock
            .Setup(x => x.GetByEmail(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        _emailValidatorMock
            .Setup(x => x.ValidateEmail(It.IsAny<string>()))
            .Returns(true);

        _userRepositoryMock
            .Setup(x => x.Add(It.IsAny<User>()))
            .ReturnsAsync(new User { Id = 1, Email = "test@test.cz", Login = "Login", Name = "Name", Password = "password", Role = "user", IsActive = false });

        _emailServiceMock
            .Setup(x => x.SendRegistrationCompleteEmailAsync(It.IsAny<SendRegistrationEmailDto>()))
            .Returns(Task.CompletedTask);

        var request = new RegisterUserCommand(new RegisterUserRequestDto() { Email = "test@test.cz", Login = "Login", Name = "Name", Password = "password", Role = "user" });

        var userForRegistration = new User() { Email = "test@test.cz", Login = "Login", Name = "Name", Password = "password", Role = "user" };

        //Act
        var result = await _sut.Handle(request, CancellationToken.None);

        //Assert
        Assert.IsType<RegisterUserResponseDto>(result);
        Assert.Equal(UserConstants.RegistrationSuccessfullMailSuccessfull, result.Result);

        _userRepositoryMock.Verify(x => x.GetByEmail(It.IsAny<string>()), Times.Once);
        _emailValidatorMock.Verify(x => x.ValidateEmail(It.IsAny<string>()), Times.Once);
        _userRepositoryMock.Verify(
            x => x.Add(It.Is<User>(u =>
            u.Email == "test@test.cz" &&
            u.Login == "Login" &&
            u.Name == "Name" &&
            u.Password == "password" &&
            u.Role == "user"
        )),
        Times.Once
        );
        _emailServiceMock.Verify(x => x.SendRegistrationCompleteEmailAsync(It.IsAny<SendRegistrationEmailDto>()), Times.Once);

        _loggerMock.VerifyLogStartsWith(
          LogLevel.Information,
          "REGISTER-USER-START Began registering user with email test@test.cz",
          Times.Once()
         );
        _loggerMock.VerifyLogStartsWith(
          LogLevel.Information,
          "REGISTER-USER-SUCCESS User test@test.cz successfully registered",
          Times.Once()
          );
    }
}
