using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrazskaLitacka.Infrastructure.Exceptions;
public class StopsNotRetrievedException : Exception
{
    public StopsNotRetrievedException() { }

    public StopsNotRetrievedException(string message) : base(message) { }
}

