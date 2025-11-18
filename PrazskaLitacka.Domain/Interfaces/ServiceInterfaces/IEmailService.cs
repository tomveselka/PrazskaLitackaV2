using PrazskaLitacka.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrazskaLitacka.Domain.Interfaces.ServiceInterfaces;
public interface IEmailService
{
    public Task SendRegistrationCompleteEmailAsync(SendRegistrationEmailDto dto);
}
