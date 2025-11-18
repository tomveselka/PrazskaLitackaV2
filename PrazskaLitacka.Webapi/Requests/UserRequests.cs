using MediatR;
using PrazskaLitacka.Domain.Dto;
using PrazskaLitacka.Domain.Entities;

namespace PrazskaLitacka.Webapi.Requests;

public class UserRequests
{
    public record GetUserByIdQuery(int userId) : IRequest<User>;
    public record ResetPasswordCommand(string email)  : IRequest<Result>;
    public record RegisterUserCommand(RegisterUserRequestDto dto) : IRequest<RegisterUserResponseDto>;
}
