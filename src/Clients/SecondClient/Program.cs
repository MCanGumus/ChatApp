using Microsoft.AspNetCore.SignalR.Client;

Console.WriteLine("Welcome to ChatApp. Please enter your nickname.");
//string name = Console.ReadLine().ToString();
string name = "deneme_2";

#region Connection States
var connection = new HubConnectionBuilder()
    .WithUrl($"http://localhost:7040/chatHub?userId={name}") // Kullanıcı kimliği query string'de
    .Build();

try
{
    await connection.StartAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"Bağlantı hatası: {ex.Message}");
}
#endregion


#region Listeners
connection.On<string>("ReceiveMessage", message =>
{
    Console.WriteLine($"{message.Split(':')[0]}: {message.Split(':')[1]}");
});

connection.On<IDictionary<string, string>>("GetConnectedUsers", users =>
{
    foreach (var user in users)
    {
        Console.WriteLine($"{user.Key} / {user.Value}");
    }
});
#endregion



Console.WriteLine("Mesajınızı girin (çıkmak için 'q' yazın):");
//// Mesaj gönderme
while (true)
{
    var input = Console.ReadLine();
    if (input?.ToLower() == "q") break;

    try
    {
        await connection.InvokeAsync("SendMessage", "deneme_1", name + ":" + input);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Mesaj gönderme hatası: {ex.Message}");
    }
}

// Bağlantıyı sonlandır
await connection.StopAsync();
Console.WriteLine("Bağlantı sonlandırıldı.");