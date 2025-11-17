using Microsoft.Extensions.Logging;
using Moq;
using PrazskaLitacka.Domain.Entities;
using PrazskaLitacka.Domain.Exceptions;
using PrazskaLitacka.Domain.Interfaces;
using PrazskaLitacka.Webapi.Handlers.UserHandlers;
using PrazskaLitacka.WebApi.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PrazskaLitacka.Webapi.Requests.UserRequests;

namespace PrazskaLitacka.WebApi.Tests.Handlers.UserHandlers;
public class GetUserByIdHandlerTest
{
    private readonly Mock<ILogger<GetUserByIdHandler>> _loggerMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly GetUserByIdHandler _sut;

    public GetUserByIdHandlerTest()
    {
        _loggerMock = new Mock<ILogger<GetUserByIdHandler>>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _sut = new GetUserByIdHandler(_loggerMock.Object, _userRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsUserData_WhenIdExists()
    {
        //Arrange
        _userRepositoryMock
            .Setup(x => x.GetById(It.IsAny<int>()))
            .ReturnsAsync(new User
            {
                Id = 1,
                Email = "email@email.cz",
                Name = "Name",
                Login = "Login",
                Password = "Password"
            });

        //Act
        var result = await _sut.Handle(new GetUserByIdQuery(1), CancellationToken.None);

        //Assert
        Assert.Equal("Name", result.Name);

        _userRepositoryMock
            .Verify(x => x.GetById(It.IsAny<int>()), Times.Once);

        _loggerMock.VerifyLogStartsWith(
           LogLevel.Information,
           "USER-BY-ID Requested user with id 1",
           Times.Once()
       );
        _loggerMock.VerifyLogStartsWith(
           LogLevel.Information,
           "USER-BY-ID Retrieved user with id 1 and email email@email.cz",
           Times.Once()
       );
    }

    [Fact]
    public async Task Handle_ThrowsException_WhenIdDoesntExist()
    {
        //Arrange
        _userRepositoryMock
            .Setup(x => x.GetById(It.IsAny<int>()))
            .ReturnsAsync((User)null);

        //Act
        await Assert.ThrowsAsync<UserNotFoundException>(() =>
            _sut.Handle(new GetUserByIdQuery(1), CancellationToken.None));

        //Assert
        _userRepositoryMock
            .Verify(x => x.GetById(It.IsAny<int>()), Times.Once);

        _loggerMock.VerifyLogStartsWith(
           LogLevel.Information,
           "USER-BY-ID Requested user with id 1",
           Times.Once()
       );
        _loggerMock.VerifyLogStartsWith(
           LogLevel.Warning,
           "USER-BY-ID No User found with id 1",
           Times.Once()
       );
    }
}
