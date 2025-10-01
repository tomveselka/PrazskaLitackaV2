using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrazskaLitacka.WebApi.Tests.Extensions;
public static class MoqLoggerExtensions
{
    public static void VerifyLogStartsWith<T>(
        this Mock<ILogger<T>> logger,
        LogLevel level,
        string startsWith,
        Times times)
    {
        logger.Verify(
            x => x.Log(
                level,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString().StartsWith(startsWith)),
                null, // ignore exception
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ),
            times
        );
    }
}
