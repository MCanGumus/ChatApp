using MediatR;
using Shared;

namespace ChatApp.Features.SendMessage
{
    public record SendMessageCommandRequest(Guid toWho, string message) : IRequestByServiceResult<SendMessageCommandResponse>;
}
