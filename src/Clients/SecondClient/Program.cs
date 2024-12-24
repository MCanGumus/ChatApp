using Microsoft.AspNetCore.SignalR.Client;

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:7037/chatHub?userId=f9c03b33-89c3-4e34-b63f-5ada43f4ff57") // Kullanıcı kimliği query string'de
    .Build();

// Sunucudan gelen mesajları dinlemek için bir yöntem tanımlayın
connection.On<string>("ReceiveMessage", message =>
{
    Console.WriteLine($"{message.Split(':')[0]}: {message.Split(':')[1]}");
});

try
{
    // Bağlantıyı başlat
    await connection.StartAsync();
    Console.WriteLine("SignalR bağlantısı başarılı!");
}
catch (Exception ex)
{
    Console.WriteLine($"Bağlantı hatası: {ex.Message}");
}

string name = "Can";

// Mesaj gönderme
while (true)
{
    Console.WriteLine("Mesajınızı girin (çıkmak için 'q' yazın):");
    var input = Console.ReadLine();
    if (input?.ToLower() == "q") break;

    try
    {
        await connection.InvokeAsync("SendMessage", Guid.Parse("ba6b870b-e74c-42b3-8c31-5d3f18ee2f6a"), name + ":" + input);
        Console.WriteLine("Mesaj gönderildi!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Mesaj gönderme hatası: {ex.Message}");
    }
}

// Bağlantıyı sonlandır
await connection.StopAsync();
Console.WriteLine("Bağlantı sonlandırıldı.");
