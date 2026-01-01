using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using PrazskaLitacka.Domain.DbContexts;
using PrazskaLitacka.Domain.Entities;
using PrazskaLitacka.Domain.Interfaces;
using PrazskaLitacka.Domain.Interfaces.ServiceInterfaces;
using PrazskaLitacka.Webapi.Mappers;
using PrazskaLitacka.WebApi.Handlers.UserHandlers;
using PrazskaLitacka.WebApi.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrazskaLitacka.WebApi.Tests.Handlers.UserHandlers;
public class RegisterUserHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _dbContext;
    private readonly Mock<ILogger<RegisterUserHandler>> _loggerMock;
    private readonly RegisterUserHandler _sut;

    public RegisterUserHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _emailServiceMock = new Mock<IEmailService>();
        _loggerMock = new Mock<ILogger<RegisterUserHandler>>();
        _mapper = MapperExtension.CreateMapper();

        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;


        _dbContext = new ApplicationDbContext(options);
        _dbContext.Database.EnsureCreated();

        
        var users = new List<User>
        {
            new User { Id = 1, Name = "Name1", Login = "Login1", Password = "Password1", Email = "Email1", Role="User" },
            new User { Id = 2, Name = "Name2", Login = "Login2", Password = "Password2", Email = "Email2", Role="User" }
        };
        _dbContext.Users.AddRange(users);

        _sut = new RegisterUserHandler(_userRepositoryMock.Object, _emailServiceMock.Object, _mapper, _dbContext, _loggerMock.Object);
    }

    [Fact]
    public async Task RegisterUser_ReturnsUserExists_WhenEmailExistsInDatabase()
    {
        //Arrange
        
        
    }
}
