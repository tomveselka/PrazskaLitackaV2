using PrazskaLitacka.Domain.Dto;
using PrazskaLitacka.Domain.Interfaces.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrazskaLitacka.Infrastructure.Services.Impl;
public class EmailService : IEmailService
{
    public Task SendRegistrationCompleteEmailAsync(SendRegistrationEmailDto dto)
    {
        return Task.CompletedTask;
    }
}
