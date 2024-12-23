namespace ChatApp.Features.SendMessage
{
    public static class SendMessageGroupEndpoint
    {
        public static void AddMessageGroupEndpointExt(this WebApplication app)
        {
            app.MapGroup("api/v1/messages").WithTags("Message")
                .SendMessageGroupItemEndpoint();
        }
    }
}
