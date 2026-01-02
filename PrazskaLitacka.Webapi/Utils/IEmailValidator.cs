using System;
using System.Collections.Generic;
using System.Text;

namespace PrazskaLitacka.WebApi.Utils;
public interface IEmailValidator
{
    public bool ValidateEmail(string email);
}
