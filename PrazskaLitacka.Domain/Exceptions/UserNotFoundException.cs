using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrazskaLitacka.Domain.Exceptions;
public class UserNotFoundException : Exception
{
    public string IdentifierType { get; }
    public string IdentifierValue { get; }


    public UserNotFoundException(string identifierType, string identifierValue)
        : base($"User with {identifierType}={identifierValue} was not found.")
    {
        IdentifierType = identifierType;
        IdentifierValue = identifierValue;
    }
}
