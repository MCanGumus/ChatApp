using MediatR;
using Shared;

namespace ChatApp.Features.SendMessage
{
    public class SendMessageCommandHandler(ChatHub hub) : IRequestHandler<SendMessageCommandRequest, ServiceResult>
    {
        public async Task<ServiceResult> Handle(SendMessageCommandRequest request, CancellationToken cancellationToken)
        {
            await hub.SendMessage(request.toWho, request.message);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
