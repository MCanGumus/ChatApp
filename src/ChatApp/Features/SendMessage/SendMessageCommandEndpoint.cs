using MediatR;
using Shared.Extension;

namespace ChatApp.Features.SendMessage
{
    public static class SendMessageCommandEndpoint
    {
        public static RouteGroupBuilder SendMessageGroupItemEndpoint(this RouteGroupBuilder group)
        {
            group.MapPost("/sendMessage", async (SendMessageCommandRequest command, IMediator mediator)
                => (await mediator.Send(command)).ToGenericResult())
                .WithName("SendMessage");

            return group;
        }
    }
}
