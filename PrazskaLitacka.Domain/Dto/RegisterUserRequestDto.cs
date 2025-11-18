using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrazskaLitacka.Domain.Dto;
public class RegisterUserRequestDto
{
    public required string Name { get; set; }
    public required string Login { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public string Role { get; set; } = "player";
}
