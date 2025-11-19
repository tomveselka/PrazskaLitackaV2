using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using PrazskaLitacka.Domain.DbContexts;
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

    public RegisterUserHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _emailServiceMock = new Mock<IEmailService>();
        _loggerMock = new Mock<ILogger<RegisterUserHandler>>();
        _mapper = MapperExtension.CreateMapper();


    }
}
