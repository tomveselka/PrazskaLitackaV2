using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrazskaLitacka.WebApi.Utils;
public class EmailValidator : IEmailValidator
{
    public bool ValidateEmail(string email)
    {
        return new EmailAddressAttribute().IsValid(email);
    }
}
